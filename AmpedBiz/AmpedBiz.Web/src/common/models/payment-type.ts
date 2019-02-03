import { Lookup } from "../custom_types/lookup";

export type PaymentType = {
  id?: string;
  name?: string;
}

export type PaymentTypePageItem = {
  id?: string;
  name?: string;
}

const cash = <Lookup<string>>{ id: "CS", name: "Cash" };
const check = <Lookup<string>>{ id: "CK", name: "Check" };
const creditCard = <Lookup<string>>{ id: "CC", name: "Credit Card" };
const mixed = <Lookup<string>>{ id: "MX", name: "Mixed" };

export const paymentType = {
  cash: cash,
  check: check,
  creditCard: creditCard,
  mixed: mixed
};
