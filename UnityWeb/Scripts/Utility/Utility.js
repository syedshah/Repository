var utility = function () {

};

(function ($) {

    var siteName;

    utility.sendAjaxRequest = function (ajaxObject) {
        var data, url, dataType, type, contentType, async, traditional;
        traditional = false;
        var done = function () {
        };

        var fail = function () {
        };

        data = ajaxObject.data;
        url = ajaxObject.url;
        dataType = ajaxObject.dataType;
        type = ajaxObject.type;
        done = ajaxObject.success;
        fail = ajaxObject.error;
        if (ajaxObject.traditional != undefined) {
            traditional = ajaxObject.traditional;
        }

        $.ajax({
            type: type,
            url: url,
            data: data,
            dataType: dataType,
            contentType: contentType,
            async: async,
            traditional : traditional
        })
        .done(done)
        .fail(fail)
        .always();

    };

    utility.onSuccess = function (data, receiver) {
        receiver.success(data);
    };

    utility.onError = function (data, receiver) {
        receiver.success(data);
    };

    utility.getBaseUrl = function(address) {
        var path = location.protocol + "//" + location.host;
        var websitename = $("#SiteName").val();

        if (websitename != "/") {
            path = path + websitename;
        }

        if (address != undefined && address != "") {
            path = path + "/" + address;
        }

        return path;

    };

    utility.getSplit = function(dataString, delimiter) {
        return dataString.split(delimiter);
    };

    utility.getItemInArray = function(dataArray, itemNumber) {
        return dataArray[itemNumber];
    };

    var initialize = function () {
        siteName = $("#SiteName").val();
    };

    (function () {
        initialize();
    })();

})(jQuery);