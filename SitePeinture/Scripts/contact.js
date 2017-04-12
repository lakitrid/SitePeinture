(function () {
    'use strict';

    angular.module('contact', [
        'ngRoute'
        
    ])
    .controller('ContactController', ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {
        $rootScope.IdentityService.isAuth();
        $rootScope.currentView = 'contact';

        $scope.hasSuccess = false;
        $scope.hasError = false;

        $scope.contact = {};

        $scope.Send = function () {
            $scope.hasSuccess = false;
            $scope.hasError = false;
            if ($scope.contactForm.$valid) {
                $http.post('service/contact', $scope.contact).then(function (successResult) {
                    $scope.hasSuccess = true;
                    $scope.contact = {};
                }, function (errorResult) {
                    $scope.hasError = true;
                });
            }
        };
    }])
    ;
})();