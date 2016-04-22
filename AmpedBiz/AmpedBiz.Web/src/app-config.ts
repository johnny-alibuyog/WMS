// https://auth0.com/blog/2015/08/05/creating-your-first-aurelia-app-from-authentication-to-calling-an-api/
/*
var appConfig = {
  serviceApiUrl: 'http://localhost:49242',
}

export default appConfig;
*/
import {PageConfig} from './common/controls/pager';

export let appConfig = {
  api: {
    baseUrl: 'http://localhost:49242',
  },
  page: <PageConfig>{
    maxSize: 5,
    itemsPerPage: 10,
    boundaryLinks: true,
    directionLinks: true,
    firstText: '<<',
    previousText: '<',
    nextText: '>',
    lastText: '>>',
    rotate: true
  }
}
