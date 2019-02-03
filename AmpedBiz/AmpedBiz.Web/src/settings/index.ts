import { PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  public heading: string = "Settings";
  public router: Router;

  public configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Settings";
    config.map([
      {
        route: ['', 'update-invoice-report-setting'],
        name: 'update-invoice-report-setting',
        moduleId: PLATFORM.moduleName('./update-invoice-report-setting'),
        nav: true,
        title: 'Invoice Report',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager
            ]
          }
        },
      },
      {
        route: ['update-user-setting'],
        name: 'update-user-setting',
        moduleId: PLATFORM.moduleName('./update-user-setting'),
        nav: true,
        title: 'User',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager
            ]
          }
        },
      },
    ]);
    this.router = router;
  }
}
