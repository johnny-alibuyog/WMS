import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['branch-page'],
        name: 'branch-page',
        moduleId: './branches/branch-page',
        nav: true,
        title: 'Branches',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
            ]
          }
        },
      },
      {
        route: ['customer-page'],
        name: 'customer-page',
        moduleId: './customers/customer-page',
        nav: true,
        title: 'Customers',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        }
      },
      {
        route: ['customer-create'],
        name: 'customer-create',
        moduleId: './customers/customer-create',
        nav: false,
        title: 'Create Customer',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        }
      },
      {
        route: ['payment-type-page'],
        name: 'payment-type-page',
        moduleId: './payment-types/payment-type-page',
        nav: true,
        title: 'Payment Types',
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
        route: ['product-category-page'],
        name: 'product-category-page',
        moduleId: './product-categories/product-category-page',
        nav: true,
        title: 'Product Categories',
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
        route: ['supplier-page'],
        name: 'supplier-page',
        moduleId: './suppliers/supplier-page',
        nav: true,
        title: 'Suppliers',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        }
      },      {
        route: ['unit-of-measure-page'],
        name: 'unit-of-measure-page',
        moduleId: './unit-of-measures/unit-of-measure-page',
        nav: true,
        title: 'Unit of Measures',
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
        route: ['user-page'],
        name: 'user-page',
        moduleId: './users/user-page',
        nav: true,
        title: 'Users',
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
        route: [""],
        redirect: "customer-page"
      }
    ]);

    this.router = router;
  }
}

