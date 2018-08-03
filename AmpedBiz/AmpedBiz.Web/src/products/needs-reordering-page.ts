import { autoinject, BindingEngine, Disposable } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { NeedsReorderingPageItem, ForPurchasing } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { SessionData } from '../services/session-data';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class NeedsReorderingPage {

  private _subscriptions: Disposable[] = [];

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<NeedsReorderingPageItem> = new Pager<NeedsReorderingPageItem>();
  public forPurchasing: ForPurchasing;
  public suppliers: Lookup<string>[] = [];
  public categories: Lookup<string>[] = [];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _sessionData: SessionData,
    private readonly _notification: NotificationService,
    private readonly _bindingEngine: BindingEngine
  ) {
    this.filter["supplierId"] = this._sessionData.forPurchasing && this._sessionData.forPurchasing.supplierId || '';
    this.filter.onFilter = () => this.getPage();
    this.sorter["supplierId"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["reorderLevel"] = SortDirection.None;
    this.sorter["available"] = SortDirection.None;
    this.sorter["currentLevel"] = SortDirection.None;
    this.sorter["targetLevel"] = SortDirection.None;
    this.sorter["belowTarget"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();

    this.forPurchasing = this._sessionData.forPurchasing;
    this.forPurchasing.selectedProductIds = this.forPurchasing.selectedProductIds || [];
    this._sessionData.forPurchasing = {};
  }

  public async attached(): Promise<void> {
    this._subscriptions = [
      this._bindingEngine
        .collectionObserver(this.forPurchasing.selectedProductIds)
        .subscribe((changeRecords) => this.computeSelectAll(changeRecords)),
    ];

    [this.suppliers, this.categories] = await Promise.all([
      this._api.suppliers.getLookups(),
      this._api.productCategories.getLookups()
    ]);

    await this.getPage();
  }

  public detached(): void {
    this._subscriptions.forEach(x => x.dispose());
  }

  public create(): void {
    this._sessionData.forPurchasing = {
      supplierId: this.filter['supplierId'],
      selectedProductIds: this.forPurchasing.selectedProductIds,
      purchaseAllBelowTarget: this.forPurchasing.purchaseAllBelowTarget || false
    };
    this._router.navigateToRoute('new-purchase-order');
  }

  public async getPage(): Promise<void> {
    try {
      this.forPurchasing.supplierId = this.filter['supplierId'] || '';
      let data = await this._api.products.getNeedsReorderingPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<NeedsReorderingPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
      if (this.forPurchasing.purchaseAllBelowTarget) {
        this.toggleSelectAll(true);
      }
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
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