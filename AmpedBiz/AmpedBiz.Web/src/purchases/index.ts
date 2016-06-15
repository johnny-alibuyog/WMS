import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Purchases";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Purchases";
    config.map([
      {
        route: ['', 'new-page'],
        name: 'new-page',
        moduleId: './new-page',
        nav: true,
        title: 'New'
      },
      {
        route: ['active-page'],
        name: 'active-page',
        moduleId: './active-page',
        nav: true,
        title: 'Active'
      },
      {
        route: ['approved-page'],
        name: 'approved-page',
        moduleId: './approved-page',
        nav: true,
        title: 'Approved'
      },
      {
        route: ['completed-page'],
        name: 'completed-page',
        moduleId: './completed-page',
        nav: true,
        title: 'Completed'
      },
      {
        route: ['purchase-order-page'],
        name: 'purchase-order-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Purchases'
      }
    ]);
    
    this.router = router;
  }
}

