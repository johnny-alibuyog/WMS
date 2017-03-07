import { Lookup } from '../custom_types/lookup';
import { ProductInventory } from './product';

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
let retailPrice = <Lookup<string>>{ id: 'RP', name: 'Retail Price' };
let wholesalePrice = <Lookup<string>>{ id: 'WP', name: 'Wholesale Price' };
let badStockPrice = <Lookup<string>>{ id: 'BSP', name: 'Bad Stock Price' };

export let pricing = {
  basePrice: basePrice,
  retailPrice: retailPrice,
  wholesalePrice: wholesalePrice,
  badStockPrice: badStockPrice,
  getPriceAmount: (pricing: Pricing, product: ProductInventory) => {
    switch (pricing.id) {
      case basePrice.id:
        return product.basePriceAmount;
      case retailPrice.id:
        return product.retailPriceAmount;
      case wholesalePrice.id:
        return product.wholeSalePriceAmount;
      case badStockPrice.id:
        return product.badStockPriceAmount;
      default:
        return 0;
    }
  }
};