import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Purchases";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Purchases";
    config.map([
      {
        route: ['', 'awaiting-approval'],
        name: 'awaiting-approval',
        moduleId: './awaiting-approval',
        nav: true,
        title: 'Awaiting Approval'
      },
      {
        route: ['completed-purchases'],
        name: 'completed-purchases',
        moduleId: './completed-purchases',
        nav: true,
        title: 'Completed Purchases'
      },
      {
        route: ['receive-inventory'],
        name: 'receive-inventory',
        moduleId: './receive-inventory',
        nav: true,
        title: 'Receive Inventory'
      },
      
    ]);
    
    this.router = router;
  }
}

