import { User } from '../common/models/user'
import { ForPurchasing } from '../common/models/product';

export class SessionData {

  private setValue(key: string, value: any) {
    sessionStorage[key] = JSON.stringify(value);
  }

  private getValue<T>(key: string): T {
    return <T>JSON.parse(sessionStorage[key] || '{}');
  }

  public set forPurchasing(value: ForPurchasing) {
    this.setValue('products:for-purchasing', value);
  }

  public get forPurchasing(): ForPurchasing {
    return <ForPurchasing>this.getValue('products:for-purchasing');
  }

  public set user(user: User) {
    this.setValue('auth:current-user', user);
  };

  public get user(): User {
    return <User>this.getValue('auth:current-user');
  };
}