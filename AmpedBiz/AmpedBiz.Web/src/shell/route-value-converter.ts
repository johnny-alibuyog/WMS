import { NavModel } from "aurelia-router";

export class GroupedRouteValueConverter {
  public toView(navModels: NavModel[]): Map<string, NavModel[]> {
    let groups = new Map<string, NavModel[]>();
    for (let model of navModels) {
      let key = model.settings.group || model.title;
      let routes = groups.get(key);
      if (!routes) {
        groups.set(key, (routes = []));
      }
      routes.push(model);
    }
    return groups;
  }
}

export class GroupedRouteSizeValueConverter {
  public toView(navModels: NavModel[]): number {
    const groupedRoutes = new GroupedRouteValueConverter().toView(navModels);
    return groupedRoutes.size;
  }
}
