// Reference: https://gist.github.com/jdanyow/d9d8dd9df7be2dd2f59077bad3bfb399
// NOTE: there is an issue (infinite loop) when binding to a property with value NaN, not sure how to fix this yet

import { DOM, autoinject, bindable, bindingMode, customAttribute } from 'aurelia-framework';

@autoinject
@customAttribute('numeric-value', bindingMode.twoWay)
export class NumericValue {

  public value: number;

  private _input: HTMLInputElement;

  constructor(input: Element) {
    this._input = <HTMLInputElement>input;
  }

  private _ensureNumber(value: any): number {
    // todo: use numbro
    let number = parseFloat(value);
    return !isNaN(number) && isFinite(value) ? number : NaN;
  }

  public valueChanged(): void {
    // synchronize the input element with the bound value
    const number = this._ensureNumber(this.value);
    if (this.value < number) {
      this.value = number;
    }

    if (isNaN(number)) {
      this._input.value = '';
    } else {
      this._input.value = number.toString(10);
    }
  }

  public keyup = () => {
    // blank input maps to null value
    if (this._input.value === '') {
      this.value = null;
      return;
    }
    // do we have a number?
    const number = this._ensureNumber(this._input.value);
    if (isNaN(number)) {
      // no! reset the input.
      this.valueChanged();
    } else {
      // yes! update the binding.
      this.value = number;
    }
  }

  public bind(): void {
    this.valueChanged();
    this._input.addEventListener('keyup', this.keyup);
  }

  public unbind(): void {
    this._input.removeEventListener('keyup', this.keyup);
  }
}
