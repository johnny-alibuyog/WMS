'use strict';

var fs = require('fs');
var gulp = require('gulp');

gulp.task('swagger:compile', function(done) {
  var CodeGen = require('swagger-js-codegen').CodeGen;
  var apis = [
    {
      swagger: 'src/services/swagger.json',
      moduleName: 'service-api',
      className: 'ServiceApi'
    }
  ];
  var dest = 'src/services';
  apis.forEach(function(api) {
    var swagger = JSON.parse(fs.readFileSync(api.swagger, 'utf-8'));
    var source = CodeGen.getNodeCode({ className: api.className, swagger: swagger }); 
    //CodeGen.getTypescriptCode({ moduleName: api.moduleName, className: api.className, swagger: swagger });
    fs.writeFileSync(dest + '/' + api.moduleName + '.js', source, 'UTF-8');
  });
  done();
});

gulp.task('swagger', ['swagger:compile']);