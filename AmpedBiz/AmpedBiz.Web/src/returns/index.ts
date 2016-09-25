import {Router, RouterConfiguration} from 'aurelia-router'
import {OrderStatus} from '../common/models/order';

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
    ]);
    this.router = router;
  }
}

