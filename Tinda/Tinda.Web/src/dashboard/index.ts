import {Router, RouterConfiguration} from 'aurelia-router'

export class Dashboard {
  heading: string = "Dashboard";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Dashboard";
    config.map([
      {
        route: ['', 'pending-list'],
        name: 'pending-list',
        moduleId: './pending-list',
        nav: true,
        title: 'Pending List'
      },
      {
        route: 'new-customer-order',
        name: 'new-customer-order',
        moduleId: './new-customer-order',
        nav: true,
        title: 'New Customer Order'
      },
      {
        route: 'new-purchase-order',
        name: 'new-purchase-order',
        moduleId: './new-purchase-order',
        nav: true,
        title: 'New Purchase Order'
      }
    ]);
    
    this.router = router;
  }
}

