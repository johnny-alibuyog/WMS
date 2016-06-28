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
  basePrice?: string;
  retailPrice?: string;
  wholeSalePrice?: string;
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