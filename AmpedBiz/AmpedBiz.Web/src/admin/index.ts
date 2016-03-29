import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['', 'branch-list'],
        name: 'branch-list',
        moduleId: './branch-list',
        nav: true,
        title: 'Branches'
      },
      {
        route: ['customer-list'],
        name: 'customer-list',
        moduleId: './customer-list',
        nav: true,
        title: 'Customers'
      },
      {
        route: ['payment-type-list'],
        name: 'payment-type-list',
        moduleId: './payment-type-list',
        nav: true,
        title: 'Payment Types'
      },
      {
        route: ['product-list'],
        name: 'product-list',
        moduleId: './product-list',
        nav: true,
        title: 'Products'
      },
      {
        route: ['product-category-list'],
        name: 'product-category-list',
        moduleId: './product-category-list',
        nav: true,
        title: 'Product Categories'
      },
      {
        route: ['role-list'],
        name: 'role-list',
        moduleId: './role-list',
        nav: true,
        title: 'Roles'
      },
      {
        route: ['supplier-list'],
        name: 'supplier-list',
        moduleId: './supplier-list',
        nav: true,
        title: 'Suppliers'
      },
      {
        route: ['users-list'],
        name: 'users-list',
        moduleId: './users-list',
        nav: true,
        title: 'Users'
      },
      {
        route: ['unit-of-measure-list'],
        name: 'unit-of-measure-list',
        moduleId: './unit-of-measure-list',
        nav: true,
        title: 'Unit of Measures'
      },
    ]);
    
    this.router = router;
  }
}

