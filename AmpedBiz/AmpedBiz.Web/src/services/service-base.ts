import {autoinject} from 'aurelia-framework';
import {HttpClientFacade} from './http-client-facade';
import {PageRequest} from '.././common/models/paging';

@autoinject
export class ServiceBase<TEntity> {
  protected _resouce: string;
  protected _httpClient: HttpClientFacade;

  constructor(resource: string, httpClient: HttpClientFacade) {
    this._resouce = resource;
    this._httpClient = httpClient;
  }

  get(id: string): Promise<any> {
    var url = this._resouce + '/' + id;
    return this._httpClient.get(url);
  }

  getLists(params: any): Promise<any> {
    var url = this._resouce;
    return this._httpClient.get(url);
  }

  getPages(page: PageRequest): Promise<any> {
    var url = this._resouce + '/pages';
    return this._httpClient.post(url, page);
  }

  create(entity: TEntity): Promise<any> {
    var url = this._resouce;
    return this._httpClient.post(url, entity);
  }

  update(entity: TEntity): Promise<any> {
    var url = this._resouce;
    return this._httpClient.put(url, entity);
  }

  delete(id: any, entity?: TEntity): Promise<any> {
    var url = this._resouce + "/" + id;
    return this._httpClient.delete(url, entity);
  }
}