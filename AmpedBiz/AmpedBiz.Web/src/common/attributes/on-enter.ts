import { customAttribute, autoinject } from 'aurelia-framework';

const ENTER: number = 13;

@autoinject
@customAttribute('on-enter')
export class OnEnter {
  private _element: Element;

  public value: Function;

  private enterPressHandler = (ev: KeyboardEvent) => {
    let keyCode = ev.which || ev.keyCode;
    if (keyCode === ENTER) {
      this.value();
    }
  }

  constructor(element: Element) {
    this._element = element;
  }

  public attached() {
    this._element.addEventListener("keyup", this.enterPressHandler);
  }

  public detached() {
    this._element.removeEventListener("keyup", this.enterPressHandler);
  }
}
