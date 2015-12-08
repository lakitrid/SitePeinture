(function () {
    'use strict';

    angular.module('appPeinture', [
        'ngRoute',
        'ngDialog',
        'technical',
        'theme',
        'slider',
        'ngAnimate'
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
          when('/login', {
              templateUrl: 'views/login.html',
              controller: 'LoginController'
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
    controller('MainController', ['$rootScope', '$location', '$scope', '$http', 'IdentityService',
        function ($rootScope, $location, $scope, $http, IdentityService) {
            $rootScope.goto = function (target) {
                $location.path(target);
            };

            $rootScope.isConnected = false;

            $rootScope.IdentityService = IdentityService;
        }])
    .controller('HomeController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
        $rootScope.IdentityService.isAuth();

        $rootScope.currentView = 'home';

        $scope.homeArticle = "";

        $http.get('service/home').then(function (result) {
            $scope.homeArticle = result.data;
        });

        $scope.nextEvents = [];
        $http.get('service/event/next').then(function (result) {
            $scope.nextEvents = result.data;
        });
    }])
    .controller('MenuController', ['$scope', '$http', '$location', 'ThemeService',
        function ($scope, $http, $location, ThemeService) {
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
    .controller('ContactController', ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {
        $rootScope.IdentityService.isAuth();
        $rootScope.currentView = 'contact';

        $scope.hasSuccess = false;

        $scope.contact = {};

        $scope.Send = function () {
            $scope.hasSuccess = false;

            $http.post('service/contact', $scope.contact).then(function (successResult) {
                $scope.hasSuccess = true;
                $scope.contact = {};
            }, function (errorResult) {

            });
        };
    }])
    .controller('AdminController', ['$rootScope', '$scope', '$http', 'ngDialog', 'ThemeService', '$location',
        function ($rootScope, $scope, $http, ngDialog, ThemeService, $location) {
            $rootScope.IdentityService.isAuth().then(
                function () {
                    if ($rootScope.isConnected === false) {
                        $location.path('login');
                    }
                });

            $rootScope.currentView = 'admin';

            $scope.home = false;
            $scope.event = false;
            $scope.theme = false;
            $scope.painting = true;
            $scope.pass = false;

            $scope.paints = [];
            $scope.themeService = ThemeService;
            $scope.events = [];
            $scope.article = { text: "" };

            $scope.homeSuccess = false;

            var Load = function () {
                $http.get('service/painting').then(function (result) {
                    $scope.paints = result.data;
                });

                $scope.themeService.Load();

                $http.get('service/event').then(function (result) {
                    $scope.events = result.data;
                })

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

            $scope.AddEvent = function () {
                ngDialog.open({ template: 'EditEvent', controller: 'EditEventController' });
            }

            $scope.EditEvent = function (event) {
                ngDialog.open({ template: 'EditEvent', controller: 'EditEventController', data: event });
            }

            $scope.DeleteEvent = function (event, index) {
                $scope.confirm = ngDialog.openConfirm({
                    template: 'ConfirmDelete', controller: 'ConfirmDeleteController'
                }).then(function () {
                    $http.delete('service/event/' + event.Id);
                    Load();
                });
            };

            $scope.SaveHomeArticle = function () {
                $scope.homeSuccess = false;

                $http.post("service/home", JSON.stringify($scope.article.text)).then(function () {
                    $scope.homeSuccess = true;

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

            $scope.themes.forEach(function (tm) {
                if (angular.isDefined($scope.paint) && tm.Id == $scope.paint.ThemeId) {
                    $scope.paint.currentTheme = tm;
                }
            })
        });

        $scope.Save = function (paint) {
            if ($scope.paintingForm.$valid) {
                var data = angular.copy(paint);

                if (angular.isDefined(paint.file)) {
                    data.FileName = paint.file.Filename;
                    data.Data = paint.file.Data;
                }

                data.ThemeId = $scope.paint.currentTheme.Id;
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
            $scope.parentThemes.forEach(function (pt) {
                if (pt.Id == $scope.theme.ParentId) {
                    $scope.theme.currentParent = pt;
                }
            })
        });

        $scope.Save = function (theme) {
            if ($scope.themeForm.$valid) {

                if (angular.isDefined($scope.theme.currentParent)) {
                    theme.ParentId = $scope.theme.currentParent.Id;
                } 

                $http.post('service/theme', theme).then(function () {
                    $scope.closeThisDialog();
                });
            }
        };
    }])
    .controller('EditEventController', ['$scope', '$http', function ($scope, $http) {
        if (angular.isDefined($scope.ngDialogData)) {
            $scope.isEdit = true;
            $scope.event = angular.copy($scope.ngDialogData);
        } else {
            $scope.isEdit = false;
            $scope.event = { Id: 0 };
        }

        $scope.Save = function (event) {
            if ($scope.eventForm.$valid) {
                $http.post('service/event', event).then(function () {
                    $scope.closeThisDialog();
                });
            }
        };
    }])
    .controller('ConfirmDeleteController', ['$scope', '$http', function ($scope, $http) {
    }])
    .controller('ChangePasswordController', ['$scope', '$http', function ($scope, $http) {
        $scope.user = {};

        $scope.error = {
            hasError: false,
            errors: []
        };

        $scope.hasSuccess = false;

        $scope.ChangePassword = function () {
            $scope.hasSuccess = false;

            if ($scope.passForm.$valid) {
                if ($scope.user.NewPassword === $scope.user.ConfirmPassword) {
                    $http.post('service/user/change', $scope.user).then(function (result) {
                        $scope.error.hasError = false;
                        $scope.hasSuccess = true;
                        $scope.user = {};
                    }, function (result) {
                        if (angular.isDefined(result.data)) {
                            $scope.error.hasError = true;
                            $scope.error.errors = result.data;
                        }
                    });
                } else {
                    $scope.error.hasError = true;
                    $scope.error.errors = [ "Le nouveau mot de passe et sa confirmation doivent être identiques"];
                }
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