import { Supplier } from './../common/models/supplier';
import { autoinject, observable } from 'aurelia-framework';
import { role } from '../common/models/role';
import { AuthService } from '../services/auth-service';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Product } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { ActionResult } from '../common/controls/notification';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { inventoryEvents } from '../common/models/inventory';
import { Pager } from '../common/models/paging';
import * as Enumerable from 'linq';

@autoinject
export class ProductCreate {

  private _subscriptions: Subscription[] = [];

  public header: string = 'Create Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public product: Product;
  public suppliers: Lookup<string>[] = [];
  public categories: Lookup<string>[] = [];
  public supplierPager: Pager<Supplier> = new Pager<Supplier>();

  @observable()
  public isAllSuppliersAssigned: boolean = false;

  @observable()
  public isOnlyAssignedSuppliersVisible: boolean = false;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.supplierPager.onPage = () => this.initializeSupplierPage();
    this.canSave = this._auth.isAuthorized([role.admin, role.manager]);
  }

  public async activate(product: Product): Promise<void> {
    try {
      this.isEdit = ((product && product.id)) ? true : false;
      this.header = (this.isEdit) ? "Edit Product" : "Create Product";
      [this.suppliers, this.categories, this.product] = await Promise.all([
        this._api.suppliers.getLookups(),
        this._api.productCategories.getLookups(),
        this.isEdit
          ? this._api.products.get(product.id)
          : this.initialProduct()
      ]);

      this.initializeSupplierPage();
    }
    catch (error) {
      this._notification.error(error);
    }
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        inventoryEvents.adjustment.created,
        response => this.activate(this.product)
      ),
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.product = (this.isEdit)
          ? await this._api.products.update(this.product)
          : await this._api.products.create(this.product);
        await this._notification.success("Product has been saved.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public createAdjustment(): void {
    this._eventAggregator.publish(inventoryEvents.adjustment.create);
  }

  public toggle(supplier: Supplier): void {
    supplier.assigned = !supplier.assigned;
    this.initializeSupplierPage();
  }

  public isOnlyAssignedSuppliersVisibleChanged(newValue: boolean, oldValue: boolean): void {
    this.initializeSupplierPage();
  }

  public isAllSuppliersAssignedChanged(newValue: boolean, oldValue: boolean): void {
    if (!this.product || !this.product.suppliers){
      return;
    }
    this.product.suppliers.forEach(x => x.assigned = newValue);
    this.initializeSupplierPage();
  }

  private async initialProduct(): Promise<Product> {
    var suppliers = await this._api.suppliers.getList();

    return <Product>{
      suppliers: suppliers,
      unitOfMeasures: [],
    };
  }

  public initializeSupplierPage(): void {
    if (!this.product || !this.product.suppliers) {
      return;
    }

    let suppliers = this.isOnlyAssignedSuppliersVisible
      ? Enumerable.from(this.product.suppliers).where(x => x.assigned).toArray()
      : this.product.suppliers ;

    this.supplierPager.count = suppliers.length;
    this.supplierPager.items = suppliers.slice(
      this.supplierPager.start,
      this.supplierPager.end
    );
  }
}
