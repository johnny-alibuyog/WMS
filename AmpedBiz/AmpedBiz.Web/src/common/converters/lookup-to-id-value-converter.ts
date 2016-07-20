/* http://stackoverflow.com/questions/33920610/binding-a-select-to-an-array-of-objects-in-aurelia-and-matching-on-id */

export class LookupToIdValueConverter {
  toView(item, items) {
    return item ? item.id : null;
  }
  
  fromView(id, items) {
    return items.find(x => x.id === id);
  }
}