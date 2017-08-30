import { Router, RouterConfiguration } from 'aurelia-router'

import { AuthSettings } from "../../services/auth-service";
import { role } from "../../common/models/role";

export class Index {
  heading: string = "User Profile";
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "User Profile";
    config.map([
      {
        route: ['info'],
        name: 'info',
        moduleId: './info',
        nav: true,
        title: 'Info',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['address'],
        name: 'address',
        moduleId: './address',
        nav: true,
        title: 'Address',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: ['change-password'],
        name: 'change-password',
        moduleId: './change-password',
        nav: true,
        title: 'Change Password',
        settings: {
          auth: <AuthSettings>{
            roles: role.all()
          }
        },
      },
      {
        route: [""],
        redirect: "info"
      }
    ]);
    this.router = router;
  }
}

