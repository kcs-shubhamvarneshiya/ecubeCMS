
const gulp = require ('gulp');
const sass = require ('gulp-sass');
const browserSync = require ('browser-sync').create();

// Compile scss to css
function style() {
    // 1. Where is my scss file
    return gulp.src('./scss/**/*.scss')
    // 2. Pass this file throgh scss compiler
    .pipe(sass().on('error', sass.logError))
    // 3. Where do I save the compiled CSS?
    .pipe(gulp.dest('./assets/css'))
    // 4 . stream changes to all browser
    .pipe(browserSync.stream());
    
}

function watch() {
    browserSync.init({
        server:{
            baseDir: './localhost:1044'
        }
    })
    gulp.watch('./scss/**/*.scss', style)
    gulp.watch('./*.html').on('change', browserSync.reload);
    gulp.watch('./assets/js/**/*.js').on('change', browserSync.reload);

}

exports.style = style;
exports.watch = watch;