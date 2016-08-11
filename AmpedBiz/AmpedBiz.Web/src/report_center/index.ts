import {Router, RouterConfiguration} from 'aurelia-router'
import {ReportName} from '../common/models/reports';

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
          status: ReportName.PriceList
        }
      },
      {
        route: ['uom-list-page'],
        name: 'uom-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Unit of Measurements',
        settings: {
          status: ReportName.UOMList
        },
      },
      {
        route: ['customer-list-page'],
        name: 'customer-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Customers',
        settings: {
          status: ReportName.CustomerList
        },
      },
      {
        route: ['supplier-list-page'],
        name: 'supplier-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Suppliers',
        settings: {
          status: ReportName.SupplierList
        },
      },
      {
        route: ['order-list-page'],
        name: 'order-list-page',
        moduleId: './report-page',
        nav: true,
        title: 'Orders',
        settings: {
          status: ReportName.OrderList
        },
      }
    ]);
    
    this.router = router;
  }
}

