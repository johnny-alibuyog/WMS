import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { AccessKeyed } from 'aurelia-framework';

@autoinject
export class Notification {
  private _controller: DialogController;

  public prompt: Prompt;

  constructor(controller: DialogController) {
    this._controller = controller;
  }

  public activate(prompt: Prompt): void {
    this.prompt = prompt;
  }

  public ok(): void {
    this._controller.ok(this.prompt);
  }

  public cancel(): void {
    this._controller.cancel(this.prompt);
  }

  public confirm(result: ActionResult) {
    switch (result) {
      case ActionResult.Yes:
      case ActionResult.Okay:
      case ActionResult.Close:
        this._controller.ok(result);
        break;
      case ActionResult.No:
      case ActionResult.Cancel:
        this._controller.cancel(result);
        break;
      default:
        this._controller.ok(result);
        break;
    }
  }
}

export interface Prompt {
  type?: PromptType;
  emphasis?: string;
  message?: string;
  actions?: Action[];
}

export interface Action {
  text?: string;
  type?: ActionType;
  result?: ActionResult
}

export type PromptType = 'info' | 'success' | 'warning' | 'danger';

export type ActionType = 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark';

export enum ActionResult { Yes, No, Cancel, Okay, Close }