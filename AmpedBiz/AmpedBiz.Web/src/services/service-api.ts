import { autoinject } from 'aurelia-framework';
import { AuthService } from './auth-service';
import { BranchService } from './branch-service';
import { CustomerService } from './customer-service';
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

@autoinject
export class ServiceApi {
  private _auth: AuthService;
  private _branches: BranchService;
  private _customers: CustomerService;
  private _paymentTypes: PaymentTypeService;
  private _pricings: PricingService;
  private _productCategories: ProductCategoryService;
  private _products: ProductService;
  private _purchaseOrders: PurchaseOrderService;
  private _returnReasons: ReturnReasonService;
  private _returns: ReturnService;
  private _orders: OrderService;
  private _suppliers: SupplierService;
  private _users: UserService;
  private _unitOfMeasures: UnitOfMeasureService;

  public get auth(): AuthService {
    return this._auth;
  }

  public get branches(): BranchService {
    return this._branches;
  }

  public get customers(): CustomerService {
    return this._customers;
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

  public get suppliers(): SupplierService {
    return this._suppliers;
  }

  public get users(): UserService {
    return this._users;
  }

  public get unitOfMeasures(): UnitOfMeasureService {
    return this._unitOfMeasures;
  }

  constructor(auth: AuthService, branches: BranchService, customers: CustomerService, paymentTypes: PaymentTypeService,
    pricings: PricingService, products: ProductService, productCategories: ProductCategoryService,
    purchaseOrders: PurchaseOrderService, returnReasons: ReturnReasonService, returns: ReturnService,
    orders: OrderService, suppliers: SupplierService, users: UserService, unitOfMeasures: UnitOfMeasureService) {

    this._auth = auth;
    this._branches = branches;
    this._customers = customers;
    this._paymentTypes = paymentTypes;
    this._pricings = pricings;
    this._productCategories = productCategories;
    this._products = products;
    this._purchaseOrders = purchaseOrders;
    this._returnReasons = returnReasons;
    this._returns = returns;
    this._orders = orders;
    this._suppliers = suppliers;
    this._users = users;
    this._unitOfMeasures = unitOfMeasures;
  }
}