(function () {
    'use strict';

    angular.module('painting', [
        'ngRoute'
    ])
    .controller('PaintingController', ['$scope', '$http', '$routeParams', '$location', 'BreadcrumbService',
        function ($scope, $http, $routeParams, $location, BreadcrumbService) {
            $scope.paint = {};

            $http.get('service/painting/' + $routeParams.paintingId).then(function (result) {
                $scope.paint = result.data;

                if (angular.isDefined($scope.paint)) {
                    $http.get('service/theme/' + $scope.paint.ThemeId).then(function (result) {
                        var elements = [];

                        var theme = result.data;

                        if (angular.isDefined(theme)) {
                            if (theme.ParentId !== 0) {
                                elements.push({ label: theme.ParentTitle, hasTarget: true, target: '/theme/' + theme.ParentId });
                            }

                            elements.push({ label: theme.Title, hasTarget: true, target: '/theme/' + theme.Id });
                        }

                        elements.push({ label: $scope.paint.Title, hasTarget: false });

                        BreadcrumbService.setElements(elements);
                    });
                }
            });
        }])
    ;
})();