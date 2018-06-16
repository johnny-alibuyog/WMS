import { Router, RouterConfiguration } from 'aurelia-router'
import { appConfig } from '../app-config';
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Report Center";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Report Center";
    config.map([
      {
        route: ['customer-report-page'],
        name: 'customer-report-page',
        moduleId: './customer-report-page',
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
        moduleId: './product-listing-report-page',
        nav: true,
        title: 'Products Listings',
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
        route: ['supplier-report-page'],
        name: 'supplier-report-page',
        moduleId: './supplier-report-page',
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
        moduleId: './customer-sales-report-page',
        nav: true,
        title: 'Customer Sales',
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
        route: ['customer-order-delivery-report-page'],
        name: 'customer-order-delivery-report-page',
        moduleId: './customer-order-delivery-report-page',
        nav: true,
        title: 'Customer Order Delivery',
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
        route: ['customer-payments-report-page'],
        name: 'customer-payments-report-page',
        moduleId: './customer-payments-report-page',
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
      //   moduleId: './customer-order-report-page',
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
        moduleId: './products-delivered-report-page',
        nav: true,
        title: 'Products Delivered',
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
        moduleId: './inventory-movements-report-page',
        nav: true,
        title: 'Product Inventory Movement',
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
        route: ['unit-of-measure-report-page'],
        name: 'unit-of-measure-report-page',
        moduleId: './unit-of-measure-report-page',
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
        route: [""],
        redirect: "customer-report-page"
      },
    ]);

    this.router = router;
  }
}

