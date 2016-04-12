import {Router, RouterConfiguration} from 'aurelia-router'

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Nicon Sales';
    config.map([
      {
        route: ['dashboard'],
        name: 'dashboard',
        moduleId: './dashboard/index',
        nav: true,
        main: true,
        title: 'Dashboard'
      },
      {
        route: ['products'],
        name: 'products',
        moduleId: './products/index',
        nav: true,
        main: true,
        title: 'Products'
      },
      {
        route: ['orders'],
        name: 'orders',
        moduleId: './orders/index',
        nav: true,
        main: true,
        title: 'Orders'
      },
      {
        route: ['purchases'],
        name: 'purchases',
        moduleId: './Purchases/index',
        nav: true,
        main: true,
        title: 'Purchases'
      },
      {
        route: ['report_center'],
        name: 'report_center',
        moduleId: './report_center/index',
        nav: true,
        main: true,
        title: 'Report Center'
      },
      {
        route: ['admin'],
        name: 'admin',
        moduleId: './admin/index',
        nav: true,
        main: true,
        title: 'Admin'
      },
      {
        route: [""],
        redirect: "dashboard"
      }
    ]);
    
    this.router = router;
  }
}
