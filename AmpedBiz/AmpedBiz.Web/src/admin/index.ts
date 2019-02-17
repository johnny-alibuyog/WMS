import { PLATFORM } from 'aurelia-framework';
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
        moduleId: PLATFORM.moduleName('./branches/branch-page'),
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
        route: ['branch-create'],
        name: 'branch-create',
        moduleId: PLATFORM.moduleName('./branches/branch-create'),
        nav: false,
        title: 'Create Branch',
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
        moduleId: PLATFORM.moduleName('./customers/customer-page'),
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
        moduleId: PLATFORM.moduleName('./customers/customer-create'),
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
        moduleId: PLATFORM.moduleName('./payment-types/payment-type-page'),
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
        route: ['payment-type-create'],
        name: 'payment-type-create',
        moduleId: PLATFORM.moduleName('./payment-types/payment-type-create'),
        nav: false,
        title: 'Create Payment Type',
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
        route: ['product-category-page'],
        name: 'product-category-page',
        moduleId: PLATFORM.moduleName('./product-categories/product-category-page'),
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
        route: ['product-category-create'],
        name: 'product-category-create',
        moduleId: PLATFORM.moduleName('./product-categories/product-category-create'),
        nav: false,
        title: 'Create Product Category',
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
        route: ['return-reason-page'],
        name: 'return-reason-page',
        moduleId: PLATFORM.moduleName('./return-reasons/return-reason-page'),
        nav: true,
        title: 'Return Reasons',
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
        route: ['return-reason-create'],
        name: 'return-reason-create',
        moduleId: PLATFORM.moduleName('./return-reasons/return-reason-create'),
        nav: false,
        title: 'Create Return Reason',
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
        route: ['supplier-page'],
        name: 'supplier-page',
        moduleId: PLATFORM.moduleName('./suppliers/supplier-page'),
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
      },
      {
        route: ['supplier-create'],
        name: 'supplier-create',
        moduleId: PLATFORM.moduleName('./suppliers/supplier-create'),
        nav: false,
        title: 'Create Supplier',
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
        route: ['unit-of-measure-page'],
        name: 'unit-of-measure-page',
        moduleId: PLATFORM.moduleName('./unit-of-measures/unit-of-measure-page'),
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
        route: ['unit-of-measure-create'],
        name: 'unit-of-measure-create',
        moduleId: PLATFORM.moduleName('./unit-of-measures/unit-of-measure-create'),
        nav: false,
        title: 'Create Unit of Measure',
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
        route: ['user-page'],
        name: 'user-page',
        moduleId: PLATFORM.moduleName('./users/user-page'),
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
        route: ['user-create'],
        name: 'user-create',
        moduleId: PLATFORM.moduleName('./users/user-create'),
        nav: false,
        title: 'Create User',
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
        route: [""],
        redirect: "customer-page"
      }
    ]);

    this.router = router;
  }
}

