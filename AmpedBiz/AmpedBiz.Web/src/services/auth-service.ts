import {autoinject, Aurelia} from 'aurelia-framework';
import {User} from '../common/models/user'
import {Lookup} from '../common/custom_types/lookup';
import {HttpClientFacade} from './http-client-facade';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class AuthService {
  private _resouce: string = 'users';
  private _app: Aurelia;
  private _httpClient: HttpClientFacade;
  private _notification: NotificationService;

  private readonly AUTH_TOKEN: string = "token:auth-user";

  public get user(): User {
    return <User>JSON.parse(localStorage[this.AUTH_TOKEN] || "{}");
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
    if (!this.isAuthenticated){
      return  null;
    }

    return <Lookup<string>>{
      id: this.user.id,
      name: this.userFullname
    };
  }

  constructor(app: Aurelia, httpClient: HttpClientFacade, notification: NotificationService) {
    this._app = app;
    this._httpClient = httpClient;
    this._notification = notification;
  }

  public isAuthenticated(): boolean {
    return this.user != null && this.user.id != null;
  }

  public login(user: User): Promise<any> {
    var url = this._resouce + '/login';
    return this._httpClient.send({ url: url, method: 'POST', data: user })
      .then(data => {
        this.user = <User>data;
        this._app.setRoot('app')
      })
      .catch(error => {
        this._notification.warning(error)
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