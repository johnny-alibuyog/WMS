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
  encoder: <Role>{
    id: 'E',
    name: 'Encoder'
  },
  manager: <Role>{
    id: 'M',
    name: 'Manager'
  },
  sales: <Role>{
    id: 'S',
    name: 'Sales'
  },
  superAdmin: <Role>{
    id: 'SA',
    name: 'Super Admin'
  },
  warehouse: <Role>{
    id: 'W',
    name: 'Warehouse'
  },
  unknownRole: <Role>{
    id: 'UR',
    name: 'Unknown Role'
  },
  all: <Role[]>[
    this.admin,
    this.encoder,
    this.manager,
    this.sales,
    this.superAdmin,
    this.warehouse,
  ]
}
