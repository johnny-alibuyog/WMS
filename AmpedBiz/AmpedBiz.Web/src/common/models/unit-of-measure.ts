export interface UnitOfMeasure {
  id?: string;
  name?: string;
  isBaseUnit?: boolean;
  convertionFactor?: number;
  unitOfMeasureClassId?: string;
}

export interface UnitOfMeasurePageItem {
  id?: string;
  name?: string;
  isBaseUnit?: boolean;
  convertionFactor?: number;
  unitOfMeasureClassName?: string;
}