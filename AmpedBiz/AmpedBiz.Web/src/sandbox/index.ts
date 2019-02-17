import { PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router'
import { AuthSettings } from '../services/auth-service';
import { role } from '../common/models/role';

export class Index {
  heading: string = "Admin";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Admin";
    config.map([
      {
        route: ['content-projection'],
        name: 'content-projection',
        moduleId: PLATFORM.moduleName('./content-projections/content-projection'),
        nav: true,
        title: 'Content Projection',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
            ]
          }
        },
      },
      {
        route: ['basic-composition'],
        name: 'basic-composition',
        moduleId: PLATFORM.moduleName('./compositions/basic-composition'),
        nav: true,
        title: 'Basic Composition',
        settings: {
          auth: <AuthSettings>{
            roles: [
              role.admin,
            ]
          }
        },
      },
      {
        route: [""],
        redirect: "content-projection"
      }
    ]);

    this.router = router;
  }
}

