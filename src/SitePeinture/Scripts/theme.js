(function () {
    'use strict';

    angular.module('theme', [
        'ngRoute'        
    ])
    .controller('ThemeController', [function () {

    }])
    .service('ThemeService', ['$http', function ($http) {
        var themes = [];

        var firstLevelThemes = [];

        var Load = function () {
                $http.get('service/theme').then(function (result) {
                    themes = result.data;
                });

                $http.get('service/theme/parents').then(function (result) {
                    firstLevelThemes = result.data;
                });
        }

        return {
            Load: Load,
            getThemes: function () {
                return themes;
            },
            getFirstLevelThemes: function () {
                return firstLevelThemes;
            }
        }
    }])
    ;
})();