function otf() {
    func = function () {
    };
};

(function($) {
  (function() {
    $(document).ready(function() {
      LoadEmails();
    });
  })();

  $(document).ready(function() {
    $("#SearchOtfs").on("click", Search);
  });
  
  var editClick = function () {
    $('.editOtf').click(function () {

        var origionalEmail = $("#OrigionalEmail").val();
        var email = $("#OtfEmail").val();

        if (origionalEmail != email) {
            $('#confirmOtfEditeModal').modal('show');

            $('button#otfEditconfirm').click(function (e) {

                if (email != $(".confirmEmail").val()) {
                    $("#confirmError").show();
                    return false;
                } else {
                    $('#confirmOtfDeleteModal').modal('hide');
                    otf.GetOtfData();
                }

                return false;
            });
        } else {
            otf.GetOtfData();
        }
        return false;
    });
  };

    otf.GetOtfData = function() {
        var id = $("#Id").val();
        var applicationId = $("#ApplicationId").val();
        var manCoId = $("#ManCoId").val();
        var docTypeId = $("#DocTypeId").val();
        var otfAccountNumber = $("#OtfAccountNumber").val();
        var email = $("#OtfEmail").val();

        var editAppManCoEmailViewModel = {
            Id: id,
            ApplicationId: applicationId,
            ManCoId: manCoId,
            DocTypeId: docTypeId,
            OtfAccountNumber: otfAccountNumber,
            OtfEmail: email
        };

        otf.editOTF(editAppManCoEmailViewModel);

        return false;
    }

    otf.editOTF = function (EditAppManCoEmailViewModel) {

    $.ajax({
      url: utility.getBaseUrl() + "/" + "Otf/Edit",
      type: 'POST',
      data: JSON.stringify({ editAppManCoEmailViewModel: EditAppManCoEmailViewModel }),
      dataType: 'html',
      contentType: 'application/json; charset=utf-8',
      error: function (xhr) {
        alert('Error: ' + xhr.statusText);
      },
      success: function (data) {
        var otfHome = $("#otfHome").data('request-url');
        window.location.href = otfHome + '?1=1';
      },
      async: true,
      processData: false
    });
    
    return false;
  };

  var deleteClick = function() {
    $('.DeleteOtf').click(function() {

      var editButton = this;

      $('#confirmOtfDeleteModal').modal('show');
      $('button#confirm').click(function(e) {
        $('#confirmOtfDeleteModal').modal('hide');
        var url = $("#deleteOtfUrl").data('request-url');
        var selectedAppManCoEmailId = $(editButton).next().val();

        $.post(url, { appManCoEmailId: selectedAppManCoEmailId },
          function(data) {
            var otfHome = $("#otfHome").data('request-url');
            window.location.href = otfHome + '?1=1';
          });
      });

      return false;
    });
  };

  function Search() {
    $("#otfs").empty();
    var accountNumber = $('#SelectedAccountNumber').val();
    var appId = $('#SelectApplication').val();
    var manCoId = $('#SelectManCo').val();
    var page = 1;
    otf.appManCoEmails(accountNumber, appId, manCoId, page);
  }

  function LoadEmails() {
    $("#otfs").empty();
    var accountNumber = $('#SelectedAccountNumber').val();
    var appId = $('#SelectApplication').val();
    var manCoId = $('#SelectManCo').val();
    var page = $('#Page').val();
    otf.appManCoEmails(accountNumber, appId, manCoId, page);
  }

  otf.appManCoEmails = function (accountNumber, appId, manCoId, page) {
    var ajaxObject = {
      data: { AccountNumber: accountNumber, AppId: appId, ManCoId: manCoId, Page: page, IsAjaxCall: true },
      url: utility.getBaseUrl() + "/" + "Otf/Show",
      type: "GET",
      dataType: "html",
      async: true,
      success: function(data, texStatus, jqXHR) {
        $("#otfs").html(data);

        deleteClick();
        editClick();
      },
      error: function(jqXHR, texStatus, errorThrown) {
      }
    };

    utility.sendAjaxRequest(ajaxObject);
  };

})(jQuery);