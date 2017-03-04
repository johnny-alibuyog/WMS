import { customAttribute, autoinject } from "aurelia-framework";

@autoinject
@customAttribute('on-enter')
export class OnEnter {
  action: any;
  onEnter: any;
  element: Element;

  constructor(element: Element) {
    this.element = element;
    this.onEnter = (ev: KeyboardEvent) => {
      //Enter keyCode is 13
      if (ev.keyCode !== 13) {
        return;
      }

      this.action();
    };
  }

  attached() {
    this.element.addEventListener("keyup", this.onEnter);
  }

  valueChanged(func) {
    this.action = func;
  }

  detached() {
    this.element.removeEventListener("keyup", this.onEnter);
  }
}