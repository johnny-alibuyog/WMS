import { inject } from 'aurelia-dependency-injection';
import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';

export interface ReportModel {
  title: string;
  content: any;
}

@autoinject
export class ReportViewer {
  private readonly _controller: DialogController;

  @bindable()
  public report?: ReportModel;

  constructor(controller: DialogController) {
    this._controller = controller;
  }

  public close(): void {
    this._controller.ok();
  }

  public activate(report: ReportModel): void {
    //document.getElementById('pdfV').src = outDoc
    this.report = report;
  }
}