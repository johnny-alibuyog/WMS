import {autoinject} from 'aurelia-framework';
import {AuthService} from './services/auth-service';
import {Router, RouterConfiguration} from 'aurelia-router'

@autoinject
export class App {
  public auth: AuthService;
  public router: Router;

  constructor(auth: AuthService) {
    this.auth = auth;
  }

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Nicon Sales';
    config.map([
      {
        route: ['dashboard'],
        name: 'dashboard',
        moduleId: './dashboard/index',
        nav: true,
        main: true,
        title: 'Dashboard'
      },
      {
        route: ['products'],
        name: 'products',
        moduleId: './products/index',
        nav: true,
        main: true,
        title: 'Products'
      },
      {
        route: ['purchases'],
        name: 'purchases',
        moduleId: './purchases/index',
        nav: true,
        main: true,
        title: 'Purchases'
      },
      {
        route: ['orders'],
        name: 'orders',
        moduleId: './orders/index',
        nav: true,
        main: true,
        title: 'Orders'
      },
      {
        route: ['returns'],
        name: 'returns',
        moduleId: './returns/index',
        nav: true,
        main: true,
        title: 'Returns'
      },
      {
        route: ['routes'],
        name: 'routes',
        moduleId: './routes/index',
        nav: true,
        main: true,
        title: 'Routes'
      },
      {
        route: ['report_center'],
        name: 'report_center',
        moduleId: './report_center/index',
        nav: true,
        main: true,
        title: 'Report Center'
      },
      {
        route: ['admin'],
        name: 'admin',
        moduleId: './admin/index',
        nav: true,
        main: true,
        title: 'Admin'
      },
      {
        route: [""],
        redirect: "dashboard"
      }
    ]);

    this.router = router;
  }
}