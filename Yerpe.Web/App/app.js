var yerpeModule = angular.module("yerpeModule", ['ngRoute', 'ui.bootstrap', 'angular-loading-bar', 'SignalR', 'mdo-angular-cryptography']);

yerpeModule.config(["$routeProvider", "$locationProvider", "$cryptoProvider", function ($routeProvider, $locationProvider, $cryptoProvider) {

    $routeProvider.when("/", {
        controller: "yerpeController",
        templateUrl: "/templates/chat.html"
    });

    $routeProvider.when("/rooms", {
        controller: "roomController",
        templateUrl: "/templates/rooms.html"
    });

    $routeProvider.when("/users", {
        controller: "userController",
        templateUrl: "/templates/users.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });

    $cryptoProvider.setCryptographyKey('M1QnJM/XkqPpw5JF7DKhpQkyYgw2VfSpcGqHfRA5cs0=');

}]);

yerpeModule.factory("dataService", ["$http", "$q", '$crypto', function ($http, $q, $crypto) {

    var _messages = [];
    var _rooms = [];
    var _users = [];
    var _usersInRoom = [];
    var _isInit = false;
    var _unRead = 0;
    var _skip = 0;
    var _myName = "";
    var _isAdmin = false;
    var _receivedMessage = false;
    var _roomId = 0;
    var _message = "";

    var _isReady = function () {
        return _isInit;
    };

    var _getName = function () {
        return _myName;
    };

    var _getSkip = function () {
        return _skip;
    };

    var _getIsAdmin = function () {
        return _isAdmin;
    };

    var _getRoomId = function () {
        return _roomId;
    };

    var _getMessages = function () {

        var deferred = $q.defer();

        $http.get("/api/yerpe/messages")
          .then(function (result) {
              // success
              angular.copy(result.data.UsersInRoom, _usersInRoom);
              _messages.push.apply(_messages, result.data.Messages);

              _messages.forEach(function (obj, i) {
                  obj.Message = $crypto.decrypt(obj.Message);
              });

              _myName = result.data.Name;
              _isAdmin = result.data.IsAdmin;
              _skip += 10;
              _isInit = true;
              _roomId = result.data.Room.Id
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getMoreMessages = function () {

        var deferred = $q.defer();

        $http.get("/api/yerpe/messages/" + _skip)
          .then(function (result) {
              // success
              result.data.Messages.forEach(function (obj, i) {
                  obj.Message = $crypto.decrypt(obj.Message);
              });

              _messages.unshift.apply(_messages, result.data.Messages);
              _skip += 10;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getRooms = function () {
        var deferred = $q.defer();

        $http.get("/api/room/")
          .then(function (result) {
              // success
              angular.copy(result.data, _rooms);
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _createRoom = function (name) {
        var deferred = $q.defer();

        $http.post("/api/room", name)
         .then(function (result) {
             // success
             _rooms.push(result.data);
             deferred.resolve();
         },
         function (result) {
             // error
             deferred.reject(result);
         });

        return deferred.promise;
    };

    var _getUsers = function () {

        var deferred = $q.defer();

        $http.get("/api/user/")
          .then(function (result) {
              // success
              angular.copy(result.data, _users);
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    }

    var _createUser = function (user) {
        var deferred = $q.defer();

        $http.post("/api/user", user)
         .then(function (result) {
             // success
             _users.push(result.data);
             deferred.resolve();
         },
         function (result) {
             // error
             deferred.reject(result);
         });

        return deferred.promise;
    };
    return {
        messages: _messages,
        message: _message,
        rooms: _rooms,
        usersInRoom: _usersInRoom,
        getMessages: _getMessages,
        getMoreMessages: _getMoreMessages,
        isReady: _isReady,
        unRead: _unRead,
        myName: _myName,
        getName: _getName,
        receivedMessage: _receivedMessage,
        getRooms: _getRooms,
        getIsAdmin: _getIsAdmin,
        createRoom: _createRoom,
        getUsers: _getUsers,
        users: _users,
        createUser: _createUser,
        getRoomId: _getRoomId
    };
}]);

yerpeModule.factory('yerpeHub', ['$rootScope', 'Hub', 'dataService', '$q', '$crypto', function ($rootScope, Hub, dataService, $q, $crypto) {

    //declaring the hub connection
    var hub = new Hub('YerpeHub', {

        //client side methods
        listeners: {
            'addNewMessageToPage': function (message) {
                message.Message = $crypto.decrypt(message.Message);
                dataService.receivedMessage = true;
                dataService.messages.push(message);
                var fullName = dataService.getName();
                if (message.From != fullName.substr(0, fullName.indexOf('@'))) {
                    dataService.unRead += 1;
                    $.titleAlert("(" + dataService.unRead + ")");
                }
                $rootScope.$apply();
            }
        },

        //server side methods
        methods: ['Send'],

        //handle connection error
        errorHandler: function (error) {
            console.error(error);
        },

    });

    var send = function (name, message) {
        var deferred = $q.defer();
        var encryptedMessage = $crypto.encrypt(message);
        deferred.resolve(hub.Send(name, encryptedMessage));
        return deferred.promise;
    };

    return {
        sendMessage: send
    };
}]);

yerpeModule.directive('globalEvents', function(dataService) {
    return function(scope, element, attrs) {
        element.bind('click', function(e) {
            dataService.unRead = 0;
        });
    };
});

yerpeModule.directive('autoFocus', function ($timeout) {
    return {
        restrict: 'AC',
        link: function (_scope, _element) {
            $timeout(function () {
                _element[0].focus();
            }, 0);
        }
    };
});

yerpeModule.controller("yerpeController", function($scope, $http, dataService, yerpeHub, $location, $anchorScroll, $window) {
    $scope.data = dataService;

    if (!dataService.isReady()) {
        dataService.getMessages()
            .then(function() {
                scrollToMessage();
            },
                function() {
                    alert("could not load messages");
                });
    }

    $scope.send = function() {
        if ($scope.data.message) {
            yerpeHub.sendMessage(dataService.getName(), $scope.data.message)
                .then(function() {
                    $scope.data.message = "";
                    scrollToMessage();
                });
        }
    };

    $scope.getMoreMessages = function() {
        dataService.getMoreMessages()
            .then(function(name) {
                //success
                scrollToTop();
            },
                function() {
                    //error
                    alert("could not load messages");
                });
    };

    $scope.logOff = function() {
        $http.post("/account/logoff")
            .then(function() {
                $window.location.reload();
            });
    };

    $scope.$watchCollection('data.messages', function(newMessages, oldMessages) {
        if (newMessages !== oldMessages && dataService.receivedMessage) {
            scrollToMessage();
            dataService.receivedMessage = false;
        }
    });

    var scrollToMessage = function() {
        $location.hash('message-' + (dataService.messages.length - 1));
        $anchorScroll();
    };

    var scrollToTop = function() {
        $location.hash('message-0');
        $anchorScroll.yOffset = -50;
        $anchorScroll();
    };

});

yerpeModule.controller("headController", function($scope, dataService, $window) {
    $scope.data = dataService;

    $window.onfocus = function() {
        dataService.unRead = 0;
    }

});

yerpeModule.controller("roomController", function($scope, dataService) {
    $scope.data = dataService;
    $scope.room = {};

    $scope.init = function() {
        dataService.getRooms();
    };

    $scope.createRoom = function() {
        dataService.createRoom($scope.room);
    };

});

yerpeModule.controller("userController", function($scope, dataService) {
    $scope.data = dataService;
    $scope.user = {};

    $scope.init = function() {
        dataService.getRooms();
        dataService.getUsers();
    };

    $scope.createUser = function() {
        dataService.createUser($scope.user);
    };

});