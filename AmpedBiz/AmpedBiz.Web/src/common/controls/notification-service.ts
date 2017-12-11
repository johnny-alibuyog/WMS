import { autoinject, noView } from 'aurelia-framework';
import { DialogService, DialogOpenPromise, DialogOpenResult, DialogCancelResult } from 'aurelia-dialog';
import { Notification, Prompt, Action, ActionResult } from './notification';

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
      model: <Prompt>{
        type: 'info',
        emphasis: 'Info!',
        message: message,
        actions: [
          <Action>{
            text: 'Close',
            type: 'primary',
            result: ActionResult.Close
          }
        ]
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
      model: <Prompt>{
        type: 'success',
        emphasis: 'Success!',
        message: message,
        actions: [
          <Action>{
            text: 'Close',
            type: 'primary',
            result: ActionResult.Close
          }
        ]
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
      model: <Prompt>{
        type: 'warning',
        emphasis: 'Warning!',
        message: message,
        actions: [
          <Action>{
            text: 'Close',
            type: 'primary',
            result: ActionResult.Close
          }
        ]
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
      model: <Prompt>{
        type: 'danger',
        emphasis: 'Error!',
        message: message,
        actions: [
          <Action>{
            text: 'Close',
            type: 'primary',
            result: ActionResult.Close
          }
        ]
      }
    });
  }

  public confirm(message: string): DialogOpenPromise<DialogOpenResult | DialogCancelResult> {
    //TODO: there is an issue currently with aurelia-dialog, 
    // return implementaion as soon as fix is available
    // https://github.com/aurelia/dialog/issues/180

    //return Promise.resolve(<DialogResult>{ wasCancelled: false, output: null });

    return this._dialog.open({
      viewModel: Notification,
      model: <Prompt>{
        type: 'info',
        emphasis: 'Confirm!',
        message: message,
        actions: [
          <Action>{
            text: 'Yes',
            type: 'primary',
            result: ActionResult.Yes
          },
          <Action>{
            text: 'No',
            type: 'danger',
            result: ActionResult.No
          }
        ]
      }
    });
  }}
