import { AuthService } from '../services/auth-service';
import { DialogController } from 'aurelia-dialog';
import { NotificationService } from '../common/controls/notification-service';
import { User } from '../common/models/user'
import { autoinject } from 'aurelia-framework';
import { isNullOrWhiteSpace } from '../common/utils/string-helpers';

@autoinject
export class Override {
  public header: string = 'Override';
  public user: User;

  constructor(  
    private readonly _auth: AuthService,
    private readonly _controller: DialogController,
    private readonly _notification: NotificationService,
  ) { }

  public async override(): Promise<void> {
    try {
      if (isNullOrWhiteSpace(this.user.username) || isNullOrWhiteSpace(this.user.password)) {
        this._notification.warning('Please enter a username and password.')
        return;
      }      
      let result = await this._auth.override(this.user);
      this._controller.ok({ output: <User>result });
    } 
    catch (error) {
      this._notification.warning(error);
    }
  }

  cancel() {
    this._controller.cancel();
  }

  activate() {
    this.user = {
      username: '',
      password: ''
    };
  }
}
