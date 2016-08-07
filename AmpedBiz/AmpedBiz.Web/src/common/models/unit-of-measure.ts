export interface UnitOfMeasure {
  id?: string;
  name?: string;
  isBaseUnit?: boolean;
  conversionFactor?: number;
  unitOfMeasureClassId?: string;
}

export interface UnitOfMeasurePageItem {
  id?: string;
  name?: string;
  isBaseUnit?: boolean;
  conversionFactor?: number;
  unitOfMeasureClassName?: string;
}