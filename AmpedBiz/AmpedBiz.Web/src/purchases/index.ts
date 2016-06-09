import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Purchases";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Purchases";
    config.map([
      {
        route: ['','active'],
        name: 'active',
        moduleId: './po-active-list',
        nav: true,
        title: 'Active'
      },
      {
        route: ['awaiting-approval'],
        name: 'awaiting-approval',
        moduleId: './po-awaiting-approval-list',
        nav: true,
        title: 'Awaiting Approval'
      },
      {
        route: ['awaiting-completion'],
        name: 'awaiting-completion',
        moduleId: './po-awaiting-completion-list',
        nav: true,
        title: 'Awaiting Completion'
      },
      {
        route: ['completed'],
        name: 'completed',
        moduleId: './po-completed-list',
        nav: true,
        title: 'Completed Purchases'
      }
      
    ]);
    
    this.router = router;
  }
}

