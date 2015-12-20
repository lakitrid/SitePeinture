(function () {
    'use strict';

    angular.module('technical', [])
    .constant("appVersion", "v1.0.6")
    .factory('SiteHttpInterceptor', ['$q', 'appVersion', '$rootScope', function ($q, appVersion, $rootScope) {
        return {
            'request': function (config) {

                $rootScope.connection.count++;

                // Add app version in the request to bypass the cache for static assets
                if (config.url.search(/^(\.?\/)?(css|app|views|scripts)\//) === 0) {
                    var char = (config.url.indexOf("?") > -1) ? "&" : "?";
                    config.url = config.url + char + appVersion;
                } else if (config.url.search(/^(\.?\/)?(service)\//) === 0) {
                    var char = (config.url.indexOf("?") > -1) ? "&" : "?";
                    config.url = config.url + char + new Date().getTime();
                }

                return config || $q.when(config);
            },
            'response': function (response) {
                $rootScope.connection.count--;
                return response || $q.when(response);
            },
            'responseError': function (response) {
                $rootScope.connection.count--;
                return response || $q.when(response);
            }
        };
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
    .controller('LoginController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.user = {};

        $scope.error = false;

        $scope.Login = function () {
            $scope.error = false;

            if ($scope.loginForm.$valid) {
                $http.post('service/user/login', $scope.user).then(function (result) {
                    if (result.data === true) {
                        $location.path("/admin");
                    } else {
                        $scope.error = true;
                    }
                });
            }
        };
    }])
    .factory("IdentityService", ['$rootScope', '$http', '$location', function ($rootScope, $http, $location) {
        var IsAuth = function () {
            var promise = $http.get('service/user/isAuth');
            promise.then(function (result) {
                if (result.data === true) {
                    $rootScope.isConnected = true;
                } else {
                    $rootScope.isConnected = false;
                }
            }, function (error) {
                $rootScope.isConnected = false;
            });

            return promise;
        };

        var SignOut = function () {
            var promise = $http.get('service/user/signOut').then(function () {
                $rootScope.isConnected = false;
                $location.path('home');
            });

            return promise;
        }

        return {
            isAuth: IsAuth,
            signOut: SignOut
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
    .controller('BreadCrumbController', ['$scope', '$location', 'BreadcrumbService', function ($scope, $location, BreadcrumbService) {
        $scope.breadcrumbService = BreadcrumbService;

        $scope.gotoElement = function (element) {
            $location.path(element.target);
        };
    }])
    .factory('BreadcrumbService', function () {
        var elements = [];

        return {
            getElements: function () {
                return elements;
            },
            setElements: function (elementsVal) {
                elements = elementsVal;
                elements.unshift({ label: 'Accueil', hasTarget: true, target: '/home' })
            }
        };
    });
    ;
})();