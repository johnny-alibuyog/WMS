import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Products";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Products";
    config.map([
      {
        route: ['', 'product-list'],
        name: 'product-list',
        moduleId: './product-list',
        nav: true,
        title: 'Product List'
      },
      {
        route: 'inventory-level',
        name: 'inventory-level',
        moduleId: './inventory-level',
        nav: true,
        title: 'Inventory Level'
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

