import { Inventory } from './inventory';

export interface Product {
  id?: string;
  code?: string;
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
  code?: string;
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

export interface DiscontinuedPageItem {
  id?: string;
  code?: string;
  name?: string
  description?: string;
  supplierName?: string;
  categoryName?: string;
  image?: string;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  wholesalePriceAmount?: number;
}

export interface ProductInventory {
  id?: string;
  code?: string;
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

export interface InventoryLevelPageItem {
  id?: string;
  code?: string;
  name?: string;
  unitOfMeasure?: string;
  onHandValue?: number;
  allocatedValue?: number;
  availableValue?: number;
  onOrderValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetLevelValue?: number;
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

export interface NeedsReorderingPageItem {
  id?: string;
  productCode?: string;
  productName?: string;
  supplierName?: string;
  categoryName?: string;
  availableValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetValue?: number;
}

export interface ForPurchasing {
  supplierId?: string;
  selectedProductIds?: string[];
  purchaseAllBelowTarget?: boolean;
}

export class ProductReportPageItem {
  id?: string;
  productCode?: string;
  productName?: string;
  categoryName?: string;
  supplierName?: string;
  onHandValue?: number;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  wholesalePriceAmount?: number;
  totalBasePriceAmount?: number;
  totalRetailPriceAmount?: number;
  totalWholesalePriceAmount?: number;
}