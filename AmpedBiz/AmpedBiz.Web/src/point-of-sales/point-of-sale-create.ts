import { PointOfSaleDetail, PointOfSaleStatus, PointOfSalePayable } from './../common/models/point-of-sale';
import { autoinject, computedFrom } from "aurelia-framework";
import { ServiceApi } from "../services/service-api";
import { AuthService } from "../services/auth-service";
import { Router } from "aurelia-router";
import { DialogService } from "aurelia-dialog";
import { NotificationService } from "../common/controls/notification-service";
import { PointOfSale, pointOfSaleEvents, initialPointOfSale } from "../common/models/point-of-sale";
import { Lookup } from "../common/custom_types/lookup";
import { role } from "../common/models/role";
import { isNullOrWhiteSpace } from "../common/utils/string-helpers";
import { EventAggregator, Subscription } from "aurelia-event-aggregator";
import { ReceiptGenerator } from "./receipt-generator";

@autoinject
export class PointOfSaleCreate {
  private _subscriptions: Subscription[] = [];

  public readonly header: string = "Point Of Sale";
  public readonly canTender: boolean = false;
  public activeTab: 'itemsTab' | 'paymentsTab' = 'itemsTab';

  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];
  public customers: Lookup<string>[] = [];
  public paymentTypes: Lookup<string>[] = [];
  public pricings: Lookup<string>[] = [];
  public statuses: Lookup<PointOfSaleStatus>[] = [];
  public pointOfSale: PointOfSale = {};
  public payable: PointOfSalePayable = {};

  constructor(
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _router: Router,
    private readonly _dialog: DialogService,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator,
    private readonly _receiptGenerator: ReceiptGenerator
  ) {
    this.pointOfSale = initialPointOfSale(this._api);
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
        this.statuses,
        this.pointOfSale
      ] = await Promise.all([
        this._api.products.getLookups(),
        this._api.branches.getLookups(),
        this._api.customers.getLookups(),
        this._api.paymentTypes.getLookups(),
        this._api.pricings.getLookups(),
        this._api.pointOfSales.getStatusLookup(),
        (pointOfSale && pointOfSale.id || null)
          ? this._api.pointOfSales.get(pointOfSale.id)
          : Promise.resolve(initialPointOfSale(this._api))
      ]);
      this.activeTab = "itemsTab";
      window.addEventListener('keydown', this.handleKeyInput, false);
    }
    catch (error) {
      this._notification.error(error);
    }
  }

  public async deactivate() {
    this._subscriptions.forEach(subscription => subscription.dispose());
    window.removeEventListener('keydown', this.handleKeyInput);
  }

  private handleKeyInput = (event: KeyboardEvent) => {
    if (event.ctrlKey && event.altKey) {
      switch (event.keyCode) {
        case 83:  /* Ctrl + Alt + s */
          this.tender();
          break;
        case 82: /* Ctrl + Alt + n */
          this.showReceipt();
          break;
        case 78: /* Ctrl + Alt + n */
          this.clear();
          break;
        case 73: /* Ctrl + Alt + i */
          this.addItem();
          break;
        case 80: /* Ctrl + Alt + p */
          this.addPayment();
          break;
      }
    }
  }

  @computedFrom("pointOfSale.id")
  public get isSaleTendered(): boolean {
    return !isNullOrWhiteSpace(this.pointOfSale && this.pointOfSale.id || null);
  }

  @computedFrom("pointOfSale.status")
  public get isFullyPaid(): boolean {
    return (this.pointOfSale && this.pointOfSale.status || PointOfSaleStatus.unPaid) === PointOfSaleStatus.fullyPaid;
  }

  private generateReceiptData(): PointOfSaleDetail {
    return <PointOfSaleDetail>{
      branchName: this.pointOfSale.branch && this.pointOfSale.branch.name || '',
      customerName: this.pointOfSale.customer && this.pointOfSale.customer.name || '',
      invoiceNumber: this.pointOfSale.invoiceNumber,
      tenderedOn: this.pointOfSale.tenderedOn,
      tenderedByName: this.pointOfSale.tenderedBy.name,
      pricingName: this.pointOfSale.pricing && this.pointOfSale.pricing.name || '',
      paymentTypeName: this.pointOfSale.paymentType && this.pointOfSale.paymentType.name || '',
      subTotalAmount: this.pointOfSale.subTotalAmount,
      discountAmount: this.pointOfSale.discountAmount,
      totalAmount: this.pointOfSale.totalAmount,
      receivedAmount: this.pointOfSale.receivedAmount,
      changeAmount: this.pointOfSale.changeAmount,
      paidAmount: this.pointOfSale.paidAmount,
      balanceAmount: this.pointOfSale.balanceAmount,
      items: this.pointOfSale.items
    };
  }

  public async showReceipt(): Promise<void> {
    let data = this.generateReceiptData();
    await this._receiptGenerator.show(data);
  }

  public async printReceipt(): Promise<void> {
    if (this.isSaleTendered) {
      let data = this.generateReceiptData();
      await this._receiptGenerator.print(data);
    }
  }

  public addItem(): void {
    this.activeTab = 'itemsTab';
    this._eventAggregator.publish(pointOfSaleEvents.item.add);
  }

  public addPayment(): void {
    this.activeTab = 'paymentsTab';
    this._eventAggregator.publish(pointOfSaleEvents.payment.add);
  }

  public async tender(): Promise<void> {
    try {
      if (this.isSaleTendered && this.isFullyPaid) {
        return;
      }

      let message = !this.isSaleTendered
        ? 'Do you want to tender sale?'
        : 'Do you want to save sale';

      let confirmation = await this._notification.confirm(message).whenClosed();

      if (!confirmation.wasCancelled) {
        this.pointOfSale = await this._api.pointOfSales.create(this.pointOfSale);
        this._eventAggregator.publish(pointOfSaleEvents.saved);
        await this.printReceipt();
        await this._notification.success('Sale has been tendred').whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async clear(): Promise<void> {
    let message = this.isSaleTendered
      ? 'Do you want to create new sale?'
      : 'Do you want to clear entries?';

    let confirmation = await this._notification.confirm(message).whenClosed();

    if (!confirmation.wasCancelled) {
      this.pointOfSale = initialPointOfSale(this._api);
      await this.activate({});
    }
  }
}
