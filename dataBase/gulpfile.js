'use strict';

//noinspection JSUnresolvedFunction
var
	gulp = require('gulp'),
	watch = require('gulp-watch'),
	concat = require('gulp-concat'),
	prefixer = require('gulp-autoprefixer'),
	uglify = require('gulp-uglify'),
	sourcemaps = require('gulp-sourcemaps'),
	rigger = require('gulp-rigger'),
	cssmin = require('gulp-minify-css'),
	browserSync = require('browser-sync'),
	reload = browserSync.reload;


var path = {
	build: {
		html: 'build/',
		js: 'build/resources/js/',
		css: 'build/resources/css/',
		js_concat: 'script.min.js',
		css_concat: 'style.min.css'
	},
	src: {
		html: 'src/template/pages/*.html',
		js: 'src/js/core.js',
		css: 'src/css/main.css'
	},
	watch: {
		html: 'src/template/*.html',
		js: 'src/js/core.js',
		css: 'src/css/main.css'
	}
};

var config = {
	server: {
		baseDir: "./build"
	},
	tunnel: true,
	host: 'localhost',
	port: 3030,
	logPrefix: "frontendDemon"
};

// for autoreload
gulp.task('webserver', function () {
	browserSync(config);
});

// build template
gulp.task('html:build', function () {
	gulp.src(path.src.html)
		.pipe(rigger())
		.pipe(gulp.dest(path.build.html))
		.pipe(reload({stream: true}));
});

// build scripts
gulp.task('js:build', function () {
	gulp.src(path.src.js)
		.pipe(rigger())
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(sourcemaps.write())
		.pipe(concat(path.build.js_concat))
		.pipe(gulp.dest(path.build.js))
		.pipe(reload({stream: true}));
});

// build styles
gulp.task('style:build', function () {
	gulp.src(path.src.css)
		.pipe(sourcemaps.init())
		.pipe(prefixer())
		.pipe(cssmin())
		.pipe(sourcemaps.write())
		.pipe(concat(path.build.css_concat))
		.pipe(gulp.dest(path.build.css))
		.pipe(reload({stream: true}));
});

// build all
gulp.task('build', [
	'html:build',
	'js:build',
	'style:build'
]);

// gulp watchers
gulp.task('watch', function(){
	gulp.watch(path.watch.html, function(event) {
		gulp.run('html:build');
	});
	gulp.watch(path.watch.js, function(event) {
		gulp.run('js:build');
	});
	gulp.watch(path.watch.css, function(event) {
		gulp.run('style:build');
	});
});


//init tasks
gulp.task('default', ['build', 'webserver', 'watch']);
