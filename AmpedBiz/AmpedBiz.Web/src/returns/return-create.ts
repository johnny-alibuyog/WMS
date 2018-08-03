import { autoinject } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Return, returnEvents } from '../common/models/return';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';

@autoinject
export class ReturnCreate {

  private _subscriptions: Subscription[] = [];

  public header: string = 'Return';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public customers: Lookup<string>[] = [];
  public _return: Return;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) { }

  public getInitializedReturn(): Return {
    return <Return>{
      branch: this._api.auth.userBranchAsLookup,
      returnedBy: this._api.auth.userAsLookup,
      returnedOn: new Date()
    };
  }

  public async activate(_return: Return): Promise<void> {
    try {
      let [customersResponse, returnResponse] = await Promise.all([
        this._api.customers.getLookups(),
        (_return.id)
          ? this._api.returns.get(_return.id)
          : Promise.resolve(this.getInitializedReturn())
      ]);
      this.customers = customersResponse;
      this.setReturn(returnResponse);
    }
    catch (error) {
      this._notification.error(error);
    }
  }

  public deactivate(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  private setReturn(_return: Return): void {
    this.isEdit = (_return.id) ? true : false;
    this._return = _return;
    this._return.items = this._return.items || [];
  }

  public addItem(): void {
    this._eventAggregator.publish(returnEvents.item.add);
  }

  public async save(): Promise<void> {
    try {
      if (this.isEdit) {
        return;
      }
      let data = await this._api.returns.create(this._return);
      this.setReturn(data);
      await this._notification.success("Return has been created.");
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    this._router.navigateBack();
  }
}