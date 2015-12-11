(function () {
    'use strict';

    angular.module('appPeinture', [
        'ngRoute',
        'ngDialog',
        'technical',
        'admin',
        'theme',
        'slider',
        'contact',
        'painting',
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
          when('/painting/:paintingId', {
              templateUrl: 'views/painting.html',
              controller: 'PaintingController'
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
            className: 'ngdialog-theme-default fullsize-theme-default',
            showClose: true,
            closeByDocument: false,
            closeByEscape: false
        });

        $httpProvider.interceptors.push('SiteHttpInterceptor');
    }]).
    run(['$rootScope', function ($rootScope) {
        $rootScope.connection = { count: 0 };
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
    ;
})();