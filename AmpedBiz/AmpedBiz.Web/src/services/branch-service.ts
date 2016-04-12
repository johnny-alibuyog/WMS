import {autoinject} from 'aurelia-framework';
import {Branch} from './common/models/branch'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class BranchService {
  private resouce: string = 'branches';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getBranch(id: string) : Promise<any> {
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url}); 
  }
  
  getBranches(params: any) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url}); 
  }
  
  createBranch(branch: Branch) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: branch}); 
  }

  updateBranch(branch: Branch) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: branch}); 
  }
}