import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Lookup } from '../common/custom_types/lookup';
import { Return, returnEvents } from '../common/models/return';
import { ProductInventory } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
export class ReturnCreate {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;

  private _subscriptions: Subscription[] = [];

  public header: string = 'Return';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public customers: Lookup<string>[] = [];
  public _return: Return;

  constructor(api: ServiceApi, router: Router, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._router = router;
    this._notification = notification;
    this._eventAggregator = eventAggregator;
  }

  public getInitializedReturn(): Return {
    return <Return>{
      branch: this._api.auth.userBranchAsLookup,
      returnedBy: this._api.auth.userAsLookup,
      returnedOn: new Date()
    };
  }

  public activate(_return: Return): void {
    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Return>] = [
        this._api.customers.getLookups(),
        _return.id
          ? this._api.returns.get(_return.id)
          : Promise.resolve(this.getInitializedReturn())
      ];

    Promise.all(requests)
      .then(responses => {
        this.customers = responses[0];
        this.setReturn(responses[1]);
      })
      .catch(error => {
        this._notification.error(error);
      });
  }

  public deactivate(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  private resetAndNoify(_return: Return, notificationMessage: string): void {
    this.setReturn(_return);

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }

  private setReturn(_return: Return): void {
    if (_return.id) {
      this.isEdit = true;
    }
    else {
      this.isEdit = false;
    }

    this._return = _return;
    this._return.items = this._return.items || [];
  }

  public addItem(): void {
    this._eventAggregator.publish(returnEvents.item.add);
  }

  public save(): void {
    if (this.isEdit) {
      return;
    }

    this._api.returns.create(this._return)
      .then(data => {
        this._notification.success("Return has been created.");
        this._router.navigateBack();
      })
      .catch(error => this._notification.warning(error));
  }

  public back(): void {
    this._router.navigateBack();
  }
}