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
  discontinued?: number;
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
  discontinued?: number;
}