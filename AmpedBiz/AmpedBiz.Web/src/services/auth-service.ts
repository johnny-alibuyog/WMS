import { Aurelia, autoinject } from 'aurelia-framework';
import { Role } from '../common/models/role';
import { Branch } from "../common/models/branch";
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Tenant } from "../common/models/tenant";
import { User } from '../common/models/user';
import Enumerable from 'linq';

export interface AuthSettings {
  authorize?: boolean;
  roles?: Role | Role[];
}

export abstract class AuthStorage {
  private static readonly AUTH_TOKEN: string = 'token:auth-user';

  public static set user(user: User) {
    localStorage[this.AUTH_TOKEN] = JSON.stringify(user);
  }

  public static get user(): User {
    return <User>JSON.parse(localStorage[this.AUTH_TOKEN] || '{}');
  }

  public static get userId(): string {
    return this.user && this.user.id || '';
  }

  public static get branch(): Branch {
    return this.user && this.user.branch || null;
  }

  public static get branchId(): string {
    return this.branch && this.branch.id || '';
  }

  public static get tenant(): Tenant {
    return this.branch && this.branch.tenant || null;
  }

  public static get tenantId(): string {
    return this.tenant && this.tenant.id || '';
  }
}

@autoinject
export class AuthService {
  private readonly _resouce: string = 'users';

  constructor(
    private readonly _app: Aurelia,
    private readonly _httpClient: HttpClientFacade,
    private readonly _notification: NotificationService
  ) { }

  public get user(): User {
    return AuthStorage.user;
  };

  public set user(user: User) {
    AuthStorage.user = user;
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

  public matchRoles(params: Role[]): boolean {
    if (!this.isAuthenticated()) {
      return false;
    }

    if (params == null || params == undefined) {
      throw new Error("Parameter roles should not be null.");
    }

    if (params.length !== this.user.roles.length) {
      return false;
    }

    for (let index = 0; index < params.length; index++) {
      const element = params[index];
      const exist = Enumerable
        .from(this.user.roles)
        .any(x => x.id === element.id);

      if (!exist) {
        return false;
      }
    }

    return true;
  }

  public async login(user: User): Promise<void> {
    try {
      var url = this._resouce + '/login';
      this.user = await this._httpClient.send({ url: url, method: 'POST', data: user });
      this._app.setRoot('shell/shell')
    }
    catch (error) {
      this._notification.warning(error)
    }
  }

  public async override(user: User): Promise<User> {
    try {
      var url = this._resouce + '/login';
      return await this._httpClient.send({ url: url, method: 'POST', data: user });
    }
    catch (error) {
      throw Error('Wrong credentials or no override rights.');
    }
  }

  public logout(): Promise<void> {
    this.user = null;
    this._app.setRoot('users/login');
    return Promise.resolve();
  }
}
