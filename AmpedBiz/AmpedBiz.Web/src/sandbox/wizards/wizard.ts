import Enumerable from "linq";
import { computedFrom, bindingMode } from "aurelia-binding";
import { bindable } from "aurelia-framework";

export class Wizard {
  //https://github.com/PWKad/aurelia-samples/tree/master/src/routes/modal

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public activeStep: Step;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public steps: Step[];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public submitAction: () => void | Promise<void>;

  @computedFrom('activeStep', 'steps')
  public get canMoveNext() {
    const lastStep = Enumerable.from(this.steps).last();
    return this.activeStep != lastStep;
  }

  @computedFrom('activeStep', 'steps')
  public get canMovePrevious() {
    const firstStep = Enumerable.from(this.steps).first();
    return this.activeStep != firstStep;
  }

  public next(): void {
    if (!this.activeStep.isValid === false) {
      return;
    }

    if (!this.canMoveNext) {
      return;
    }
  }

  public previous(): void {

  }
}

export interface Step {
  isValid?: boolean;

}
