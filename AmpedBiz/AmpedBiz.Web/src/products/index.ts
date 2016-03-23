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
        route: ['inventory-level'],
        name: 'inventory-level',
        moduleId: './inventory-level',
        nav: true,
        title: 'Inventory Level'
      },
    ]);
    
    this.router = router;
  }
}

