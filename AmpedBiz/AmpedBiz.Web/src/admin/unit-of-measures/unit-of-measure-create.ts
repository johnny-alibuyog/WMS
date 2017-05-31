import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { UnitOfMeasure } from '../../common/models/unit-of-measure';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';

@autoinject
export class UnitOfMeasureCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Unit of Measure Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public unitOfMeasure: UnitOfMeasure;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(unitOfMeasure: UnitOfMeasure) {
    if (unitOfMeasure) {
      this.header = "Edit Unit of Measure";
      this.isEdit = true;
      this._api.unitOfMeasures.get(unitOfMeasure.id)
        .then(data => this.unitOfMeasure = <UnitOfMeasure>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Unit of Measure";
      this.isEdit = false;
      this.unitOfMeasure = <UnitOfMeasure>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {

    if (this.isEdit) {

      this._api.unitOfMeasures.update(this.unitOfMeasure)
        .then(data => {
          this._notification.success("Unit of Measure has been saved.")
            .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <UnitOfMeasure>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.unitOfMeasures.create(this.unitOfMeasure)
        .then(data => {
          this._notification.success("Unit of Measure has been saved.")
            .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <UnitOfMeasure>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}