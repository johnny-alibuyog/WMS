import { Router, RouterConfiguration } from 'aurelia-router'
import { OrderStatus } from '../common/models/order';
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Customer Orders";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Customer Orders";
    config.map([
      {
        route: ['', 'created-page'],
        name: 'created-page',
        moduleId: './order-page',
        nav: true,
        title: 'Created',
        settings: {
          status: OrderStatus.created,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      {
        route: ['invoiced-page'],
        name: 'invoiced-page',
        moduleId: './order-page',
        nav: true,
        title: 'Invoiced',
        settings: {
          status: OrderStatus.invoiced,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      {
        route: ['staged-page'],
        name: 'staged-page',
        moduleId: './order-page',
        nav: true,
        title: 'Staged',
        settings: {
          status: OrderStatus.staged,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      /*
      {
        route: ['routed-page'],
        name: 'routed-page',
        moduleId: './order-page',
        nav: true,
        title: 'Routed',
        settings: {
          status: OrderStatus.routed,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      */
      {
        route: ['shipped-page'],
        name: 'shipped-page',
        moduleId: './order-page',
        nav: true,
        title: 'Shipped',
        settings: {
          status: OrderStatus.shipped,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      /*
      {
        route: ['delivered-page'],
        name: 'delivered-page',
        moduleId: './order-page',
        nav: true,
        title: 'Dilivered',
        settings: {
          status: OrderStatus.dilivered,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      */
      {
        route: ['completed-page'],
        name: 'completed-page',
        moduleId: './order-page',
        nav: true,
        title: 'Completed',
        settings: {
          status: OrderStatus.completed,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      {
        route: ['cancelled-page'],
        name: 'cancelled-page',
        moduleId: './order-page',
        nav: true,
        title: 'Cancelled',
        settings: {
          status: OrderStatus.cancelled,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      {
        route: ['subdivide-invoice'],         //this route should be hidden. The BIR will not like this! :)
        name: 'subdivide-invoice',
        moduleId: './subdivide-invoice',
        nav: false,
        title: 'Subdivide Invoice',
        settings: {
          status: OrderStatus.cancelled,
          auth: <AuthSettings>{
            roles: role.all()
          }
        }
      },
      {
        route: ['order-create'],
        name: 'order-create',
        moduleId: './order-create',
        nav: false,
        title: 'Create Order',
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
            ]
          }
      },
    ]);
    this.router = router;
  }
}

