import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@autoinject
export class Notification {
  private _controller: DialogController;
  
  public alert: Alert;
  
  constructor(controller: DialogController) {
    this._controller = controller;
  }
  
  activate(alert: Alert){
    this.alert = alert;
  }
  
  ok() {
    this._controller.ok(this.alert);
  }
}

export interface Alert {
  type?: string;
  emphasis?: string;
  message?: string;
}