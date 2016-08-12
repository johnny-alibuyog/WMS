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
        route: ['discontinued-page'],
        name: 'discontinued-page',
        moduleId: './discontinued-page',
        nav: true,
        title: 'Discontinued Products'
      },
      {
        route: ['product-category-page'],
        name: 'product-category-page',
        moduleId: './product-category-page',
        nav: true,
        title: 'Categories'
      },
      {
        route: ['supplier-page'],
        name: 'supplier-page',
        moduleId: './supplier-page',
        nav: true,
        title: 'Suppliers'
      },
      {
        route: ['unit-of-measure-page'],
        name: 'unit-of-measure-page',
        moduleId: './unit-of-measure-page',
        nav: true,
        title: 'Unit of Measures'
      },
      {
        route: ['unit-of-measure-class-page'],
        name: 'unit-of-measure-class-page',
        moduleId: './unit-of-measure-class-page',
        nav: true,
        title: 'Unit of Measures Class'
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

