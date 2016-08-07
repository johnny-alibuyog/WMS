import {FrameworkConfiguration} from 'aurelia-framework';

export function configure(config: FrameworkConfiguration) {
  config.globalResources([
    'common/converters/age-value-conveter',
    'common/converters/date-format-value-converter',
    'common/converters/date-input-format-value-converter',
    'common/converters/lookup-id-to-name-value-converter',
    'common/converters/lookup-to-id-value-converter',
    'common/converters/lookup-to-name-value-converter',
    'common/converters/number-format-value-converter',
    'common/converters/object-keys-value-converter',
    'common/converters/object-values-value-converter',
    'common/converters/relative-date-value-converter',
    'common/converters/sort-value-converter',
    'common/converters/to-id-value-converter'
  ]);
}