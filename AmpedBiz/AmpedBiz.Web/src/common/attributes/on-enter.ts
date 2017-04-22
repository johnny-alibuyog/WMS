import { customAttribute, bindable, autoinject } from 'aurelia-framework';
  
const ENTER: number = 13;

@autoinject
@customAttribute('on-enter')
export class OnEnter {
  public value: Function;
  private _element: Element;
  private _keyup: EventListener;

  constructor(element: Element) {
    this._element = element;
    this._keyup = (ev: KeyboardEvent) => {
      if (ev.keyCode !== ENTER) this.value();
    };
  }

  public attached = () => this._element.addEventListener("keyup", this._keyup);

  public detached = () => this._element.removeEventListener("keyup", this._keyup);
}