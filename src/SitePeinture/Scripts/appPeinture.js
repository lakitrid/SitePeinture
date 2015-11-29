﻿(function () {
    'use strict';

    angular.module('appPeinture', [
        'ngRoute',
        'ngDialog',
        'technical',
        'theme'
    ]).
    config(['$routeProvider', 'ngDialogProvider', '$httpProvider', function ($routeProvider, ngDialogProvider, $httpProvider) {
        $routeProvider.
          when('/home', {
              templateUrl: 'views/home.html',
              controller: 'HomeController'
          }).
          when('/contact', {
              templateUrl: 'views/contact.html',
              controller: 'ContactController'
          }).
          when('/theme/:themeId', {
              templateUrl: 'views/theme.html',
              controller: 'ThemeController'
          }).
          when('/admin', {
              templateUrl: 'views/admin.html',
              controller: 'AdminController'
          }).
          otherwise({
              redirectTo: '/home'
          });

        ngDialogProvider.setDefaults({
            className: 'ngdialog-theme-default',
            showClose: true,
            closeByDocument: false,
            closeByEscape: false
        });

        $httpProvider.interceptors.push('SiteHttpInterceptor');
    }]).
    controller('MainController', ['$rootScope', '$location', '$scope', '$http', function ($rootScope, $location, $scope, $http) {
        $rootScope.goto = function (target) {
            $location.path(target);
        };
    }])
    .controller('HomeController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
        $rootScope.currentView = 'home';

        $scope.paints = [];

        $http.get('service/painting/slider').then(function (result) {
            $scope.paints = result.data;
        });

        $scope.homeArticle = "";

        $http.get('service/home').then(function (result) {
            $scope.homeArticle = result.data;
        });
    }])
    .controller('MenuController', ['$scope', '$http', '$location', 'ThemeService', function ($scope, $http, $location, ThemeService) {
        $scope.paintingMenu = false;

        $scope.themeService = ThemeService;

        $scope.themeService.Load();

        $scope.display = function (display) {
            if (display === true && $scope.paintingMenu === true) {
                $scope.paintingMenu = !display;
            } else {
                $scope.paintingMenu = display;
            }
        };

        $scope.gotoPainting = function (theme) {
            $location.path('theme/' + theme.Id);
        };
    }])
    .controller('ContactController', ['$rootScope', function ($rootScope) {
        $rootScope.currentView = 'contact';
    }])
    .controller('AdminController', ['$rootScope', '$scope', '$http', 'ngDialog', 'ThemeService', function ($rootScope, $scope, $http, ngDialog, ThemeService) {
        $rootScope.currentView = 'admin';

        $scope.home = false;
        $scope.event = false;
        $scope.theme = false;
        $scope.painting = true;

        $scope.paints = [];
        $scope.themeService = ThemeService;
        $scope.article = { text: "" };

        var Load = function () {
            $http.get('service/painting').then(function (result) {
                $scope.paints = result.data;
            });

            $scope.themeService.Load();

            $http.get('service/home').then(function (result) {
                $scope.article.text = result.data;
            });
        }

        $rootScope.$on('ngDialog.closing', function (e, $dialog) {
            Load();
        });

        Load();

        $scope.Add = function () {
            ngDialog.open({ template: 'EditPainting', controller: 'EditPaintingController' });
        };

        $scope.Edit = function (paint) {
            ngDialog.open({ template: 'EditPainting', controller: 'EditPaintingController', data: paint })
        };

        $scope.Delete = function (paint, index) {
            $scope.confirm = ngDialog.openConfirm({
                template: 'ConfirmDelete', controller: 'ConfirmDeleteController'
            }).then(function () {
                $http.delete('service/painting/' + paint.Id);
                Load();
            });

        };

        $scope.AddTheme = function () {
            ngDialog.open({ template: 'EditTheme', controller: 'EditThemeController' });
        };

        $scope.EditTheme = function (theme) {
            ngDialog.open({ template: 'EditTheme', controller: 'EditThemeController', data: theme })
        };

        $scope.DeleteTheme = function (theme, index) {
            $scope.confirm = ngDialog.openConfirm({
                template: 'ConfirmDelete', controller: 'ConfirmDeleteController'
            }).then(function () {
                $http.delete('service/theme/' + theme.Id);
                Load();
            });
        };

        $scope.SaveHomeArticle = function () {
            $http.post("service/home", JSON.stringify($scope.article.text)).then(function () {
                Load();
            });
        };

        $scope.Reload = function () {
            Load();
        };
    }])
    .controller('EditPaintingController', ['$scope', '$http', function ($scope, $http) {
        if (angular.isDefined($scope.ngDialogData)) {
            $scope.isEdit = true;
            $scope.paint = angular.copy($scope.ngDialogData);
        } else {
            $scope.isEdit = false;
        }

        $scope.themes = [];

        $http.get('service/theme').then(function (result) {
            $scope.themes = result.data;
        });

        $scope.Save = function (paint) {
            if ($scope.paintingForm.$valid) {
                var data = angular.copy(paint);

                if (angular.isDefined(paint.file)) {
                    data.FileName = paint.file.Filename;
                    data.Data = paint.file.Data;
                }

                $http.post('service/painting', data).then(function () {
                    $scope.closeThisDialog();
                });
            }
        };
    }])
    .controller('EditThemeController', ['$scope', '$http', function ($scope, $http) {
        if (angular.isDefined($scope.ngDialogData)) {
            $scope.isEdit = true;
            $scope.theme = angular.copy($scope.ngDialogData);
        } else {
            $scope.isEdit = false;
            $scope.theme = { Id: 0 };
        }

        $scope.parentThemes = [];

        $http.get('service/theme/parents/' + $scope.theme.Id).then(function (result) {
            $scope.parentThemes = result.data;
        });

        $scope.Save = function (theme) {
            if ($scope.themeForm.$valid) {

                $http.post('service/theme', theme).then(function () {
                    $scope.closeThisDialog();
                });
            }
        };
    }])
    .controller('ConfirmDeleteController', ['$scope', '$http', function ($scope, $http) {
    }])
    .directive("fileread", [function () {
        return {
            scope: {
                fileread: "="
            },
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {
                    var reader = new FileReader();
                    reader.onload = function (loadEvent) {
                        scope.$apply(function () {
                            var filedata = {
                                Filename: changeEvent.target.files[0].name,
                                Data: loadEvent.target.result
                            }

                            scope.fileread = filedata;
                        });
                    }
                    reader.readAsDataURL(changeEvent.target.files[0]);
                });
            }
        }
    }])
    ;
})();