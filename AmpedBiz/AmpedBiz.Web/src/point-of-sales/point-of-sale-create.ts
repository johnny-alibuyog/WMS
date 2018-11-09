import { pricing } from './../common/models/pricing';
import { autoinject, computedFrom, observable } from "aurelia-framework";
import { ServiceApi } from "../services/service-api";
import { AuthService } from "../services/auth-service";
import { Router } from "aurelia-router";
import { DialogService } from "aurelia-dialog";
import { NotificationService } from "../common/controls/notification-service";
import { PointOfSale, pointOfSaleEvents } from "../common/models/point-of-sale";
import { Lookup } from "../common/custom_types/lookup";
import { role } from "../common/models/role";
import { isNullOrWhiteSpace } from "../common/utils/string-helpers";
import { EventAggregator, Subscription } from "aurelia-event-aggregator";

@autoinject
export class PointOfSaleCreate {
  private _subscriptions: Subscription[] = [];

  public readonly header: string = "Point Of Sale";
  public readonly canTender: boolean = false;

  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];
  public customers: Lookup<string>[] = [];
  public paymentTypes: Lookup<string>[] = [];
  public pricings: Lookup<string>[] = [];
  public pointOfSale: PointOfSale = <PointOfSale>{
    id: null,
    branch: this._auth.userBranchAsLookup,
    tenderedBy: this._auth.userAsLookup,
    tenderedOn: new Date(),
    pricing: pricing.retailPrice
  };

  constructor(
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _router: Router,
    private readonly _dialog: DialogService,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.canTender = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
  }

  public async activate(pointOfSale: PointOfSale): Promise<void> {
    try {
      [
        this.products,
        this.branches,
        this.customers,
        this.paymentTypes,
        this.pricings,
        this.pointOfSale
      ] = await Promise.all([
        this._api.products.getLookups(),
        this._api.branches.getLookups(),
        this._api.customers.getLookups(),
        this._api.paymentTypes.getLookups(),
        this._api.pricings.getLookups(),
        (pointOfSale && pointOfSale.id || null)
          ? this._api.pointOfSales.get(pointOfSale.id)
          : Promise.resolve(this.pointOfSale)
      ]);
    }
    catch (error) {
      this._notification.error(error);
    }
  }

  public async deactivate() {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  @computedFrom("pointOfSale.id")
  public get isSaleTendred(): boolean {
    return !isNullOrWhiteSpace(this.pointOfSale && this.pointOfSale.id || null);
  }

  public async printInvoice(): Promise<void> {
    return Promise.resolve();
    // let data = await this._api.orders.getInvoiceDetail(this.order.id);
    // await this._invoiceReport.show(data);
  }

  public addItem(): void {
    this._eventAggregator.publish(pointOfSaleEvents.item.add);
  }


  public addPayment(): void {
    this._eventAggregator.publish(pointOfSaleEvents.payment.add);
  }

  public async tender(): Promise<void> {
    try {
      let confirmation = await this._notification.confirm('Do you want to tender sale?').whenClosed();

      if (!confirmation.wasCancelled) {
        this.pointOfSale = await this._api.pointOfSales.create(this.pointOfSale)
        await this._notification.success('Sale has been tendred?').whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}
