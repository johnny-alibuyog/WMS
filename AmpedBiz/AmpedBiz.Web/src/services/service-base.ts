import {autoinject} from 'aurelia-framework';
import {HttpClientFacade} from './http-client-facade';
import {PageRequest} from '.././common/models/paging';
import {KeyValuePair} from '.././common/custom_types/key-value-pair';

@autoinject
export class ServiceBase<TEntity> {
  protected _resouce: string;
  protected _httpClient: HttpClientFacade;

  constructor(resource: string, httpClient: HttpClientFacade) {
    this._resouce = resource;
    this._httpClient = httpClient;
  }

  get(id: string): Promise<TEntity> {
    var url = this._resouce + '/' + id;
    return this._httpClient.get(url)
      .then(data => <TEntity>data);
  }

  getList(): Promise<TEntity[]> {
    var url = this._resouce;
    return this._httpClient.get(url)
      .then(data => <TEntity[]>data);
  }

  getLookup<TKey, TValue>(): Promise<KeyValuePair<TKey, TValue>[]> {
    var url = this._resouce + '/lookup';
    return this._httpClient.get(url)
      .then(data => {
        return <KeyValuePair<TKey, TValue>[]>data;
      });
  }

  getPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/page';
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