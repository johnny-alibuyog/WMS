var gulp = require('gulp');
var gutil = require('gulp-util');
var ftp = require('vinyl-ftp');

var config = {
  options: {
    host: gutil.env.host,
    user: gutil.env.username,
    password: gutil.env.password,
    parallel: 10,
    log: gutil.log
  },
  localFolder: ['export/**'],
  remoteFolder: gutil.env.folder,
};

/**
 * Deploy task.
 * Copies the new files to the server
 *
 * Usage: `gulp deploy --target gear_host`
 */
gulp.task('deploy', ['export'], function () {
  gutil.log(config);
  var conn = ftp.create(config.options);

  return gulp.src(config.localFolder, { base: 'export', buffer: false })
    .pipe(conn.newer(config.remoteFolder))
    .pipe(conn.dest(config.remoteFolder));
});


/**
 * Watch deploy task.
 * Watches the local copy for changes and copies the new files to the server whenever an update is detected
 *
 * Usage: `gulp deploy-watch --target gear_host`
 */
gulp.task('deploy-watch', ['export'], function () {
  var conn = ftp.create(config.options);

  gulp.watch(config.localFolder)
    .on('change', function (event) {
      console.log('Changes detected! Uploading file "' + event.path + '", ' + event.type);

      return gulp.src([event.path], { base: 'export', buffer: false })
        .pipe(conn.newer(config.remoteFolder)) // only upload newer files 
        .pipe(conn.dest(config.remoteFolder))
        ;
    });
});