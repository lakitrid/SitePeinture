(function () {
    'use strict';

    angular.module('appPeinture', [
        'ngRoute',
        'ngDialog'
    ]).
    config(['$routeProvider', 'ngDialogProvider', function ($routeProvider, ngDialogProvider) {
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

        ngDialogProvider.setDefaults({
            className: 'ngdialog-theme-default',
            showClose: true,
            closeByDocument: false,
            closeByEscape: false
        });
    }]).
    controller('MainController', ['$rootScope', '$location', function ($rootScope, $location) {
        $rootScope.goto = function (target) {
            $location.path(target);
        };
    }])
    .controller('HomeController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
        $rootScope.currentView = 'home';

        $scope.paints = [];

        $http.get('painting/slider').then(function (result) {
            $scope.paints = result.data;
        });
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
        $scope.paintsSlider = [];
        $scope.themes = [];

        var Load = function () {
            $http.get('painting').then(function (result) {
                $scope.paints = result.data;
            });

            $http.get('theme').then(function (result) {
                $scope.themes = result.data;
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
            $scope.paint = angular.copy($scope.ngDialogData);
        } else {
            $scope.isEdit = false;
        }

        $scope.themes = [];

        $http.get('theme').then(function (result) {
            $scope.themes = result.data;
        });

        $scope.Save = function (paint) {
            if ($scope.paintingForm.$valid) {
                var data = angular.copy(paint);

                if (angular.isDefined(paint.file)) {
                    data.FileName = paint.file.Filename;
                    data.Data = paint.file.Data;
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
            $scope.theme = angular.copy($scope.ngDialogData);
        } else {
            $scope.isEdit = false;
            $scope.theme = { Id: 0 };
        }

        $scope.parentThemes = [];

        $http.get('theme/parents/' + $scope.theme.Id).then(function (result) {
            $scope.parentThemes = result.data;
        });

        $scope.Save = function (theme) {
            if ($scope.themeForm.$valid) {

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
    .directive("markdown", [function () {
        return {
            link: function (scope, element, attributes) {
                var converter = new showdown.Converter();

                scope.$watch(function () { return attributes.markdown; }, function (newValue, oldvalue) {
                    if (angular.isDefined(attributes.markdown)) {
                        element[0].innerHTML = converter.makeHtml(attributes.markdown);
                    }
                });
            }
        }
    }])
    ;
})();