import { Router, RouterConfiguration } from 'aurelia-router'

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
        route: ['returns-by-customer-page'],
        name: 'returns-by-customer-page',
        moduleId: './returns-by-customer-page',
        nav: true,
        title: 'Returns By Customer',
      },
      {
        route: ['returns-by-product-page'],
        name: 'returns-by-product-page',
        moduleId: './returns-by-product-page',
        nav: true,
        title: 'Returns By Product',
      },
      {
        route: ['returns-by-reason-page'],
        name: 'returns-by-reason-page',
        moduleId: './returns-by-reason-page',
        nav: true,
        title: 'Returns By Reason',
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

