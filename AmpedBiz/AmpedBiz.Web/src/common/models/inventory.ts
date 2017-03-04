import { Lookup } from '../custom_types/lookup';

export interface Inventory {
  id?: string;
  unitOfMeasure?: Lookup<string>;
  unitOfMeasureBase?: Lookup<string>;
  conversionFactor?: number;
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