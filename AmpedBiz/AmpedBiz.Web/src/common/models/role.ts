export type Role = {
  id: string;
  name: string;
  assigned: boolean;
}

export const role = {
  admin: <Role>{
    id: 'A',
    name: 'Admin'
  },
  manager: <Role>{
    id: 'M',
    name: 'Manager'
  },
  salesclerk: <Role>{
    id: 'S',
    name: 'Salesclerk'
  },
  cashier: <Role>{
    id: 'C',
    name: 'Cashier'
  },
  warehouseman: <Role>{
    id: 'W',
    name: 'Warehouseman'
  },
  unknown: <Role>{
    id: 'UR',
    name: 'Unknown'
  },  
  all: () => [
    role.admin,
    role.manager,
    role.salesclerk,
    role.warehouseman,
  ],
}
