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
  reportMapping: {
    baseUrl: 'http://localhost:64038/ReportViewers/MainViewer.aspx?DFD18FD7-9D34-4692-B7F9-7E54BFB4B0EB=',
    priceList: 'ucpricelistselector.ascx',
    uom: 'ucUOMDetails.ascx',
    customerList: 'ucCustomerList.ascx',
    supplierList: 'ucSupplierList.ascx',
    orderList:'ucOrderListSelector.ascx'
  },
  page: <PageConfig>{
    maxSize: 5,
    itemsPerPage: 10,
    boundaryLinks: true,
    directionLinks: true,
    firstText: '|<',
    previousText: '<',
    nextText: '>',
    lastText: '>|',
    rotate: true
  },
  default: {
    dateFormat: 'MM/DD/YYYY',
    numberFormat: '0,0.00'
  }
}