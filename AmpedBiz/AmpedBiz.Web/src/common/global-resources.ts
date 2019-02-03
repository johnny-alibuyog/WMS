import { FrameworkConfiguration, PLATFORM } from 'aurelia-framework';

export function configure(config: FrameworkConfiguration) {
  config.globalResources([
    PLATFORM.moduleName('common/attributes/numeric-value'),
    PLATFORM.moduleName('common/converters/age-value-converter'),
    PLATFORM.moduleName('common/converters/authorize-value-converter'),
    PLATFORM.moduleName('common/converters/date-format-value-converter'),
    PLATFORM.moduleName('common/converters/date-input-format-value-converter'),
    PLATFORM.moduleName('common/converters/filter-value-converter'),
    PLATFORM.moduleName('common/converters/instruction-filter-vaule-converter'),
    PLATFORM.moduleName('common/converters/lookup-id-to-name-value-converter'),
    PLATFORM.moduleName('common/converters/lookup-to-id-value-converter'),
    PLATFORM.moduleName('common/converters/lookup-to-name-value-converter'),
    PLATFORM.moduleName('common/converters/number-format-value-converter'),
    PLATFORM.moduleName('common/converters/object-keys-value-converter'),
    PLATFORM.moduleName('common/converters/object-values-value-converter'),
    PLATFORM.moduleName('common/converters/percentage-value-converter'),
    PLATFORM.moduleName('common/converters/relative-date-value-converter'),
    PLATFORM.moduleName('common/converters/sort-value-converter'),
    PLATFORM.moduleName('common/converters/to-id-value-converter'),
  ]);
}
