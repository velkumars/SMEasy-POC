
app.controller('contactController', function ($scope, service, Constant, uiGridConstants) {
    var primaryListUrl = config.appServicesHostName + "/api/Contact/";
    var saveUrl = config.appServicesHostName + "/api/Contact/SaveContact/0";
    var updateUrl = config.appServicesHostName + "/api/Contact/UpdateContact/0";
    var deleteUrl = config.appServicesHostName + "/api/Contact/";

    $scope.datePickerOptions = {
         parseFormats: ["yyyy-MM-ddTHH:mm:ss"]      
    };

    var currentEntity;

    $scope.IsNewRecord = 1;
    $scope.hide = true;    
    getPrimaryEntityList(service, $scope, primaryListUrl);

     
    $scope.add = function () {
        currentEntity = null;
        $scope.revertDirty();
        $scope.IsNewRecord = 1;
        $scope.disabled = false;
        $scope.show = true;
        $scope.model = {};
        $scope.model.Born = "";
        $scope.model.IsActive = true;
        $scope.validationErrors = {};
        $('#btnSave').show();
    };

     
    $scope.PrimaryEntityList = {
        paginationPageSizes: [5, 10, 25, 50, 100],
        paginationPageSize: 10,
        enableRowSelection: true,
        multiSelect: false,
        enableRowHeaderSelection: false,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                $scope.selectedEntity = row.entity;
                    });
        },
        columnDefs: [
            { name: 'FirstName', displayName: 'First Name', enableFiltering: true,   enableHiding: false },
            { name: 'LastName', displayName: 'Last Name', enableFiltering: false,   enableHiding: false },
            { name: 'Email', displayName: 'E-Mail', enableFiltering: true,  enableHiding: false },
            { name: 'Cellphone', displayName: 'Cell Phone', enableFiltering: true,  enableHiding: false },
            { name: 'IsActive', displayName: 'Is Active', enableFiltering: true,   enableHiding: false },
            { name: 'Born', displayName: 'DOB', enableFiltering: true,   enableHiding: false, cellFilter: 'date:"MM/dd/yyyy"' },
        ],
        data: [

        ]
    };

    $scope.save = function () {
        if ($scope.IsNewRecord==1)
            Save(service, $scope, saveUrl, primaryListUrl);
        else
            Update(service, $scope, updateUrl, primaryListUrl);
    };

    $scope.edit = function (entity) {
        $scope.fillForm(entity);
        $scope.disabled = false;
        $scope.IsNewRecord = 0;
        $('#btnSave').show();
    };

    $scope.view = function (entity) {
        $scope.fillForm(entity);
        $scope.disabled = true;
        $('#btnSave').hide();
    };

    $scope.fillForm = function (entity) {
        validSelection(entity);
        currentEntity = angular.copy(entity); 0
        $scope.revertDirty();
        $scope.model = currentEntity;
        $scope.show = true;
        $scope.validationErrors = {};
    };

    $scope.cancel = function () {
        if ($scope.contactForm.$dirty) {
            swal({
                title: "There are some unsaved changes. Do you want continue without save?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes, Continue!",
                confirmButtonColor: "#DD6B55",
                closeOnConfirm: true
            }, $scope.back);
        }
        else {
            $scope.back();
        }
    };

    $scope.delete = function (id) {
        currentEntity = null;
        Delete(service, $scope, id, deleteUrl);
    };

    $scope.back = function () {
        $scope.model.Code = "";
        $scope.show = false;
        $scope.disabled = false;
        $scope.validationErrors = {};
        $scope.$digest();
        
    };
    $scope.revertDirty = function () {
        $scope.contactForm.$setPristine();
    }

    $scope.refresh = function () {
        currentEntity = null;        
        getPrimaryEntityList(service, $scope, primaryListUrl);
    };
 
    $scope.actionButtonClick = function (parameter) {
        actionButtonClick($scope, parameter, Constant)
    }

    getlayoutcontrols($scope);

})