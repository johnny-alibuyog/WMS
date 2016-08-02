import 'bootstrap';
import {Aurelia} from 'aurelia-framework';
import {AuthService} from './services/auth-service';

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .developmentLogging()
    .plugin('common/global-resources')
    .plugin('aurelia-dialog', config => {
      config.useDefaults();
      config.settings.lock = true;
      config.settings.centerVerticalOnly = true;
      config.settings.startingZIndex = 5;
    });

  //.plugin('aurelia-computed', config => {
  //  config.enableLogging = true;
  //})
  //.plugin('aurelia-validation', config => {
  //});

  //Uncomment the line below to enable animation.
  //aurelia.use.plugin('aurelia-animator-css');

  //Anyone wanting to use HTMLImports to load views, will need to install the following plugin.
  //aurelia.use.plugin('aurelia-html-import-template-loader')

  aurelia.start().then(() => {
    var auth = aurelia.container.get(AuthService);
    let root = auth.isAuthenticated() ? 'app' : 'users/login';
    aurelia.setRoot(root);
    aurelia.setRoot();
  });
}