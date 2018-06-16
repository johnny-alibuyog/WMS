import { Inventory } from './inventory';
import { Lookup } from "../custom_types/lookup";
import { Measure } from "./measure";
import { UnitOfMeasure } from "./unit-of-measure";

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
  unitOfMeasures?: ProductUnitOfMeasure[];
}

export interface ProductUnitOfMeasure {
  id?: string;
  size?: string;
  barcode?: string;
  unitOfMeasure?: UnitOfMeasure;
  standardEquivalentValue?: number;
  isStandard?: boolean;
  isDefault?: boolean;
  prices?: ProductUnitOfMeasurePrice[];
}

export interface ProductUnitOfMeasurePrice {
  id?: string;
  pricing?: Lookup<string>;
  priceAmount?: number;
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
  wholesalePriceAmount?: number;
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
}

export interface ProductInventory {
  id?: string;
  inventoryId?: string;
  code?: string;
  name?: string;
  unitOfMeasures?: ProductInventoryUnitOfMeasure[];
}

export interface ProductInventoryUnitOfMeasure {
  unitOfMeasure?: UnitOfMeasure;
  isStandard?: boolean;
  isDefault?: boolean;
  available?: Measure;
  standard?: Measure;
  prices?: ProductInventoryUnitOfMeasurePrice[];
}

export interface ProductInventoryUnitOfMeasurePrice {
  pricing?: Lookup<string>;
  priceAmount?: number;
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
  reorderLevelValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetLevelValue?: number;
}

export class ProductReturnPageItem {
  id?: string;
  reasonName?: string;
  returnedOn?: Date;
  returnedByName?: string;
  returnedAmount?: number;
  quantityValue?: number;
}

export class ProductOrderReturnPageItem {
  id?: string;
  reasonName?: string;
  returnedOn?: Date;
  returnedByName?: string;
  returnedAmount?: number;
  quantity?: Measure;
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
  quantity?: Measure;
}

export interface NeedsReorderingPageItem {
  id?: string;
  productCode?: string;
  productName?: string;
  supplierName?: string;
  categoryName?: string;
  reorderLevelValue?: number;
  availableValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetValue?: number;
  minimumReorderQuantity?: number;
  reorderQuantity?: number;
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
  onHandUnit?: string;
  onHandValue?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  retailPriceAmount?: number;
  totalBasePriceAmount?: number;
  totalWholesalePriceAmount?: number;
  totalRetailPriceAmount?: number;
}

export class ProductsDeliveredReportPageItem {
  completedOn?: Date;
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

export class ProductListingReportPageItem {
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

  public getPriceAmount(inventory: ProductInventory, unitOfMeasure: UnitOfMeasure, pricing: Lookup<string>): number {
    var price = this.getPrice(inventory, unitOfMeasure, pricing);
    return price && price.priceAmount || 0;
  }
};
