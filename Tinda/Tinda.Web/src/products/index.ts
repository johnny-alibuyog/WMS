import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Products";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Dashboard";
    config.map([
      {
        route: ['', 'inventory-levels'],
        name: 'inventory-levels',
        moduleId: './inventory-levels',
        nav: true,
        title: 'Inventory Levels'
      },
      /*
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
      */
    ]);
    
    this.router = router;
  }
}

