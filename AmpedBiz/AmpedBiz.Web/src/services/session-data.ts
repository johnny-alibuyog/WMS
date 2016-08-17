import {ForPurchasing} from '../common/models/product';

export class SessionData {

  private readonly FOR_PURCHASING: string = 'products:for-purchasing';

  public set forPurchasing(value: ForPurchasing) { 
    sessionStorage[this.FOR_PURCHASING] = JSON.stringify(value);
  }

  public get forPurchasing(): ForPurchasing {
    return <ForPurchasing>JSON.parse(sessionStorage[this.FOR_PURCHASING] || "{}");
  }
}