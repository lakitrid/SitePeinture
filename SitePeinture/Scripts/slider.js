(function () {
    'use strict';

    angular.module('slider', [
        'ngRoute',
        'ngAnimate'
    ])
    .directive('slider', ['$timeout', '$http', function ($timeout, $http) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
            },
            link: function (scope, elem, attrs) {
                scope.paints = [];

                $http.get('service/painting/slider').then(function (result) {
                    scope.paints = result.data;
                    scope.currentIndex = 0;
                });

                scope.next = function (byTimer) {
                    scope.currentIndex = (scope.currentIndex + 1) % scope.paints.length;
                    if (!byTimer) {
                        $timeout.cancel(timer);
                        timer = $timeout(sliderFunc, 4000);
                    }
                };

                scope.prev = function (byTimer) {
                    scope.currentIndex > 0 ? scope.currentIndex-- : scope.currentIndex = scope.paints.length - 1;
                    $timeout.cancel(timer);
                    timer = $timeout(sliderFunc, 4000);
                };

                scope.$watch('currentIndex', function () {
                    scope.paints.forEach(function (image) {
                        image.visible = false; // make every image invisible
                    });

                    if (scope.paints.length > 0) {
                        scope.paints[scope.currentIndex].visible = true; // make the current image visible
                    }
                });

                var timer;
                var sliderFunc = function () {
                    timer = $timeout(function () {
                        scope.next(true);
                        timer = $timeout(sliderFunc, 4000);
                    }, 4000);
                };

                sliderFunc();

                scope.$on('$destroy', function () {
                    $timeout.cancel(timer); // when the scope is getting destroyed, cancel the timer
                });
            },
            templateUrl: 'views/slider.html'
        };
    }])
    ;
})();