import { isNullOrWhiteSpace } from './../common/utils/string-helpers';
import { autoinject, computedFrom } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Return, returnEvents } from '../common/models/return';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { FocusOn } from './return-item-page';
import { pricing } from '../common/models/pricing';
import * as KeyCode from 'keycode-js';

@autoinject
export class ReturnCreate {

  private _subscriptions: Subscription[] = [];

  public header: string = 'Return';

  public isEdit: boolean = false;

  public canSave: boolean = true;
  
  public pricings: Lookup<string>[] = [];

  public customers: Lookup<string>[] = [];

  public _return: Return;

  @computedFrom("_return.id")
  public get isReturnProcessed(): boolean {
    return !isNullOrWhiteSpace(this._return && this._return.id || null);
  }

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) { }

  public getInitializedReturn(): Return {
    return <Return>{
      pricing: pricing.retailPrice,
      branch: this._api.auth.userBranchAsLookup,
      returnedBy: this._api.auth.userAsLookup,
      returnedOn: new Date()
    };
  }

  public async activate(_return: Return): Promise<void> {
    try {
      let [
        customersResponse,
        pricingsResponse,
        returnResponse
      ] = await Promise.all([
        this._api.customers.getLookups(),
        this._api.pricings.getLookups(),
        (_return.id)
          ? this._api.returns.get(_return.id)
          : Promise.resolve(this.getInitializedReturn())
      ]);
      this.customers = customersResponse;
      this.pricings = pricingsResponse;
      this.setReturn(returnResponse);
      window.addEventListener('keydown', this.handleKeyInput, false);
    }
    catch (error) {
      this._notification.error(error);
    }
  }

  public deactivate(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  
    window.removeEventListener('keydown', this.handleKeyInput);
  }

  private handleKeyInput = (event: KeyboardEvent) => {
    if (event.ctrlKey && event.altKey) {
      switch (event.keyCode) {
        case KeyCode.KEY_S:  /* Ctrl + Alt + s */
          this.save();
          break;
        case KeyCode.KEY_I: /* Ctrl + Alt + i */
          this.addItem({ focusOn: 'product' });
          break;
      }
    }
    else {
      switch (event.keyCode) {
        case KeyCode.KEY_F2:
          this.addItem({ focusOn: 'product' });
          break;
      }
    }
    return true;
  }

  private setReturn(_return: Return): void {
    this.isEdit = (_return.id) ? true : false;
    this._return = _return;
    this._return.items = this._return.items || [];
  }

  public addItem(param: { focusOn: FocusOn } = null): void {
    this._eventAggregator.publish(returnEvents.item.add, param && param.focusOn || null);
  }

  public async save(): Promise<void> {
    try {
      if (this.isEdit) {
        return;
      }
      let message = 'Do you want to save return?';
      let confirmation = await this._notification.confirm(message).whenClosed();
      if (!confirmation.wasCancelled) {
        let data = await this._api.returns.create(this._return);
        this.setReturn(data);
        await this._notification.success("Return has been created.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    this._router.navigateBack();
  }
}
