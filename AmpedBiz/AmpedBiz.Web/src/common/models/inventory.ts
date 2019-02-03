import { Lookup } from '../custom_types/lookup';
import { Measure } from './measure';

export const inventoryEvents = {
  adjustment: {
    create: 'inventory-adjustment-create',
    created: 'inventory-adjustment-created',
  },
}

export enum InventoryAdjustmentType {
  increase = 1,
  decrease = 2
}

export type Inventory = {
  id?: string;
  individualBarcode?: string;
  packagingBarcode?: string;
  unitOfMeasure?: Lookup<string>;
  packagingUnitOfMeasure?: Lookup<string>;
  packagingSize?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  retailPriceAmount?: number;
  badStockPriceAmount?: number;
  badStockValue?: number;
  receivedValue?: number;
  onOrderValue?: number;
  onHandValue?: number;
  allocatedValue?: number;
  shippedValue?: number;
  backOrderedValue?: number;
  returnedValue?: number;
  availableValue?: number;
  initialLevelValue?: number;
  shrinkageValue?: number;
  currentLevelValue?: number;
  targetLevelValue?: number;
  belowTargetLevelValue?: number;
  reorderLevelValue?: number;
  reorderQuantityValue?: number;
  minimumReorderQuantityValue?: number;
  increaseAdjustmentValue?: number;
  decreaseAdjustmentValue?: number;
}

export type InventoryAdjustmentPageItem = {
  id?: string;
  adjustedBy?: string;
  adjustedOn?: Date;
  reason?: string;
  remarks?: string,
  type?: string;
  quantity?: string;
}

export type InventoryAdjustment = {
  id?: string;
  inventoryId?: string;
  adjustedBy?: Lookup<string>;
  adjustedOn?: Date;
  reason?: InventoryAdjustmentReason;
  remarks?: string,
  type?: InventoryAdjustmentType;
  quantity?: Measure;
  standard?: Measure;
}

export type InventoryAdjustmentReason = {
  id?: string;
  name?: string;
  description?: string;
  type?: InventoryAdjustmentType;
}

export type InventoryMovementsReportPageItem = {
  date?: Date;
  branchName?: string;
  productName?: string;
  out?: string;
  in?: string;
}
