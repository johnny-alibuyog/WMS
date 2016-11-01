import {Router, RouterConfiguration} from 'aurelia-router'
import {appConfig} from '../app-config';

export class Index {
  heading: string = "Report Center";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Report Center";
    config.map([
      /*
      {
        route: ['', 'price-list-page'],
        name: 'price-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Price List',
        settings: {
          reportSource: appConfig.reportMapping.baseUrl + appConfig.reportMapping.priceList
        }
      },
      {
        route: ['uom-list-page'],
        name: 'uom-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Unit of Measurements',
        settings: {
          reportSource: appConfig.reportMapping.baseUrl + appConfig.reportMapping.uom
        },
      },
      {
        route: ['customer-list-page'],
        name: 'customer-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Customers',
        settings: {
          reportSource: appConfig.reportMapping.baseUrl + appConfig.reportMapping.customerList
        },
      },
      {
        route: ['supplier-list-page'],
        name: 'supplier-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Suppliers',
        settings: {
          reportSource: appConfig.reportMapping.baseUrl + appConfig.reportMapping.supplierList
        },
      },
      {
        route: ['order-list-page'],
        name: 'order-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Orders',
        settings: {
          reportSource: appConfig.reportMapping.baseUrl + appConfig.reportMapping.orderList
        },
      },
      */
      {
        route: ['', 'customer-report-page'],
        name: 'customer-report-page',
        moduleId: './customer-report-page',
        nav: true,
        title: 'Customer Reports',
      },
      {
        route: ['supplier-report-page'],
        name: 'supplier-report-page',
        moduleId: './supplier-report-page',
        nav: true,
        title: 'Supplier Reports',
      },
      {
        route: ['order-report-page'],
        name: 'order-report-page',
        moduleId: './order-report-page',
        nav: true,
        title: 'Orders Reports',
      },
      {
        route: ['product-report-page'],
        name: 'product-report-page',
        moduleId: './product-report-page',
        nav: true,
        title: 'Products Reports',
      },
      {
        route: ['unit-of-measure-class-report-page'],
        name: 'unit-of-measure-class-report-page',
        moduleId: './unit-of-measure-class-report-page',
        nav: true,
        title: 'Unit of Measure Reports',
      }
    ]);

    this.router = router;
  }
}

