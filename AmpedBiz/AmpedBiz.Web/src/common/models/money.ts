import { Currency } from "./currency";

export interface Money {
  currency?: Currency;
  amount?: number;
}

export const getAmount = (money: Money) => money && money.amount || 0;