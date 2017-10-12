System.config({
  defaultJSExtensions: true,
  transpiler: false,
  paths: {
    "*": "dist/*",
    "github:*": "jspm_packages/github/*",
    "npm:*": "jspm_packages/npm/*"
  },
  meta: {
    "bootstrap": {
      "deps": [
        "jquery"
      ]
    }
  },
  map: {
    "aurelia-animator-css": "npm:aurelia-animator-css@1.0.2",
    "aurelia-bootstrapper": "npm:aurelia-bootstrapper@1.0.1",
    "aurelia-dialog": "npm:aurelia-dialog@1.0.0-rc.1.0.3",
    "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
    "aurelia-fetch-client": "npm:aurelia-fetch-client@1.1.2",
    "aurelia-framework": "npm:aurelia-framework@1.1.4",
    "aurelia-history-browser": "npm:aurelia-history-browser@1.0.0",
    "aurelia-loader-default": "npm:aurelia-loader-default@1.0.2",
    "aurelia-logging-console": "npm:aurelia-logging-console@1.0.0",
    "aurelia-pal-browser": "npm:aurelia-pal-browser@1.2.1",
    "aurelia-path": "npm:aurelia-path@1.1.1",
    "aurelia-polyfills": "npm:aurelia-polyfills@1.2.2",
    "aurelia-router": "npm:aurelia-router@1.3.0",
    "aurelia-templating-binding": "npm:aurelia-templating-binding@1.3.0",
    "aurelia-templating-resources": "npm:aurelia-templating-resources@1.4.0",
    "aurelia-templating-router": "npm:aurelia-templating-router@1.1.0",
    "aurelia-validation": "npm:aurelia-validation@1.1.1",
    "bootstrap": "github:twbs/bootstrap@3.3.7",
    "fetch": "github:github/fetch@1.1.1",
    "font-awesome": "npm:font-awesome@4.6.3",
    "moment": "npm:moment@2.18.1",
    "numeral": "npm:numeral@1.5.6",
    "pdfmake": "github:bpampuch/pdfmake@0.1.31",
    "text": "github:systemjs/plugin-text@0.0.8",
    "github:jspm/nodelibs-assert@0.1.0": {
      "assert": "npm:assert@1.4.1"
    },
    "github:jspm/nodelibs-buffer@0.1.1": {
      "buffer": "npm:buffer@5.0.6"
    },
    "github:jspm/nodelibs-process@0.1.2": {
      "process": "npm:process@0.11.10"
    },
    "github:jspm/nodelibs-util@0.1.0": {
      "util": "npm:util@0.10.3"
    },
    "github:jspm/nodelibs-vm@0.1.0": {
      "vm-browserify": "npm:vm-browserify@0.0.4"
    },
    "github:twbs/bootstrap@3.3.7": {
      "jquery": "npm:jquery@3.2.1"
    },
    "npm:assert@1.4.1": {
      "assert": "github:jspm/nodelibs-assert@0.1.0",
      "buffer": "github:jspm/nodelibs-buffer@0.1.1",
      "process": "github:jspm/nodelibs-process@0.1.2",
      "util": "npm:util@0.10.3"
    },
    "npm:aurelia-animator-css@1.0.2": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:aurelia-binding@1.2.1": {
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0"
    },
    "npm:aurelia-bootstrapper@1.0.1": {
      "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
      "aurelia-framework": "npm:aurelia-framework@1.1.4",
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-history-browser": "npm:aurelia-history-browser@1.0.0",
      "aurelia-loader-default": "npm:aurelia-loader-default@1.0.2",
      "aurelia-logging-console": "npm:aurelia-logging-console@1.0.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-pal-browser": "npm:aurelia-pal-browser@1.2.1",
      "aurelia-polyfills": "npm:aurelia-polyfills@1.2.2",
      "aurelia-router": "npm:aurelia-router@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2",
      "aurelia-templating-binding": "npm:aurelia-templating-binding@1.3.0",
      "aurelia-templating-resources": "npm:aurelia-templating-resources@1.4.0",
      "aurelia-templating-router": "npm:aurelia-templating-router@1.1.0"
    },
    "npm:aurelia-dependency-injection@1.3.1": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-dialog@1.0.0-rc.1.0.3": {
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:aurelia-event-aggregator@1.0.1": {
      "aurelia-logging": "npm:aurelia-logging@1.3.1"
    },
    "npm:aurelia-framework@1.1.4": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:aurelia-history-browser@1.0.0": {
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-loader-default@1.0.2": {
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-loader@1.0.0": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-path": "npm:aurelia-path@1.1.1"
    },
    "npm:aurelia-logging-console@1.0.0": {
      "aurelia-logging": "npm:aurelia-logging@1.3.1"
    },
    "npm:aurelia-metadata@1.0.3": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-pal-browser@1.2.1": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-polyfills@1.2.2": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-route-recognizer@1.1.0": {
      "aurelia-path": "npm:aurelia-path@1.1.1"
    },
    "npm:aurelia-router@1.3.0": {
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-route-recognizer": "npm:aurelia-route-recognizer@1.1.0"
    },
    "npm:aurelia-task-queue@1.2.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-templating-binding@1.3.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:aurelia-templating-resources@1.4.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:aurelia-templating-router@1.1.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-router": "npm:aurelia-router@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:aurelia-templating@1.4.2": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0"
    },
    "npm:aurelia-validation@1.1.1": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.1",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.2"
    },
    "npm:buffer@5.0.6": {
      "base64-js": "npm:base64-js@1.2.1",
      "ieee754": "npm:ieee754@1.1.8"
    },
    "npm:font-awesome@4.6.3": {
      "css": "github:systemjs/plugin-css@0.1.35"
    },
    "npm:inherits@2.0.1": {
      "util": "github:jspm/nodelibs-util@0.1.0"
    },
    "npm:numeral@1.5.6": {
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:process@0.11.10": {
      "assert": "github:jspm/nodelibs-assert@0.1.0",
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "vm": "github:jspm/nodelibs-vm@0.1.0"
    },
    "npm:util@0.10.3": {
      "inherits": "npm:inherits@2.0.1",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:vm-browserify@0.0.4": {
      "indexof": "npm:indexof@0.0.1"
    }
  },
  bundles: {
    "aurelia.js": [
      "github:github/fetch@1.1.1.js",
      "github:github/fetch@1.1.1/fetch.js",
      "github:twbs/bootstrap@3.3.7.js",
      "github:twbs/bootstrap@3.3.7/css/bootstrap.css!github:systemjs/plugin-text@0.0.8.js",
      "github:twbs/bootstrap@3.3.7/js/bootstrap.js",
      "npm:aurelia-animator-css@1.0.2.js",
      "npm:aurelia-animator-css@1.0.2/aurelia-animator-css.js",
      "npm:aurelia-binding@1.2.1.js",
      "npm:aurelia-binding@1.2.1/aurelia-binding.js",
      "npm:aurelia-bootstrapper@1.0.1.js",
      "npm:aurelia-bootstrapper@1.0.1/aurelia-bootstrapper.js",
      "npm:aurelia-dependency-injection@1.3.1.js",
      "npm:aurelia-dependency-injection@1.3.1/aurelia-dependency-injection.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/attach-focus.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/aurelia-dialog.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/dialog-cancel-error.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/dialog-configuration.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/dialog-controller.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/dialog-renderer.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/dialog-service.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/dialog-settings.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/lifecycle.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/renderer.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/ux-dialog-body.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/ux-dialog-footer.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/ux-dialog-header.js",
      "npm:aurelia-dialog@1.0.0-rc.1.0.3/ux-dialog.js",
      "npm:aurelia-event-aggregator@1.0.1.js",
      "npm:aurelia-event-aggregator@1.0.1/aurelia-event-aggregator.js",
      "npm:aurelia-fetch-client@1.1.2.js",
      "npm:aurelia-fetch-client@1.1.2/aurelia-fetch-client.js",
      "npm:aurelia-framework@1.1.4.js",
      "npm:aurelia-framework@1.1.4/aurelia-framework.js",
      "npm:aurelia-history-browser@1.0.0.js",
      "npm:aurelia-history-browser@1.0.0/aurelia-history-browser.js",
      "npm:aurelia-history@1.0.0.js",
      "npm:aurelia-history@1.0.0/aurelia-history.js",
      "npm:aurelia-loader-default@1.0.2.js",
      "npm:aurelia-loader-default@1.0.2/aurelia-loader-default.js",
      "npm:aurelia-loader@1.0.0.js",
      "npm:aurelia-loader@1.0.0/aurelia-loader.js",
      "npm:aurelia-logging-console@1.0.0.js",
      "npm:aurelia-logging-console@1.0.0/aurelia-logging-console.js",
      "npm:aurelia-logging@1.3.1.js",
      "npm:aurelia-logging@1.3.1/aurelia-logging.js",
      "npm:aurelia-metadata@1.0.3.js",
      "npm:aurelia-metadata@1.0.3/aurelia-metadata.js",
      "npm:aurelia-pal-browser@1.2.1.js",
      "npm:aurelia-pal-browser@1.2.1/aurelia-pal-browser.js",
      "npm:aurelia-pal@1.3.0.js",
      "npm:aurelia-pal@1.3.0/aurelia-pal.js",
      "npm:aurelia-path@1.1.1.js",
      "npm:aurelia-path@1.1.1/aurelia-path.js",
      "npm:aurelia-polyfills@1.2.2.js",
      "npm:aurelia-polyfills@1.2.2/aurelia-polyfills.js",
      "npm:aurelia-route-recognizer@1.1.0.js",
      "npm:aurelia-route-recognizer@1.1.0/aurelia-route-recognizer.js",
      "npm:aurelia-router@1.3.0.js",
      "npm:aurelia-router@1.3.0/aurelia-router.js",
      "npm:aurelia-task-queue@1.2.0.js",
      "npm:aurelia-task-queue@1.2.0/aurelia-task-queue.js",
      "npm:aurelia-templating-binding@1.3.0.js",
      "npm:aurelia-templating-binding@1.3.0/aurelia-templating-binding.js",
      "npm:aurelia-templating-resources@1.4.0.js",
      "npm:aurelia-templating-resources@1.4.0/abstract-repeater.js",
      "npm:aurelia-templating-resources@1.4.0/analyze-view-factory.js",
      "npm:aurelia-templating-resources@1.4.0/array-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.4.0/attr-binding-behavior.js",
      "npm:aurelia-templating-resources@1.4.0/aurelia-hide-style.js",
      "npm:aurelia-templating-resources@1.4.0/aurelia-templating-resources.js",
      "npm:aurelia-templating-resources@1.4.0/binding-mode-behaviors.js",
      "npm:aurelia-templating-resources@1.4.0/binding-signaler.js",
      "npm:aurelia-templating-resources@1.4.0/compose.js",
      "npm:aurelia-templating-resources@1.4.0/css-resource.js",
      "npm:aurelia-templating-resources@1.4.0/debounce-binding-behavior.js",
      "npm:aurelia-templating-resources@1.4.0/dynamic-element.js",
      "npm:aurelia-templating-resources@1.4.0/focus.js",
      "npm:aurelia-templating-resources@1.4.0/hide.js",
      "npm:aurelia-templating-resources@1.4.0/html-resource-plugin.js",
      "npm:aurelia-templating-resources@1.4.0/html-sanitizer.js",
      "npm:aurelia-templating-resources@1.4.0/if.js",
      "npm:aurelia-templating-resources@1.4.0/map-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.4.0/null-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.4.0/number-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.4.0/repeat-strategy-locator.js",
      "npm:aurelia-templating-resources@1.4.0/repeat-utilities.js",
      "npm:aurelia-templating-resources@1.4.0/repeat.js",
      "npm:aurelia-templating-resources@1.4.0/replaceable.js",
      "npm:aurelia-templating-resources@1.4.0/sanitize-html.js",
      "npm:aurelia-templating-resources@1.4.0/self-binding-behavior.js",
      "npm:aurelia-templating-resources@1.4.0/set-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.4.0/show.js",
      "npm:aurelia-templating-resources@1.4.0/signal-binding-behavior.js",
      "npm:aurelia-templating-resources@1.4.0/throttle-binding-behavior.js",
      "npm:aurelia-templating-resources@1.4.0/update-trigger-binding-behavior.js",
      "npm:aurelia-templating-resources@1.4.0/with.js",
      "npm:aurelia-templating-router@1.1.0.js",
      "npm:aurelia-templating-router@1.1.0/aurelia-templating-router.js",
      "npm:aurelia-templating-router@1.1.0/route-href.js",
      "npm:aurelia-templating-router@1.1.0/route-loader.js",
      "npm:aurelia-templating-router@1.1.0/router-view.js",
      "npm:aurelia-templating@1.4.2.js",
      "npm:aurelia-templating@1.4.2/aurelia-templating.js",
      "npm:aurelia-validation@1.1.1.js",
      "npm:aurelia-validation@1.1.1/aurelia-validation.js",
      "npm:aurelia-validation@1.1.1/get-target-dom-element.js",
      "npm:aurelia-validation@1.1.1/implementation/expression-visitor.js",
      "npm:aurelia-validation@1.1.1/implementation/rules.js",
      "npm:aurelia-validation@1.1.1/implementation/standard-validator.js",
      "npm:aurelia-validation@1.1.1/implementation/validation-message-parser.js",
      "npm:aurelia-validation@1.1.1/implementation/validation-messages.js",
      "npm:aurelia-validation@1.1.1/implementation/validation-rules.js",
      "npm:aurelia-validation@1.1.1/property-accessor-parser.js",
      "npm:aurelia-validation@1.1.1/property-info.js",
      "npm:aurelia-validation@1.1.1/util.js",
      "npm:aurelia-validation@1.1.1/validate-binding-behavior-base.js",
      "npm:aurelia-validation@1.1.1/validate-binding-behavior.js",
      "npm:aurelia-validation@1.1.1/validate-event.js",
      "npm:aurelia-validation@1.1.1/validate-result.js",
      "npm:aurelia-validation@1.1.1/validate-trigger.js",
      "npm:aurelia-validation@1.1.1/validation-controller-factory.js",
      "npm:aurelia-validation@1.1.1/validation-controller.js",
      "npm:aurelia-validation@1.1.1/validation-errors-custom-attribute.js",
      "npm:aurelia-validation@1.1.1/validation-renderer-custom-attribute.js",
      "npm:aurelia-validation@1.1.1/validator.js",
      "npm:jquery@3.2.1.js",
      "npm:jquery@3.2.1/dist/jquery.js",
      "npm:moment@2.18.1.js",
      "npm:moment@2.18.1/moment.js",
      "npm:numeral@1.5.6.js",
      "npm:numeral@1.5.6/numeral.js"
    ],
    "app-build.js": [
      "admin/branches/branch-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/branches/branch-create.js",
      "admin/branches/branch-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/branches/branch-page.js",
      "admin/customers/customer-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/customers/customer-create.js",
      "admin/customers/customer-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/customers/customer-page.js",
      "admin/index.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/index.js",
      "admin/payment-types/payment-type-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/payment-types/payment-type-create.js",
      "admin/payment-types/payment-type-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/payment-types/payment-type-page.js",
      "admin/product-categories/product-category-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/product-categories/product-category-create.js",
      "admin/product-categories/product-category-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/product-categories/product-category-page.js",
      "admin/suppliers/supplier-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/suppliers/supplier-create.js",
      "admin/suppliers/supplier-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/suppliers/supplier-page.js",
      "admin/unit-of-measures/unit-of-measure-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/unit-of-measures/unit-of-measure-create.js",
      "admin/unit-of-measures/unit-of-measure-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/unit-of-measures/unit-of-measure-page.js",
      "admin/users/user-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/users/user-create.js",
      "admin/users/user-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/users/user-page.js",
      "app-config.js",
      "common/attributes/blur-image.js",
      "common/attributes/datepicker.js",
      "common/attributes/numeric-value.js",
      "common/attributes/on-enter.js",
      "common/attributes/on-escape.js",
      "common/controls/address-display.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/address-input.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/checkbox.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/contact-display.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/contact-input.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/notification-service.js",
      "common/controls/notification.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/notification.js",
      "common/controls/pager.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/pager.js",
      "common/controls/person-display.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/person-input.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/report-viewer.html!github:systemjs/plugin-text@0.0.8.js",
      "common/controls/report-viewer.js",
      "common/controls/validations/bootstrap-form-renderer.js",
      "common/converters/age-value-converter.js",
      "common/converters/authorize-value-converter.js",
      "common/converters/date-format-value-converter.js",
      "common/converters/date-input-format-value-converter.js",
      "common/converters/filter-value-converter.js",
      "common/converters/instruction-filter-vaule-converter.js",
      "common/converters/lookup-id-to-name-value-converter.js",
      "common/converters/lookup-to-id-value-converter.js",
      "common/converters/lookup-to-name-value-converter.js",
      "common/converters/number-format-value-converter.js",
      "common/converters/numeric-value-converter.js",
      "common/converters/object-keys-value-converter.js",
      "common/converters/object-values-value-converter.js",
      "common/converters/percentage-value-converter.js",
      "common/converters/relative-date-value-converter.js",
      "common/converters/sort-value-converter.js",
      "common/converters/to-id-value-converter.js",
      "common/custom_types/dictionary.js",
      "common/custom_types/key-value-pair.js",
      "common/custom_types/lookup.js",
      "common/global-resources.js",
      "common/matchers/lookup-matcher.js",
      "common/models/address.js",
      "common/models/branch.js",
      "common/models/contact.js",
      "common/models/currency.js",
      "common/models/customer.js",
      "common/models/inventory.js",
      "common/models/measure.js",
      "common/models/money.js",
      "common/models/order.js",
      "common/models/paging.js",
      "common/models/payment-type.js",
      "common/models/person.js",
      "common/models/pricing.js",
      "common/models/product-category.js",
      "common/models/product.js",
      "common/models/purchase-order.js",
      "common/models/reports.js",
      "common/models/return-reason.js",
      "common/models/return.js",
      "common/models/role.js",
      "common/models/stage-definition.js",
      "common/models/supplier.js",
      "common/models/tenant.js",
      "common/models/unit-of-measure.js",
      "common/models/user.js",
      "common/utils/ensure-numeric.js",
      "dashboard/index.html!github:systemjs/plugin-text@0.0.8.js",
      "dashboard/index.js",
      "dashboard/new-customer-order.html!github:systemjs/plugin-text@0.0.8.js",
      "dashboard/new-customer-order.js",
      "dashboard/new-purchase-order.html!github:systemjs/plugin-text@0.0.8.js",
      "dashboard/new-purchase-order.js",
      "dashboard/pending-page.html!github:systemjs/plugin-text@0.0.8.js",
      "dashboard/pending-page.js",
      "main.js",
      "orders/active-order-page.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/active-order-page.js",
      "orders/index.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/index.js",
      "orders/invoice-report.js",
      "orders/order-create.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/order-create.js",
      "orders/order-item-page.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/order-item-page.js",
      "orders/order-page.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/order-page.js",
      "orders/order-payment-page.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/order-payment-page.js",
      "orders/order-return-page.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/order-return-page.js",
      "orders/state-verification-value-converter.js",
      "products/discontinued-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/discontinued-page.js",
      "products/index.html!github:systemjs/plugin-text@0.0.8.js",
      "products/index.js",
      "products/inventory-level-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/inventory-level-page.js",
      "products/needs-reordering-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/needs-reordering-page.js",
      "products/product-create.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-create.js",
      "products/product-inventory.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-inventory.js",
      "products/product-order-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-order-page.js",
      "products/product-order-return-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-order-return-page.js",
      "products/product-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-page.js",
      "products/product-purchase-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-purchase-page.js",
      "products/product-return-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-return-page.js",
      "products/product-uom.css!github:systemjs/plugin-text@0.0.8.js",
      "products/product-uom.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-uom.js",
      "products/value-converters.js",
      "purchases/active-purchase-order-page.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/active-purchase-order-page.js",
      "purchases/index.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/index.js",
      "purchases/purchase-order-create.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/purchase-order-create.js",
      "purchases/purchase-order-item-page.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/purchase-order-item-page.js",
      "purchases/purchase-order-page.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/purchase-order-page.js",
      "purchases/purchase-order-payment-page.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/purchase-order-payment-page.js",
      "purchases/purchase-order-receipt-page.html!github:systemjs/plugin-text@0.0.8.js",
      "purchases/purchase-order-receipt-page.js",
      "purchases/state-verification-value-converter.js",
      "purchases/voucher-report.js",
      "report-center/customer-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/customer-report-page.js",
      "report-center/customer-report.js",
      "report-center/index.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/index.js",
      "report-center/order-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/order-report-page.js",
      "report-center/order-report.js",
      "report-center/product-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/product-report-page.js",
      "report-center/product-report.js",
      "report-center/report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/report-page.js",
      "report-center/reports.js",
      "report-center/supplier-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/supplier-report-page.js",
      "report-center/supplier-report.js",
      "report-center/unit-of-measure-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report-center/unit-of-measure-report-page.js",
      "report-center/unit-of-measure-report.js",
      "returns/index.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/index.js",
      "returns/return-create.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/return-create.js",
      "returns/return-item-page.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/return-item-page.js",
      "returns/return-page.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/return-page.js",
      "returns/returns-by-customer-page.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/returns-by-customer-page.js",
      "returns/returns-by-product-page.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/returns-by-product-page.js",
      "returns/returns-by-reason-page.html!github:systemjs/plugin-text@0.0.8.js",
      "returns/returns-by-reason-page.js",
      "returns/state-verification-value-converters.js",
      "routes/index.html!github:systemjs/plugin-text@0.0.8.js",
      "routes/index.js",
      "routes/route-page.html!github:systemjs/plugin-text@0.0.8.js",
      "routes/route-page.js",
      "services/auth-service.js",
      "services/branch-service.js",
      "services/customer-service.js",
      "services/formaters.js",
      "services/http-client-facade.js",
      "services/order-service.js",
      "services/payment-type-service.js",
      "services/pricing-service.js",
      "services/product-category-service.js",
      "services/product-service.js",
      "services/purchase-order-service.js",
      "services/report-builder.js",
      "services/return-reason-service.js",
      "services/return-service.js",
      "services/service-api.js",
      "services/service-base.js",
      "services/session-data.js",
      "services/stage-guard.js",
      "services/supplier-service.js",
      "services/unit-of-measure.js",
      "services/user-service.js",
      "shell/breadcrumbs.html!github:systemjs/plugin-text@0.0.8.js",
      "shell/breadcrumbs.js",
      "shell/nav-bar.html!github:systemjs/plugin-text@0.0.8.js",
      "shell/nav-bar.js",
      "shell/shell.html!github:systemjs/plugin-text@0.0.8.js",
      "shell/shell.js",
      "shell/side-bar.html!github:systemjs/plugin-text@0.0.8.js",
      "shell/side-bar.js",
      "users/login.html!github:systemjs/plugin-text@0.0.8.js",
      "users/login.js",
      "users/override.html!github:systemjs/plugin-text@0.0.8.js",
      "users/override.js",
      "users/profiles/address.html!github:systemjs/plugin-text@0.0.8.js",
      "users/profiles/address.js",
      "users/profiles/change-password.html!github:systemjs/plugin-text@0.0.8.js",
      "users/profiles/change-password.js",
      "users/profiles/index.html!github:systemjs/plugin-text@0.0.8.js",
      "users/profiles/index.js",
      "users/profiles/info.html!github:systemjs/plugin-text@0.0.8.js",
      "users/profiles/info.js"
    ]
  },
  depCache: {
    "admin/branches/branch-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "aurelia-validation",
      "../../common/controls/validations/bootstrap-form-renderer"
    ],
    "admin/branches/branch-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./branch-create",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "admin/customers/customer-create.js": [
      "aurelia-router",
      "aurelia-framework",
      "../../services/service-api",
      "../../common/controls/notification-service"
    ],
    "admin/customers/customer-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "admin/index.js": [
      "../common/models/role"
    ],
    "admin/payment-types/payment-type-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../../services/payment-type-service",
      "../../common/controls/notification-service"
    ],
    "admin/payment-types/payment-type-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./payment-type-create",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "admin/product-categories/product-category-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../../services/service-api",
      "../../common/controls/notification-service"
    ],
    "admin/product-categories/product-category-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./product-category-create",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "admin/suppliers/supplier-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../../services/service-api",
      "../../common/controls/notification-service"
    ],
    "admin/suppliers/supplier-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./supplier-create",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "admin/unit-of-measures/unit-of-measure-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../../services/service-api",
      "../../common/controls/notification-service"
    ],
    "admin/unit-of-measures/unit-of-measure-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./unit-of-measure-create",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "admin/users/user-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../../common/models/role",
      "../../services/service-api",
      "../../common/controls/notification-service"
    ],
    "admin/users/user-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./user-create",
      "../../common/models/role",
      "../../services/service-api",
      "../../common/controls/notification-service",
      "../../common/models/paging"
    ],
    "common/attributes/blur-image.js": [
      "aurelia-framework"
    ],
    "common/attributes/datepicker.js": [
      "aurelia-framework"
    ],
    "common/attributes/numeric-value.js": [
      "aurelia-framework"
    ],
    "common/attributes/on-enter.js": [
      "aurelia-framework"
    ],
    "common/attributes/on-escape.js": [
      "aurelia-framework"
    ],
    "common/controls/notification-service.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./notification"
    ],
    "common/controls/notification.js": [
      "aurelia-framework",
      "aurelia-dialog"
    ],
    "common/controls/pager.js": [
      "aurelia-framework",
      "../../app-config"
    ],
    "common/controls/report-viewer.js": [
      "aurelia-framework",
      "aurelia-dialog"
    ],
    "common/converters/age-value-converter.js": [
      "moment"
    ],
    "common/converters/authorize-value-converter.js": [
      "aurelia-framework",
      "../../services/auth-service"
    ],
    "common/converters/date-format-value-converter.js": [
      "moment",
      "../../app-config"
    ],
    "common/converters/date-input-format-value-converter.js": [
      "moment"
    ],
    "common/converters/number-format-value-converter.js": [
      "numeral"
    ],
    "common/converters/numeric-value-converter.js": [
      "../utils/ensure-numeric"
    ],
    "common/converters/relative-date-value-converter.js": [
      "moment"
    ],
    "common/converters/sort-value-converter.js": [
      "moment"
    ],
    "common/models/paging.js": [
      "aurelia-framework",
      "../../app-config"
    ],
    "dashboard/index.js": [
      "../common/models/role"
    ],
    "main.js": [
      "./services/auth-service",
      "bootstrap"
    ],
    "orders/active-order-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "aurelia-path",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "orders/index.js": [
      "../common/models/order",
      "../common/models/role"
    ],
    "orders/invoice-report.js": [
      "../services/report-builder",
      "../services/formaters",
      "../services/auth-service",
      "aurelia-framework"
    ],
    "orders/order-create.js": [
      "aurelia-router",
      "aurelia-framework",
      "aurelia-event-aggregator",
      "../common/models/order",
      "../services/service-api",
      "../users/override",
      "aurelia-dialog",
      "../common/controls/notification-service",
      "./invoice-report",
      "../services/auth-service",
      "../common/models/pricing",
      "../common/models/role"
    ],
    "orders/order-item-page.js": [
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/models/order",
      "../common/models/product",
      "aurelia-framework",
      "../common/controls/notification-service",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/models/measure"
    ],
    "orders/order-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../common/models/order",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "orders/order-payment-page.js": [
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/models/order",
      "aurelia-framework",
      "../common/controls/notification-service",
      "../services/service-api",
      "../common/utils/ensure-numeric"
    ],
    "orders/order-return-page.js": [
      "aurelia-framework",
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/controls/notification-service",
      "../services/service-api",
      "../common/utils/ensure-numeric"
    ],
    "orders/state-verification-value-converter.js": [
      "../common/models/order"
    ],
    "products/discontinued-page.js": [
      "../common/models/paging",
      "aurelia-router",
      "../common/controls/notification-service",
      "../services/service-api",
      "aurelia-framework"
    ],
    "products/index.js": [
      "../common/models/role"
    ],
    "products/inventory-level-page.js": [
      "aurelia-framework",
      "aurelia-router",
      "../common/models/paging",
      "../services/service-api"
    ],
    "products/needs-reordering-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../services/session-data",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "products/product-create.js": [
      "../services/auth-service",
      "../common/controls/notification-service",
      "aurelia-router",
      "../services/service-api",
      "aurelia-framework",
      "../common/models/role"
    ],
    "products/product-inventory.js": [
      "aurelia-framework"
    ],
    "products/product-order-page.js": [
      "../common/models/paging",
      "aurelia-framework",
      "aurelia-router",
      "../services/service-api",
      "aurelia-path"
    ],
    "products/product-order-return-page.js": [
      "../common/models/paging",
      "aurelia-framework",
      "aurelia-router",
      "../services/service-api",
      "aurelia-path"
    ],
    "products/product-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "products/product-purchase-page.js": [
      "../common/models/paging",
      "aurelia-framework",
      "aurelia-router",
      "../services/service-api",
      "aurelia-path"
    ],
    "products/product-return-page.js": [
      "../common/models/paging",
      "aurelia-framework",
      "aurelia-router",
      "../services/service-api",
      "aurelia-path"
    ],
    "products/product-uom.js": [
      "aurelia-framework",
      "aurelia-router",
      "../services/service-api"
    ],
    "purchases/active-purchase-order-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "aurelia-path",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "purchases/index.js": [
      "../common/models/purchase-order",
      "../common/models/role"
    ],
    "purchases/purchase-order-create.js": [
      "aurelia-event-aggregator",
      "../common/models/purchase-order",
      "../services/auth-service",
      "../common/controls/notification-service",
      "aurelia-router",
      "../services/service-api",
      "./voucher-report",
      "aurelia-framework",
      "../services/formaters",
      "../common/models/pricing",
      "../common/models/role"
    ],
    "purchases/purchase-order-item-page.js": [
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/models/product",
      "../common/models/purchase-order",
      "aurelia-framework",
      "../common/controls/notification-service",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/models/measure",
      "../common/models/pricing"
    ],
    "purchases/purchase-order-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../common/models/purchase-order",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "purchases/purchase-order-payment-page.js": [
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/models/purchase-order",
      "aurelia-framework",
      "../common/controls/notification-service",
      "../services/service-api",
      "../common/utils/ensure-numeric"
    ],
    "purchases/purchase-order-receipt-page.js": [
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/models/product",
      "../common/models/purchase-order",
      "aurelia-framework",
      "../common/controls/notification-service",
      "../services/service-api"
    ],
    "purchases/state-verification-value-converter.js": [
      "../common/models/purchase-order"
    ],
    "purchases/voucher-report.js": [
      "../services/report-builder",
      "../services/formaters",
      "../services/auth-service",
      "aurelia-framework"
    ],
    "report-center/customer-report-page.js": [
      "aurelia-framework",
      "./customer-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report-center/customer-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "report-center/index.js": [
      "../common/models/role"
    ],
    "report-center/order-report-page.js": [
      "aurelia-framework",
      "./order-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report-center/order-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder",
      "../common/models/order"
    ],
    "report-center/product-report-page.js": [
      "aurelia-framework",
      "./product-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report-center/product-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "report-center/report-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api"
    ],
    "report-center/reports.js": [
      "aurelia-framework",
      "../services/report-builder"
    ],
    "report-center/supplier-report-page.js": [
      "aurelia-framework",
      "./supplier-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report-center/supplier-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "report-center/unit-of-measure-report-page.js": [
      "aurelia-framework",
      "./unit-of-measure-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report-center/unit-of-measure-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "returns/index.js": [
      "../common/models/role"
    ],
    "returns/return-create.js": [
      "aurelia-event-aggregator",
      "../common/models/return",
      "../common/controls/notification-service",
      "aurelia-router",
      "../services/service-api",
      "aurelia-framework"
    ],
    "returns/return-item-page.js": [
      "aurelia-event-aggregator",
      "../common/models/paging",
      "../common/models/product",
      "../common/models/return",
      "aurelia-framework",
      "../common/controls/notification-service",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/models/measure",
      "../common/models/pricing"
    ],
    "returns/return-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "returns/returns-by-customer-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "returns/returns-by-product-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "returns/returns-by-reason-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "services/auth-service.js": [
      "aurelia-framework",
      "./http-client-facade",
      "../common/controls/notification-service"
    ],
    "services/branch-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/customer-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/formaters.js": [
      "moment",
      "numeral",
      "../app-config"
    ],
    "services/http-client-facade.js": [
      "aurelia-fetch-client",
      "../app-config",
      "aurelia-framework",
      "fetch"
    ],
    "services/order-service.js": [
      "../common/models/measure",
      "./auth-service",
      "./http-client-facade",
      "./service-base",
      "aurelia-framework"
    ],
    "services/payment-type-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/pricing-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/product-category-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/product-service.js": [
      "./http-client-facade",
      "./service-base",
      "aurelia-framework"
    ],
    "services/purchase-order-service.js": [
      "./auth-service",
      "./http-client-facade",
      "./service-base",
      "aurelia-framework"
    ],
    "services/report-builder.js": [
      "aurelia-framework",
      "aurelia-dialog"
    ],
    "services/return-reason-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/return-service.js": [
      "aurelia-framework",
      "./service-base",
      "./auth-service",
      "./http-client-facade"
    ],
    "services/service-api.js": [
      "aurelia-framework",
      "./auth-service",
      "./branch-service",
      "./customer-service",
      "./payment-type-service",
      "./pricing-service",
      "./product-category-service",
      "./product-service",
      "./purchase-order-service",
      "./return-reason-service",
      "./return-service",
      "./order-service",
      "./supplier-service",
      "./user-service",
      "./unit-of-measure"
    ],
    "services/stage-guard.js": [
      "aurelia-framework",
      "./auth-service"
    ],
    "services/supplier-service.js": [
      "./http-client-facade",
      "./service-base",
      "aurelia-framework"
    ],
    "services/unit-of-measure.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/user-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "shell/breadcrumbs.js": [
      "aurelia-framework",
      "aurelia-router"
    ],
    "shell/nav-bar.js": [
      "aurelia-framework",
      "aurelia-router",
      "../services/auth-service"
    ],
    "shell/shell.js": [
      "../services/auth-service",
      "../common/controls/notification-service",
      "aurelia-framework",
      "../common/models/role"
    ],
    "shell/side-bar.js": [
      "aurelia-framework",
      "aurelia-router"
    ],
    "users/login.js": [
      "aurelia-framework",
      "../services/auth-service"
    ],
    "users/override.js": [
      "../services/auth-service",
      "aurelia-dialog",
      "../common/controls/notification-service",
      "aurelia-framework"
    ],
    "users/profiles/address.js": [
      "../../services/auth-service",
      "../../common/controls/notification-service",
      "../../services/service-api",
      "aurelia-framework"
    ],
    "users/profiles/index.js": [
      "../../common/models/role"
    ],
    "users/profiles/info.js": [
      "../../services/auth-service",
      "../../common/controls/notification-service",
      "../../services/service-api",
      "aurelia-framework"
    ]
  }
});