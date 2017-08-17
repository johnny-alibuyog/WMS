//import {Dictionary} from "../custom_types/dictionary"
import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { ReportViewer, ReportModel } from '../common/controls/report-viewer';

declare var pdfMake: any;
declare var vfsfont: any;

// this is a wrapper for pdfMake
@autoinject
export class ReportBuilder {
  private readonly _dialog: DialogService;

  constructor(dialog: DialogService) {
    this._dialog = dialog;
  }

  /*
  public buildDataUrl(definition: DocumentDefinition): Promise<string> {
    let pdf = pdfMake;
    return pdfMake.createPdf(definition).getDataUrl();
  }
  */

  public build(config: ReportBuilderConfig) {
    //pdfMake.createPdf(definition).open();

    /*
    pdfMake.createPdf(config.document).getDataUrl(content => {
      this._dialog.open({
        viewModel: ReportViewer,
        model: <ReportModel>{
          title: config.title || 'Report',
          content: content
        }
      })
    });
    */

    let win = window.open('', '_blank');

    try {
      pdfMake.createPdf(config.document).getDataUrl(data => {
        //document.location.href = data;
        win.document.write(`
            <iframe
              src="${data}"
              frameborder="0" 
              style="border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;" 
              allowfullscreen>
            </iframe>`
        );
      });
    }
    catch (ex) {
      win.close();
      throw ex;
    }
  }
}

export interface ReportBuilderConfig {
  title?: string;
  document: DocumentDefinition;
}

export interface Report<T> {
  show(data: T): void;
}

export interface DocumentDefinition {
  info?: Info;
  content?: Content[] | any[];
  footer?: (currentPage: number, pageCount: number) => Content[] | any[];
  images?: any;
  styles?: any;
  defaultStyle?: any;
}

export interface Info {
  title?: string;
  author?: string;
  subject?: string;
  keywords?: string;
}

export interface Content { // extends Dictionary<string> {
}

export interface TextContent extends Content {
  text?: string | any[];
  style?: string;
}

export interface ImageContent extends Content {

}

export interface TableContent extends Content {

}

export interface ColumnContent extends Content {
  text?: string | any[];
  style?: string;
}