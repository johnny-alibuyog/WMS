import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';
import { PLATFORM } from 'aurelia-framework';

export class Index {
  heading: string = "Returns";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Returns";
    config.map([
      {
        route: ['', 'return-page'],
        name: 'return-page',
        moduleId: PLATFORM.moduleName('./return-page'),
        nav: true,
        title: 'Returns',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns-by-customer-page'],
        name: 'returns-by-customer-page',
        moduleId: PLATFORM.moduleName('./returns-by-customer-page'),
        nav: true,
        title: 'Returns By Customer',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns-by-customer-details-page'],
        name: 'returns-by-customer-details-page',
        moduleId: PLATFORM.moduleName('./returns-by-customer-details-page'),
        nav: false,
        title: 'Returns By Customer Details',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns-by-product-page'],
        name: 'returns-by-product-page',
        moduleId: PLATFORM.moduleName('./returns-by-product-page'),
        nav: true,
        title: 'Returns By Product',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns-by-product-details-page'],
        name: 'returns-by-product-details-page',
        moduleId: PLATFORM.moduleName('./returns-by-product-details-page'),
        nav: false,
        title: 'Returns By Product Details',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns-by-reason-page'],
        name: 'returns-by-reason-page',
        moduleId: PLATFORM.moduleName('./returns-by-reason-page'),
        nav: true,
        title: 'Returns By Reason',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['returns-by-reason-details-page'],
        name: 'returns-by-reason-details-page',
        moduleId: PLATFORM.moduleName('./returns-by-reason-details-page'),
        nav: false,
        title: 'Returns By Reason Details',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['return-create'],
        name: 'return-create',
        moduleId: PLATFORM.moduleName('./return-create'),
        nav: false,
        title: 'Create Return',
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
    ]);
    this.router = router;
  }
}

