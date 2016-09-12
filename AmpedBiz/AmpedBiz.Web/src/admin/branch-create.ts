import {inject} from 'aurelia-dependency-injection';
import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Branch} from '../common/models/branch';
import {Address} from '../common/models/address';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {ValidationRules, ValidationController, ValidationControllerFactory} from 'aurelia-validation';
import {BootstrapFormRenderer} from '../common/controls/validations/bootstrap-form-renderer';

@autoinject()
export class BranchCreate {
  private readonly _api: ServiceApi;
  private readonly _notification: NotificationService;
  private readonly _dialogController: DialogController;
  private readonly _validationController: ValidationController;

  public header: string = 'Create Branch';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public branch: Branch;

  constructor(api: ServiceApi, notification: NotificationService, dialogController: DialogController, validationControllerFactory: ValidationControllerFactory) {
    this._api = api;
    this._notification = notification;
    this._dialogController = dialogController;
    this._validationController = validationControllerFactory.createForCurrentScope();
    this._validationController.addRenderer(new BootstrapFormRenderer());

    ValidationRules

      .ensure((x: Branch) => x.name)
        .required()
        .maxLength(150)
    
      .ensure((x: Branch) => x.description)
        .required()
        .maxLength(150)
    
      .ensure((x: Address) => x.street)
        .maxLength(150)

      .ensure((x: Address) => x.barangay)
          .maxLength(150)

      .ensure((x: Address) => x.city)
          .maxLength(150)

      .ensure((x: Address) => x.province)
          .maxLength(150)

      .ensure((x: Address) => x.region)
          .maxLength(150)

      .ensure((x: Address) => x.country)
          .maxLength(150)

      .ensure((x: Address) => x.zipCode)
          .maxLength(150)        

      .on(this.branch);
  }

  public activate(branch: Branch): void {
    if (branch) {
      this.header = "Edit Branch";
      this.isEdit = true;
      this._api.branches.get(branch.id)
        .then(data => this.branch = <Branch>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Branch";
      this.isEdit = false;
      this.branch = <Branch>{};
    }
  }

  public cancel() {
    this._dialogController.cancel({ wasCancelled: true, output: null });
  }

  public save() {
    if (this.isEdit) {

      this._api.branches.update(this.branch)
        .then(data => {
          this._notification.success("Branch  has been saved.")
            .then((data) => this._dialogController.ok({ wasCancelled: true, output: <Branch>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.branches.create(this.branch)
        .then(data => {
          this._notification.success("Branch has been saved.")
            .then((data) => this._dialogController.ok({ wasCancelled: true, output: <Branch>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}