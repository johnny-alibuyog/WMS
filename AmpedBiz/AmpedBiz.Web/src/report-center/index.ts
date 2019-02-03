import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router'
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
      {
        route: ['customer-report-page'],
        name: 'customer-report-page',
        moduleId: PLATFORM.moduleName('./customer-report-page'),
        nav: true,
        title: 'Customer Listings',
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
        route: ['product-listing-report-page'],
        name: 'product-listing-report-page',
        moduleId: PLATFORM.moduleName('./product-listing-report-page'),
        nav: true,
        title: 'Products Listings',
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
        route: ['customer-sales-report-page'],
        name: 'customer-sales-report-page',
        moduleId: PLATFORM.moduleName('./customer-sales-report-page'),
        nav: true,
        title: 'Customer Sales',
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
        route: ['customer-order-delivery-report-page'],
        name: 'customer-order-delivery-report-page',
        moduleId: PLATFORM.moduleName('./customer-order-delivery-report-page'),
        nav: true,
        title: 'Customer Order Delivery',
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
        route: ['customer-payments-report-page'],
        name: 'customer-payments-report-page',
        moduleId: PLATFORM.moduleName('./customer-payments-report-page'),
        nav: true,
        title: 'Customer Payments',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        },
      },
      // {
      //   route: ['customer-order-report-page'],
      //   name: 'customer-order-report-page',
      //   moduleId: PLATFORM.moduleName('./customer-order-report-page',
      //   nav: true,
      //   title: 'Customer Orders',
      //   settings: {
      //     auth: <AuthSettings>{
      //       roles: [
      //         role.admin,
      //         role.manager,
      //       ]
      //     }
      //   },
      // },
      {
        route: ['products-delivered-report-page'],
        name: 'products-delivered-report-page',
        moduleId: PLATFORM.moduleName('./products-delivered-report-page'),
        nav: true,
        title: 'Products Delivered',
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
            ? "product-listing-report-page" 
            : "customer-report-page"
      },
    ]);

    this.router = router;
  }
}

