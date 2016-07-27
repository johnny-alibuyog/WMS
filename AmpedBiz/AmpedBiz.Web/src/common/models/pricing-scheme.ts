import {Lookup} from '../custom_types/lookup';
import {ProductInventory} from './product';

export interface PricingScheme {
  id?: string;
  name?: string;
}

export interface PricingSchemePageItem {
  id?: string;
  name?: string;
}


// TODO: need a better implementation of this. remodel the domain. work with this for now
let basePrice = <Lookup<string>>{ id: 'BP', name: 'Base Price' };
let retailPrice = <Lookup<string>>{ id: 'RP', name: 'Retail Price' };
let wholesalePrice = <Lookup<string>>{ id: 'WP', name: 'Wholesale Price' };

export let pricingScheme = {
  basePrice: basePrice,
  retailPrice: retailPrice,
  wholesalePrice: wholesalePrice,
  getPriceAmount: (pricingScheme: PricingScheme, product: ProductInventory) => {
    console.log(basePrice);

    if (pricingScheme.id === basePrice.id) {
      return product.basePriceAmount;
    }

    if (pricingScheme.id === retailPrice.id) {
      return product.retailPriceAmount;
    }

    if (pricingScheme.id === wholesalePrice.id) {
      return product.wholeSalePriceAmount;
    }

    return 0;
  }
};