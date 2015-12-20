(function () {
    'use strict';

    angular.module('theme', [
        'ngRoute'
    ])
    .controller('ThemeController', ['$scope', '$http', '$routeParams', '$location', 'BreadcrumbService', '$interval',
        function ($scope, $http, $routeParams, $location, BreadcrumbService, $interval) {
            $scope.stop;

            $scope.pause = false;

            $scope.theme = {};

            $scope.subthemes = [];

            $scope.paints = [];

            $http.get('service/theme/' + $routeParams.themeId).then(function (result) {
                $scope.theme = result.data;

                var elements = [];

                if (angular.isDefined($scope.theme)) {
                    if ($scope.theme.ParentId !== 0) {
                        elements.push({ label: $scope.theme.ParentTitle, hasTarget: true, target: '/theme/' + $scope.theme.ParentId });
                    }

                    elements.push({ label: $scope.theme.Title, hasTarget: false });
                }

                BreadcrumbService.setElements(elements);
            });

            $http.get('service/theme/subthemes/' + $routeParams.themeId).then(function (result) {
                $scope.subthemes = result.data;
            });

            $http.get('service/painting/theme/' + $routeParams.themeId).then(function (result) {
                $scope.paints = result.data;
                if (angular.isDefined($scope.paints) && $scope.paints.length > 0) {
                    $scope.index = 0;
                    if (!$scope.theme.WithText) {
                        startInterval();
                    }
                }
            });

            $scope.prev = function () {
                if ($scope.index == 0) {
                    $scope.index = $scope.paints.length - 1;
                } else {
                    $scope.index--;
                }

                resetInterval();
            };

            $scope.next = function () {
                if ($scope.index > $scope.paints.length - 2) {
                    $scope.index = 0;
                } else {
                    $scope.index++;
                }

                resetInterval();
            };

            $scope.select = function (index) {
                $scope.index = index;

                resetInterval();
            };

            $scope.setPause = function () {
                $scope.pause = true;
                stopInterval();
            };

            $scope.restart = function () {
                $scope.pause = false;
                startInterval();
            };

            var resetInterval = function () {
                if (!$scope.pause && !$scope.theme.WithText) {
                    stopInterval();
                    startInterval();
                }
            };

            var startInterval = function () {
                if (angular.isUndefined($scope.stop)) {
                    $scope.stop = $interval(function () {
                        $scope.next();
                    }, 4000);
                }
            };

            var stopInterval = function () {
                if (angular.isDefined($scope.stop)) {
                    $interval.cancel($scope.stop);
                    $scope.stop = undefined;
                }
            };

            $scope.gotoTheme = function (theme) {
                $location.path('theme/' + theme.Id);
            };

            $scope.gotoThemeId = function (id) {
                $location.path('theme/' + id);
            };

            $scope.gotoPaint = function (paintId) {
                $location.path('painting/' + paintId);
            };

            $scope.$on('$destroy', function () {
                stopInterval();
            });
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