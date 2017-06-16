export interface Role {
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
  warehouseman: <Role>{
    id: 'W',
    name: 'Warehouseman'
  },
  unknownRole: <Role>{
    id: 'UR',
    name: 'Unknown Role'
  },
  all: <Role[]>[
    this.admin,
    this.manager,
    this.salesclerk,
    this.warehouseman,
  ]
}
