import { autoinject } from 'aurelia-framework';
import { Branch } from '../../common/models/branch';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ValidationController, ValidationControllerFactory } from 'aurelia-validation';
import { BootstrapFormRenderer } from '../../common/controls/validations/bootstrap-form-renderer';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject()
export class BranchCreate {

  public header: string = 'Create Branch';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public name: string;
  public branch: Branch = this.initializeBranch();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
    private readonly _validationController: ValidationController,
    validationControllerFactory: ValidationControllerFactory
  ) {

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

  public async activate(branch: Branch): Promise<void> {
    try {
      this.isEdit = (branch && branch.id || null) ? true : false;
      this.header = (this.isEdit) ? "Edit Branch" : "Create Branch";
      this.branch = (this.isEdit) ? await this._api.branches.get(branch.id) : this.initializeBranch();
    } catch (error) {
      this._notification.warning(error)
    }
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm("Do you want to save?").whenClosed();
      if (result.output === ActionResult.Yes) {
        this.branch = (this.isEdit)
          ? await this._api.branches.update(this.branch)
          : await this._api.branches.create(this.branch);
        await this._notification.success("Branch  has been saved.").whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error)
    }
  }
}
