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
        route: ['', 'customer-report-page'],
        name: 'customer-report-page',
        moduleId: './customer-report-page',
        nav: true,
        title: 'Customer Reports',
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
        title: 'Supplier Reports',
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
        route: ['order-report-page'],
        name: 'order-report-page',
        moduleId: './order-report-page',
        nav: true,
        title: 'Orders Reports',
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
        route: ['product-report-page'],
        name: 'product-report-page',
        moduleId: './product-report-page',
        nav: true,
        title: 'Products Reports',
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
        title: 'Unit of Measure Reports',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
            ]
          }
        },
      }
    ]);

    this.router = router;
  }
}
