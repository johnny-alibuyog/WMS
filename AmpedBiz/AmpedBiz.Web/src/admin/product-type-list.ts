import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ProductCreate} from './product-type-create';
import {ProductType} from './common/models/product-type';
import {ProductTypeService} from '../services/product-type-service';


@autoinject
export class ProductTypeList {
  private _service: ProductTypeService;
  private _dialog: DialogService;
  public productTypes: ProductType[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: ProductTypeService) {
    this._dialog = dialog;
    this._service = service;
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
      success: (data) => this.productTypes = <ProductType[]>data,
      error: (error) => console.log('error')
    });
  }

  create() {
    this._dialog
      .open({ viewModel: ProductCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: ProductType) {
    this._dialog
      .open({ viewModel: ProductCreate, model: item })
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