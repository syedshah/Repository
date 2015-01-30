function user() {
    func = function () {   
    };
};

(function ($) {
    
    $('#password-popover').hide();
    
    $(document).ready(function () {
        $('#PasswordHelp').popover({ header: '#password-popover > .header', content: '#password-popover > .content' });
    });


    $(document).ready(function () {
        $("#SelectDomicile").change(function () {
            $("#UserManCoes").empty();
            var domicileId = $(this).val();
            var username = $('.UserNameHidden').val();
            user.retrieveManCoes(domicileId, username);
        });
    });
    
    $(document).ready(function () {
      var domicileId = $("#SelectDomicile").val();
      var username = $('.UserNameHidden').val();
        if (domicileId >= 1) {
            $("#UserManCoes").empty();
            user.retrieveManCoes(domicileId, username);
        }
    });

    user.retrieveManCoes = function (domicileId, username) {
        var ajaxObject = {
            data: { domicileId: domicileId, username: username },
            url: utility.getBaseUrl() + "/" + "User/RetrieveManCoes",
            type: "GET",
            dataType: "html",
            async: true,
            success: function (data, texStatus, jqXHR) {
                $("#UserManCoes").html(data);
            },
            error: function (jqXHR, texStatus, errorThrown) {
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    var initialize = function () {
        $(document).ready(function () {
            
            userManCoesDiv = $("#UserManCoes");
            domicileDropDown = $("#SelectDomicile");

        });
    };

    (function () {
        initialize();
    })();

})(jQuery);