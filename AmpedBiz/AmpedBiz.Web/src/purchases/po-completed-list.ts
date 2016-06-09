export class CompletedPurchases {
  awaitingApprovalPurchases = [];

  constructor() {

    this.awaitingApprovalPurchases = [
      { 'code': 'XYZ', 'creationDate': 'March 15, 2016', 'user': 'J.Co', 'status': 'pending', 'supplier' : 'San Miguel', 'submittedBy' : 'Employee1', 'approvedBy' : 'Employee2', 'completedBy' : 'Employee3', 'paymentDate' : 'March 20, 2016', total : 700 },
    ];
  }
}