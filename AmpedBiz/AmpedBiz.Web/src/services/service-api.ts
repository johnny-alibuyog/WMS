import {autoinject} from 'aurelia-framework';
import {AuthService} from './auth-service';
import {BranchService} from './branch-service';
import {CustomerService} from './customer-service';
import {PaymentTypeService} from './payment-type-service';
import {ProductCategoryService} from './product-category-service';
import {ProductService} from './product-service';
import {PurchaseOrderService} from './purchase-order-service';
import {SupplierService} from './supplier-service';
import {UserService} from './user-service';
import {UnitOfMeasureService} from './unit-of-measure';
import {UnitOfMeasureClassService} from './unit-of-measure-class';

@autoinject
export class ServiceApi {
  private _auth: AuthService;
  private _branches: BranchService;
  private _customers: CustomerService;
  private _paymentTypes: PaymentTypeService;
  private _productCategories: ProductCategoryService;
  private _products: ProductService;
  private _purchaseOrders: PurchaseOrderService; 
  private _suppliers: SupplierService;
  private _users: UserService;
  private _unitOfMeasures: UnitOfMeasureService;
  private _unitOfMeasureClasses: UnitOfMeasureClassService;

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

  public get productCategories(): ProductCategoryService {
    return this._productCategories;
  }

  public get products(): ProductCategoryService {
    return this._products;
  }

  public get purchaseOrders(): PurchaseOrderService {
    return this._purchaseOrders;
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
  
  public get unitOfMeasureClasses(): UnitOfMeasureClassService {
    return  this._unitOfMeasureClasses;
  }

  constructor(auth: AuthService, branches: BranchService, customers: CustomerService, paymentTypes: PaymentTypeService,
    products: ProductService, productCategories: ProductCategoryService, purchaseOrders: PurchaseOrderService, 
    suppliers: SupplierService, users: UserService, unitOfMeasures: UnitOfMeasureService, 
    unitOfMeasureClasses: UnitOfMeasureClassService) {

    this._auth = auth;
    this._branches = branches;
    this._customers = customers;
    this._paymentTypes = paymentTypes;
    this._productCategories = productCategories;
    this._products = products;
    this._purchaseOrders = purchaseOrders;
    this._suppliers = suppliers;
    this._users = users;
    this._unitOfMeasures = unitOfMeasures;
    this._unitOfMeasureClasses = unitOfMeasureClasses;
  }
}