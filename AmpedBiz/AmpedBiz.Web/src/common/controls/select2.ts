/* https://gist.github.com/mujimu/c2da3ecb61f832bac9e0#file-sel ect2-multiselect-js-L1 */
import { bindable, bindingMode, customElement, autoinject } from 'aurelia-framework';
import 'select2';
import $ from 'jquery';

@customElement('select2')
@autoinject()
export class Select2 {

  @bindable
  public name: string = null;    // name/id of custom select

  @bindable
  public options: any[] = [];   // array of options with id/name properties

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public selected: any | any[] = [];  // default selected values

  @bindable
  public opened: boolean = false;

  @bindable
  public placeholder: string = "";

  @bindable
  public multiple: boolean = false;

  @bindable
  public allow_clear: boolean = false;

  @bindable
  public $parent: any = null;

  protected element: Element;

  private _select: any;

  constructor(element: Element) {
    this.element = element;
  }

  placeholderChanged(newValue: string, oldValue: string) {
  }

  openedChanged(newValue: boolean, oldValue: boolean): void {
    let command = newValue ? 'open' : 'close';
    this._select.select2(command);
  }

  selectedChanged(): void {
    this.element.dispatchEvent(
      new CustomEvent('selected:change', { 
        bubbles: true, 
        cancelable: true, 
        detail: { selected: this.selected }
      })
    );
  }

  attached() {
    let el = $(this.element).find('select');

    if (this.multiple) {
      el.attr('multiple', 'multiple');
    }

    this._select = el.select2({ tags: true });

    // preload selected values
    this._select.val(this.selected).trigger('change');

    // on any change, propagate it to underlying select to trigger two-way bind
    this._select.on('change', (event) => {
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

    Select2

    if (this.opened) {
      this._select.select2('open');
      const input = document.querySelector('.select2-search__field') as HTMLInputElement;
      input?.focus();
      // $(this.element).find('.select2-search__field').trigger('focus');
    }

    /* https://ilikekillnerds.com/2015/08/aurelia-custom-elements-custom-callback-events-tutorial/ */
  }

  bind(bindingContext: any, overrideContext: any) {
    // this.$parent = overrideContext.parentOverrideContext.bindingContext;
    this.$parent = bindingContext;
  }

  detached() {
    //$(this.element).find('select').select2('destroy');
    this._select.select2('destroy');
  }
}
