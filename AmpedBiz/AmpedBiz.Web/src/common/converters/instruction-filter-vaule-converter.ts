import {NavigationInstruction} from 'aurelia-router';

export class InstructionFilterValueConverter {
  toView(navigationInstructions: NavigationInstruction[]) {
    return navigationInstructions ? navigationInstructions.filter(i => i.config.title) : [];
  }
}