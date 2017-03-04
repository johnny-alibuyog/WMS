import { bindable, bindingMode } from 'aurelia-framework';
import { Router } from 'aurelia-router';

export class SideBar {
  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public heading: string;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public router: Router;
}