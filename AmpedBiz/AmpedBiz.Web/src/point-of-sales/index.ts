import { role } from './../common/models/role';
import { Router, RouterConfiguration } from 'aurelia-router';
import { AuthSettings } from '../services/auth-service';
export class Index {
  public heading: string = "Point Of Sales";
  public router: Router;

  //https://github.com/bpampuch/pdfmake/issues/1194

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Purchases";
    config.map([
      {
        route: ['', 'point-of-sale-create'],
        name: 'point-of-sale-create',
        moduleId: './point-of-sale-create',
        nav: true,
        title: 'Point Of Sale',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
              role.manager,
              role.salesclerk
            ]
          }
        }
      },
    ]);

    this.router = router;
  }
}

