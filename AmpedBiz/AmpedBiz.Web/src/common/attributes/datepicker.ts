import { autoinject, customAttribute } from 'aurelia-framework';


@customAttribute('datepicker')
export class DatePicker {
  public element: Element;

  constructor(element: Element) {
    this.element = element;
  }

  attached() {
    /*
    $(this.element).datepicker()
      .on('change', e => fireEvent(e.target, 'input'));
    */
  }

  detached() {
    /*
    $(this.element).datepicker('destroy')
      .off('change');
    */
  }
}

function fireEvent(element, name) {
  var event = createEvent(name);
  element.dispatchEvent(event);
}

function createEvent(name) {
  var event = document.createEvent('Event');
  event.initEvent(name, true, true);
  return event;
}