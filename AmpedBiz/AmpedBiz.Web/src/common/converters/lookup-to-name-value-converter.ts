/* http://stackoverflow.com/questions/33920610/binding-a-select-to-an-array-of-objects-in-aurelia-and-matching-on-id */

export class LookupToNameValueConverter {
  toView(item, items) {
    return item ? item.name : null;
  }
  
  fromView(name, items) {
    return items.find(x => x.name === name);
  }
}