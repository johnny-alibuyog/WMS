export interface UserSetting {
  defaultPassword?: string;
}

export interface ModulesAvailedSettings {
  orders: boolean;
  pointOfSales: boolean;
  purchaseOrders: boolean;
}

export const defaultModules = <ModulesAvailedSettings>{
  orders: true,
  pointOfSales: true,
  purchaseOrders: true
};
