import { PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Products";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Products";
    config.map([
      {
        route: ['', 'product-page'],
        name: 'product-page',
        moduleId: PLATFORM.moduleName('./product-page'),
        nav: true,
        title: 'Products',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['inventory-level-page'],
        name: 'inventory-level-page',
        moduleId: PLATFORM.moduleName('./inventory-level-page'),
        nav: true,
        title: 'Inventory Level',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['discontinued-page'],
        name: 'discontinued-page',
        moduleId: PLATFORM.moduleName('./discontinued-page'),
        nav: true,
        title: 'Discontinued Products',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['product-create'],
        name: 'product-create',
        moduleId: PLATFORM.moduleName('./product-create'),
        nav: false,
        title: 'Product',
        settings: {
          auth: role.all()
        },
      },
    ]);

    this.router = router;
  }
}

