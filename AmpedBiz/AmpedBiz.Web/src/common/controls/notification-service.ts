import { autoinject, noView } from 'aurelia-framework';
import { DialogService, DialogResult } from 'aurelia-dialog';
import { Notification, Alert } from './notification';

@autoinject
export class NotificationService {
  private _dialog: DialogService;

  constructor(dialog: DialogService) {
    this._dialog = dialog;
  }

  info(message: string): Promise<any> {
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

  success(message: string): Promise<any> {
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

  warning(message: string): Promise<any> {
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

  error(message: string): Promise<any> {
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
