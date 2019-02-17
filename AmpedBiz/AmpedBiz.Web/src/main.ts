//import * as jQuery from 'jquery';
//import 'bootstrap';
//import 'font-awesome/css/all.min.css';

import * as $ from 'jquery';
let winObj: any = <any>window;
winObj['jQuery'] = $;
winObj['$'] = $;

import 'whatwg-fetch';
import 'assets/common/styles/styles.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import '@fortawesome/fontawesome-free/css/all.min.css';

import { Aurelia, PLATFORM } from 'aurelia-framework';
import { AuthService } from './services/auth-service';
import environment from 'environment';
import { DialogConfiguration } from 'aurelia-dialog';

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .developmentLogging()
    .plugin(PLATFORM.moduleName('common/global-resources'))
    .plugin(PLATFORM.moduleName('aurelia-validation'))
    //.plugin(PLATFORM.moduleName('aurelia-combo'))
    .plugin(PLATFORM.moduleName('aurelia-dialog'), (config: DialogConfiguration) => {
      config.useDefaults();
      config.settings.lock = true;
      config.settings.centerVerticalOnly = true;
      config.settings.startingZIndex = 5;
      config.settings.keyboard = true; //['Enter', 'Escape'];
    });

  if (environment.debug) {
    aurelia.use.developmentLogging();
  }

  if (environment.testing) {
    aurelia.use.plugin(PLATFORM.moduleName('aurelia-testing'));
  }

  //.plugin('aurelia-computed', config => {
  //  config.enableLogging = true;
  //})
  //.plugin('aurelia-validation', config => {
  //  https://gist.run/?id=381fdb1a4b0865a4c25026187db865ce&sha=6b1e99aab075f2314eb94edb0368218f4d1a6f29
  //});

  //Uncomment the line below to enable animation.
  //aurelia.use.plugin('aurelia-animator-css');

  //Anyone wanting to use HTMLImports to load views, will need to install the following plugin.
  //aurelia.use.plugin('aurelia-html-import-template-loader')

  aurelia.start().then(() => {
    var auth = aurelia.container.get(AuthService);
    let root = auth.isAuthenticated() ? PLATFORM.moduleName('shell/shell') : PLATFORM.moduleName('users/login');
    aurelia.setRoot(root);
  });
}
