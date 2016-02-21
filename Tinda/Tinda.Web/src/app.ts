import {Router, RouterConfiguration} from 'aurelia-router'

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = '2wheels Adventures Inc';
    config.map([
      {
        route: ['', 'dashboard'],
        name: 'dashboard',
        moduleId: './dashboard/index',
        nav: true,
        main: true,
        title: 'Dashboard'
      },
      {
        route: 'products',
        name: 'products',
        moduleId: './products/index',
        nav: true,
        main: true,
        title: 'Products'
      },
      {
        route: 'users',
        name: 'users',
        moduleId: './users',
        nav: true,
        main: true,
        title: 'Users'
      },
      {
        route: 'child-router',
        name: 'child-router',
        moduleId: './child-router',
        nav: true,
        main: true,
        title: 'Child Router'
      }
    ]);
    
    this.router = router;
  }
}
