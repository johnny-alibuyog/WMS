export class PendingList {
  activeOrders = [];
  inventoryToReorder = [];

  constructor() {

    this.activeOrders = [
      { id: 0, orderDate: "11/20/2015", status: "Invoiced", salesPerson: "Robert Zare", customerName: "Bernard Tham", company:"Company U", subTotal: "₱477.20", orderTotal: "₱551.06" },
      { id: 0, orderDate: "11/25/2015", status: "Invoiced", salesPerson: "Jan Kotas", customerName: "Carlos Grilo", company:"Company Q", subTotal: "₱637.50", orderTotal: "₱719.37" },
      { id: 0, orderDate: "12/10/2015", status: "Shiped", salesPerson: "Steven Thorpe", customerName: "Helena Kupkova", company:"Company L", subTotal: "₱586.25", orderTotal: "₱665.56" },
      { id: 0, orderDate: "12/12/2015", status: "New", salesPerson: "Laura Giussani", customerName: "Anna Bedecs", company:"Company M", subTotal: "₱334.00", orderTotal: "₱334.00" },
      { id: 0, orderDate: "12/25/2015", status: "Invoiced", salesPerson: "Robert Zare", customerName: "Christina Lee", company:"Company X", subTotal: "₱172.50", orderTotal: "₱201.12" },
      { id: 0, orderDate: "12/26/2015", status: "Shiped", salesPerson: "Jan Kotas", customerName: "Anna Bedecs", company:"Company Z", subTotal: "₱1,468.75", orderTotal: "₱1,542.18" },
    ];

    this.inventoryToReorder = [
      { 'productName': 'Alaska', 'available': 120, 'currentLevel': 120, 'targetLevel': 125, 'belowTarget': 5 },
      { 'productName': 'San Miguel Beer', 'available': 100, 'currentLevel': 100, 'targetLevel': 120, 'belowTarget': 20 },
      { 'productName': 'Ginebra San Miguel', 'available': 20, 'currentLevel': 20, 'targetLevel': 60, 'belowTarget': 10 },
      { 'productName': 'Rain Or Shine Paint', 'available': 50, 'currentLevel': 50, 'targetLevel': 100, 'belowTarget': 50 },
      { 'productName': 'Purefoods Tender Juicy Hotdog', 'available': 50, 'currentLevel': 50, 'targetLevel': 200, 'belowTarget': 150 },
      { 'productName': 'Mang Tomas', 'available': 0, 'currentLevel': 0, 'targetLevel': 40, 'belowTarget': 40 },
      { 'productName': 'Joy All-Purpose Cleaner', 'available': 0, 'currentLevel': 0, 'targetLevel': 200, 'belowTarget': 200 },
      { 'productName': 'Coca Cola', 'available': 120, 'currentLevel': 120, 'targetLevel': 200, 'belowTarget': 80 },
    ];
  }
  
  selectPage(pageNumber: any) {
    console.log(pageNumber);
  }
}