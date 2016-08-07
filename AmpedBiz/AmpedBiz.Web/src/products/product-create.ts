import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {Product} from '../common/models/product';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = 'Create Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public product: Product;
  public suppliers: Lookup<string>[];
  public categories: Lookup<string>[];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;
  }

  activate(product: Product) {
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
        : Promise.resolve(<Product>{})
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

  back() {
    return this._router.navigateBack();
  }

  save() {
    this._api.products.update(this.product)
      .then(data => this._notification.success("Product has been saved."))
      .catch(error => this._notification.warning(error));
  }
}