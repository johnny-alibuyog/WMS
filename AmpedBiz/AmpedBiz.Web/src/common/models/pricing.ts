import { Lookup } from '../custom_types/lookup';
import { ProductInventory } from './product';
import { UnitOfMeasure } from "./unit-of-measure";

export interface Pricing {
  id?: string;
  name?: string;
}

export interface PricingPageItem {
  id?: string;
  name?: string;
}


// TODO: need a better implementation of this. remodel the domain. work with this for now
let basePrice = <Lookup<string>>{ id: 'BP', name: 'Base Price' };
let wholesalePrice = <Lookup<string>>{ id: 'WSP', name: 'Wholesale Price' };
let retailPrice = <Lookup<string>>{ id: 'RTP', name: 'Retail Price' };
let badStockPrice = <Lookup<string>>{ id: 'BSP', name: 'Bad Stock Price' };

export let pricing = {
  basePrice: basePrice,
  wholesalePrice: wholesalePrice,
  retailPrice: retailPrice,
  badStockPrice: badStockPrice,
  getPriceAmount1: (inventory: ProductInventory, pricing: Pricing, unitOfMeasure: UnitOfMeasure) => {
    if (inventory == null || inventory.unitOfMeasures == null || inventory.unitOfMeasures.length == 0) {
      return 0;
    }

    if (pricing == null) {
      return 0;
    }

    if (unitOfMeasure == null) {
      return 0;
    }

    var productUnitOfMeasure = inventory.unitOfMeasures
      .find(x => x.unitOfMeasure.id == unitOfMeasure.id);

    if (productUnitOfMeasure == null) {
      return 0;
    }

    var price = productUnitOfMeasure.prices.find(x => x.pricing.id === pricing.id);

    if (price == null) {
      return 0;
    }

    return price.priceAmount;
  },
};