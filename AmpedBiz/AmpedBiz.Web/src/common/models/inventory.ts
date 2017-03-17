import { Lookup } from '../custom_types/lookup';

export interface Inventory {
  id?: string;
  barcode?: string;
  packagingBarcode?: string;
  unitOfMeasure?: Lookup<string>;
  packagingUnitOfMeasure?: Lookup<string>;
  packagingSize?: number;
  basePriceAmount?: number;
  retailPriceAmount?: number;
  wholesalePriceAmount?: number;
  badStockPriceAmount?: number;
  badStockValue?: number;
  receivedValue?: number;
  onOrderValue?: number;
  onHandValue?: number;
  allocatedValue?: number;
  shippedValue?: number;
  backOrderedValue?: number;
  availableValue?: number;
  initialLevelValue?: number;
  shrinkageValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetLevelValue?: number;
  reorderLevelValue?: number;
  reorderQuantityValue?: number;
  minimumReorderQuantityValue?: number;
}