import { ensureNumeric } from "../utils/ensure-numeric";

export class NumericValueConverter {
  fromView(value: any): number {
    return ensureNumeric(value);
  }

  toView(value: any): number {
    return ensureNumeric(value);
  }
}