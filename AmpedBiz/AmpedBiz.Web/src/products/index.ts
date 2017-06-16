import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthService, AuthSettings } from '../services/auth-service';
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
        moduleId: './product-page',
        nav: true,
        title: 'Products',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        },
      },
      {
        route: ['inventory-level-page'],
        name: 'inventory-level-page',
        moduleId: './inventory-level-page',
        nav: true,
        title: 'Inventory Level',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        },
      },
      {
        route: ['discontinued-page'],
        name: 'discontinued-page',
        moduleId: './discontinued-page',
        nav: true,
        title: 'Discontinued Products',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        },
      },
      {
        route: ['product-create'],
        name: 'product-create',
        moduleId: './product-create',
        nav: false,
        title: 'Product',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        },
      },
    ]);

    this.router = router;
  }
}

