import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ProductTypeCreate} from './product-type-create';
import {ProductType} from './common/models/product-type';
import {ProductTypeService} from '../services/product-type-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductTypeList {
  private _notification: NotificationService;
  private _service: ProductTypeService;
  private _dialog: DialogService;
  
  public productTypes: ProductType[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: ProductTypeService, notification: NotificationService) {
    this._dialog = dialog;
    this._service = service;
    this._notification = notification;
  }

  activate() {
    this.refreshList();
  }

  refreshList() {
    this.filterText = '';
    this.filter();
  }
  
  filter() {
    this._service.getProductTypes(this.filterText, {
      success: (data) => {
        this.productTypes = <ProductType[]>data
        if (!this.productTypes || this.productTypes.length == 0){
          this._notification.error("Error encountered during search!");
        }
      },
      error: (error) => {
        this._notification.error("Error encountered during search!");
      }
    });
  }

  create() {
    this._dialog
      .open({ viewModel: ProductTypeCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: ProductType) {
    this._dialog
      .open({ viewModel: ProductTypeCreate, model: item })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  delete(item: any) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}