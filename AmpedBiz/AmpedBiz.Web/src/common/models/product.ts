import {Inventory} from './inventory';

export interface Product {
  id?: string;
  name?: string
  description?: string;
  supplierId?: string;
  categoryId?: string;
  image?: string;
  discontinued?: boolean;
  inventory?: Inventory;
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
  badStockValue?: number;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  wholeSalePriceAmount?: number;
  badStockPriceAmount?: number;
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