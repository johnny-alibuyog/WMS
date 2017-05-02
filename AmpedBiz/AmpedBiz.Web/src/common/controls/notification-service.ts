import { autoinject, noView } from 'aurelia-framework';
import { DialogService, DialogOpenPromise, DialogOpenResult, DialogCancelResult } from 'aurelia-dialog';
import { Notification, Alert } from './notification';

@autoinject
export class NotificationService {
  private _dialog: DialogService;

  constructor(dialog: DialogService) {
    this._dialog = dialog;
  }

  public info(message: string): DialogOpenPromise<DialogOpenResult | DialogCancelResult> {
    //TODO: there is an issue currently with aurelia-dialog, 
    // return implementaion as soon as fix is available
    // https://github.com/aurelia/dialog/issues/180

    //return Promise.resolve(<DialogResult>{ wasCancelled: false, output: null });

    return this._dialog.open({
      viewModel: Notification,
      model: <Alert>{
        type: 'info',
        emphasis: 'Info!',
        message: message
      }
    });
  }

  public success(message: string): DialogOpenPromise<DialogOpenResult | DialogCancelResult> {
    //TODO: there is an issue currently with aurelia-dialog, 
    // return implementaion as soon as fix is available
    // https://github.com/aurelia/dialog/issues/180

    //return Promise.resolve(<DialogResult>{ wasCancelled: false, output: null });

    return this._dialog.open({
      viewModel: Notification,
      model: <Alert>{
        type: 'success',
        emphasis: 'Success!',
        message: message
      }
    });
  }

  public warning(message: string): DialogOpenPromise<DialogOpenResult | DialogCancelResult> {
    //TODO: there is an issue currently with aurelia-dialog, 
    // return implementaion as soon as fix is available
    // https://github.com/aurelia/dialog/issues/180

    //return Promise.resolve(<DialogResult>{ wasCancelled: false, output: null });

    return this._dialog.open({
      viewModel: Notification,
      model: <Alert>{
        type: 'warning',
        emphasis: 'Warning!',
        message: message
      }
    });
  }

  public error(message: string): DialogOpenPromise<DialogOpenResult | DialogCancelResult> {
    //TODO: there is an issue currently with aurelia-dialog, 
    // return implementaion as soon as fix is available
    // https://github.com/aurelia/dialog/issues/180

    //return Promise.resolve(<DialogResult>{ wasCancelled: false, output: null });

    return this._dialog.open({
      viewModel: Notification,
      model: <Alert>{
        type: 'danger',
        emphasis: 'Error!',
        message: message
      }
    });
  }
}
