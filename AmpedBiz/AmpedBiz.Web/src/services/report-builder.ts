//import {Dictionary} from "../custom_types/dictionary"
import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ReportViewer, ReportModel} from '../common/controls/report-viewer';

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

    pdfMake.createPdf(config.document).getDataUrl(content => {
      this._dialog.open({
        viewModel: ReportViewer,
        model: <ReportModel>{
          title: config.title || 'Report',
          content: content
        }
      })
    });
    
    /*
    try {
      pdfMake.createPdf(config.document)
        .getDataUrl(data => win.location.href = data);
    }
    catch (ex) {
      win.close();
      throw ex;
    }
    */
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