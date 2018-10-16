import { AuthStorage } from "../services/auth-service";

declare var pdfMake: any;
declare var vfsfont: any;

declare type Option = {
  pageOrientation: 'portrait' | 'landscape'
}

export abstract class Report<T> {

  protected readonly builder: ReportBuilder<T>;

  protected readonly viewer: ReportViewer;

  protected abstract buildBody(data: T): Promise<Content[] | any[]>;

  public option: Option = {
    pageOrientation: 'portrait'
  };

  constructor() {
    this.viewer = new ReportViewer();
    this.builder = new ReportBuilder<T>();
    this.builder.buildBody = (data: T) => this.buildBody(data);
  }

  public async show(data: T): Promise<void> {
    var document = await this.builder.buildPdf(data, this.option);
    this.viewer.show(document);
  }
}

export class ReportViewer {
  public show(data: any) {

    let win = window.open('', '_blank');

    try {
      win.document.write(`
        <iframe
          src="${data}"
          frameborder="0" 
          style="border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;" 
          allowfullscreen>
        </iframe>`
      );
    }
    catch (ex) {
      win.close();
      throw ex;
    }
  }
}

export class ReportBuilder<T> {

  public buildBody = (data: T): Promise<Content[] | any[]> => Promise.resolve([]);

  protected buildHeader = (): Promise<Content[] | any[]> => {
    let branch = AuthStorage.branch;
    let data = {
      title: branch.description,
      address: branch && branch.address && `${branch.address.barangay || ''}, ${branch.address.city || ''}, ${branch.address.province || ''}`,
      telephoneNumber: branch && branch.contact && branch.contact.landline || "",
      tinNumber: branch && branch.taxpayerIdentificationNumber || ""
    };

    var header = [
      {
        text: data.title,
        style: "headerTitle"
      },
      {
        text: data.address,
        style: "header3"
      },
      {
        text: `TEL NO. ${data.telephoneNumber}`,
        style: "header3"
      },
      {
        text: `TIN ${data.tinNumber}`,
        style: "header3"
      },
      {
        text: " ",
        style: "spacer"
      },
    ];

    return Promise.resolve(header);
  };

  protected buildFooter = (): Promise<Footer> => {
    let footer = (currentPage: number, pageCount: number): Content[] | any[] => ([
      {
        text: `Page ${currentPage} of ${pageCount}`,
        style: "footer"
      }
    ]);

    return Promise.resolve(footer);
  };

  protected buildStyle = (): Promise<any> => {
    let style = {
      title: {
        fontSize: 16,
        bold: true,
        alignment: "center",
        margin: [0, 10, 0, 10]
      },
      headerTitle: {
        fontSize: 28,
        bold: true,
      },
      header: {
        fontSize: 18,
        bold: true,
        margin: [0, 10, 0, 10]
      },
      header3: {
        bold: true,
        fontSize: 10,
        alignment: "left",
      },
      label: {
        fontSize: 10,
        alignment: "right",
      },
      spacer: {
        margin: [0, 0, 0, 2]
      },
      value: {
        fontSize: 10,
        color: "gray",
        alignment: "left",
      },
      itemHeaderLabel: {
        fontSize: 10,
        bold: true
      },
      itemHeaderInfo: {
        fontSize: 12,
        bold: true,
        italics: true
      },
      tablePlain: {
        alignment: "right",
        margin: [0, 0, 0, 0]
      },
      tableHeader: {
        bold: true,
        fontSize: 10,
        color: "black"
      },
      tableData: {
        fontSize: 10,
        color: "gray"
      },
      footer: {
        color: "gray",
        fontSize: 10,
        alignment: "right",
        margin: [40, 0]
      },
    };

    return Promise.resolve(style);
  }

  protected async buildDocument(data: T, option: Option): Promise<DocumentDefinition> {
    let parts = await Promise.all([
      this.buildHeader(),
      this.buildBody(data),
      this.buildFooter(),
      this.buildStyle()
    ]);

    return <DocumentDefinition>{
      pageOrientation: option.pageOrientation,
      content: [
        ...parts[0],  // header
        ...parts[1],  // body
      ],
      footer: parts[2],
      styles: parts[3]
    };
  }

  public async buildPdf(data: T, option: Option): Promise<any> {
    return new Promise<any>(async (resolve, reject) => {
      let document = await this.buildDocument(data, option);
      pdfMake.createPdf(document).getDataUrl(dataUrl => resolve(dataUrl));
    });
  }
}

type Footer = (currentPage: number, pageCount: number) => Content[] | any[];

export interface DocumentDefinition {
  info?: Info;
  header?: any;
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

export interface Content { /* extends Dictionary<string> */ }

export interface TextContent extends Content {
  text?: string | any[];
  style?: string;
}

export interface ImageContent extends Content { /* */ }

export interface TableContent extends Content { /* */ }

export interface ColumnContent extends Content {
  text?: string | any[];
  style?: string;
}
