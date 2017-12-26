import { AuthService, AuthSettings } from '../services/auth-service';
import { NavigationInstruction, Next, PipelineStep, Redirect, Router, RouterConfiguration } from 'aurelia-router';

import { NotificationService } from '../common/controls/notification-service';
import { autoinject } from 'aurelia-framework';
import { role } from '../common/models/role';

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
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
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
            roles: role.all()
          }
        },
      },
      {
        route: ['purchases'],
        name: 'purchases',
        moduleId: '../purchases/index',
        nav: true,
        main: true,
        title: 'Purchases',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['orders'],
        name: 'orders',
        moduleId: '../orders/index',
        nav: true,
        main: true,
        title: 'Customer Orders',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns'],
        name: 'returns',
        moduleId: '../returns/index',
        nav: true,
        main: true,
        title: 'Returns',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['routes'],
        name: 'routes',
        moduleId: '../routes/index',
        nav: true,
        main: true,
        title: 'Routes',
        settings: {
          auth: <AuthSettings>{
            roles: role.unknown
          }
        },
      },
      {
        route: ['report-center'],
        name: 'report-center',
        moduleId: '../report-center/index',
        nav: true,
        main: true,
        title: 'Report Center',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
            ]
          }
        },
      },
      {
        route: ['admin'],
        name: 'admin',
        moduleId: '../admin/index',
        nav: true,
        main: true,
        title: 'Admin',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        },
      },
      {
        route: ['settings'],
        name: 'settings',
        moduleId: '../settings/index',
        nav: false,
        main: true,
        title: 'Settings',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        },
      },
      {
        route: ['user-profile'],
        name: 'user-profile',
        moduleId: '../users/profiles/index',
        nav: false,
        main: true,
        title: 'User Profile',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
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
          this._notification.warning(`You are not authorized to view ${instructionConfig.title}`);
          return next.cancel();
        }
      }
    }

    return next();
  }
}