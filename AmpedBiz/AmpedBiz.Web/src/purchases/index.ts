import {Router, RouterConfiguration} from 'aurelia-router';
import {PurchaseOrderStatus} from '../common/models/purchase-order';

export class Index {
  heading: string = "Purchases";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Purchases";
    config.map([
      {
        route: ['', 'new-page'],
        name: 'new-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'New',
        settings: {
          status: PurchaseOrderStatus.new
        }
      },
      {
        route: ['submitted-page'],
        name: 'submitted-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Submitted',
        settings: {
          status: PurchaseOrderStatus.submitted
        }
      },
      {
        route: ['approved-page'],
        name: 'approved-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Approved',
        settings: {
          status: PurchaseOrderStatus.approved
        }
      },
      {
        route: ['paid-page'],
        name: 'paid-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Paid',
        settings: {
          status: PurchaseOrderStatus.paid
        }
      },
      {
        route: ['received-page'],
        name: 'received-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Received',
        settings: {
          status: PurchaseOrderStatus.received
        }
      },
      {
        route: ['completed-page'],
        name: 'completed-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Completed',
        settings: {
          status: PurchaseOrderStatus.completed
        }
      }, 
      {
        route: ['cancelled-page'],
        name: 'cancelled-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Cancelled',
        settings: {
          status: PurchaseOrderStatus.cancelled
        }
      }, 
      {
        route: ['purchase-order-create'],
        name: 'purchase-order-create',
        moduleId: './purchase-order-create',
        nav: false,
        title: 'Create Purchase'
      }
    ]);

    this.router = router;
  }
}

