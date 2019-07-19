import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration, RouteConfig } from 'aurelia-router'
import { AuthSettings, AuthService } from '../services/auth-service';
import { role } from '../common/models/role';

@autoinject()
export class Index {
  private readonly _auth: AuthService;

  public heading: string = "Report Center";
  public router: Router;

  public constructor(auth: AuthService) {
    this._auth = auth;
  }

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Report Center";
    config.map([
      ...customerRoutes,
      ...productRoutes,
      {
        route: ['supplier-report-page'],
        name: 'supplier-report-page',
        moduleId: PLATFORM.moduleName('./supplier-report-page'),
        nav: true,
        title: 'Supplier Listings',
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
        route: ['inventory-movements-report-page'],
        name: 'inventory-movements-report-page',
        moduleId: PLATFORM.moduleName('./inventory-movements-report-page'),
        nav: true,
        title: 'Inventory Movement',
        settings: {
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
        route: ['purchase-order-report-page'],
        name: 'purchase-order-report-page',
        moduleId: PLATFORM.moduleName('./purchase-order-report-page'),
        nav: true,
        title: 'Purchases',
        settings: {
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
        route: ['unit-of-measure-report-page'],
        name: 'unit-of-measure-report-page',
        moduleId: PLATFORM.moduleName('./unit-of-measure-report-page'),
        nav: true,
        title: 'Unit of Measure Listings',
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
        route: ['returns-details-report-page'],
        name: 'returns-details-report-page',
        moduleId: PLATFORM.moduleName('./returns-details-report-page'),
        nav: true,
        title: 'Return Details',
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
        redirect:
          this._auth.isAuthorized(role.salesclerk)
            ? "products/product-listing-report-page"
            : "customers/customer-report-page"
      },
    ]);

    this.router = router;
  }
}

const customerRoutes: RouteConfig[] = [
  {
    route: ['customers/customer-report-page'],
    name: 'customer-report-page',
    moduleId: PLATFORM.moduleName('./customers/customer-report-page'),
    nav: true,
    title: 'Listings',
    settings: {
      group: 'Customer',
      auth: <AuthSettings>{
        roles: [
          role.admin,
          role.manager,
        ]
      }
    },
  },
  {
    route: ['customers/customer-sales-report-page'],
    name: 'customer-sales-report-page',
    moduleId: PLATFORM.moduleName('./customers/customer-sales-report-page'),
    nav: true,
    title: 'Sales',
    settings: {
      group: 'Customer',
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
    route: ['customers/customer-order-delivery-report-page'],
    name: 'customer-order-delivery-report-page',
    moduleId: PLATFORM.moduleName('./customers/customer-order-delivery-report-page'),
    nav: true,
    title: 'Order Delivery',
    settings: {
      group: 'Customer',
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
    route: ['customers/customer-payments-report-page'],
    name: 'customer-payments-report-page',
    moduleId: PLATFORM.moduleName('./customers/customer-payments-report-page'),
    nav: true,
    title: 'Payments',
    settings: {
      group: 'Customer',
      auth: <AuthSettings>{
        roles: [
          role.admin,
          role.manager,
        ]
      }
    },
  },
];

const productRoutes: RouteConfig[] = [
  {
    route: ['products/product-listing-report-page'],
    name: 'product-listing-report-page',
    moduleId: PLATFORM.moduleName('./products/product-listing-report-page'),
    nav: true,
    title: 'Listing',
    settings: {
      group: 'Product',
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
    route: ['products/product-delivery-report-page'],
    name: 'product-delivery-report-page',
    moduleId: PLATFORM.moduleName('./products/product-delivery-report-page'),
    nav: true,
    title: 'Delivery',
    settings: {
      group: 'Product',
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
    route: ['products/product-sales-report-page'],
    name: 'product-sales-report-page',
    moduleId: PLATFORM.moduleName('./products/product-sales-report-page'),
    nav: true,
    title: 'Sales',
    settings: {
      group: 'Product',
      auth: <AuthSettings>{
        roles: [
          role.admin,
          role.manager,
          role.salesclerk,
        ]
      }
    },
  },
];
