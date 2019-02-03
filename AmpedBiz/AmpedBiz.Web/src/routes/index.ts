import { PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router'

export class Index {
  heading: string = "Routes";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Routes";
    config.map([
      {
        route: ['', 'route-page'],
        name: 'route-page',
        moduleId: PLATFORM.moduleName('./route-page'),
        nav: true,
        title: 'Routes',
      },
    ]);
    this.router = router;
  }
}

