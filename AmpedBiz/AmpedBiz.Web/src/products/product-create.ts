import { autoinject } from 'aurelia-framework';
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

@autoinject
export class ProductCreate {

  private _subscriptions: Subscription[] = [];

  public header: string = 'Create Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public product: Product;
  public suppliers: Lookup<string>[];
  public categories: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) {
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
          : Promise.resolve(<Product>{ unitOfMeasures: [] })
      ]);
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
}