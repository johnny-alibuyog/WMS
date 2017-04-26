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
let wholesalePrice = <Lookup<string>>{ id: 'WSP', name: 'Wholesale Price' };
let retailPrice = <Lookup<string>>{ id: 'RTP', name: 'Retail Price' };
let badStockPrice = <Lookup<string>>{ id: 'BSP', name: 'Bad Stock Price' };

export let pricing = {
  basePrice: basePrice,
  wholesalePrice: wholesalePrice,
  retailPrice: retailPrice,
  badStockPrice: badStockPrice,
  getPriceAmount: (pricing: Lookup<string>, product: ProductInventory) => {
    switch (pricing.id) {
      case basePrice.id:
        return product.basePriceAmount;
      case wholesalePrice.id:
        return product.wholesalePriceAmount;
      case retailPrice.id:
        return product.retailPriceAmount;
      case badStockPrice.id:
        return product.badStockPriceAmount;
      default:
        return 0;
    }
  }
};