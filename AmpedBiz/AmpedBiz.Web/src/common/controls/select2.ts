/* https://gist.github.com/mujimu/c2da3ecb61f832bac9e0#file-sel ect2-multiselect-js-L1 */

import { bindable, customElement, autoinject, Binary } from 'aurelia-framework';
import 'select2';
import * as $ from 'jquery';

@customElement('select2')
@autoinject()
export class Select2 {
  @bindable name = null;    // name/id of custom select

  @bindable selected = [];  // default selected values

  @bindable options = [];   // array of options with id/name properties

  @bindable focus: boolean = false;

  @bindable placeholder: string = "";

  @bindable multiple: boolean = false;

  @bindable allow_clear: boolean = false;

  protected element: Element;

  constructor(element: Element) {
    this.element = element;
  }

  selectedChanged(): void {
    var event = document.createEvent('CustomEvent');
    event.initCustomEvent('selected:change', true, true, { selected: this.selected });
    this.element.dispatchEvent(event);

    console.log(this.selected);
  }

  attached() {
    let el = $(this.element).find('select');

    if (this.multiple) {
      el.attr('multiple', 'multiple');
    }

    let sel = el.select2({ tags: true });

    // preload selected values
    sel.val(this.selected).trigger('change');

    // on any change, propagate it to underlying select to trigger two-way bind
    sel.on('change', (event) => {
      // don't propagate endlessly
      // see: http://stackoverflow.com/a/34121891/4354884
      if (event.originalEvent) {
        return;
      }
      // dispatch to raw select within the custom element
      // bubble it up to allow change handler on custom element
      var notice = new CustomEvent('change', { bubbles: true });
      $(el)[0].dispatchEvent(notice);
    });

    /* https://ilikekillnerds.com/2015/08/aurelia-custom-elements-custom-callback-events-tutorial/ */
    /*
    sel.on('change', (event) => {
      let changeEvent: CustomEvent;

      if (event.originalEvent) {
        changeEvent = document.createEvent('CustomEvent');
        changeEvent.initCustomEvent('change', true, true, {
          detail: {
            value: event.target.value
          }
        });
      } else {
        changeEvent = new CustomEvent('change', {
          detail: {
            value: event.target.value
          },
          bubbles: true
        });
      }
      this.element.dispatchEvent(changeEvent);
    });
    */
  }

  detached() {
    $(this.element).find('select').select2('destroy');
  }
}
