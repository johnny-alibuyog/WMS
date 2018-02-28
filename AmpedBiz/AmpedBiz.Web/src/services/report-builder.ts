import { autoinject } from 'aurelia-framework';

declare var pdfMake: any;
declare var vfsfont: any;

// this is a wrapper for pdfMake
@autoinject
export class ReportBuilder {
  public build(config: ReportBuilderConfig) {

    let win = window.open('', '_blank');

    try {
      pdfMake.createPdf(config.document).getDataUrl(data => {
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
  header?: Content[] | any[];
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

export interface Content { // extends Dictionary<string>
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