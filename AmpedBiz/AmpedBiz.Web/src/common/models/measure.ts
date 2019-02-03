import { UnitOfMeasure } from "./unit-of-measure";

export type Measure = {
  value?: number;
  unit?: UnitOfMeasure;
}

export const getValue = (measure: Measure) => measure && measure.value || 0;

export const getUnitId = (measure: Measure) => measure && measure.unit && measure.unit.id || '';

