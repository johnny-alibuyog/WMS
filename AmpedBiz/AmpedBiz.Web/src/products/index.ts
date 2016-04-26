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
        moduleId: '.././admin/product-list',
        nav: true,
        title: 'Product List'
      },
      {
        route: ['inventory-level'],
        name: 'inventory-level',
        moduleId: './inventory-level',
        nav: true,
        title: 'Inventory Level'
      },
      {
        route: ['product-order-history'],
        name: 'product-order-history',
        moduleId: './product-order-history',
        nav: true,
        title: 'Order History'
      },
      {
        route: ['product-purchase-history'],
        name: 'product-purchase-history',
        moduleId: './product-purchase-history',
        nav: true,
        title: 'Purchase History'
      },
    ]);
    
    this.router = router;
  }
}

