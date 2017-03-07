export class PercentageValueConverter {
  fromView(value: number): number {
    if (!value) {
      return 0;
    }

    if (value === 0) {
      return 0;
    }

    return value / 100;
  }

  toView(value: number): number {
    if (!value) {
      return 0;
    }

    if (value === 0) {
      return 0;
    }

    return value * 100;
  }
}