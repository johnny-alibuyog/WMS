import {Router, RouterConfiguration} from 'aurelia-router'

export class Index {
  heading: string = "Orders";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Orders";
    config.map([
      {
        route: ['', 'active'],
        name: 'active',
        moduleId: './order-active',
        nav: true,
        title: 'Active Orders'
      },
      {
        route: ['on-staging'],
        name: 'on-staging',
        moduleId: './order-on-staging',
        nav: true,
        title: 'On Staging'
      },
      {
        route: ['on-route'],
        name: 'on-route',
        moduleId: './order-on-route',
        nav: true,
        title: 'On Route'
      },
      {
        route: ['for-invoicing'],
        name: 'for-invoicing',
        moduleId: './order-for-invoicing',
        nav: true,
        title: 'For Invoicing'
      },
      {
        route: ['invoiced'],
        name: 'invoiced',
        moduleId: './order-invoiced',
        nav: true,
        title: 'Invoiced'
      },
      {
        route: ['incomplete-payment'],
        name: 'incomplete-payment',
        moduleId: './order-incomplete-payment',
        nav: true,
        title: 'Incomplete Payments'
      },
      {
        route: ['completed'],
        name: 'completed',
        moduleId: './order-completed',
        nav: true,
        title: 'Completed Orders'
      },
      { 
        //this route should be hidden. The BIR will not like this! :)
        route: ['subdivide-invoice'],
        name: 'subdivide-invoice',
        moduleId: './subdivide-invoice',
        nav: false,
        title: 'Subdivide Invoice'
      },
    ]);
    /*
    On-Staging
    On-Route
    For Invoicing
    Invoiced
    Incomplete Payments
    Completed Orders
     */
    this.router = router;
  }
}

