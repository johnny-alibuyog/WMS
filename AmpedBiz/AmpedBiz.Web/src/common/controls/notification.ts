import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';

@autoinject
export class Notification {
  private _controller: DialogController;

  public alert: Alert;

  constructor(controller: DialogController) {
    this._controller = controller;
  }

  public activate(alert: Alert): void {
    this.alert = alert;
  }

  public ok(): void {
    this._controller.ok(this.alert);
  }

  public cancel(): void {
    this._controller.cancel(this.alert);
  }
}

export interface Alert {
  type?: string;
  emphasis?: string;
  message?: string;
}