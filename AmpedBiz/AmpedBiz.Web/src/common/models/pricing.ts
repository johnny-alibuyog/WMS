import { Lookup } from '../custom_types/lookup';
import { ProductInventory } from './product';
import { UnitOfMeasure } from "./unit-of-measure";

export type Pricing = {
  id?: string;
  name?: string;
}

export type PricingPageItem = {
  id?: string;
  name?: string;
}

const basePrice = <Lookup<string>>{ id: 'BP', name: 'Base Price' };
const wholesalePrice = <Lookup<string>>{ id: 'WSP', name: 'Wholesale Price' };
const retailPrice = <Lookup<string>>{ id: 'RTP', name: 'Retail Price' };
const badStockPrice = <Lookup<string>>{ id: 'BSP', name: 'Bad Stock Price' };

export const pricing = {
  basePrice: basePrice,
  wholesalePrice: wholesalePrice,
  retailPrice: retailPrice,
  badStockPrice: badStockPrice,
  getPriceAmount: (inventory: ProductInventory, pricing: Pricing, unitOfMeasure: UnitOfMeasure) => {
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
