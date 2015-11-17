/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.initConfig({
        uglify: {
            my_target: {
                options: {
                    beautify: true
                },
                files: {
                    'wwwroot/app/app.js': [
                        'Scripts/app.js',
                        'Scripts/**/*.js'
                    ]
                }
            }
        },

        copy: {
            main: {
                files: [
                    { expand: true, flatten: true, cwd: 'css/', src: ['**/*.png'], dest: 'wwwroot/css/' },
                    { expand: true, flatten: true, cwd: 'node_modules/font-awesome/fonts', src: ['**'], dest: 'wwwroot/fonts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/bootstrap/fonts', src: ['**'], dest: 'wwwroot/fonts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['**/*.min.css'], dest: 'wwwroot/css/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['angular*/**/*.min.js'], dest: 'wwwroot/scripts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['bootstrap/**/*.min.js'], dest: 'wwwroot/scripts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['font-awesome/**/*.min.js'], dest: 'wwwroot/scripts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['jquery/**/jquery.min.js'], dest: 'wwwroot/scripts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['ng-dialog/**/*.min.js'], dest: 'wwwroot/scripts/' },
                ]
            }
        },

        cssmin: {
            options: {
                shorthandCompacting: false,
                roundingPrecision: -1,
                keepSpecialComments: 0
            },
            target: {
                files: {
                    'wwwroot/css/content.css': [
                        'css/**/*.css'
                    ]
                }
            }
        },

        watch: {
            scripts: {
                files: ['Scripts/**/*.js', 'css/**/*.css'],
                tasks: ['cssmin', 'uglify']
            }
        }
    });

    grunt.registerTask('default', ['cssmin', 'uglify', 'copy', 'watch']);
};