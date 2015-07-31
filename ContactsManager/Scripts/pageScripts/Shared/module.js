var app;
var webAPIURL = "http://localhost:9093/";
var config = {
    pageSize: 10,
    appServicesHostName: webAPIURL,
    dateFormat: "MM-dd-yyyy",
    dateFormatWithTime: "MM-dd-yyyy hh:mm tt",
    dateFormatWithTime24Hours: "MM-dd-yyyy HH:mm",
    applicationDateFormat: "yyyy-MM-ddTHH:mm:ss"
};

var headers = {
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Methods': ['GET', 'POST', 'OPTIONS', 'PUT', 'DELETE'],
    'Access-Control-Allow-Headers': 'true',
    'Access-Control-Allow-Credendtials': 'true',
    'Content-Type': 'application/json;charset=utf-8'
};

(function () {
    app = angular.module("smEasyModule", ['kendo.directives', 'ngRoute', 'ngStorage', 'ui.bootstrap',
        'SharedServices', 'ngSanitize', 'ngAnimate', 'focus-if',  
        'ui.grid',
    'ui.grid.pagination',
    'ui.grid.edit',
    'ui.grid.resizeColumns',
    'ui.grid.pinning',
    'ui.grid.selection',
    'ui.grid.moveColumns', 'ui.router'])
      .config(function ($routeProvider) {
          $routeProvider
              .when('/index', { templateUrl: '/App/views/contactsManager.html', controller: 'contactController' })
              .otherwise({ redirectTo: '/' });
      });
    app.run(['$rootScope', '$location', '$http', '$localStorage',
    function ($rootScope, $location, $http, $localStorage) {
        $location.path('/index');

    }]);
    app.value("config", config);
})();

app.directive('xenButton', function ($compile) {
    return {
        scope: {
            label: '@', // optional
            click: '&',
            options: '=',
            caption: '@',
            hide: '=',
            disabled: '='
        },
        restrict: 'E',
        replace: true, // optional 
        templateUrl: '../../PartialView/CustomButton.html',
        link: function (scope, element, attr) {
        }
    };
});

app.directive('numeric', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attr, ctrl) {
            function inputValue(val) {
                if (val) {
                    var digits = val.replace(/[^0-9]/g, '');

                    if (digits !== val) {
                        ctrl.$setViewValue(digits);
                        ctrl.$render();
                    }
                    return parseInt(digits, 10);
                }
                else if (val.trim() == '')
                    return true;
                return undefined;
            }
            ctrl.$parsers.push(inputValue);
        }
    };
});

app.directive('disableContents', function () {
    return {
        compile: function (tElem, tAttrs) {

            var inputs = tElem.find('input');
            var textarea = tElem.find('textarea');
            var multiselect = tElem.find('select');
            var button = tElem.find('button');
            var images = tElem.find('image');
            inputs.attr('ng-disabled', tAttrs['disableContents']);
            textarea.attr('ng-disabled', tAttrs['disableContents']);
            multiselect.attr('ng-disabled', tAttrs['disableContents']);
            button.attr('ng-disabled', tAttrs['disableContents']);
            images.attr('ng-disabled', tAttrs['disableContents']);
        }
    }
});

angular.module('SharedServices', []).config(['$httpProvider', function ($httpProvider) {

    $httpProvider.interceptors.push(function ($q, $injector, $rootScope) {
        var $http;
        return { //On Initial Request
            request: function (config) {
                //Loader Is Visible
                $('#mydiv').show();
                $('.loaderClass').removeClass("hideLoader");
                return config;
            },

            response: function (response) { //On Request End
                //$http.pendingRequests Used To Get Pending Request Length               
                $http = $http || $injector.get('$http');
                $rootScope.isRequestCompleted = $http.pendingRequests.length == 0;
                if ($http.pendingRequests.length < 1) {  //If There Is No Length Then There is No Pending Request                       
                    $('#mydiv').hide();
                    $('.loaderClass').addClass("hideLoader");
                }
                return response;
            },
            responseError: function (response) {
                //$http.pendingRequests Used To Get Pending Request Length
                $http = $http || $injector.get('$http');
                if ($http.pendingRequests.length < 1) {    //If There Is No Length Then There is No Request     
                    $('#mydiv').hide();
                    $('.loaderClass').addClass("hideLoader");
                }
                return $q.reject(response);
            }
        }
    });
}
]);
 