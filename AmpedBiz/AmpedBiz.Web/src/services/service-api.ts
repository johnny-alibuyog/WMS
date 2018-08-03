import { autoinject } from 'aurelia-framework';
import { AuthService } from './auth-service';
import { BranchService } from './branch-service';
import { CustomerService } from './customer-service';
import { InventoryService } from './inventory-service';
import { PaymentTypeService } from './payment-type-service';
import { PricingService } from './pricing-service';
import { ProductCategoryService } from './product-category-service';
import { ProductService } from './product-service';
import { PurchaseOrderService } from './purchase-order-service';
import { ReturnReasonService } from './return-reason-service';
import { ReturnService } from './return-service';
import { OrderService } from './order-service';
import { SupplierService } from './supplier-service';
import { UserService } from './user-service';
import { UnitOfMeasureService } from './unit-of-measure';
import { SettingService } from './setting-service';

@autoinject
export class ServiceApi {
  constructor(
    public readonly auth: AuthService,
    public readonly branches: BranchService,
    public readonly customers: CustomerService,
    public readonly inventories: InventoryService,
    public readonly paymentTypes: PaymentTypeService,
    public readonly pricings: PricingService,
    public readonly products: ProductService,
    public readonly productCategories: ProductCategoryService,
    public readonly purchaseOrders: PurchaseOrderService,
    public readonly returnReasons: ReturnReasonService,
    public readonly returns: ReturnService,
    public readonly orders: OrderService,
    public readonly settings: SettingService,
    public readonly suppliers: SupplierService,
    public readonly users: UserService,
    public readonly unitOfMeasures: UnitOfMeasureService
  ) { }
}