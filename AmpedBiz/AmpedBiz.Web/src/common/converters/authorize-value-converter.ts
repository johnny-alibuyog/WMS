import { autoinject } from 'aurelia-framework';
import { AuthService } from '../../services/auth-service';
import { Role } from '../../common/models/role';

@autoinject
export class AuthorizeValueConverter {
  private readonly _authService

  constructor(authService: AuthService) {
    this._authService = authService;
  }

  public toView(roles: Role | Role[]): boolean {
    return this._authService.isAuthorized(roles);
  }
}