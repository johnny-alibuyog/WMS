/* http://stackoverflow.com/questions/33920610/binding-a-select-to-an-array-of-objects-in-aurelia-and-matching-on-id */

export class ToIdValueConverter {
  toView(obj, idPropertyName, objs) {
    return obj ? obj[idPropertyName] : null;
  }

  fromView(id, idPropertyName, objs) {
    return objs.find(o => o[idPropertyName] === id);
  }
}