import {autoinject, Aurelia} from 'aurelia-framework';
import {User} from './common/models/user'
import {HttpClientFacade} from './http-client-facade';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class AuthService {
  private _resouce: string = 'users';
  private _app: Aurelia;
  private _httpClient: HttpClientFacade;
  private _notification: NotificationService;

  public get user(): User {
    return <User>JSON.parse(localStorage["token:auth-user"]);
  };

  public set user(user: User) {
    localStorage["token:auth-user"] = JSON.stringify(user);
  };

  constructor(app: Aurelia, httpClient: HttpClientFacade, notification: NotificationService) {
    this._app = app;
    this._httpClient = httpClient;
    this._notification = notification;
  }

  isAuthenticated(): boolean {
    return this.user != "null";
  }

  login(user: User): Promise<any> {
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

  logout(): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.user = null;
      this._app.setRoot('users/login');
      resolve();
    });
  }
}