import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['', 'payment-type-list'],
        name: 'payment-type-list',
        moduleId: './payment-type-list',
        nav: true,
        title: 'Payment Type List'
      },
    ]);
    
    this.router = router;
  }
}

