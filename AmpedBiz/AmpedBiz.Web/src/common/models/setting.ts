export type UserSetting = {
  defaultPassword?: string;
}

export type ModulesAvailedSettings = {
  orders: boolean;
  pointOfSales: boolean;
  purchaseOrders: boolean;
}

export const defaultModules = <ModulesAvailedSettings>{
  orders: true,
  pointOfSales: true,
  purchaseOrders: true
};
