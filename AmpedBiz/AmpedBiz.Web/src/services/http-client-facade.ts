import {autoinject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {appConfig} from '../app-config';
import 'fetch';

@autoinject
export class HttpClientFacade {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
    this.httpClient.configure(config => {
      config
        .withBaseUrl(appConfig.api.baseUrl)
        .withDefaults({
          credentials: 'same-origin',
          headers: {
            'Accept': 'application/json',
            'X-Requested-With': 'Fetch'
          }
        })
        .withInterceptor({
          request(request) {
            console.log(`Requesting ${request.method} ${request.url}`);
            return request; // you can return a modified Request, or you can short-circuit the request by returning a Response
          },
          response(response) {
            console.log(`Received ${response.status} ${response.url}`);
            return response; // you can return a modified Response
          }
        });
    });
  }

  send(param: SendParameters) : Promise<any> {
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
  
  get(url: string) : Promise<any> {
    return this.send({ url: url, method: "GET" });
  }
  
  post(url: string, data: any) : Promise<any> {
    return this.send({ url: url, method: "POST", data: data });
  }
  
  put(url: string, data: any) : Promise<any> {
    return this.send({ url: url, method: "PUT", data: data });
  }
  
  patch(url: string, data: any) : Promise<any> {
    return this.send({ url: url, method: "PATCH", data: data });
  }
  
  delete(url: string, data: any) : Promise<any> {
    return this.send({ url: url, method: "DELETE", data: data });
  }
}

export interface SendParameters {
  data?: any;
  method?: string;
  url: string;
}