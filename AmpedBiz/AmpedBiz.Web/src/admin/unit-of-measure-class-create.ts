import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {UnitOfMeasure} from './common/models/unit-of-measure';
import {UnitOfMeasureClass} from './common/models/unit-of-measure-class';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class UnitOfMeasureClassCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Unit of Measure Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public unitOfMeasureClass: UnitOfMeasureClass;
  public selectedUnit: UnitOfMeasure;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(unitOfMeasureClass: UnitOfMeasureClass) {
    if (unitOfMeasureClass) {
      this.header = "Edit Unit of Measure Class";
      this.isEdit = true;
      this._api.unitOfMeasureClasses.get(unitOfMeasureClass.id)
        .then(data => this.unitOfMeasureClass = <UnitOfMeasureClass>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Unit of Measure Class";
      this.isEdit = false;
      this.unitOfMeasureClass = <UnitOfMeasureClass>{
        units: []
      };
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {

    if (this.isEdit) {

      this._api.unitOfMeasureClasses.update(this.unitOfMeasureClass)
        .then(data => {
          this._notification.success("Unit of Measure Class has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <UnitOfMeasureClass>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.unitOfMeasureClasses.create(this.unitOfMeasureClass)
        .then(data => {
          this._notification.success("Unit of Measure Class has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <UnitOfMeasureClass>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }

  createItem(): void {
    var unit = <UnitOfMeasure>{};
    this.unitOfMeasureClass.units.push(unit);
    this.selectedUnit = unit;
  }

  editItem(unit: UnitOfMeasure): void {
    this.selectedUnit = unit;
  }

  deleteItem(unit: UnitOfMeasure): void {
    var index = this.unitOfMeasureClass.units.indexOf(unit);
    if (index > -1) {
      this.unitOfMeasureClass.units.splice(index, 1);
    }
  }
}