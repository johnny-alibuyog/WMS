import { UnitOfMeasure } from "./unit-of-measure";

export interface Measure {
  value?: number;
  unit?: UnitOfMeasure;
}

export const getValue = (measure: Measure) => measure && measure.value || 0;