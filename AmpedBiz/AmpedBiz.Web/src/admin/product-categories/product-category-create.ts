import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ProductCategory } from '../../common/models/product-category';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class ProductCategoryCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Product Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public productCategory: ProductCategory;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  public async activate(productCategory: ProductCategory): Promise<void> {
    try {
      if (productCategory) {
        this.header = "Edit Product Category";
        this.isEdit = true;
        this.productCategory = await this._api.productCategories.get(productCategory.id);
      }
      else {
        this.header = "Create Product Category";
        this.isEdit = false;
        this.productCategory = <ProductCategory>{};
      }

    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public cancel(): void {
    this._controller.cancel();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = (this.isEdit)
          ? await this._api.productCategories.update(this.productCategory)
          : await this._api.productCategories.create(this.productCategory);
        await this._notification.success("Product Category has been saved.").whenClosed();
        this._controller.ok(<ProductCategory>data);
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}