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
        .withBaseUrl(appConfig.serviceApiBase)
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

  send(param: SendParameters) {
    this.httpClient
      .fetch('/' + param.url, {
        method: param.method || "GET",
        body: param.data ? json(param.data) : null
      })
      .then(response => response.json())
      .then(data => {
        if (param.callback && param.callback.success) {
          param.callback.success(data);
        }
      })
      .catch(error => {
        if (param.callback && param.callback.error) {
          param.callback.error(error);
        }
      });
  }
}

export interface SendParameters {
  data?: any;
  method?: string;
  url: string;
  callback: Callback;
}

export interface Callback {
  success: (data: any) => void;
  error: (error: any) => void;
}