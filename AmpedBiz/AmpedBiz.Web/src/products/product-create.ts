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
  private readonly _api: ServiceApi;
  private readonly _auth: AuthService;
  private readonly _router: Router;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  public header: string = 'Create Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public product: Product;
  public suppliers: Lookup<string>[];
  public categories: Lookup<string>[];

  constructor(
    api: ServiceApi,
    auth: AuthService,
    router: Router,
    notification: NotificationService,
    eventAggregator: EventAggregator
  ) {
    this._api = api;
    this._auth = auth;
    this._router = router;
    this._notification = notification;
    this._eventAggregator = eventAggregator;
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

      console.log('ff');
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

  public save(): void {
    this._notification.confirm('Do you want to save?').whenClosed(result => {
      if (result.output === ActionResult.Yes) {

        if (this.isEdit) {
          this._api.products.update(this.product)
            .then(data => this.resetAndNoify(data, "Product has been saved."))
            .catch(error => this._notification.warning(error));
        }
        else {
          this._api.products.create(this.product)
            .then(data => this.resetAndNoify(data, "Product has been saved."))
            .catch(error => this._notification.warning(error));
        }
      }
    });
  }

  public createAdjustment(): void {
    this._eventAggregator.publish(inventoryEvents.adjustment.create);
  }

  private resetAndNoify(product: Product, notificationMessage: string) {
    this.product = product;

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }
}