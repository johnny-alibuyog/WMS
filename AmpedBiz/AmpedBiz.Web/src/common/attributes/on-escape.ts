import { customAttribute, autoinject } from "aurelia-framework";

@autoinject
@customAttribute('on-escape')
export class OnEscape {
  action: any;
  onEscape: any;
  element: Element;

  constructor(element: Element) {
    this.element = element;
    this.onEscape = (ev: KeyboardEvent) => {
      //Escape keyCode is 27
      if (ev.keyCode !== 27) {
        return;
      }

      this.action();
    };
  }

  attached() {
    this.element.addEventListener("keyup", this.onEscape);
  }

  valueChanged(func) {
    this.action = func;
  }

  detached() {
    this.element.removeEventListener("keyup", this.onEscape);
  }
}