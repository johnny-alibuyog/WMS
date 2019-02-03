import { PLATFORM } from 'aurelia-pal';
import { role } from './../common/models/role';
import { Router, RouterConfiguration } from 'aurelia-router';
import { AuthSettings } from '../services/auth-service';
export class Index {
  public heading: string = "Point Of Sales";
  public router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Purchases";
    config.map([
      {
        route: ['', 'point-of-sale-create'],
        name: 'point-of-sale-create',
        moduleId: PLATFORM.moduleName('./point-of-sale-create'),
        nav: true,
        title: 'Point Of Sale',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.cashier
            ]
          }
        }
      },
      {
        route: ['point-of-sale-page'],
        name: 'point-of-sale-page',
        moduleId: PLATFORM.moduleName('./point-of-sale-page'),
        nav: true,
        title: 'History',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.cashier
            ]
          }
        }
      },
    ]);

    this.router = router;
  }
}

