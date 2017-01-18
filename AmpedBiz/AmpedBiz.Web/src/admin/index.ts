import {Router, RouterConfiguration} from 'aurelia-router'
import {AuthSettings} from '../services/auth-service';
import {role} from '../common/models/role';

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['', 'branch-page'],
        name: 'branch-page',
        moduleId: './branch-page',
        nav: true,
        title: 'Branches',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.encoder,
              role.manager,
              role.sales,
              role.superAdmin,
              role.warehouse
            ]
          }
        },
      },
      {
        route: ['payment-type-page'],
        name: 'payment-type-page',
        moduleId: './payment-type-page',
        nav: true,
        title: 'Payment Types',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.encoder,
              role.manager,
              role.sales,
              role.superAdmin,
              role.warehouse
            ]
          }
        },
      },
      {
        route: ['user-page'],
        name: 'user-page',
        moduleId: './user-page',
        nav: true,
        title: 'Users',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.encoder,
              role.manager,
              role.sales,
              role.superAdmin,
              role.warehouse
            ]
          }
        },
      },
    ]);
    
    this.router = router;
  }
}

