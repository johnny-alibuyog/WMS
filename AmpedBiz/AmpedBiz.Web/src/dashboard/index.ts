import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Dashboard";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Dashboard";
    config.map([
      {
        route: ['', 'pending-page'],
        name: 'pending-page',
        moduleId: './pending-page',
        nav: true,
        title: 'Pending List'
      },
      {
        route: ['new-customer-order'],
        name: 'new-customer-order',
        moduleId: '../orders/order-create',
        nav: true,
        title: 'New Customer Order'
      },
      {
        route: ['new-purchase-order'],
        name: 'new-purchase-order',
        moduleId: '../purchases/purchase-order-create',
        nav: true,
        title: 'New Purchase Order'
      }
    ]);
    
    this.router = router;
  }
}

