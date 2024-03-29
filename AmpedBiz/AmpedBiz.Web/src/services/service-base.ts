import { HttpClientFacade } from './http-client-facade';
import { PageRequest, PagerResponse } from '../common/models/paging';

export abstract class ServiceBase<TEntity> {
  protected _resouce: string;
  protected _httpClient: HttpClientFacade;

  protected constructor(resource: string, httpClient: HttpClientFacade) {
    this._resouce = resource;
    this._httpClient = httpClient;
  }

  public get(id: string): Promise<TEntity> {
    var url = this._resouce + '/' + id;
    return this._httpClient.get(url)
      .then(data => <TEntity>data);
  }

  public getList(): Promise<TEntity[]> {
    var url = this._resouce;
    return this._httpClient.get(url)
      .then(data => <TEntity[]>data);
  }

  public getPage(page: PageRequest): Promise<PagerResponse<any>> {
    var url = this._resouce + '/page';
    return this._httpClient.post(url, page);
  }

  // TODO: Refactor paging.ts
  // public getPage<T>(page: PageRequest): Promise<PagerResponse<T>>{
  //   var url = this._resouce + '/page';
  //   return this._httpClient.post(url, page)  
  // }

  public create(entity: TEntity): Promise<TEntity> {
    var url = this._resouce;
    return this._httpClient.post(url, entity);
  }

  public update(entity: TEntity): Promise<TEntity> {
    var url = this._resouce;
    return this._httpClient.put(url, entity);
  }

  public delete(id: any, entity?: TEntity): Promise<TEntity> {
    var url = this._resouce + "/" + id;
    return this._httpClient.delete(url, entity);
  }
}