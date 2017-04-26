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
    "aurelia-animator-css": "npm:aurelia-animator-css@1.0.1",
    "aurelia-bootstrapper": "npm:aurelia-bootstrapper@1.0.1",
    "aurelia-dialog": "npm:aurelia-dialog@1.0.0-beta.3.0.1",
    "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
    "aurelia-fetch-client": "npm:aurelia-fetch-client@1.1.1",
    "aurelia-framework": "npm:aurelia-framework@1.1.0",
    "aurelia-history-browser": "npm:aurelia-history-browser@1.0.0",
    "aurelia-loader-default": "npm:aurelia-loader-default@1.0.1",
    "aurelia-logging-console": "npm:aurelia-logging-console@1.0.0",
    "aurelia-pal-browser": "npm:aurelia-pal-browser@1.1.0",
    "aurelia-path": "npm:aurelia-path@1.1.1",
    "aurelia-polyfills": "npm:aurelia-polyfills@1.2.0",
    "aurelia-router": "npm:aurelia-router@1.2.1",
    "aurelia-templating-binding": "npm:aurelia-templating-binding@1.3.0",
    "aurelia-templating-resources": "npm:aurelia-templating-resources@1.3.1",
    "aurelia-templating-router": "npm:aurelia-templating-router@1.1.0",
    "aurelia-validation": "npm:aurelia-validation@0.12.5",
    "bootstrap": "github:twbs/bootstrap@3.3.7",
    "fetch": "github:github/fetch@1.1.1",
    "font-awesome": "npm:font-awesome@4.6.3",
    "moment": "npm:moment@2.17.1",
    "numeral": "npm:numeral@1.5.6",
    "pdfmake": "github:bpampuch/pdfmake@0.1.26",
    "text": "github:systemjs/plugin-text@0.0.8",
    "github:jspm/nodelibs-assert@0.1.0": {
      "assert": "npm:assert@1.4.1"
    },
    "github:jspm/nodelibs-buffer@0.1.0": {
      "buffer": "npm:buffer@3.6.0"
    },
    "github:jspm/nodelibs-process@0.1.2": {
      "process": "npm:process@0.11.9"
    },
    "github:jspm/nodelibs-util@0.1.0": {
      "util": "npm:util@0.10.3"
    },
    "github:jspm/nodelibs-vm@0.1.0": {
      "vm-browserify": "npm:vm-browserify@0.0.4"
    },
    "github:twbs/bootstrap@3.3.7": {
      "jquery": "npm:jquery@3.1.1"
    },
    "npm:assert@1.4.1": {
      "assert": "github:jspm/nodelibs-assert@0.1.0",
      "buffer": "github:jspm/nodelibs-buffer@0.1.0",
      "process": "github:jspm/nodelibs-process@0.1.2",
      "util": "npm:util@0.10.3"
    },
    "npm:aurelia-animator-css@1.0.1": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:aurelia-binding@1.2.0": {
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0"
    },
    "npm:aurelia-bootstrapper@1.0.1": {
      "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
      "aurelia-framework": "npm:aurelia-framework@1.1.0",
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-history-browser": "npm:aurelia-history-browser@1.0.0",
      "aurelia-loader-default": "npm:aurelia-loader-default@1.0.1",
      "aurelia-logging-console": "npm:aurelia-logging-console@1.0.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-pal-browser": "npm:aurelia-pal-browser@1.1.0",
      "aurelia-polyfills": "npm:aurelia-polyfills@1.2.0",
      "aurelia-router": "npm:aurelia-router@1.2.1",
      "aurelia-templating": "npm:aurelia-templating@1.3.0",
      "aurelia-templating-binding": "npm:aurelia-templating-binding@1.3.0",
      "aurelia-templating-resources": "npm:aurelia-templating-resources@1.3.1",
      "aurelia-templating-router": "npm:aurelia-templating-router@1.1.0"
    },
    "npm:aurelia-dependency-injection@1.3.0": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-dialog@1.0.0-beta.3.0.1": {
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:aurelia-event-aggregator@1.0.1": {
      "aurelia-logging": "npm:aurelia-logging@1.3.0"
    },
    "npm:aurelia-framework@1.1.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.0",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:aurelia-history-browser@1.0.0": {
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-loader-default@1.0.1": {
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-loader@1.0.0": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-path": "npm:aurelia-path@1.1.1"
    },
    "npm:aurelia-logging-console@1.0.0": {
      "aurelia-logging": "npm:aurelia-logging@1.3.0"
    },
    "npm:aurelia-metadata@1.0.3": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-pal-browser@1.1.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-polyfills@1.2.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-route-recognizer@1.1.0": {
      "aurelia-path": "npm:aurelia-path@1.1.1"
    },
    "npm:aurelia-router@1.2.1": {
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-route-recognizer": "npm:aurelia-route-recognizer@1.1.0"
    },
    "npm:aurelia-task-queue@1.2.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-templating-binding@1.3.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:aurelia-templating-resources@1.3.1": {
      "aurelia-binding": "npm:aurelia-binding@1.2.0",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:aurelia-templating-router@1.1.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.0",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-router": "npm:aurelia-router@1.2.1",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:aurelia-templating@1.3.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.0",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0"
    },
    "npm:aurelia-validation@0.12.5": {
      "aurelia-binding": "npm:aurelia-binding@1.2.0",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.3.0"
    },
    "npm:buffer@3.6.0": {
      "base64-js": "npm:base64-js@0.0.8",
      "child_process": "github:jspm/nodelibs-child_process@0.1.0",
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "ieee754": "npm:ieee754@1.1.8",
      "isarray": "npm:isarray@1.0.0",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:font-awesome@4.6.3": {
      "css": "github:systemjs/plugin-css@0.1.32"
    },
    "npm:inherits@2.0.1": {
      "util": "github:jspm/nodelibs-util@0.1.0"
    },
    "npm:numeral@1.5.6": {
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:process@0.11.9": {
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
      "npm:aurelia-animator-css@1.0.1.js",
      "npm:aurelia-animator-css@1.0.1/aurelia-animator-css.js",
      "npm:aurelia-binding@1.2.0.js",
      "npm:aurelia-binding@1.2.0/aurelia-binding.js",
      "npm:aurelia-bootstrapper@1.0.1.js",
      "npm:aurelia-bootstrapper@1.0.1/aurelia-bootstrapper.js",
      "npm:aurelia-dependency-injection@1.3.0.js",
      "npm:aurelia-dependency-injection@1.3.0/aurelia-dependency-injection.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog-body.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog-footer.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog-header.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/attach-focus.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/aurelia-dialog.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-configuration.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-controller.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-options.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-renderer.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-result.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-service.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/lifecycle.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/renderer.js",
      "npm:aurelia-event-aggregator@1.0.1.js",
      "npm:aurelia-event-aggregator@1.0.1/aurelia-event-aggregator.js",
      "npm:aurelia-fetch-client@1.1.1.js",
      "npm:aurelia-fetch-client@1.1.1/aurelia-fetch-client.js",
      "npm:aurelia-framework@1.1.0.js",
      "npm:aurelia-framework@1.1.0/aurelia-framework.js",
      "npm:aurelia-history-browser@1.0.0.js",
      "npm:aurelia-history-browser@1.0.0/aurelia-history-browser.js",
      "npm:aurelia-history@1.0.0.js",
      "npm:aurelia-history@1.0.0/aurelia-history.js",
      "npm:aurelia-loader-default@1.0.1.js",
      "npm:aurelia-loader-default@1.0.1/aurelia-loader-default.js",
      "npm:aurelia-loader@1.0.0.js",
      "npm:aurelia-loader@1.0.0/aurelia-loader.js",
      "npm:aurelia-logging-console@1.0.0.js",
      "npm:aurelia-logging-console@1.0.0/aurelia-logging-console.js",
      "npm:aurelia-logging@1.3.0.js",
      "npm:aurelia-logging@1.3.0/aurelia-logging.js",
      "npm:aurelia-metadata@1.0.3.js",
      "npm:aurelia-metadata@1.0.3/aurelia-metadata.js",
      "npm:aurelia-pal-browser@1.1.0.js",
      "npm:aurelia-pal-browser@1.1.0/aurelia-pal-browser.js",
      "npm:aurelia-pal@1.3.0.js",
      "npm:aurelia-pal@1.3.0/aurelia-pal.js",
      "npm:aurelia-path@1.1.1.js",
      "npm:aurelia-path@1.1.1/aurelia-path.js",
      "npm:aurelia-polyfills@1.2.0.js",
      "npm:aurelia-polyfills@1.2.0/aurelia-polyfills.js",
      "npm:aurelia-route-recognizer@1.1.0.js",
      "npm:aurelia-route-recognizer@1.1.0/aurelia-route-recognizer.js",
      "npm:aurelia-router@1.2.1.js",
      "npm:aurelia-router@1.2.1/aurelia-router.js",
      "npm:aurelia-task-queue@1.2.0.js",
      "npm:aurelia-task-queue@1.2.0/aurelia-task-queue.js",
      "npm:aurelia-templating-binding@1.3.0.js",
      "npm:aurelia-templating-binding@1.3.0/aurelia-templating-binding.js",
      "npm:aurelia-templating-resources@1.3.1.js",
      "npm:aurelia-templating-resources@1.3.1/abstract-repeater.js",
      "npm:aurelia-templating-resources@1.3.1/analyze-view-factory.js",
      "npm:aurelia-templating-resources@1.3.1/array-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.3.1/attr-binding-behavior.js",
      "npm:aurelia-templating-resources@1.3.1/aurelia-hide-style.js",
      "npm:aurelia-templating-resources@1.3.1/aurelia-templating-resources.js",
      "npm:aurelia-templating-resources@1.3.1/binding-mode-behaviors.js",
      "npm:aurelia-templating-resources@1.3.1/binding-signaler.js",
      "npm:aurelia-templating-resources@1.3.1/compose.js",
      "npm:aurelia-templating-resources@1.3.1/css-resource.js",
      "npm:aurelia-templating-resources@1.3.1/debounce-binding-behavior.js",
      "npm:aurelia-templating-resources@1.3.1/dynamic-element.js",
      "npm:aurelia-templating-resources@1.3.1/focus.js",
      "npm:aurelia-templating-resources@1.3.1/hide.js",
      "npm:aurelia-templating-resources@1.3.1/html-resource-plugin.js",
      "npm:aurelia-templating-resources@1.3.1/html-sanitizer.js",
      "npm:aurelia-templating-resources@1.3.1/if.js",
      "npm:aurelia-templating-resources@1.3.1/map-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.3.1/null-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.3.1/number-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.3.1/repeat-strategy-locator.js",
      "npm:aurelia-templating-resources@1.3.1/repeat-utilities.js",
      "npm:aurelia-templating-resources@1.3.1/repeat.js",
      "npm:aurelia-templating-resources@1.3.1/replaceable.js",
      "npm:aurelia-templating-resources@1.3.1/sanitize-html.js",
      "npm:aurelia-templating-resources@1.3.1/self-binding-behavior.js",
      "npm:aurelia-templating-resources@1.3.1/set-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.3.1/show.js",
      "npm:aurelia-templating-resources@1.3.1/signal-binding-behavior.js",
      "npm:aurelia-templating-resources@1.3.1/throttle-binding-behavior.js",
      "npm:aurelia-templating-resources@1.3.1/update-trigger-binding-behavior.js",
      "npm:aurelia-templating-resources@1.3.1/with.js",
      "npm:aurelia-templating-router@1.1.0.js",
      "npm:aurelia-templating-router@1.1.0/aurelia-templating-router.js",
      "npm:aurelia-templating-router@1.1.0/route-href.js",
      "npm:aurelia-templating-router@1.1.0/route-loader.js",
      "npm:aurelia-templating-router@1.1.0/router-view.js",
      "npm:aurelia-templating@1.3.0.js",
      "npm:aurelia-templating@1.3.0/aurelia-templating.js",
      "npm:aurelia-validation@0.12.5.js",
      "npm:aurelia-validation@0.12.5/aurelia-validation.js",
      "npm:aurelia-validation@0.12.5/implementation/rules.js",
      "npm:aurelia-validation@0.12.5/implementation/standard-validator.js",
      "npm:aurelia-validation@0.12.5/implementation/util.js",
      "npm:aurelia-validation@0.12.5/implementation/validation-messages.js",
      "npm:aurelia-validation@0.12.5/implementation/validation-parser.js",
      "npm:aurelia-validation@0.12.5/implementation/validation-rules.js",
      "npm:aurelia-validation@0.12.5/property-info.js",
      "npm:aurelia-validation@0.12.5/validate-binding-behavior.js",
      "npm:aurelia-validation@0.12.5/validate-trigger.js",
      "npm:aurelia-validation@0.12.5/validation-controller-factory.js",
      "npm:aurelia-validation@0.12.5/validation-controller.js",
      "npm:aurelia-validation@0.12.5/validation-error.js",
      "npm:aurelia-validation@0.12.5/validation-errors-custom-attribute.js",
      "npm:aurelia-validation@0.12.5/validation-renderer-custom-attribute.js",
      "npm:aurelia-validation@0.12.5/validator.js",
      "npm:jquery@3.1.1.js",
      "npm:jquery@3.1.1/dist/jquery.js",
      "npm:moment@2.17.1.js",
      "npm:moment@2.17.1/moment.js",
      "npm:numeral@1.5.6.js",
      "npm:numeral@1.5.6/numeral.js"
    ],
    "app-build.js": [
      "admin/branch-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/branch-create.js",
      "admin/branch-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/branch-page.js",
      "admin/index.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/index.js",
      "admin/payment-type-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/payment-type-create.js",
      "admin/payment-type-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/payment-type-page.js",
      "admin/user-create.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/user-create.js",
      "admin/user-page.html!github:systemjs/plugin-text@0.0.8.js",
      "admin/user-page.js",
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
      "common/converters/age-value-conveter.js",
      "common/converters/date-format-value-converter.js",
      "common/converters/date-input-format-value-converter.js",
      "common/converters/instruction-filter-vaule-converter.js",
      "common/converters/lookup-id-to-name-value-converter.js",
      "common/converters/lookup-to-id-value-converter.js",
      "common/converters/lookup-to-name-value-converter.js",
      "common/converters/number-format-value-converter.js",
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
      "common/models/customer.js",
      "common/models/inventory.js",
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
      "orders/customer-create.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/customer-create.js",
      "orders/customer-page.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/customer-page.js",
      "orders/index.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/index.js",
      "orders/order-create.html!github:systemjs/plugin-text@0.0.8.js",
      "orders/order-create.js",
      "orders/order-invoice-detail-report.js",
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
      "products/product-category-create.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-category-create.js",
      "products/product-category-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-category-page.js",
      "products/product-create.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-create.js",
      "products/product-inventory.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-inventory.js",
      "products/product-order-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-order-page.js",
      "products/product-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-page.js",
      "products/product-purchase-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/product-purchase-page.js",
      "products/supplier-create.html!github:systemjs/plugin-text@0.0.8.js",
      "products/supplier-create.js",
      "products/supplier-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/supplier-page.js",
      "products/unit-of-measure-create.html!github:systemjs/plugin-text@0.0.8.js",
      "products/unit-of-measure-create.js",
      "products/unit-of-measure-page.html!github:systemjs/plugin-text@0.0.8.js",
      "products/unit-of-measure-page.js",
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
      "report_center/customer-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/customer-report-page.js",
      "report_center/customer-report.js",
      "report_center/index.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/index.js",
      "report_center/order-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/order-report-page.js",
      "report_center/order-report.js",
      "report_center/product-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/product-report-page.js",
      "report_center/product-report.js",
      "report_center/report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/report-page.js",
      "report_center/reports.js",
      "report_center/supplier-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/supplier-report-page.js",
      "report_center/supplier-report.js",
      "report_center/unit-of-measure-report-page.html!github:systemjs/plugin-text@0.0.8.js",
      "report_center/unit-of-measure-report-page.js",
      "report_center/unit-of-measure-report.js",
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
      "users/override.js"
    ]
  },
  depCache: {
    "admin/branch-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../services/service-api",
      "../common/controls/notification-service",
      "aurelia-validation",
      "../common/controls/validations/bootstrap-form-renderer"
    ],
    "admin/branch-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./branch-create",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "admin/index.js": [
      "../common/models/role"
    ],
    "admin/payment-type-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../services/payment-type-service",
      "../common/controls/notification-service"
    ],
    "admin/payment-type-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./payment-type-create",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "admin/user-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../services/user-service",
      "../services/branch-service",
      "../common/controls/notification-service"
    ],
    "admin/user-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./user-create",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
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
    "common/converters/age-value-conveter.js": [
      "moment"
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
    "orders/customer-create.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service"
    ],
    "orders/customer-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "orders/index.js": [
      "../common/models/order"
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
      "./order-invoice-detail-report",
      "../common/models/pricing"
    ],
    "orders/order-invoice-detail-report.js": [
      "aurelia-framework",
      "../services/report-builder",
      "../services/formaters",
      "moment"
    ],
    "orders/order-item-page.js": [
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/models/order",
      "../common/models/pricing",
      "../common/utils/ensure-numeric",
      "../common/controls/notification-service"
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
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/models/order",
      "../common/controls/notification-service"
    ],
    "orders/order-return-page.js": [
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/controls/notification-service"
    ],
    "orders/state-verification-value-converter.js": [
      "../common/models/order"
    ],
    "products/discontinued-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
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
    "products/product-category-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../services/service-api",
      "../common/controls/notification-service"
    ],
    "products/product-category-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./product-category-create",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "products/product-create.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service"
    ],
    "products/product-inventory.js": [
      "aurelia-framework",
      "aurelia-router",
      "../common/models/paging",
      "../services/service-api"
    ],
    "products/product-order-page.js": [
      "aurelia-framework",
      "aurelia-path",
      "aurelia-router",
      "../common/models/paging",
      "../services/service-api"
    ],
    "products/product-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "products/product-purchase-page.js": [
      "aurelia-framework",
      "aurelia-path",
      "aurelia-router",
      "../common/models/paging",
      "../services/service-api"
    ],
    "products/supplier-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../services/service-api",
      "../common/controls/notification-service"
    ],
    "products/supplier-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./supplier-create",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "products/unit-of-measure-create.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "../services/service-api",
      "../common/controls/notification-service"
    ],
    "products/unit-of-measure-page.js": [
      "aurelia-framework",
      "aurelia-dialog",
      "./unit-of-measure-create",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
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
      "../common/models/purchase-order"
    ],
    "purchases/purchase-order-create.js": [
      "aurelia-router",
      "aurelia-framework",
      "aurelia-event-aggregator",
      "../common/models/purchase-order",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/pricing",
      "./voucher-report"
    ],
    "purchases/purchase-order-item-page.js": [
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/models/purchase-order",
      "../common/controls/notification-service"
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
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/utils/ensure-numeric",
      "../common/models/purchase-order",
      "../common/controls/notification-service"
    ],
    "purchases/purchase-order-receipt-page.js": [
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/models/purchase-order",
      "../common/controls/notification-service"
    ],
    "purchases/state-verification-value-converter.js": [
      "../common/models/purchase-order"
    ],
    "purchases/voucher-report.js": [
      "aurelia-framework",
      "../services/report-builder",
      "../services/formaters"
    ],
    "report_center/customer-report-page.js": [
      "aurelia-framework",
      "./customer-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report_center/customer-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "report_center/order-report-page.js": [
      "aurelia-framework",
      "./order-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report_center/order-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder",
      "../common/models/order"
    ],
    "report_center/product-report-page.js": [
      "aurelia-framework",
      "./product-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report_center/product-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "report_center/report-page.js": [
      "aurelia-router",
      "aurelia-framework",
      "../services/service-api"
    ],
    "report_center/reports.js": [
      "aurelia-framework",
      "../services/report-builder"
    ],
    "report_center/supplier-report-page.js": [
      "aurelia-framework",
      "./supplier-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report_center/supplier-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "report_center/unit-of-measure-report-page.js": [
      "aurelia-framework",
      "./unit-of-measure-report",
      "../services/service-api",
      "../common/controls/notification-service",
      "../common/models/paging"
    ],
    "report_center/unit-of-measure-report.js": [
      "aurelia-framework",
      "../services/formaters",
      "../services/report-builder"
    ],
    "returns/return-create.js": [
      "aurelia-router",
      "aurelia-framework",
      "aurelia-event-aggregator",
      "../common/models/return",
      "../services/service-api",
      "../common/controls/notification-service"
    ],
    "returns/return-item-page.js": [
      "aurelia-dialog",
      "aurelia-event-aggregator",
      "aurelia-framework",
      "../common/models/paging",
      "../services/service-api",
      "../common/models/return",
      "../common/models/pricing",
      "../common/controls/notification-service"
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
      "aurelia-framework",
      "aurelia-fetch-client",
      "../app-config",
      "fetch"
    ],
    "services/order-service.js": [
      "aurelia-framework",
      "./service-base",
      "./auth-service",
      "./http-client-facade"
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
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
    ],
    "services/purchase-order-service.js": [
      "aurelia-framework",
      "./service-base",
      "./auth-service",
      "./http-client-facade"
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
    "services/supplier-service.js": [
      "aurelia-framework",
      "./service-base",
      "./http-client-facade"
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
      "aurelia-framework",
      "../services/auth-service",
      "../common/models/role",
      "../common/controls/notification-service"
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
      "aurelia-framework",
      "aurelia-dialog",
      "../services/auth-service",
      "../common/controls/notification-service"
    ]
  }
});