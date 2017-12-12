// this file provides a list of unbundled files that
// need to be included when exporting the application
// for production.
module.exports = {
  'list': [
    'index.html',
    'config.js',
    'assets/**',
    'favicon.ico',
    'LICENSE',
    'jspm_packages/system.js',
    'jspm_packages/system-polyfills.js',
    'jspm_packages/system-csp-production.js',
    'jspm_packages/github/select2/select2@4.0.5.js',
    'jspm_packages/github/select2/select2@4.0.5/js/select2.js',
    'jspm_packages/github/select2/select2@4.0.5/css/select2.min.css',
    'jspm_packages/github/systemjs/plugin-css@0.1.36.js',
    'jspm_packages/github/systemjs/plugin-css@0.1.36/css.js',
    'jspm_packages/github/systemjs/plugin-text@0.0.8.js',
    'jspm_packages/github/systemjs/plugin-text@0.0.8/text.js',
    'styles/styles.css'
  ],
  // this section lists any jspm packages that have
  // unbundled resources that need to be exported.
  // these files are in versioned folders and thus
  // must be 'normalized' by jspm to get the proper
  // path.
  'normalize': [
    [
      // include font-awesome.css and its fonts files
      'font-awesome', [
        '/css/font-awesome.min.css',
        '/fonts/*'
      ]
    ], [
      // include bootstrap's font files
      'bootstrap', [
        '/fonts/*'
      ]
    ]
  ]
};
