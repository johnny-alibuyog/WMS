import { autoinject, bindable, bindingMode, customAttribute } from "aurelia-framework";
//import 'select2/css/select2.min.css!'
import $ from 'jquery';
import 'select2/dist/css/select2.min.css!';
//import { Element } from "protractor";
// NOTE: a better implementation of this is in common/controls/select2

//declare var $;

@autoinject()
@customAttribute("select2")
export class Select2Attribute {
  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public value: any;

  @bindable({ defaultBindingMode: bindingMode.oneTime })
  public allowClear: boolean = false;

  private select: any;

  private element: Element;

  constructor(element: Element) { }

  attached() {
    let clear = ((this.allowClear as any) === "true");

    if (this.select) {
      return;
    }

    this.select = $(this.element).select2({
      allowClear: clear,
      placeholder: "Select an option..."
    });

    this.select.on("change", (event: any, options: any) => {
      if (event.originalEvent) {
        return;
      }
      if (this.value != (event.target).value) {
        this.value = (event.target).value;
      }
    });
  }

  valueChanged(newVal, oldVal) {
    if (this.select)
      this.select.trigger("change");
  }

  detached() {
    if (this.select) {
      this.select.off("change");
      this.select.select2("destroy");
    }
    this.select = null;
  }
}
