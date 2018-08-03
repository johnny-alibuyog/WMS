import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { UnitOfMeasure } from '../../common/models/unit-of-measure';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class UnitOfMeasureCreate {

  public header: string = 'Create Unit of Measure Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public unitOfMeasure: UnitOfMeasure;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _controller: DialogController,
    private readonly _notification: NotificationService,
  ) { }

  public async activate(unitOfMeasure: UnitOfMeasure): Promise<void> {
    try {
      this.isEdit = (unitOfMeasure) ? true : false;
      this.header = (this.isEdit) ? "Edit Unit of Measure" : "Create Unit of Measure";
      this.unitOfMeasure = (this.isEdit) ? await this._api.unitOfMeasures.get(unitOfMeasure.id) : <UnitOfMeasure>{};
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public cancel(): void {
    this._controller.cancel();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = (this.isEdit)
          ? await this._api.unitOfMeasures.update(this.unitOfMeasure)
          : await this._api.unitOfMeasures.create(this.unitOfMeasure);
        await this._notification.success("Unit of Measure has been saved.").whenClosed();
        await this._controller.ok(<UnitOfMeasure>data);
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}