import { autoinject, Aurelia } from 'aurelia-framework';
import { User } from '../common/models/user';
import { Role, role } from '../common/models/role';
import { Lookup } from '../common/custom_types/lookup';
import { HttpClientFacade } from './http-client-facade';
import { NotificationService } from '../common/controls/notification-service';


export interface AuthSettings {
  authorize?: boolean;
  roles?: Role | Role[];
}

@autoinject
export class AuthService {
  private _resouce: string = 'users';
  private _app: Aurelia;
  private _httpClient: HttpClientFacade;
  private _notification: NotificationService;

  private readonly AUTH_TOKEN: string = 'token:auth-user';

  public get user(): User {
    return <User>JSON.parse(localStorage[this.AUTH_TOKEN] || '{}');
  };

  public set user(user: User) {
    localStorage[this.AUTH_TOKEN] = JSON.stringify(user);
  };

  public get userFullname(): string {
    var userFullname = '';
    if (this.user && this.user.person) {
      userFullname = this.user.person.firstName + ' ' + this.user.person.lastName;
    }

    return userFullname;
  }

  public get userAsLookup(): Lookup<string> {
    if (!this.isAuthenticated) {
      return null;
    }

    if (!this.user) {
      return null;
    }

    return <Lookup<string>>{
      id: this.user.id,
      name: this.userFullname
    };
  }

  public get userBranchAsLookup(): Lookup<string> {
    if (!this.isAuthenticated) {
      return null;
    }

    if (!this.user || !this.user.branch) {
      return null;
    }

    return <Lookup<string>>{
      id: this.user.branch.id,
      name: this.user.branch.name
    };
  }

  constructor(app: Aurelia, httpClient: HttpClientFacade, notification: NotificationService) {
    this._app = app;
    this._httpClient = httpClient;
    this._notification = notification;
  }

  public isAuthenticated(): boolean {
    if (this.user == null) {
      return false;
    }

    if (this.user.id == null) {
      return false;
    }

    return true;
  }

  public isAuthorized(params: Role | Role[]): boolean {
    if (!this.isAuthenticated()) {
      return false;
    }

    if (params == null || params == undefined) {
      return true;
    }

    if (!this.user.roles) {
      return false;
    }

    var roles = (params instanceof Array)
      ? <Role[]>params : [<Role>params];

    for (var i = 0; i < roles.length; i++) {
      if (this.user.roles.find(x => x.id == roles[i].id)) {
        return true;
      }
    }

    return false;
  }

  public login(user: User): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      var url = this._resouce + '/login';
      return this._httpClient.send({ url: url, method: 'POST', data: user })
        .then(data => {
          this._app.setRoot('shell/shell')
          this.user = <User>data;
        })
        .catch(error => {
          this._notification.warning(error)
        });
    });
  }

  public override(user: User): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      var url = this._resouce + '/login';
      this._httpClient.send({ url: url, method: 'POST', data: user })
        .then(data => resolve(<User>data))
        .catch(error => reject('Wrong credentials or no override rights.'));
    });
  }

  public logout(): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.user = null;
      this._app.setRoot('users/login');
      resolve();
    });
  }
}