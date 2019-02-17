import { autoinject } from 'aurelia-framework';
import { ProductCategory } from '../../common/models/product-category';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject
export class ProductCategoryCreate {

  public header: string = 'Create Product Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public productCategory: ProductCategory;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) { }

  public async activate(productCategory: ProductCategory): Promise<void> {
    try {
      this.isEdit = (productCategory && productCategory.id) ? true : false;
      this.header = (this.isEdit) ? "Edit Product Category" : "Create Product Category";
      this.productCategory = (this.isEdit) ? await this._api.productCategories.get(productCategory.id) : <ProductCategory>{};
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.productCategory = (this.isEdit)
          ? await this._api.productCategories.update(this.productCategory)
          : await this._api.productCategories.create(this.productCategory);
        await this._notification.success("Product Category has been saved.").whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}
