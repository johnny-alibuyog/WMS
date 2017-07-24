import { autoinject } from "aurelia-framework";
import { AuthService } from "./auth-service";
import { Role } from "../common/models/role";
import { StageDefinition } from '../common/models/stage-definition';

@autoinject
export class StageGuard<TStatus, TAggregate> {
  private readonly _auth: AuthService;

  constructor(auth: AuthService) {
    this._auth = auth;
  }

  public IsTransitionAllowedTo(stage: StageDefinition<TStatus, TAggregate>, destination: TStatus, requiredRoles: Role[]): boolean {

    var allowed = stage.allowedTransitions.find(x => x == destination) !== undefined;
    if (!allowed) {
      return false;
    }

    var authorized = this._auth.isAuthorized(requiredRoles);
    if (!authorized) {
      return false;
    }

    return true;
  }

  public IsModificationAllowedTo(stage: StageDefinition<TStatus, TAggregate>, aggregate: TAggregate, requiredRoles: Role[]): boolean {

    var allowed = stage.allowedModifications.find(x => x == aggregate) !== undefined;
    if (!allowed) {
      return false;
    }

    var authorized = this._auth.isAuthorized(requiredRoles);
    if (!authorized) {
      return false;
    }

    return true;
  }

  public CanSave(requiredRoles: Role[]){
    return this._auth.isAuthorized(requiredRoles);
  }
}