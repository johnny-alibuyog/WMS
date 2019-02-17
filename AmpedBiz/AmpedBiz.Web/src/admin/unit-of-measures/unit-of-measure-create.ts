import { autoinject } from 'aurelia-framework';
import { UnitOfMeasure } from '../../common/models/unit-of-measure';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject
export class UnitOfMeasureCreate {

  public header: string = 'Create Unit of Measure';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public unitOfMeasure: UnitOfMeasure;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
  ) { }

  public async activate(unitOfMeasure: UnitOfMeasure): Promise<void> {
    try {
      this.isEdit = (unitOfMeasure && unitOfMeasure.id) ? true : false;
      this.header = (this.isEdit) ? "Edit Unit of Measure" : "Create Unit of Measure";
      this.unitOfMeasure = (this.isEdit) ? await this._api.unitOfMeasures.get(unitOfMeasure.id) : <UnitOfMeasure>{};
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.unitOfMeasure = (this.isEdit)
          ? await this._api.unitOfMeasures.update(this.unitOfMeasure)
          : await this._api.unitOfMeasures.create(this.unitOfMeasure);
        await this._notification.success("Unit of Measure has been saved.").whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}
