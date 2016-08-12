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
        route: ['payment-type-page'],
        name: 'payment-type-page',
        moduleId: './payment-type-page',
        nav: true,
        title: 'Payment Types'
      },
      {
        route: ['user-page'],
        name: 'user-page',
        moduleId: './user-page',
        nav: true,
        title: 'Users'
      },
    ]);
    
    this.router = router;
  }
}

