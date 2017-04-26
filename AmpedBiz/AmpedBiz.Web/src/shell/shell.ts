import { autoinject } from 'aurelia-framework';
import { NavigationInstruction, Next, PipelineStep, Redirect, Router, RouterConfiguration } from 'aurelia-router';
import { AuthService, AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
export class Shell {
  public router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Nicon Sales';
    config.addPipelineStep('authorize', AuthorizeStep);
    config.map([
      {
        route: ['dashboard'],
        name: 'dashboard',
        moduleId: '../dashboard/index',
        nav: true,
        main: true,
        title: 'Dashboard',
      },
      {
        route: ['products'],
        name: 'products',
        moduleId: '../products/index',
        nav: true,
        main: true,
        title: 'Products',
        settings: {
          auth: <AuthSettings>{
            roles: role.unknownRole
          }
        },
      },
      {
        route: ['purchases'],
        name: 'purchases',
        moduleId: '../purchases/index',
        nav: true,
        main: true,
        title: 'Purchases'
      },
      {
        route: ['orders'],
        name: 'orders',
        moduleId: '../orders/index',
        nav: true,
        main: true,
        title: 'Customer Orders'
      },
      {
        route: ['returns'],
        name: 'returns',
        moduleId: '../returns/index',
        nav: true,
        main: true,
        title: 'Returns'
      },
      {
        route: ['routes'],
        name: 'routes',
        moduleId: '../routes/index',
        nav: true,
        main: true,
        title: 'Routes'
      },
      {
        route: ['report_center'],
        name: 'report_center',
        moduleId: '../report_center/index',
        nav: true,
        main: true,
        title: 'Report Center'
      },
      {
        route: ['admin'],
        name: 'admin',
        moduleId: '../admin/index',
        nav: true,
        main: true,
        title: 'Admin'
      },
      {
        route: [""],
        redirect: "dashboard"
      }
    ]);

    this.router = router;
  }
}

@autoinject
class AuthorizeStep implements PipelineStep {
  private readonly _auth: AuthService;
  private readonly _notification: NotificationService;

  constructor(auth: AuthService, notification: NotificationService) {
    this._auth = auth;
    this._notification = notification;
  }

  public run(navigationInstruction: NavigationInstruction, next: Next): Promise<any> {
    let allInstructions = navigationInstruction.getAllInstructions();
    let currentInstruction = allInstructions[allInstructions.length - 1];
    let instructionConfig = currentInstruction.config;
    let authSettings = <AuthSettings>instructionConfig.settings.auth;

    if (authSettings) {
      if (!this._auth.isAuthenticated()) {
        this._auth.logout();
        return next.cancel();
      }

      if (authSettings.roles) {
        if (!this._auth.isAuthorized(authSettings.roles)) {
          this._notification.warning(`You are not authorized for ${instructionConfig.title}`);
          return next.cancel();
        }
      }
    }

    return next();
  }
}