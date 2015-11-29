(function () {
    'use strict';

    angular.module('technical', [])
    .constant("appVersion", "v1.0")
    .factory('SiteHttpInterceptor', ['$q', 'appVersion', function ($q, appVersion) {
        return {
            'request': function (config) {

                // Add app version in the request to bypass the cache for static assets
                if (config.url.search(/^(\.?\/)?(css|app|views|scripts)\//) === 0) {
                    var char = (config.url.indexOf("?") > -1) ? "&" : "?";
                    config.url = config.url + char + appVersion;
                } else if (config.url.search(/^(\.?\/)?(service)\//) === 0) {
                    var char = (config.url.indexOf("?") > -1) ? "&" : "?";
                    config.url = config.url + char + new Date().getTime();
                }

                return config || $q.when(config);
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
    ;
})();