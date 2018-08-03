import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { NotificationService } from '../common/controls/notification-service';
import { ActionResult } from '../common/controls/notification';
import { InventoryAdjustment, InventoryAdjustmentReason, InventoryAdjustmentType } from '../common/models/inventory';
import { ProductInventory } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { UnitOfMeasure } from '../common/models/unit-of-measure';

@autoinject
export class ProductInventoryAdjustmentCreate {
  public header: string = 'Adjust Inventory';
  public adjustment: InventoryAdjustment = {};
  public canSave: boolean = true;

  public lookup = {
    inventory: <ProductInventory>{},
    unitOfMeasures: <UnitOfMeasure[]>[],
    types: <Lookup<InventoryAdjustmentType>[]>[],
    reasons: <InventoryAdjustmentReason[]>[]
  }

  constructor(
    private readonly _api: ServiceApi,
    private readonly _controller: DialogController,
    private readonly _notification: NotificationService
  ) { }

  public async activate(params: { inventoryId: string, productId: string }): Promise<void> {
    try {
      [this.lookup.inventory, this.lookup.types] = await Promise.all([
        this._api.products.getInventory(params.productId),
        this._api.inventories.getAdjustmentTypeLookup()
      ]);
      this.adjustment.inventoryId = params.inventoryId;
      this.adjustment.adjustedBy = this._api.auth.userAsLookup;
      this.adjustment.adjustedOn = new Date();
      this.lookup.unitOfMeasures = this.lookup.inventory.unitOfMeasures.map(x => x.unitOfMeasure);
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async typeChanged(type?: InventoryAdjustmentType): Promise<void> {
    this.lookup.reasons = (type) ? await this._api.inventories.getAdjustmentReasonList(type) : [];
  }

  public unitChanged(unit?: UnitOfMeasure): void {
    this.adjustment.standard = (unit) ? this.lookup.inventory.unitOfMeasures.find(x => x.unitOfMeasure.id == unit.id).standard : null;
  }

  public cancel(): void {
    this._controller.cancel();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.adjustment.adjustedOn = new Date();
        let data = await this._api.inventories.createAdjustment(this.adjustment);
        await this._notification.success("Adjustment has been saved.").whenClosed();
        await this._controller.ok(data);
      }
    }
    catch (error) {
      this._notification.warning(error)
    }
  }
}