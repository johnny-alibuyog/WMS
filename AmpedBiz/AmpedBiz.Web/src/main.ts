import 'bootstrap';
import { Aurelia } from 'aurelia-framework';
import { AuthService } from './services/auth-service';

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .developmentLogging()
    .plugin('common/global-resources')
    .plugin('aurelia-dialog', config => {
      config.useDefaults();
      config.settings.lock = false;
      config.settings.centerVerticalOnly = true;
      config.settings.startingZIndex = 5;
    })
    .plugin('aurelia-validation');

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
    let root = auth.isAuthenticated() ? 'shell/shell' : 'users/login';
    aurelia.setRoot(root);
  });
}