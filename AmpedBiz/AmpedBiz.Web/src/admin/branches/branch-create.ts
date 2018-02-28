import { inject } from 'aurelia-dependency-injection';
import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Branch } from '../../common/models/branch';
import { Address } from '../../common/models/address';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ValidationRules, ValidationController, ValidationControllerFactory } from 'aurelia-validation';
import { BootstrapFormRenderer } from '../../common/controls/validations/bootstrap-form-renderer';
import { ActionResult } from '../../common/controls/notification';

@autoinject()
export class BranchCreate {
  private readonly _api: ServiceApi;
  private readonly _notification: NotificationService;
  private readonly _dialogController: DialogController;
  private readonly _validationController: ValidationController;

  public header: string = 'Create Branch';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public name: string;
  public branch: Branch = this.initializeBranch();

  private initializeBranch(): Branch {
    return {
      id: '',
      name: '',
      description: '',
      taxpayerIdentificationNumber: '',
      contact: {},
      address: {}
    };
  }

  constructor(api: ServiceApi, notification: NotificationService, dialogController: DialogController, validationControllerFactory: ValidationControllerFactory) {
    this._api = api;
    this._notification = notification;
    this._dialogController = dialogController;
    this._validationController = validationControllerFactory.createForCurrentScope();
    this._validationController.addRenderer(new BootstrapFormRenderer());

    // Validation support for subproperties (#283) is not yet supported. for future enhancement
    // https://github.com/aurelia/validation/issues/283
    //temporarily remove validation rules to enable creation of branch 
    /*
    ValidationRules
      .ensure('branch.name')
        .required()
        .maxLength(150)
      .ensure('branch.description')
        .required()
        .maxLength(150)
      .ensure('branch.address.street')
        .maxLength(150)
      .ensure('branch.address.barangay')
        .maxLength(150)
      .ensure('branch.address.city')
        .maxLength(150)
      .ensure('branch.address.province')
        .maxLength(150)
      .ensure('branch.address.region')
        .maxLength(150)
      .ensure('branch.address.country')
        .maxLength(150)
      .ensure('branch.address.zipCode')
        .maxLength(150)        
      .on(this.branch);*/
  }

  public async activate(branch: Branch): Promise<void> {
    try {
      if (branch) {
        this.header = "Edit Branch";
        this.isEdit = true;
        this.branch = await this._api.branches.get(branch.id)
      }
      else {
        this.header = "Create Branch";
        this.isEdit = false;
        this.branch = this.initializeBranch();
      }
    } catch (error) {
      this._notification.warning(error)
    }
  }

  public cancel(): void {
    this._dialogController.cancel({ wasCancelled: true, output: null });
  }

  public async save(): Promise<void> {
    try {
      this._notification.confirm('Do you want to save?').whenClosed(async result => {
        if (result.output === ActionResult.Yes) {

          if (this.isEdit) {
            let data = await this._api.branches.update(this.branch);
            this._notification.success("Branch  has been saved.")
              .whenClosed((data) => this._dialogController.ok({ wasCancelled: true, output: <Branch>data }));
          }
          else {
            let data = await this._api.branches.create(this.branch)
            this._notification.success("Branch has been saved.")
              .whenClosed((data) => this._dialogController.ok({ wasCancelled: true, output: <Branch>data }));
          }
        }
      });
    } catch (error) {
      this._notification.warning(error)
    }
  }
}