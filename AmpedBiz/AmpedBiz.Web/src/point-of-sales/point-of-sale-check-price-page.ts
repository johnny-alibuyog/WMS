import { ServiceApi } from './../services/service-api';
import { ProductRetailPriceDetails } from './../common/models/product';
import { autoinject, customElement } from "aurelia-framework";
import { EventAggregator, Subscription } from "aurelia-event-aggregator";
import { pointOfSaleEvents } from "./../common/models/point-of-sale";
import { Filter, Sorter, Pager, PagerRequest, PagerResponse } from "common/models/paging";
import { NotificationService } from './../common/controls/notification-service';
import * as Enumerable from 'linq';

@autoinject()
@customElement('point-of-sale-check-price-page')
export class PointOfSaleCheckPricePage {
  private _subscriptions: Subscription[] = [];

  public isFocused: boolean = false;
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public selectedItem: ProductRetailPriceDetails = null;
  public pager: Pager<ProductRetailPriceDetails> = new Pager<ProductRetailPriceDetails>({ pageSize: 3 });

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.filter["key"] = '';
    this.filter.onFilter = () => this.getPage();
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public select(item: ProductRetailPriceDetails): void {
    this.selectedItem = item;
  }

  public async getPage(): Promise<void> {
    try {
      const key = String(this.filter["key"]);

      if (!key || key.trim() === '') {
        this.pager.clear();
        return;
      }

      let data = await this._api.products.getProductRetailPriceDetails({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ProductRetailPriceDetails>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
      this.selectedItem = Enumerable.from(this.pager.items).firstOrDefault();
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        pointOfSaleEvents.checkPrices,
        () => this.isFocused = true
      )
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(
      subscription => subscription.dispose()
    );
  }
} 
