//import {Dictionary} from "../custom_types/dictionary"

declare var pdfMake: any;
declare var vfsfont: any;

// this is a wrapper for pdfMake
export class ReportBuilder {
  public build(definition: DocumentDefinition) {
    let pdf = pdfMake;
    pdfMake.createPdf(definition).open();
  }
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