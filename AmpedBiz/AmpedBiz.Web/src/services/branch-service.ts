import {autoinject} from 'aurelia-framework';
import {Branch} from './common/models/branch'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class BranchService {
  private resouce: string = 'branches';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getBranch(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getBranches(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createBranch(branch: Branch, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: branch, callback: callback}); 
  }

  updateBranch(branch: Branch, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: branch, callback: callback}); 
  }
}