import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Routes";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Routes";
    config.map([
      {
        route: ['', 'route-page'],
        name: 'route-page',
        moduleId: './route-page',
        nav: true,
        title: 'Routes',
      },
    ]);
    this.router = router;
  }
}

