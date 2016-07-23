import {Router, RouterConfiguration} from 'aurelia-router'
import {OrderStatus} from '../common/models/order';

export class Index {
  heading: string = "Orders";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Orders";
    config.map([
      {
        route: ['', 'new-page'],
        name: 'new-page',
        moduleId: './order-page',
        nav: true,
        title: 'New',
        settings: {
          status: OrderStatus.new
        }
      },
      {
        route: ['staged-page'],
        name: 'staged-page',
        moduleId: './order-page',
        nav: true,
        title: 'Staged',
        settings: {
          status: OrderStatus.staged
        }
      },
      {
        route: ['routed-page'],
        name: 'routed-page',
        moduleId: './order-page',
        nav: true,
        title: 'Routed',
        settings: {
          status: OrderStatus.routed
        }
      },
      {
        route: ['invoiced-page'],
        name: 'invoiced-page',
        moduleId: './order-page',
        nav: true,
        title: 'Invoiced',
        settings: {
          status: OrderStatus.invoiced
        }
      },
      {
        route: ['paid-page'],
        name: 'paid-page',
        moduleId: './order-page',
        nav: true,
        title: 'Paid',
        settings: {
          status: OrderStatus.paid
        }
      },
      {
        route: ['completed-page'],
        name: 'completed-page',
        moduleId: './order-page',
        nav: true,
        title: 'Completed',
        settings: {
          status: OrderStatus.completed
        }
      },
      {
        route: ['cancelled-page'],
        name: 'cancelled-page',
        moduleId: './order-page',
        nav: true,
        title: 'Cancelled',
        settings: {
          status: OrderStatus.cancelled
        }
      },
      {
        route: ['subdivide-invoice'],         //this route should be hidden. The BIR will not like this! :)
        name: 'subdivide-invoice',
        moduleId: './subdivide-invoice',
        nav: false,
        title: 'Subdivide Invoice'
      },
      {
        route: ['order-create'],
        name: 'order-create',
        moduleId: './order-create',
        nav: false,
        title: 'Create Order'
      }
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

