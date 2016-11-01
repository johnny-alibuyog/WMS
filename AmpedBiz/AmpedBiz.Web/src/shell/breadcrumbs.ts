import {autoinject, bindable, bindingMode} from 'aurelia-framework';
import {NavigationInstruction} from 'aurelia-router';
import {Router} from 'aurelia-router';

@autoinject()
export class Breadcrumbs {

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public router: Router;

  constructor(router: Router) {
    while (router.parent) {
      router = router.parent;
    }

    this.router = router;
  }

  public navigate(navigationInstruction: NavigationInstruction) {
    this.router.navigateToRoute(navigationInstruction.config.name, navigationInstruction.params);
  }
}