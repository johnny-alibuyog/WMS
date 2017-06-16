import { autoinject } from 'aurelia-framework';
import { AuthService, AuthSettings } from '../../services/auth-service';

@autoinject
export class AuthorizedValueConverter {
  private readonly _authService

  constructor(authService: AuthService) {
    this._authService = authService;
  }

  public toView(authSettings: AuthSettings): boolean {
    return this._authService.isAuthorized(authSettings.roles);
  }
}