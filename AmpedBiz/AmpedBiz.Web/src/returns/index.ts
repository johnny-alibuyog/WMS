import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Returns";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Returns";
    config.map([
      {
        route: ['', 'return-page'],
        name: 'return-page',
        moduleId: './return-page',
        nav: true,
        title: 'Returns',
      },
      {
        route: ['return-create'],
        name: 'return-create',
        moduleId: './return-create',
        nav: false,
        title: 'Create Return'
      },
    ]);
    this.router = router;
  }
}

