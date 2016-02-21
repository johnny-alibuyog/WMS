export class InventoryLevel {
  inventoryLevels = [];

  constructor() {

    this.inventoryLevels = [
      { 'productName': 'Northwind Traders Gnocchi', 'available': 100, 'currentLevel': 100, 'targetLevel': 120, 'belowTarget': 20 },
      { 'productName': 'Northwind Traders Gnocchi', 'available': 100, 'currentLevel': 100, 'targetLevel': 120, 'belowTarget': 20 },
      { 'productName': 'Northwind Traders Mustard', 'available': 20, 'currentLevel': 20, 'targetLevel': 60, 'belowTarget': 10 },
      { 'productName': 'Northwind Traders Green Tea', 'available': 120, 'currentLevel': 120, 'targetLevel': 125, 'belowTarget': 5 },
      { 'productName': 'Northwind Traders Hot Cereal', 'available': 50, 'currentLevel': 50, 'targetLevel': 100, 'belowTarget': 50 },
      { 'productName': 'Northwind Traders Granola', 'available': 50, 'currentLevel': 50, 'targetLevel': 200, 'belowTarget': 150 },
      { 'productName': 'Northwind Traders Potato Chips', 'available': 0, 'currentLevel': 0, 'targetLevel': 40, 'belowTarget': 40 },
      { 'productName': 'Northwind Traders Green Beans', 'available': 0, 'currentLevel': 0, 'targetLevel': 200, 'belowTarget': 200 },
      { 'productName': 'Northwind Traders Vegetable Soup', 'available': 120, 'currentLevel': 120, 'targetLevel': 200, 'belowTarget': 80 },
    ];
  }
}