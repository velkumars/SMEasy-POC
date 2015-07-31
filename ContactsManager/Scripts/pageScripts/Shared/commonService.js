app.service('service', function ($http) {
     //Create new record
    this.post = function (entity, url) {
        var request = $http({
            method: "post",
            url: url,
            data: entity,
            
        });
        return request;
    };

    //Get Single Record
    this.get = function (id, url) {
        //return $http.get(url + "/" + id);
        var request = $http({
            method: "get",
            url: url + "/" + id,
            async: true
        })
        return request;
    };

    //Get All Records
    this.getEntityList = function (url) {
        var request = $http({
            method: "get",             
            url: url,
            async: true
        })
        return request;
    };

   // Get All Records by Filter
    this.getEntityListByFilter = function (searchString, url) {
        var request = $http({
            method: "get",
            url: url + "/" + searchString,          
            async: true
        })
        return request; 
    };

    this.bindDropDownList = function (url) {
        var request = $http({
            method: "get",
            url: url,
            async: true
        })
        return request;       
    };

    //this.bindCountry = function (url) {
    //    var request = $http({
    //        method: "get",
    //        url: url,
    //        async: true
    //    })
    //    return request;
    //};

    this.sendMail = function (mailId, url) {
        var request = $http({
            method: "get",
            url: url + "/" + mailId + "/",         
            async: true
        })
        return request; 
    };

    this.uploadFile = function (entity, url) {
        
        var request = $http({
            method: "post",
            url: url + "/" + entity,
            async: true
        })
        return request;
    };

    //this.changePassword = function (entity, url) {
    //    return $http.post(entity, url);
    //};

    //Update single Record
    this.put = function (id, entity, url) {
      var request = $http({
            method: "put",
            url: url + "/"+ id,
            data: entity
        });
        return request;
    };

    
    //Delete single Record
    this.delete = function (id, url) {
        debugger;
        var request = $http({
            method: "delete",
            url: url + "/" + id
         
        });
       return request;
    };

    
   


    //Get Entity List by Id
    this.getEntitybyId = function (url) {
            var request = $http({
                method: "get",
                url: url,
                async: true
            })
            return request;
    };

    
    //Get With Entity
    this.getEntitybyFilter= function (url,entity) {
        var request = $http({
            method: "post",
            url: url,
            async:true,
            data: entity
        })
        return request;
    };
});