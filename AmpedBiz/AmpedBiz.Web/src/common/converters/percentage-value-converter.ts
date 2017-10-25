export class PercentageValueConverter {
  fromView(value: number): number {
    if (value == null || value === 0) {
      return value;
    }

    return value / 100;
  }

  toView(value: number): number {
    if (value == null || value === 0) {
      return value;
    }

    return value * 100;
  }
}