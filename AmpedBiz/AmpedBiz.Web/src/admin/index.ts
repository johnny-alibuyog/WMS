import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['', 'product-type-list'],
        name: 'product-type-list',
        moduleId: './product-type-list',
        nav: true,
        title: 'Product Type List'
      },
    ]);
    
    this.router = router;
  }
}

