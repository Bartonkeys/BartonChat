var yerpeModule = angular.module("yerpeModule", ['ngRoute', 'ui.bootstrap', 'angular-loading-bar']);

yerpeModule.config(["$routeProvider", function ($routeProvider) {

    $routeProvider.when("/", {
        controller: "yerpeController",
        templateUrl: "/templates/chat.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });

}]);

yerpeModule.factory("dataService", ["$http", "$q", function ($http, $q) {

    var _messages = [];
    var _isInit = false;

    var _isReady = function() {
        return _isInit;
    };

    var _getMessages = function () {

        var deferred = $q.defer();

        $http.get("/api/yerpe")
          .then(function (result) {
              // success
              angular.copy(result.data.Messages, _messages);
              _isInit = true;
              deferred.resolve(result.data.Name);
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    return {
        messages: _messages,
        getMessages: _getMessages,
        isReady: _isReady
    };
}]);

yerpeModule.controller("yerpeController", function ($scope, $http, dataService) {
    $scope.data = dataService;
    $scope.isBusy = true;
    $scope.myName = "";

    if (dataService.isReady()) {
        $scope.isBusy = false;
    }
    else {
        dataService.getMessages()
          .then(function (name) {
              $scope.myName = name;
              // success
          },
          function () {
              // error
              alert("could not load messages");
          })
          .then(function () {
              $scope.isBusy = false;
          });
    }
})