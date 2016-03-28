import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Orders";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Orders";
    config.map([
      {
        route: ['', 'need-invoicing'],
        name: 'need-invoicing',
        moduleId: './need-invoicing',
        nav: true,
        title: 'Need Invoicing'
      },
      {
        route: ['invoiced-orders'],
        name: 'invoiced-orders',
        moduleId: './invoiced-orders',
        nav: true,
        title: 'Invoiced Orders'
      },
      {
        route: ['incomplete-payments'],
        name: 'incomplete-payments',
        moduleId: './incomplete-payments',
        nav: true,
        title: 'Incomplete Payments'
      },
      {
        route: ['completed-orders'],
        name: 'completed-orders',
        moduleId: './completed-orders',
        nav: true,
        title: 'Completed Orders'
      },
      { 
        //this route should be hidden. The BIR will not like this! :)
        route: ['subdivide-invoice'],
        name: 'subdivide-invoice',
        moduleId: './subdivide-invoice',
        nav: true,
        title: 'Subdivide Invoice'
      },
    ]);
    
    this.router = router;
  }
}

