import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject, BindingEngine, Disposable } from 'aurelia-framework';
import { ProductCreate } from './product-create';
import { Product, NeedsReorderingPageItem, ForPurchasing } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { SessionData } from '../services/session-data';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class NeedsReorderingPage {
  private _api: ServiceApi;
  private _router: Router;
  private _sessionData: SessionData;
  private _notification: NotificationService;
  private _subscriptions: Disposable[] = [];
  private _bindingEngine: BindingEngine;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<NeedsReorderingPageItem>;
  public forPurchasing: ForPurchasing;

  public suppliers: Lookup<string>[] = [];
  public categories: Lookup<string>[] = [];

  constructor(api: ServiceApi, router: Router, sessionData: SessionData, notification: NotificationService, bindingEngine: BindingEngine) {
    this._api = api;
    this._router = router;
    this._sessionData = sessionData;
    this._notification = notification;
    this._bindingEngine = bindingEngine;

    this.filter = new Filter();
    this.filter["supplierId"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["supplierId"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["reorderLevel"] = SortDirection.None;
    this.sorter["available"] = SortDirection.None;
    this.sorter["currentLevel"] = SortDirection.None;
    this.sorter["targetLevel"] = SortDirection.None;
    this.sorter["belowTarget"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<NeedsReorderingPageItem>();
    this.pager.onPage = () => this.getPage();

    this.forPurchasing = this._sessionData.forPurchasing;
    this.forPurchasing.selectedProductIds = this.forPurchasing.selectedProductIds || [];
  }

  public attached(): void {
    this._subscriptions = [
      this._bindingEngine
        .collectionObserver(this.forPurchasing.selectedProductIds)
        .subscribe((changeRecords) => this.computeSelectAll(changeRecords)),
    ];

    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<string>[]>] = [
      this._api.suppliers.getLookups(),
      this._api.productCategories.getLookups()
    ];

    Promise.all(requests).then(data => {
      this.suppliers = data[0];
      this.categories = data[1];

      this.getPage();
    });
  }

  public detached(): void {
    this._subscriptions.forEach(x => x.dispose());
    this._sessionData.forPurchasing = this.forPurchasing;
  }

  public getPage(): void {
    this._api.products
      .getNeedsReorderingPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<NeedsReorderingPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;

        if (this.forPurchasing.purchaseAllBelowTarget) {
          this.toggleSelectAll(true);
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }



  public toggleSelectAll(selectAll?: boolean): boolean {
    if (selectAll == null) {
      selectAll = !this.forPurchasing.purchaseAllBelowTarget;
    }

    if (selectAll) {
      var count = this.pager.items.length;
      for (var i = 0; i < count; i++) {
        var product = this.pager.items[i];
        var selected = this.forPurchasing.selectedProductIds.indexOf(product.id) > -1;
        if (!selected) {
          this.forPurchasing.selectedProductIds.push(product.id);
        }
      }
    }
    else {
      var count = this.forPurchasing.selectedProductIds.length;
      for (var i = 0; i < count; i++) {
        var productId = this.forPurchasing.selectedProductIds[i];
        var index = this.forPurchasing.selectedProductIds.indexOf(productId, 0);
        this.forPurchasing.selectedProductIds.splice(index, 1);
        this.forPurchasing.purchaseAllBelowTarget = false;
      }
    }

    this.forPurchasing.purchaseAllBelowTarget = selectAll;
    return true;
  }

  private computeSelectAll(changeRecords: any) {
    if (changeRecords[0].removed.length > 0) {
      this.forPurchasing.purchaseAllBelowTarget = false;
      return;
    }

    if (this.forPurchasing.selectedProductIds.length == this.pager.count) {
      this.forPurchasing.purchaseAllBelowTarget = true;
      return;
    }
  }
}