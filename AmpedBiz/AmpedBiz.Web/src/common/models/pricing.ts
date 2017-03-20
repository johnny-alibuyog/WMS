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
let distributorPrice = <Lookup<string>>{ id: 'DP', name: 'Distributor Price' };
let listPrice = <Lookup<string>>{ id: 'LP', name: 'List Price' };
let badStockPrice = <Lookup<string>>{ id: 'BSP', name: 'Bad Stock Price' };

export let pricing = {
  basePrice: basePrice,
  distributorPrice: distributorPrice,
  listPrice: listPrice,
  badStockPrice: badStockPrice,
  getPriceAmount: (pricing: Lookup<string>, product: ProductInventory) => {
    switch (pricing.id) {
      case basePrice.id:
        return product.basePriceAmount;
      case distributorPrice.id:
        return product.distributorPriceAmount;
      case listPrice.id:
        return product.listPriceAmount;
      case badStockPrice.id:
        return product.badStockPriceAmount;
      default:
        return 0;
    }
  }
};