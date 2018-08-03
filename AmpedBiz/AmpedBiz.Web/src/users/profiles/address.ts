import { autoinject } from 'aurelia-framework';
import { AuthService } from "../../services/auth-service";
import { NotificationService } from '../../common/controls/notification-service';
import { ServiceApi } from '../../services/service-api';
import { UserAddress } from '../../common/models/user';

@autoinject
export class Address {

  public header: string = 'Address';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: UserAddress;

  constructor(  
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _notificaton: NotificationService
  ) { }

  public async activate(): Promise<void> {
    try {
      this.isEdit = false;
      this.user  = await this._api.users.getAddress(this._auth.user.id);
    }
    catch (error) {
      this._notificaton.warning(error)
    }
  }

  public toggleEdit(): void {
    this.isEdit = !this.isEdit;
  }

  public async save(): Promise<void> {
    try {
      await this._api.users.updateAddress(this.user);
      await this._notificaton.success("Address has been saved succesfully.").whenClosed();
      this.isEdit = false;
    }
    catch (error) {
      this._notificaton.warning(error);
    }
  }
}