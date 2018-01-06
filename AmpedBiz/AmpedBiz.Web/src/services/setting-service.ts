import { ServiceBase } from "./service-base";
import { autoinject } from "aurelia-framework";
import { HttpClientFacade } from "./http-client-facade";
import { InvoiceReportSetting } from "../common/models/invoice-report-setting";
import { PageRequest } from "../common/models/paging";
import { UserSetting } from "../common/models/user-setting";


@autoinject
export class SettingService extends ServiceBase<any> {
  constructor(httpClient: HttpClientFacade) {
    super('settings', httpClient);

    let throwNotImplemented = () => { throw new Error('Not Implemented!') };
    this.get = (id: string) => throwNotImplemented();
    this.getList = ()  => throwNotImplemented();
    this.getPage = (page: PageRequest)  => throwNotImplemented();
    this.create = (entity: any) => throwNotImplemented();
    this.update = (entity: any) => throwNotImplemented();
    this.delete = (id: any, entity?: any) => throwNotImplemented();
  }

  public getInvoiceReportSetting(): Promise<InvoiceReportSetting> {
    var url = this._resouce + '/invoice-report';
    return this._httpClient.get(url);
  }

  public updateInvoiceReportSetting(entity: InvoiceReportSetting): Promise<InvoiceReportSetting> {
    var url = this._resouce + '/invoice-report';
    return this._httpClient.post(url, entity);
  }

  public getUserSetting(): Promise<UserSetting> {
    var url = this._resouce + '/user';
    return this._httpClient.get(url);
  }

  public updateUserSetting(entity: UserSetting): Promise<UserSetting> {
    var url = this._resouce + '/user';
    return this._httpClient.post(url, entity);
  }
}