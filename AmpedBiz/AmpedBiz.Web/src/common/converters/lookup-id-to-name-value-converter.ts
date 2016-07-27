export class LookupIdToNameValueConverter {
  toView(id, items) {
    if (!id || !items) {
      return null;
    }
    var item = items.find(x => x.id === id);
    return item.name;
  }

  fromView(name, items) {
    if (!name || !items) {
      return null;
    }
    var item = items.find(x => x.name === name);
    return item.id;
  }
}