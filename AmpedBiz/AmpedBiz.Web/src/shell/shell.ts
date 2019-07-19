import { role } from '../common/models/role';
import { autoinject, PLATFORM, Origin, relativeToFile, CompositionEngine, CompositionContext } from 'aurelia-framework';
import { defaultModules } from 'common/models/setting';
import { AuthService, AuthSettings } from '../services/auth-service';
import { NavigationInstruction, Next, PipelineStep, Router, RouterConfiguration, RouteConfig, NavModel } from 'aurelia-router';
import { NotificationService } from '../common/controls/notification-service';
import { EventAggregator } from 'aurelia-event-aggregator';

  //https://stackoverflow.com/questions/45325557/aurelia-get-child-routes
  //http://www.jeremyg.net/entry/create-a-menu-with-child-routes-using-aurelia
  //https://github.com/aurelia/router/issues/90
  //https://codesandbox.io/embed/vvo7r15020?fontsize=14

@autoinject()
export class Shell {
  public router: Router;

  constructor(
    private readonly _auth: AuthService,
    private readonly _eventAggregator: EventAggregator,
    private readonly _compositionEngine: CompositionEngine,
  ) { }

  public configureRouter(config: RouterConfiguration, router: Router): void {
    config.title = 'Nicon Sales';
    config.addPipelineStep('authorize', AuthorizeStep);
    config.map([
      {
        route: ['dashboard'],
        name: 'dashboard',
        moduleId: PLATFORM.moduleName('../dashboard/index'),
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
        moduleId: PLATFORM.moduleName('../products/index'),
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
        route: ['point-of-sales'],
        name: 'point-of-sales',
        moduleId: PLATFORM.moduleName('../point-of-sales/index'),
        nav: defaultModules.pointOfSales,
        main: true,
        title: 'POS',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.cashier
            ]
          }
        },
      },
      {
        route: ['purchases'],
        name: 'purchases',
        moduleId: PLATFORM.moduleName('../purchases/index'),
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
        moduleId: PLATFORM.moduleName('../orders/index'),
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
        moduleId: PLATFORM.moduleName('../returns/index'),
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
        moduleId: PLATFORM.moduleName('../routes/index'),
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
        moduleId: PLATFORM.moduleName('../report-center/index'),
        nav: true,
        main: true,
        title: 'Report Center',
        settings: {
          displaySubmenu: true,
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
            ]
          }
        },
      },
      {
        route: ['admin'],
        name: 'admin',
        moduleId: PLATFORM.moduleName('../admin/index'),
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
        moduleId: PLATFORM.moduleName('../settings/index'),
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
        route: ['sandbox'],
        name: 'sandbox',
        moduleId: PLATFORM.moduleName('../sandbox/index'),
        nav: true,
        main: true,
        title: 'Sandbox',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
            ]
          }
        },
      },
      {
        route: ['user-profile'],
        name: 'user-profile',
        moduleId: PLATFORM.moduleName('../users/profiles/index'),
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
        redirect: this._auth.matchRoles([role.cashier])
          ? 'point-of-sales' : 'dashboard'
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
