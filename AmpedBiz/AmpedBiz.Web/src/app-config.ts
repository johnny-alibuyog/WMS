// https://auth0.com/blog/2015/08/05/creating-your-first-aurelia-app-from-authentication-to-calling-an-api/
import { PageConfig } from './common/controls/pager';

var apiHosts = {
  'nicont130server': 'http://nicont130server/ampbiz/api/',
  'server-pc': 'http://server-pc/ampbiz/api/',
  'localhost': 'http://localhost:49561',
  'ampbiz': 'http://ampbiz/api',
  'ampbiz.local': 'http://ampbiz-api.local',
  'ampbiz.gear.host': 'http://ampbiz-api.gear.host',
};

export let appConfig = {
  api: {
    baseUrl: apiHosts[window.location.hostname],
  },
  page: <PageConfig>{
    maxSize: 5,
    itemsPerPage: 10,
    boundaryLinks: true,
    directionLinks: true,
    firstText: '«',
    previousText: '‹',
    nextText: '›',
    lastText: '»',
    rotate: true
  },
  default: {
    dateFormat: 'MM/DD/YYYY',
    numberFormat: '0,0.00'
  }
}
