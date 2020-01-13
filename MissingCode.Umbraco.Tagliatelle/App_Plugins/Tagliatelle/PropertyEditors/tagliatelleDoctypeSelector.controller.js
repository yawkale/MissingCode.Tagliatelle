angular.module("umbraco")
.controller("Tagliatelle.PropertyEditors.tagliatelleDoctypeSelector",
    function ($http, $scope, notificationsService) {
        $scope.documentTypes = [];
        
        $scope.retreiveDocumentTypes = function () {
            $http.get("/umbraco/backoffice/tagliatelle/DocumentTypesApi/GetDocumentTypes")
                .then(function (response) {
                    $scope.documentTypes = response.data;
                }, function (error) {

                });
        };

        $scope.retreiveDocumentTypes();
    }
);

