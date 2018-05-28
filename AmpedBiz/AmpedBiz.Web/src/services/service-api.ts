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
  private readonly _auth: AuthService;
  private readonly _branches: BranchService;
  private readonly _customers: CustomerService;
  private readonly _inventories: InventoryService;
  private readonly _paymentTypes: PaymentTypeService;
  private readonly _pricings: PricingService;
  private readonly _productCategories: ProductCategoryService;
  private readonly _products: ProductService;
  private readonly _purchaseOrders: PurchaseOrderService;
  private readonly _returnReasons: ReturnReasonService;
  private readonly _returns: ReturnService;
  private readonly _orders: OrderService;
  private readonly _settings: SettingService;
  private readonly _suppliers: SupplierService;
  private readonly _users: UserService;
  private readonly _unitOfMeasures: UnitOfMeasureService;

  public get auth(): AuthService {
    return this._auth;
  }

  public get branches(): BranchService {
    return this._branches;
  }

  public get customers(): CustomerService {
    return this._customers;
  }

  public get inventories(): InventoryService {
    return this._inventories;
  }

  public get paymentTypes(): PaymentTypeService {
    return this._paymentTypes;
  }

  public get pricings(): PricingService {
    return this._pricings;
  }

  public get productCategories(): ProductCategoryService {
    return this._productCategories;
  }

  public get products(): ProductService {
    return this._products;
  }

  public get purchaseOrders(): PurchaseOrderService {
    return this._purchaseOrders;
  }

  public get returnReasons(): ReturnReasonService {
    return this._returnReasons;
  }

  public get returns(): ReturnService {
    return this._returns;
  }

  public get orders(): OrderService {
    return this._orders;
  }

  public get settings(): SettingService {
    return this._settings;
  }

  public get suppliers(): SupplierService {
    return this._suppliers;
  }

  public get users(): UserService {
    return this._users;
  }

  public get unitOfMeasures(): UnitOfMeasureService {
    return this._unitOfMeasures;
  }

  constructor(
    auth: AuthService,
    branches: BranchService,
    customers: CustomerService,
    inventories: InventoryService,
    paymentTypes: PaymentTypeService,
    pricings: PricingService,
    products: ProductService,
    productCategories: ProductCategoryService,
    purchaseOrders: PurchaseOrderService,
    returnReasons: ReturnReasonService,
    returns: ReturnService,
    orders: OrderService,
    settings: SettingService,
    suppliers: SupplierService,
    users: UserService,
    unitOfMeasures: UnitOfMeasureService) {

    this._auth = auth;
    this._branches = branches;
    this._customers = customers;
    this._inventories = inventories;
    this._paymentTypes = paymentTypes;
    this._pricings = pricings;
    this._productCategories = productCategories;
    this._products = products;
    this._purchaseOrders = purchaseOrders;
    this._returnReasons = returnReasons;
    this._returns = returns;
    this._orders = orders;
    this._settings = settings;
    this._suppliers = suppliers;
    this._users = users;
    this._unitOfMeasures = unitOfMeasures;
  }
}