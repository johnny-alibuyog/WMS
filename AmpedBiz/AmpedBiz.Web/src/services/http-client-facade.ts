//import 'fetch';

import { HttpClient, json } from 'aurelia-fetch-client';

import { AuthStorage } from "./auth-service";
import { appConfig } from '../app-config';
import { autoinject } from 'aurelia-framework';

@autoinject
export class HttpClientFacade {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
    this.httpClient.configure(config => config
      .withBaseUrl(appConfig.api.baseUrl)
      .withDefaults({
        credentials: 'same-origin',
        headers: {
          'Accept': 'application/json',
          'X-Requested-With': 'Fetch',
        }
      })
      .withInterceptor({
        request(request) {
          request.headers.append('UserId', AuthStorage.userId);     // TODO: change this to jwt soon
          request.headers.append('BranchId', AuthStorage.branchId); // TODO: change this to jwt soon
          request.headers.append('TenantId',  AuthStorage.tenantId); // TODO: change this to jwt soon

          return request; // you can return a modified Request, or you can short-circuit the request by returning a Response
        },
        response(response) {
          return response; // you can return a modified Response
        }
      })
    );
  }

  public send(param: SendParameters): Promise<any> {
    return this.httpClient
      .fetch('/' + param.url, {
        method: param.method || "GET",
        body: param.data ? json(param.data) : null
      })
      .then(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.json();
        }
        else {
          throw new Error(response.statusText);
        }
      });
  }

  public get(url: string): Promise<any> {
    return this.send({ url: url, method: "GET" });
  }

  public post(url: string, data: any): Promise<any> {
    return this.send({ url: url, method: "POST", data: data });
  }

  public put(url: string, data: any): Promise<any> {
    return this.send({ url: url, method: "PUT", data: data });
  }

  public patch(url: string, data: any): Promise<any> {
    return this.send({ url: url, method: "PATCH", data: data });
  }

  delete(url: string, data: any): Promise<any> {
    return this.send({ url: url, method: "DELETE", data: data });
  }
}

export interface SendParameters {
  data?: any;
  method?: string;
  url: string;
}
