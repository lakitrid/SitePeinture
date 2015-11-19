(function () {
    'use strict';

    angular.module('appPeinture', [
        'ngRoute',
        'ngDialog'
    ]).
    config(['$routeProvider', function ($routeProvider) {
        $routeProvider.
          when('/home', {
              templateUrl: 'views/home.html',
              controller: 'HomeController'
          }).
          when('/contact', {
              templateUrl: 'views/contact.html',
              controller: 'ContactController'
          }).
          when('/admin', {
              templateUrl: 'views/admin.html',
              controller: 'AdminController'
          }).
          otherwise({
              redirectTo: '/home'
          });
    }]).
    controller('MainController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
        $rootScope.goto = function (target) {
            $location.path(target);
        };
    }])
    .controller('HomeController', ['$rootScope', function ($rootScope) {
        $rootScope.currentView = 'home';

    }])
    .controller('ContactController', ['$rootScope', function ($rootScope) {
        $rootScope.currentView = 'contact';

    }])
    .controller('AdminController', ['$rootScope', '$scope', '$http', 'ngDialog', function ($rootScope, $scope, $http, ngDialog) {
        $rootScope.currentView = 'admin';

        $scope.slider = false;
        $scope.event = false;
        $scope.theme = false;
        $scope.painting = true;

        $scope.paints = [];
        $scope.themes = [];

        var Load = function () {
            $http.get('painting').then(function (result) {
                $scope.paints = result.data;
            });

            $http.get('theme').then(function (result) {
                $scope.themes = result.data;
            });
        }

        Load();

        $scope.Add = function () {
            ngDialog.open({ template: 'EditPainting', controller: 'EditPaintingController' });
        };

        $scope.Edit = function (paint) {
            ngDialog.open({ template: 'EditPainting', controller: 'EditPaintingController', data: paint })
        };

        $scope.Delete = function (paint, index) {

        };

        $scope.AddTheme = function () {
            ngDialog.open({ template: 'EditTheme', controller: 'EditThemeController' });
        };

        $scope.EditTheme = function (theme) {
            ngDialog.open({ template: 'EditTheme', controller: 'EditThemeController', data: theme })
        };

        $scope.DeleteTheme = function (theme, index) {

        };
    }])
    .controller('EditPaintingController', ['$scope', '$http', function ($scope, $http) {
        if (angular.isDefined($scope.ngDialogData)) {
            $scope.isEdit = true;
            $scope.paint = $scope.ngDialogData;
        } else {
            $scope.isEdit = false;
        }

        $scope.Save = function (paint) {
            if ($scope.paintingForm.$valid) {
                var data = {};

                data.Title = paint.Title;

                if (angular.isDefined(paint.file)) {
                    data.Title = paint.file.Filename;
                    data.Title = paint.file.Data;
                }

                $http.post('painting', data).then(function () {
                    $scope.closeThisDialog();
                });
            }
        };
    }])
    .controller('EditThemeController', ['$scope', '$http', function ($scope, $http) {
        if (angular.isDefined($scope.ngDialogData)) {
            $scope.isEdit = true;
            $scope.theme = $scope.ngDialogData;
        } else {
            $scope.isEdit = false;
        }

        $scope.Save = function (theme) {
            if ($scope.paintingForm.$valid) {

                $http.post('theme', theme).then(function () {
                    $scope.closeThisDialog();
                });
            }
        };
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