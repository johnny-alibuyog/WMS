import { PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Dashboard";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Dashboard";
    config.map([
      {
        route: ['', 'pending-page'],
        name: 'pending-page',
        moduleId: PLATFORM.moduleName('./pending-page'),
        nav: true,
        title: 'Pending List',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      {
        route: ['new-customer-order'],
        name: 'new-customer-order',
        moduleId: PLATFORM.moduleName('../orders/order-create'),
        nav: true,
        title: 'New Customer Order',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
            ]
          }
        }
      },
      {
        route: ['new-purchase-order'],
        name: 'new-purchase-order',
        moduleId: PLATFORM.moduleName('../purchases/purchase-order-create'),
        nav: true,
        title: 'New Purchase Order',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
            ]
          }
        }
      },
    ]);

    this.router = router;
  }
}

