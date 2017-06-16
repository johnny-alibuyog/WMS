import { Router, RouterConfiguration } from 'aurelia-router';
import { PurchaseOrderStatus } from '../common/models/purchase-order';
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

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
          status: PurchaseOrderStatus.new,
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        }
      },
      {
        route: ['submitted-page'],
        name: 'submitted-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Submitted',
        settings: {
          status: PurchaseOrderStatus.submitted,
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        }
      },
      {
        route: ['approved-page'],
        name: 'approved-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Approved',
        settings: {
          status: PurchaseOrderStatus.approved,
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        }
      },
      {
        route: ['completed-page'],
        name: 'completed-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Completed',
        settings: {
          status: PurchaseOrderStatus.completed,
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        }
      },
      {
        route: ['cancelled-page'],
        name: 'cancelled-page',
        moduleId: './purchase-order-page',
        nav: true,
        title: 'Cancelled',
        settings: {
          status: PurchaseOrderStatus.cancelled,
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
        }
      },
      {
        route: ['purchase-order-create'],
        name: 'purchase-order-create',
        moduleId: './purchase-order-create',
        nav: false,
        title: 'Create Purchase',
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk,
              role.warehouseman
            ]
          }
      }
    ]);

    this.router = router;
  }
}

