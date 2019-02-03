import { Inventory } from './inventory';
import { Lookup } from "../custom_types/lookup";
import { Measure } from "./measure";
import { UnitOfMeasure } from "./unit-of-measure";

export type Product = {
  id?: string;
  code?: string;
  name?: string
  description?: string;
  supplierId?: string;
  categoryId?: string;
  image?: string;
  discontinued?: boolean;
  inventory?: Inventory;
  unitOfMeasures?: ProductUnitOfMeasure[];
}

export type ProductUnitOfMeasure = {
  id?: string;
  productId?: string;
  size?: string;
  barcode?: string;
  unitOfMeasure?: UnitOfMeasure;
  standardEquivalentValue?: number;
  isStandard?: boolean;
  isDefault?: boolean;
  prices?: ProductUnitOfMeasurePrice[];
}

export type ProductUnitOfMeasurePrice = {
  id?: string;
  pricing?: Lookup<string>;
  priceAmount?: number;
}

export type ProductPageItem = {
  id?: string;
  code?: string;
  name?: string
  description?: string;
  supplierName?: string;
  categoryName?: string;
  image?: string;
  availableValue?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  discontinued?: boolean;
}

export type DiscontinuedPageItem = {
  id?: string;
  code?: string;
  name?: string
  description?: string;
  supplierName?: string;
  categoryName?: string;
  image?: string;
}

export type ProductInventory = {
  id?: string;
  inventoryId?: string;
  code?: string;
  name?: string;
  unitOfMeasures?: ProductInventoryUnitOfMeasure[];
}

export type ProductInventoryUnitOfMeasure = {
  unitOfMeasure?: UnitOfMeasure;
  isStandard?: boolean;
  isDefault?: boolean;
  barcode?: string;
  available?: Measure;
  targetLevel?: Measure;
  badStock?: Measure;
  standard?: Measure;
  prices?: ProductInventoryUnitOfMeasurePrice[];
}

export type ProductInventoryUnitOfMeasurePrice = {
  pricing?: Lookup<string>;
  priceAmount?: number;
}

export type InventoryLevelPageItem = {
  id?: string;
  code?: string;
  name?: string;
  unitOfMeasure?: string;
  onHandValue?: number;
  allocatedValue?: number;
  availableValue?: number;
  onOrderValue?: number;
  reorderLevelValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetLevelValue?: number;
}

export type ProductReturnPageItem = {
  id?: string;
  reasonName?: string;
  returnedOn?: Date;
  returnedByName?: string;
  returnedAmount?: number;
  quantityValue?: number;
}

export type ProductOrderReturnPageItem = {
  id?: string;
  reasonName?: string;
  returnedOn?: Date;
  returnedByName?: string;
  returnedAmount?: number;
  quantity?: Measure;
}

export type ProductPurchasePageItem = {
  id?: string;
  purchaseOrderNumber?: string;
  createdOn?: Date;
  status?: string;
  supplierName?: string;
  unitCostAmount?: number;
  quantityValue?: number;
  receivedValue?: number;
}

export type ProductOrderPageItem = {
  id?: string;
  orderNumber?: string;
  createdOn?: Date;
  status?: string;
  customerName?: string;
  quantity?: Measure;
}

export type NeedsReorderingPageItem = {
  id?: string;
  productCode?: string;
  productName?: string;
  supplierName?: string;
  categoryName?: string;
  unitOfMeasureName?: string;
  reorderLevelValue?: number;
  availableValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetValue?: number;
  minimumReorderQuantity?: number;
  reorderQuantity?: number;
}

export type ForPurchasing = {
  supplierId?: string;
  selectedProductIds?: string[];
  purchaseAllBelowTarget?: boolean;
}

export type ProductReportPageItem = {
  id?: string;
  productCode?: string;
  productName?: string;
  categoryName?: string;
  supplierName?: string;
  onHandUnit?: string;
  onHandValue?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  retailPriceAmount?: number;
  totalBasePriceAmount?: number;
  totalWholesalePriceAmount?: number;
  totalRetailPriceAmount?: number;
}

export type ProductsDeliveredReportPageItem = {
  shippedOn?: Date;
  branchName?: string;
  supplierName?: string;
  categoryName?: string;
  productName?: string;
  quantityUnit?: string;
  quantityValue?: number;
  unitPriceAmount?: number;
  discountAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export type ProductListingReportPageItem = {
  branchName?: string;
  supplierName?: string;
  categoryName?: string;
  productName?: string;
  quantityUnit?: string;
  onHandValue?: number;
  availableValue?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  retailPriceAmount?: number;
  suggestedRetailPriceAmount?: number;
}

export class ProductInventoryFacade {
  private _inventory: ProductInventory;

  constructor(inventory: ProductInventory) {
    this._inventory = inventory;
  }

  public get standard(): ProductInventoryUnitOfMeasure {
    return this._inventory.unitOfMeasures.find(x => x.isStandard);
  }

  public get default(): ProductInventoryUnitOfMeasure {
    return this._inventory.unitOfMeasures.find(x => x.isDefault);
  }

  public current(unitOfMeasure: UnitOfMeasure): ProductInventoryUnitOfMeasure{
    return this._inventory.unitOfMeasures.find(x => x.unitOfMeasure.id === unitOfMeasure.id);
  }

  public getProduct(): Lookup<string> {
    return {
      id: this._inventory.id,
      name: this._inventory.name,
    };
  }

  public getUnitOfMeasures(): UnitOfMeasure[] {
    return this._inventory.unitOfMeasures.map(x => x.unitOfMeasure);
  }

  public getPrice(inventory: ProductInventory, unitOfMeasure: UnitOfMeasure, pricing: Lookup<string>): ProductInventoryUnitOfMeasurePrice {
    var productUnitOfMeasure = inventory.unitOfMeasures.find(x => x.unitOfMeasure.id === unitOfMeasure.id);
    if (!productUnitOfMeasure) {
      return null;
    }

    return productUnitOfMeasure.prices.find(x => x.pricing.id == pricing.id);
  }

  public getBarcode(inventory: ProductInventory, unitOfMeasure: UnitOfMeasure, pricing: Lookup<string>): string {
    var productUnitOfMeasure = inventory.unitOfMeasures.find(x => x.unitOfMeasure.id === unitOfMeasure.id);
    if (!productUnitOfMeasure) {
      return null;
    }

    return productUnitOfMeasure.barcode;
  }

  public getPriceAmount(inventory: ProductInventory, unitOfMeasure: UnitOfMeasure, pricing: Lookup<string>): number {
    var price = this.getPrice(inventory, unitOfMeasure, pricing);
    return price && price.priceAmount || 0;
  }
};
