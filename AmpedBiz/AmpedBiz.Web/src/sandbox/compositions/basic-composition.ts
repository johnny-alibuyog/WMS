import { autoinject, PLATFORM } from "aurelia-framework";
import { RouterConfiguration, Router } from "aurelia-router";

@autoinject
export class BasicComposition {
  public viewModel: string = "./compose-me";
}
