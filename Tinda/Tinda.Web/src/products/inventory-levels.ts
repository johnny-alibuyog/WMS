export class InventorLevels {
  productInventories = [];

  constructor() {

    this.productInventories = [
      { id: this.guid(), productCode: "NWTB-1", productName: "Product Name 1", category: "Category 1", supplier: "Supplier A", standardCost: "$0.00", listPrice: "$1.00" },
      { id: this.guid(), productCode: "NWTB-2", productName: "Product Name 1", category: "Category 2", supplier: "Supplier B", standardCost: "$0.00", listPrice: "$1.00" },
      { id: this.guid(), productCode: "NWTB-3", productName: "Product Name 1", category: "Category 3", supplier: "Supplier C", standardCost: "$0.00", listPrice: "$1.00" },
      { id: this.guid(), productCode: "NWTB-4", productName: "Product Name 1", category: "Category 4", supplier: "Supplier D", standardCost: "$0.00", listPrice: "$1.00" },
      { id: this.guid(), productCode: "NWTB-5", productName: "Product Name 1", category: "Category 5", supplier: "Supplier E", standardCost: "$0.00", listPrice: "$1.00" },
    ];
  }

  guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}