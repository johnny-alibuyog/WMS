import {autoinject} from 'aurelia-framework';
import {AuthService} from './auth-service';
import {BranchService} from './branch-service';
import {CustomerService} from './customer-service';
import {PaymentTypeService} from './payment-type-service';
import {ProductCategoryService} from './product-category-service';
import {ProductService} from './product-service';
import {SupplierService} from './supplier-service';
import {UserService} from './user-service';

@autoinject
export class ServiceApi {
  private _auth: AuthService;
  private _branches: BranchService;
  private _customers: CustomerService;
  private _paymentTypes: PaymentTypeService;
  private _productCategories: ProductCategoryService;
  private _products: ProductService;
  private _suppliers: SupplierService;
  private _users: UserService;

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

  public get suppliers(): SupplierService {
    return this._suppliers;
  }

  public get users(): UserService {
    return this._users;
  }

  constructor(auth: AuthService, branches: BranchService, customers: CustomerService, paymentTypes: PaymentTypeService,
    products: ProductService, productCategories: ProductCategoryService, suppliers: SupplierService, users: UserService) {

    this._auth = auth;
    this._branches = branches;
    this._customers = customers;
    this._paymentTypes = paymentTypes;
    this._productCategories = productCategories;
    this._products = products;
    this._suppliers = suppliers;
    this._users = users;
  }
}