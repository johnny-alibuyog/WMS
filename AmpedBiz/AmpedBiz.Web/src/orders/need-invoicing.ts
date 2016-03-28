export class NeedInvoicingOrders {
  needInvoicingOrders = [];

  constructor() {

    this.needInvoicingOrders = [
      { 'code': 'XXX', 'orderDate': 'March 15, 2016', 'user': 'J.Co', 'customer': 'Starbucks', 'status': 'pending', 'deliveryDate' : '', 'orderTotal' : 1000 },
      { 'code': 'ABC', 'orderDate': 'March 25, 2016', 'user': 'S.Co', 'customer': '7-11', 'status': 'pending', 'deliveryDate' : '', 'orderTotal' : 5000 },
    ];
  }
}