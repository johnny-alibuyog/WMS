import {Router, RouterConfiguration} from 'aurelia-router'
import {appConfig} from '../app-config';

export class Index {
  heading: string = "Report Center";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Report Center";
    config.map([
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
      {
        route: ['order-report-page'],
        name: 'order-report-page',
        moduleId: '../orders/order-report-page',
        nav: true,
        title: 'Orders Reports',
      }
    ]);

    this.router = router;
  }
}

