export class InventoryLevel {
  inventoryLevels = [];

  constructor() {

    this.inventoryLevels = [
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
}