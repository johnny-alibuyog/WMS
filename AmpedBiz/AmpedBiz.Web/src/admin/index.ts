import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['', 'branch-page'],
        name: 'branch-page',
        moduleId: './branch-page',
        nav: true,
        title: 'Branches'
      },
      {
        route: ['customer-page'],
        name: 'customer-page',
        moduleId: './customer-page',
        nav: true,
        title: 'Customers'
      },
      {
        route: ['payment-type-page'],
        name: 'payment-type-page',
        moduleId: './payment-type-page',
        nav: true,
        title: 'Payment Types'
      },
      {
        route: ['product-category-page'],
        name: 'product-category-page',
        moduleId: './product-category-page',
        nav: true,
        title: 'Product Categories'
      },
      {
        route: ['product-page'],
        name: 'product-page',
        moduleId: './product-page',
        nav: true,
        title: 'Products'
      },
      {
        route: ['supplier-page'],
        name: 'supplier-page',
        moduleId: './supplier-page',
        nav: true,
        title: 'Suppliers'
      },
      {
        route: ['user-page'],
        name: 'user-page',
        moduleId: './user-page',
        nav: true,
        title: 'Users'
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
    ]);
    
    this.router = router;
  }
}

