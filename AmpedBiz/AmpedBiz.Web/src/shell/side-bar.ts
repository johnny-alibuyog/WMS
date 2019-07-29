import { bindable, bindingMode } from 'aurelia-framework';
import { Router, NavModel } from 'aurelia-router';
import * as Enumerable from 'linq';

export class SideBar {
  @bindable()
  public displayMenuTitle: boolean;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public heading: string;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public router: Router;
}
