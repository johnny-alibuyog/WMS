import { AuthService } from '../services/auth-service';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Product } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { autoinject } from 'aurelia-framework';
import { role } from '../common/models/role';

@autoinject
export class ProductCreate {
  private readonly _api: ServiceApi;
  private readonly _auth: AuthService;
  private readonly _router: Router;
  private readonly _notification: NotificationService;

  public header: string = 'Create Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public product: Product;
  public suppliers: Lookup<string>[];
  public categories: Lookup<string>[];

  constructor(api: ServiceApi, auth: AuthService, router: Router, notification: NotificationService) {
    this._api = api;
    this._auth = auth;
    this._router = router;
    this._notification = notification;
    this.canSave = this._auth.isAuthorized([role.admin, role.manager]);
  }

  public activate(product: Product): void {
    if (product && product.id) {
      this.header = "Edit Product";
      this.isEdit = true;
    }
    else {
      this.header = "Create Product";
      this.isEdit = false;
    }

    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<string>[]>, Promise<Product>] = [
      this._api.suppliers.getLookups(),
      this._api.productCategories.getLookups(),
      this.isEdit
        ? this._api.products.get(product.id)
        : Promise.resolve(<Product>{ unitOfMeasures: [] })
    ];

    Promise.all(requests)
      .then(responses => {
        this.suppliers = responses[0];
        this.categories = responses[1];
        this.product = responses[2];
      })
      .catch(error => {
        this._notification.error(error);
      });
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public save(): void {
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

  private resetAndNoify(product: Product, notificationMessage: string) {
    this.product = product;

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }
}