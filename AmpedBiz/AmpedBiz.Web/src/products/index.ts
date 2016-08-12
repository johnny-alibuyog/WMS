import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Products";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Products";
    config.map([
      {
        route: ['', 'product-page'],
        name: 'product-page',
        moduleId: './product-page',
        nav: true,
        title: 'Products'
      },
      {
        route: ['inventory-level-page'],
        name: 'inventory-level-page',
        moduleId: './inventory-level-page',
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
        moduleId: '../purchases/po-history',
        nav: true,
        title: 'Purchase History'
      },
      {
        route: ['discontinued-page'],
        name: 'discontinued-page',
        moduleId: './discontinued-page',
        nav: true,
        title: 'Discontinued Products'
      },
      {
        route: ['product-create'],
        name: 'product-create',
        moduleId: './product-create',
        nav: false,
        title: 'Product'
      },
      {
        route: ['purchase-order-view'],
        name: 'purchase-order-view',
        moduleId: '../purchases/purchase-order-create',
        nav: false,
        title: 'Purchase Order'
      },
      {
        route: ['order-view'],
        name: 'order-view',
        moduleId: '../orders/order-create',
        nav: false,
        title: 'Order'
      },
    ]);
    
    this.router = router;
  }
}

