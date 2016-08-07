export interface Product {
  id?: string;
  name?: string
  description?: string;
  supplierId?: string;
  categoryId?: string;
  image?: string;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  wholeSalePriceAmount?: number;
  discontinued?: boolean;
}

export interface ProductPageItem {
  id?: string;
  name?: string
  description?: string;
  supplierName?: string;
  categoryName?: string;
  image?: string;
  availableValue?: number;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  discontinued?: boolean;
}

export interface ProductInventory {
  id?: string;
  name?: string;
  unitOfMeasure?: string;
  targetValue?: number;
  availableValue?: number;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  wholeSalePriceAmount?: number;
  discountAmount?: number;
}

export interface ProductPurchasePageItem {
  id?: string;
  purchaseOrderNumber?: string;
  createdOn?: Date;
  status?: string;
  supplierName?: string;
  unitCostAmount?: number;
  quantityValue?: number;
  receivedValue?: number;
}

export interface ProductOrderPageItem {
  id?: string;
  orderNumber?: string;
  createdOn?: Date;
  status?: string;
  customerName?: string;
  quantityValue?: number;
}