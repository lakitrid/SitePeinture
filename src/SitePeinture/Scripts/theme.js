(function () {
    'use strict';

    angular.module('theme', [
        'ngRoute'        
    ])
    .controller('ThemeController', ['$scope', '$http', '$routeParams', '$location', function ($scope, $http, $routeParams, $location) {
        $scope.theme = {};

        $scope.subthemes = [];

        $scope.paints = [];

        $http.get('service/theme/' + $routeParams.themeId).then(function (result) {
            $scope.theme = result.data;
        });

        $http.get('service/theme/subthemes/' + $routeParams.themeId).then(function (result) {
            $scope.subthemes = result.data;
        });

        $http.get('service/painting/theme/' + $routeParams.themeId).then(function (result) {
            $scope.paints = result.data;
        });

        $scope.gotoTheme = function (theme) {
            $location.path('theme/' + theme.Id);
        };

        $scope.gotoThemeId = function (id) {
            $location.path('theme/' + id);
        };
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