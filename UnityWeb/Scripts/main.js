$(document).ready(function () {
    //$(".login").corner("9px top");
    //$(".login input.txt").corner("4px");
    //$(".login button").corner("4px");
    //$(".loginfooter").corner("9px bottom");
    //$("header").corner("9px bottom");
    //$("nav").corner("3px bottom");
    //$(".error").corner("4px");
    //$(".reduser").corner("4px bottom");
    //$(".blueuser").corner("4px bottom");
    //$(".greenuser").corner("4px bottom");
    //$(".gridsearch").corner("9px top");
    //$(".content").corner("9px top");
    //$(".gridbtn").corner("4px");
    //$(".filterbtn").corner("4px");
    //$(".morebtn").corner("4px");
    //$(".searchbtn").corner("4px");
    //$(".resetbtn").corner("4px");
    //$("section.accordion h3").corner("4px top");
    //$("footer").corner("9px bottom");
    //$(".hotsearch").corner("3px left");
    //$(".hotsearch2").corner("3px");
    //$(".search").corner("3px");
    //$("a.btnlogout").corner("3px");
    //$(".gridsearch .gridtxt input[type=text]").corner("3px");
    //$("section.accordion section.pane fieldset div input[type=text]").corner("3px");
    //$("section.accordion div.pane table").corner("5px bottom");
    //$(".insertbtn").corner("3px");
    //$(".ddl").corner("3px");
    //$(".ddlCreate").corner("3px");
    //$(".filtertxt").corner("3px");

    $body = $("body");

    $(document).ajaxStart(function () {
      //$body.addClass("loading");
    }).ajaxStop(function () {
      $body.removeClass("loading");
    });

    //Tooltips
    $('.warning').tooltip();

    //Accordion For .accordion
    $(".accordion h3").addClass("current");
    $(".accordion h3").click(function () {
        $(this).next('.pane').slideToggle(0300);
        $(this).toggleClass('current');
    });
    $(".accordion h3 + .pane").show(0);
    $(".accordion h3.closed + .pane").hide(0);
    $(".accordion h3.closed").removeClass("closed").removeClass("current");

    $(".accordionDash .pane").hide();

    $('table.subtable th').parents('table.subtable').children('tbody').hide();
    $('table.subtable th>span.moreinfo').click(function () {
        $(this).parents('table.subtable').children('tbody').toggle();
    });

    var searchError = $("#searchError").text();
    if (searchError.length > 0) {
        $('.search').show();
    } else {
        $('.search').hide();
    }
  

    var selectedSearchValid = $('#searchValid').val();
    if (selectedSearchValid.length > 0) {
        $('.search').show();
    }

    var searchError = $('#searchDocError').text();
    if (searchError.length > 10) {
        $('#documentData .accordion').hide();
        $('#documentData .pager').hide();
        $('#showingDocumentsSpan').hide();
    }

    //SEARCH
    $('.filterbtn').click(function () {
        $('.search').slideToggle(300);
    });

    $(":input[data-autocomplete]").each(function () {
        $(this).autocomplete({
            source: $(this).attr("data-autocomplete"),
            messages: {
                noResults: '',
                results: function () {
                }
            }
        });
    });

    $(":input[data-datepicker]").datepicker({ dateFormat: "dd/mm/yy" });

    var width = $(window).width();
    var height = $(window).height();

    $(function () {
        $('#selectAll').click(function () {
            var checkedStatus = this.checked;
            $('.docSelected').each(function () {
                $(this).prop('checked', checkedStatus);
            });
        });
    });

    var getParameterByName  = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }


    var dialoginitialize = function () {
        $("#results").dialog({
            width: width * 0.98,
            height: height * 0.98,
            resizable: false,
            position: 'center',
            autoOpen: false,
            modal: true,
        });
    };
  
    var dialoginitializeStatus = function () {
      $("#status").dialog({
        width: width * 0.50,
        //height: height * 0.50,
        resizable: false,
        position: 'center',
        autoOpen: false,
        modal: true,
      });
    };
  
    showExtraFields = function () {
      var selectedSubDocTypeName = $("#SelectedSubDocId option:selected").text();
      var selectedDocTypeName = $("#SelectedDocId option:selected").text();

      //Contract Notes
      if (selectedDocTypeName == "CNT") {
        $("#contractNotes").removeClass("hidden");
      } else {
        $("#contractNotes").addClass("hidden");
      }

      //Stock Transfer
      if (selectedDocTypeName == "CNF" && (selectedSubDocTypeName == "TOP" || selectedSubDocTypeName == "TOR" || selectedSubDocTypeName == "TOS")) {
        $("#stockTransfer").removeClass("hidden");
      } else {
        $("#stockTransfer").addClass("hidden");
      }

      //Tax Voucher
      if (selectedDocTypeName == "DST" && (selectedSubDocTypeName == "ACC" || selectedSubDocTypeName == "MAN" || selectedSubDocTypeName == "REI" || selectedSubDocTypeName == "VCH")) {
        $("#taxVoucher").removeClass("hidden");
      } else {
        $("#taxVoucher").addClass("hidden");
      }

      //Val And TranStatement
      if (selectedDocTypeName == "STM" && (selectedSubDocTypeName == "VAL" || selectedSubDocTypeName == "TRN")) {
        $("#valAndTranStatement").removeClass("hidden");
      } else {
        $("#valAndTranStatement").addClass("hidden");
      }

      //Maintenance Letters
      if (selectedDocTypeName == "LTR" && (selectedSubDocTypeName == "PIN" || selectedSubDocTypeName == "EML")) {
        $("#maintananceLetters").removeClass("hidden");
      } else {
        $("#maintananceLetters").addClass("hidden");
      }

      //Change of Name
      if (selectedDocTypeName == "LTR" && selectedSubDocTypeName == "CCN") {
        $("#changeOfName").removeClass("hidden");
      } else {
        $("#changeOfName").addClass("hidden");
      }

      //Remittance
      if (selectedDocTypeName == "CHQ" && (selectedSubDocTypeName == "RMT" || selectedSubDocTypeName == "DST")) {
        $("#remittance").removeClass("hidden");
      } else {
        $("#remittance").addClass("hidden");
      }

      //Change of Address

      //Perodic Statement
      if (selectedDocTypeName == "STM" && (selectedSubDocTypeName == "PER" || selectedSubDocTypeName == "AGT")) {
        $("#periodicStatement").removeClass("hidden");
      } else {
        $("#periodicStatement").addClass("hidden");
      }

      //Initial Commission
      if (selectedDocTypeName == "STM" && selectedSubDocTypeName == "CSI") {
        $("#initialCommission").removeClass("hidden");
      } else {
        $("#initialCommission").addClass("hidden");
      }

      //Renewal Commission
      if (selectedDocTypeName == "STM" && selectedSubDocTypeName == "CSR") {
        $("#renewalCommission").removeClass("hidden");
      } else {
        $("#renewalCommission").addClass("hidden");
      }

      //DVA Statement
      if (selectedDocTypeName == "STM" && selectedSubDocTypeName == "DVA") {
        $("#dvaStatement").removeClass("hidden");
      } else {
        $("#dvaStatement").addClass("hidden");
      }
    };

    dialoginitialize();
  dialoginitializeStatus();
    showExtraFields();

    var checkApprovalResults = function () {
        var documentsAlreadyApproved = $('#DocumentsAlreadyApproved').val();
        var arrAlreadyApproved = documentsAlreadyApproved.split(',');

        $.each(arrAlreadyApproved, function (index, value) {
            var docId = $('.docId[value=' + value + ']');
            var parentRow = $(docId).parent().parent();
            $(parentRow).addClass('red');
            $(parentRow).find('.warning').prop('title', 'Document already approved');
        });

        var documentsAlreadyCheckedOut = $('#DocumentsAlreadyCheckedOut').val();
        var arrAlreadyCheckedOut = documentsAlreadyCheckedOut.split(',');

        $.each(arrAlreadyCheckedOut, function (index, value) {
            var docId = $('.docId[value=' + value + ']');
            var parentRow = $(docId).parent().parent();
            $(parentRow).addClass('red');
            $(parentRow).find('.warning').prop('title', 'Document is already checked out');
        });

        var documentsDiffMancoInBasket = $('#BasketContainsDocumentFromAnotherManCo').val();
        var arrDocumentsDiffMancoInBasket = documentsDiffMancoInBasket.split(',');

        $.each(arrDocumentsDiffMancoInBasket, function (index, value) {
          var docId = $('.docId[value=' + value + ']');

          if (docId.length > 0) {
            $('#manCoConflictModal').modal('show');
            $('button#continue').click(function (e) {
              $('#manCoConflictModal').modal('hide');
            });
          }

          var parentRow = $(docId).parent().parent();
          $(parentRow).addClass('red');
          $(parentRow).find('.warning').prop('title', 'Basket aldready contains documents for a different man co');
        });

        var documentsCheckedOutByOtherPerson = $('#DocumentsCheckedOutByOtherUser').val();
        var arrdocumentsCheckedOutByOtherUser = documentsCheckedOutByOtherPerson.split(',');

        $.each(arrdocumentsCheckedOutByOtherUser, function (index, value) {
            var docId = $('.docId[value=' + value + ']');
            var parentRow = $(docId).parent().parent();
            $(parentRow).addClass('red');
            $(parentRow).find('.warning').prop('title', 'Document is already checked out by another user');
        });

        var documentsAlreadyRejected = $('#DocumentsAlreadyRejected').val();
        var arrdocumentsAlreadyRejected = documentsAlreadyRejected.split(',');

        $.each(arrdocumentsAlreadyRejected, function (index, value) {
            var docId = $('.docId[value=' + value + ']');
            var parentRow = $(docId).parent().parent();
            $(parentRow).addClass('red');
            $(parentRow).find('.warning').prop('title', 'Document is already rejected');
        });

        var documentsApproved = $('#DocumentsApproved').val();

        if (documentsApproved != "0") {
            $("<p> " + documentsApproved + " documents were successfully approved." + "</p> ").insertBefore($("#totalNumberOfDocuments"))
        }

        var documentsRejected = $('#DocumentsRejected').val();

        if (documentsRejected != "0") {
            $("<p> " + documentsRejected + " documents were successfully rejected." + "</p> ").insertBefore($("#totalNumberOfDocuments"))
        }
    };

    var refreshGridSearch = function () {

        var newPage = $('span.current').text();
        var grid = $('#GridSearch').val();

        var url;

        if (grid.length > 0) {
            //url = $("#documentGridSearch").data('request-url');

          url = window.location.href;

            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify({ grid: grid, page: newPage, isAjaxCall: true }),
                dataType: 'html',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                },
                success: function (data) {
                    $('#documentData').empty();
                    $('#documentData').html(data);

                    checkApprovalResults();

                    dialoginitialize();
                    dialoginitializeStatus();
                },
                async: true,
                processData: false
            });
        } else {
            url = $("#documentSearch").data('request-url');

            var SearchViewModel = {
                SelectedDocText: $('#SelectedDocText').val(),
                SelectedSubDocText: $('#SelectedSubDocText').val(),
                SelectedManCoText: $('#SelectedManCoText').val(),
                AddresseeSubType: $('#AddresseeSubType').val(),
                MailingName: $('#MailingName').val(),
                ProcessingDate: $('#ProcessingDate').val(),
                PrimaryHolder: $('#PrimaryHolder').val(),
                InvestorReference: $('#InvestorReference').val(),
                AccountNumber: $('#AccountNumber').val(),
                AgentReference: $('#AgentReference').val(),
                AddresseePostCode: $('#AddresseePostCode').val(),
                EmailAddress: $('#EmailAddress').val(),
                FaxNumber: $('#FaxNumber').val(),
                ContractDate: $('#ContractDate').val(),
                PaymentDate: $('#PaymentDate').val(),
                DocumentNumber: $('#DocumentNumber').val(),
                AccountPeriodEnd: $('#AccountPeriodEnd').val(),
                DocumentNumber: $('#DocumentNumber').val(),
                ProcessingDateFrom: $('#ProcessingDateFrom').val(),
                ProcessingDateTo: $('#ProcessingDateTo ').val()
            };

            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify({ SearchViewModel: SearchViewModel, page: newPage, isAjaxCall: true }),
                dataType: 'html',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                },
                success: function (data) {
                    $('#documentData').empty();
                    $('#documentData').html(data);

                    checkApprovalResults();

                    dialoginitialize();
                    dialoginitializeStatus();
                },
                async: true,
                processData: false
            });
        }
        return false;
    };


    var refreshBasketDocuments = function () {

        var newPage = $('span.current').text();

        var url;

        url = $("#showCartItemsUrl").data('request-url');

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ page: newPage, isAjaxCall: true }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
                alert('Error: ' + xhr.statusText); j
            },
            success: function (data) {

                $('#cartDocumentData').empty();
                $('#cartDocumentData').html(data);

                if ($('#totalNumberOfDocuments span').text() == 'Total Number Of Documents 0') {

                    var dashUrl = $("#dashboardHomeUrl").data('request-url');
                    window.location.href = dashUrl;
                }

                checkApprovalResults();
                dialoginitialize();
                dialoginitializeStatus();
            },
            async: true,
            processData: false
        });

        return false;
    };


    $(".pageLink").click(function () {
        var selectedValue = $(this).text();
        alert(selectedValue);
    });

    $(function () {
        $('#SelectedDocId').change(function () {
            var selectedDocType = $(this).val();
            var url = $("#searchSubDocType").data('request-url');
            if (selectedDocType != null && selectedDocType != '') {
                $.getJSON(url, { docTypeId: selectedDocType }, function (subDocTypes) {
                    var subDocTypesSelect = $('#SelectedSubDocId');
                    subDocTypesSelect.empty();
                    subDocTypesSelect.append($('<option/>', {
                        value: "",
                        text: "DOC SUB TYPE"
                    }));
                    $.each(subDocTypes, function (index, code) {
                        subDocTypesSelect.append($('<option/>', {
                            value: code.Id,
                            text: code.Code
                        }));
                    });
                });
                showExtraFields();
            }
        });
    });

    $(function () {
        $('#formSearch').submit(function () {

            $("#SelectedDocText").val("");
            $("#SelectedSubDocText").val("");
            $("#SelectedManCoText").val("");

            var selectedSubDocTypeName = $("#SelectedSubDocId option:selected").text();
            var selectedDocTypeName = $("#SelectedDocId option:selected").text();
            var selectedManCoName = $("#SelectedManCoId option:selected").text();

            if (selectedDocTypeName != "DOC TYPE") {
                var selTypeTextDoc = $('#SelectedDocId option:selected').text();
                $("#SelectedDocText").val(selTypeTextDoc);
            } else {
            }
            if (selectedSubDocTypeName != "DOC SUB TYPE") {
                var selTypeTextSubDoc = $('#SelectedSubDocId option:selected').text();
                $("#SelectedSubDocText").val(selTypeTextSubDoc);
            }
            if (selectedManCoName != "MAN CO") {
              var selectedManCo = $('#SelectedManCoId option:selected').text().split(" ");
              var selTypeTextManCo = selectedManCo[0];
                $("#SelectedManCoText").val(selTypeTextManCo);
            }
        });
    });

    $(function () {
      $('#addToCart').click(function () {

        if ($(".documents input:checkbox:checked").length > 0) {
          $body.addClass("loading");

          var url = $("#addToCartUrl").data('request-url');

          var addCartItemViewModel = [];

          $('.documents tr.data').each(function () {
            addCartItemViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
              DocumentId: $(this).find('.documentId').val()
            });
          });

          var AddCartItemsViewModel = {
            AddCartItemViewModel: addCartItemViewModel
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ addCartItemsViewModel: AddCartItemsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {

              $('#cart').empty();
              $('#cart').html(data);

              refreshGridSearch();
            },
            async: true,
            processData: false
          });
          return false;
        } else {
          $('#pleaseSelectModal').modal('show');
          
          $('button#continue').click(function (e) {
            $('#pleaseSelectModal').modal('hide');
          });
          return false;
        }
      });
    });
  
    $(function () {
      $('#addGridToCart').click(function () {
 
          $body.addClass("loading");

          var url = $("#addGRIDToCartUrl").data('request-url');
          var grid = $("#GridSearch").val();

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ grid: grid }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {

              $('#cart').empty();
              $('#cart').html(data);

              refreshGridSearch();
            },
            async: true,
            processData: false
          });
          return false;
      });
    });
  
    $('body').on('click', '#removeFromCart', function () {

      var url = $("#removeFromCartUrl").data('request-url');

      var removeCartItemViewModel = [];

      $('.documents tr.data').each(function () {
        removeCartItemViewModel.push({
          DocumentId: $(this).find('.docId').val(),
          Selected: $(this).find('.docSelected').is(':checked'),
          ManCo: $(this).find('.manCo').val(),
          DocType: $(this).find('.docType').val(),
          SubDocType: $(this).find('.subDocType').val(),
          DocumentId: $(this).find('.documentId').val()
        });
      });

      var RemoveCartItemsViewModel = {
        RemoveCartItemViewModel: removeCartItemViewModel
      };

      $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify({ removeCartItemsViewModel: RemoveCartItemsViewModel }),
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
          alert('Error: ' + xhr.statusText);
        },
        success: function (data) {

          $('#cart').empty();
          $('#cart').html(data);

          refreshBasketDocuments();
        },
        async: true,
        processData: false
      });
      return false;
    });

    $(function () {
      $('#removeFromCart').click(function () {
          var url = $("#removeFromCartUrl").data('request-url');

          var removeCartItemViewModel = [];

          $('.documents tr.data').each(function () {
            removeCartItemViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
              DocumentId: $(this).find('.documentId').val()
            });
          });

          var RemoveCartItemsViewModel = {
            RemoveCartItemViewModel: removeCartItemViewModel
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ removeCartItemsViewModel: RemoveCartItemsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {

              $('#cart').empty();
              $('#cart').html(data);

              refreshBasketDocuments();
            },
            async: true,
            processData: false
          });
          return false;
       // });
       // return false;
      });
    });


  $('body').on('click', '#approveSelected', function() {

    if ($(".documents input:checkbox:checked").length > 0) {
      $('#approveSelectedModal').modal('show');

      $('button#confirmApproveSelected').click(function (e) {
        $('#approveSelectedModal').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $body.addClass("loading");

        var newPage = $('span.current').text();
        var grid = $('#GridSearch').val();
        var url = $("#approveDocumentUrl").data('request-url');

        var approveDocumentViewModel = [];

        $('.documents tr.data').each(function() {
          approveDocumentViewModel.push({
            DocumentId: $(this).find('.docId').val(),
            Selected: $(this).find('.docSelected').is(':checked'),
            ManCo: $(this).find('.manCo').val(),
            DocType: $(this).find('.docType').val(),
            SubDocType: $(this).find('.subDocType').val(),
            DocumentId: $(this).find('.documentId').val(),
          });
        });

        var ApproveDocumentsViewModel = {
          ApproveDocumentViewModel: approveDocumentViewModel,
          Grid: grid,
          Page: newPage
        };

        $.ajax({
          url: url,
          type: 'POST',
          data: JSON.stringify({ approveDocumentsViewModel: ApproveDocumentsViewModel }),
          dataType: 'html',
          contentType: 'application/json; charset=utf-8',
          error: function(xhr) {
            alert('Error: ' + xhr.statusText);
          },
          success: function(data) {
            $('#documentWarning').empty();
            $('#documentWarning').html(data);

            var dashUrl = $("#dashboardHomeUrl").data('request-url');

            if (dashUrl != null) {
              refreshBasketDocuments();
            } else {
              refreshGridSearch();
            }
          },
          async: true,
          processData: false
        });
        return false;
      });
      return false;
    } else {
      $('#pleaseSelectModal').modal('show');

      $('button#continue').click(function(e) {
        $('#pleaseSelectModal').modal('hide');
      });
      return false;
    }
  });
    

    $(function () {
      $('#approveSelected').click(function () {

        if ($(".documents input:checkbox:checked").length > 0) {
          $('#approveSelectedModal').modal('show');

          $('button#confirmApproveSelected').click(function (e) {
            $('#approveSelectedModal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $body.addClass("loading");

            var newPage = $('span.current').text();
            var grid = $('#GridSearch').val();
            var url = $("#approveDocumentUrl").data('request-url');

            var approveDocumentViewModel = [];

            $('.documents tr.data').each(function () {
              approveDocumentViewModel.push({
                DocumentId: $(this).find('.docId').val(),
                Selected: $(this).find('.docSelected').is(':checked'),
                ManCo: $(this).find('.manCo').val(),
                DocType: $(this).find('.docType').val(),
                SubDocType: $(this).find('.subDocType').val(),
                DocumentId: $(this).find('.documentId').val(),
              });
            });

            var ApproveDocumentsViewModel = {
              ApproveDocumentViewModel: approveDocumentViewModel,
              Grid: grid,
              Page: newPage
            };

            $.ajax({
              url: url,
              type: 'POST',
              data: JSON.stringify({ approveDocumentsViewModel: ApproveDocumentsViewModel }),
              dataType: 'html',
              contentType: 'application/json; charset=utf-8',
              error: function (xhr) {
                alert('Error: ' + xhr.statusText);
              },
              success: function (data) {
                $('#documentWarning').empty();
                $('#documentWarning').html(data);

                var dashUrl = $("#dashboardHomeUrl").data('request-url');

                if (dashUrl != null) {
                  refreshBasketDocuments();
                } else {
                  refreshGridSearch();
                }
              },
              async: true,
              processData: false
            });
            return false;
          });
          return false;
        } else {
          $('#pleaseSelectModal').modal('show');

          $('button#continue').click(function (e) {
            $('#pleaseSelectModal').modal('hide');
          });
          return false;
        }
      });
    });


    $('body').on('click', '#rejectSelected', function () {

      if ($(".documents input:checkbox:checked").length > 0) {
        $('#rejectSelectedModal').modal('show');

        $('button#confirmRejectSelected').click(function (e) {
          $('#rejectSelectedModal').modal('hide');
          $('body').removeClass('modal-open');
          $('.modal-backdrop').remove();
          $body.addClass("loading");

          var newPage = $('span.current').text();
          var grid = $('#GridSearch').val();
          var url = $("#rejectDocumentUrl").data('request-url');

          var rejectDocumentViewModel = [];

          $('.documents tr.data').each(function() {
            rejectDocumentViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
            });
          });

          var RejectDocumentsViewModel = {
            RejectDocumentViewModel: rejectDocumentViewModel,
            Grid: grid,
            Page: newPage
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ rejectDocumentsViewModel: RejectDocumentsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function(xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function(data) {
              $('#documentWarning').empty();
              $('#documentWarning').html(data);

              var dashUrl = $("#dashboardHomeUrl").data('request-url');

              if (dashUrl != null) {
                refreshBasketDocuments();
              } else {
                refreshGridSearch();
              }
            },
            async: true,
            processData: false
          });
          return false;
        });
        return false;
      } else {
        $('#pleaseSelectModal').modal('show');

        $('button#continue').click(function (e) {
          $('#pleaseSelectModal').modal('hide');
        });
        return false;
      }
    });
   

    $(function () {
      $('#rejectSelected').click(function () {

        if ($(".documents input:checkbox:checked").length > 0) {

          $('#rejectSelectedModal').modal('show');

          $('button#confirmRejectSelected').click(function (e) {
            $('#rejectSelectedModal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $body.addClass("loading");

            var newPage = $('span.current').text();
            var grid = $('#GridSearch').val();
            var url = $("#rejectDocumentUrl").data('request-url');

            var rejectDocumentViewModel = [];

            $('.documents tr.data').each(function() {
              rejectDocumentViewModel.push({
                DocumentId: $(this).find('.docId').val(),
                Selected: $(this).find('.docSelected').is(':checked'),
                ManCo: $(this).find('.manCo').val(),
                DocType: $(this).find('.docType').val(),
                SubDocType: $(this).find('.subDocType').val(),
              });
            });

            var RejectDocumentsViewModel = {
              RejectDocumentViewModel: rejectDocumentViewModel,
              Grid: grid,
              Page: newPage
            };

            $.ajax({
              url: url,
              type: 'POST',
              data: JSON.stringify({ rejectDocumentsViewModel: RejectDocumentsViewModel }),
              dataType: 'html',
              contentType: 'application/json; charset=utf-8',
              error: function(xhr) {
                alert('Error: ' + xhr.statusText);
              },
              success: function(data) {
                $('#documentWarning').empty();
                $('#documentWarning').html(data);

                var dashUrl = $("#dashboardHomeUrl").data('request-url');

                if (dashUrl != null) {
                  refreshBasketDocuments();
                } else {
                  refreshGridSearch();
                }
              },
              async: true,
              processData: false
            });
            return false;
          });
          return false;
        } else {
          $('#pleaseSelectModal').modal('show');

          $('button#continue').click(function (e) {
            $('#pleaseSelectModal').modal('hide');
          });
          return false;
        }
      });
    });

    $('body').on('click', '#rejectBasket', function () {

      $('#rejectBasketModal').modal('show');

        $('button#confirm').click(function (e) {
          $('#rejectBasketModal').modal('hide');
          $('body').removeClass('modal-open');
          $('.modal-backdrop').remove();
          $body.addClass("loading");

          var newPage = $('span.current').text();
          var grid = $('#GridSearch').val();
          var url = $("#rejectBasketUrl").data('request-url');

          var rejectDocumentViewModel = [];

          $('.documents tr.data').each(function () {
            rejectDocumentViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
            });
          });

          var RejectDocumentsViewModel = {
            RejectDocumentViewModel: rejectDocumentViewModel,
            Grid: grid,
            Page: newPage
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ rejectDocumentsViewModel: RejectDocumentsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
              $('#documentWarning').empty();
              $('#documentWarning').html(data);

              var dashUrl = $("#dashboardHomeUrl").data('request-url');

              if (dashUrl != null) {
                refreshBasketDocuments();
              } else {
                refreshGridSearch();
              }
            },
            async: true,
            processData: false
          });
          return false;
        });
        return false;
      });
   
    $(function () {
      $('#rejectBasket').click(function () {

        $('#rejectBasketModal').modal('show');

        $('button#confirm').click(function (e) {
          $('#rejectBasketModal').modal('hide');
          $('body').removeClass('modal-open');
          $('.modal-backdrop').remove();
          $body.addClass("loading");

          var newPage = $('span.current').text();
          var grid = $('#GridSearch').val();
          var url = $("#rejectBasketUrl").data('request-url');

          var rejectDocumentViewModel = [];

          $('.documents tr.data').each(function () {
            rejectDocumentViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
            });
          });

          var RejectDocumentsViewModel = {
            RejectDocumentViewModel: rejectDocumentViewModel,
            Grid: grid,
            Page: newPage
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ rejectDocumentsViewModel: RejectDocumentsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
              $('#documentWarning').empty();
              $('#documentWarning').html(data);

              var dashUrl = $("#dashboardHomeUrl").data('request-url');

              if (dashUrl != null) {
                refreshBasketDocuments();
              } else {
                refreshGridSearch();
              }
            },
            async: true,
            processData: false
          });
          return false;
        });
        return false;
      });
    });
  
    $('body').on('click', '#approveBasket', function () {

      $('#approveBasketModal').modal('show');

        $('button#confirm').click(function (e) {
          $('#approveBasketModal').modal('hide');
          $('body').removeClass('modal-open');
          $('.modal-backdrop').remove();
          $body.addClass("loading");

          var newPage = $('span.current').text();
          var grid = $('#GridSearch').val();
          var url = $("#approveBasketUrl").data('request-url');

          var approveDocumentViewModel = [];

          $('.documents tr.data').each(function () {
            approveDocumentViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
            });
          });

          var ApproveDocumentsViewModel = {
            ApproveDocumentViewModel: approveDocumentViewModel,
            Grid: grid,
            Page: newPage
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ approveDocumentsViewModel: ApproveDocumentsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
              $('#documentWarning').empty();
              $('#documentWarning').html(data);

              var dashUrl = $("#dashboardHomeUrl").data('request-url');

              if (dashUrl != null) {
                refreshBasketDocuments();
              } else {
                refreshGridSearch();
              }
            },
            async: true,
            processData: false
          });
          return false;
        });
        return false;
      });
 

    $(function () {
      $('#approveBasket').click(function () {

        $('#approveBasketModal').modal('show');

        $('button#confirm').click(function (e) {
          $('#approveBasketModal').modal('hide');
          $('body').removeClass('modal-open');
          $('.modal-backdrop').remove();
          $body.addClass("loading");

          var newPage = $('span.current').text();
          var grid = $('#GridSearch').val();
          var url = $("#approveBasketUrl").data('request-url');

          var approveDocumentViewModel = [];

          $('.documents tr.data').each(function () {
            approveDocumentViewModel.push({
              DocumentId: $(this).find('.docId').val(),
              Selected: $(this).find('.docSelected').is(':checked'),
              ManCo: $(this).find('.manCo').val(),
              DocType: $(this).find('.docType').val(),
              SubDocType: $(this).find('.subDocType').val(),
            });
          });

          var ApproveDocumentsViewModel = {
            ApproveDocumentViewModel: approveDocumentViewModel,
            Grid: grid,
            Page: newPage
          };

          $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ approveDocumentsViewModel: ApproveDocumentsViewModel }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
              alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
              $('#documentWarning').empty();
              $('#documentWarning').html(data);

              var dashUrl = $("#dashboardHomeUrl").data('request-url');

              if (dashUrl != null) {
                refreshBasketDocuments();
              } else {
                refreshGridSearch();
              }
            },
            async: true,
            processData: false
          });
          return false;
        });
        return false;
      });
    });

    $(function () {
      $('#rejectGrid').click(function () {

          $('#rejectGridModal').modal('show');

          $('button#confirm').click(function (e) {
            $('#rejectGridModal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $body.addClass("loading");

            var grid = $('#GridSearch').val();
            var url = $("#rejectGridUrl").data('request-url');

            $.ajax({
              url: url,
              type: 'POST',
              data: JSON.stringify({ grid: grid }),
              dataType: 'html',
              contentType: 'application/json; charset=utf-8',
              error: function (xhr) {
                alert('Error: ' + xhr.statusText);
              },
              success: function (data) {
                $('#documentWarning').empty();
                $('#documentWarning').html(data);

                var dashUrl = $("#dashboardHomeUrl").data('request-url');

                if (dashUrl != null) {
                  refreshBasketDocuments();
                } else {
                  refreshGridSearch();
                }
              },
              async: true,
              processData: false
            });
            return false;
          });
          return false;
      });
    });


    $(function () {
      $('#approveGrid').click(function () {
    
          $('#approveGridModal').modal('show');

          $('button#confirm').click(function (e) {
            $('#approveGridModal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $body.addClass("loading");

            var grid = $('#GridSearch').val();
            var url = $("#approveGridUrl").data('request-url');

            $.ajax({
              url: url,
              type: 'POST',
              data: JSON.stringify({ grid: grid }),
              dataType: 'html',
              contentType: 'application/json; charset=utf-8',
              error: function (xhr) {
                alert('Error: ' + xhr.statusText);
              },
              success: function (data) {
                $('#documentWarning').empty();
                $('#documentWarning').html(data);

                var dashUrl = $("#dashboardHomeUrl").data('request-url');

                if (dashUrl != null) {
                  refreshBasketDocuments();
                } else {
                  refreshGridSearch();
                }
              },
              async: true,
              processData: false
            });
            return false;
          });
          return false;
      });
    });

    $(function () {
        $('#docPager a').click(function () {

            var filterApproved = false;
            filterApproved = getParameterByName('filterApproved');

            var newLink = $(this).attr('href');

            var newPage = newLink.split('=')[1];

            if (typeof newPage === "undefined") {
                newPage = 1;
            }

            var SearchViewModel = {
                SelectedDocText: $('#SelectedDocText').val(),
                SelectedSubDocText: $('#SelectedSubDocText').val(),
                SelectedManCoText: $('#SelectedManCoText').val(),
                AddresseeSubType: $('#AddresseeSubType').val(),
                MailingName: $('#MailingName').val(),
                ProcessingDate: $('#ProcessingDate').val(),
                PrimaryHolder: $('#PrimaryHolder').val(),
                InvestorReference: $('#InvestorReference').val(),
                AccountNumber: $('#AccountNumber').val(),
                AgentReference: $('#AgentReference').val(),
                AddresseePostCode: $('#AddresseePostCode').val(),
                EmailAddress: $('#EmailAddress').val(),
                FaxNumber: $('#FaxNumber').val(),
                ContractDate: $('#ContractDate').val(),
                PaymentDate: $('#PaymentDate').val(),
                DocumentNumber: $('#DocumentNumber').val(),
                AccountPeriodEnd: $('#AccountPeriodEnd').val(),
                DocumentNumber: $('#DocumentNumber').val(),
                ProcessingDateFrom: $('#ProcessingDateFrom').val(),
                ProcessingDateTo: $('#ProcessingDateTo ').val()
            };

            var grid = $('#GridSearch').val();

            var url;
            var selectedSearchValid = $("#searchValid").val();
            if (selectedSearchValid.length == 0) {
                url = $("#documentGridSearch").data('request-url');

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: JSON.stringify({ grid: grid, page: newPage, isAjaxCall: true, filterHouseHolding: false, filterUnapproved: false, filterApproved: filterApproved }),
                    dataType: 'html',
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr) {
                        alert('Error: ' + xhr.statusText);
                    },
                    success: function (data) {
                        $('#documentData').empty();
                        $('#documentData').html(data);

                        dialoginitialize();
                        dialoginitializeStatus();
                    },
                    async: true,
                    processData: false
                });
            } else {
                url = $("#documentSearch").data('request-url');

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: JSON.stringify({ SearchViewModel: SearchViewModel, page: newPage, isAjaxCall: true }),
                    dataType: 'html',
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr) {
                        alert('Error: ' + xhr.statusText);
                    },
                    success: function (data) {
                        $('#documentData').empty();
                        $('#documentData').html(data);

                        dialoginitialize();
                        dialoginitializeStatus();
                    },
                    async: true,
                    processData: false
                });
            }
            return false;
        });
    });

    $(function () {
      $('.showGridStatus').click(function () {

        var url = $("#showGridRunStatusUrl").data('request-url');

        var GridRunId = $(this).next().val();

        $.ajax({
          url: url,
          type: 'POST',
          data: JSON.stringify({ GridRunId: GridRunId }),
          dataType: 'html',
          contentType: 'application/json; charset=utf-8',
          error: function (xhr) {
            alert('Error: ' + xhr.statusText);
          },
          success: function (data) {
            $('#status').html(data).dialog('open');
          },
          async: true,
          processData: false
        });

        return false;
      });
    });

    $(function () {
      $('.showDocumentStatus').click(function () {

        var url = $("#showDocumentStatusUrl").data('request-url');

        var DocumentId = $(this).next().val();

        $.ajax({
          url: url,
          type: 'POST',
          data: JSON.stringify({ DocumentId: DocumentId }),
          dataType: 'html',
          contentType: 'application/json; charset=utf-8',
          error: function (xhr) {
            alert('Error: ' + xhr.statusText);
          },
          success: function (data) {
            $('#status').html(data).dialog('open');
          },
          async: true,
          processData: false
        });

        return false;
      });
    });

    $('body').on('click', '.showDocumentStatus', function () {
      var url = $("#showDocumentStatusUrl").data('request-url');

      var DocumentId = $(this).next().val();

      $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify({ DocumentId: DocumentId }),
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
          alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
          $('#status').html(data).dialog('open');
        },
        async: true,
        processData: false
      });

      return false;
    });

    $('body').on('click', '#selectAll', function () {
        var checkedStatus = this.checked;
        $('.docSelected').each(function () {
            $(this).prop('checked', checkedStatus);
        });
    });

    $(function () {
        $('.showDocument').click(function () {

            var url = $("#showPDFUrl").data('request-url');

            var DocumentId = $(this).next().val();

            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify({ DocumentId: DocumentId }),
                dataType: 'html',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                },
                success: function (data) {
                    $('#results').html(data).dialog('open');
                },
                async: true,
                processData: false
            });

            return false;
        });
    });

    $('body').on('click', '.showDocument', function () {

        var url = $("#showPDFUrl").data('request-url');

        var DocumentId = $(this).next().val();

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ DocumentId: DocumentId }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                $('#results').html(data).dialog('open');
            },
            async: true,
            processData: false
        });

        return false;
    });

    $('body').on('click', '#docPager a', function () {
        var filterApproved = false;
        filterApproved = getParameterByName('filterApproved');

        var newLink = $(this).attr('href');
        var newPage = newLink.split('=')[1];

        if (typeof newPage === "undefined") {
            newPage = 1;
        }

        var SearchViewModel = {
            SelectedDocText: $('#SelectedDocText').val(),
            SelectedSubDocText: $('#SelectedSubDocText').val(),
            SelectedManCoText: $('#SelectedManCoText').val(),
            AddresseeSubType: $('#AddresseeSubType').val(),
            MailingName: $('#MailingName').val(),
            ProcessingDate: $('#ProcessingDate').val(),
            PrimaryHolder: $('#PrimaryHolder').val(),
            InvestorReference: $('#InvestorReference').val(),
            AccountNumber: $('#AccountNumber').val(),
            AgentReference: $('#AgentReference').val(),
            AddresseePostCode: $('#AddresseePostCode').val(),
            EmailAddress: $('#EmailAddress').val(),
            FaxNumber: $('#FaxNumber').val(),
            ContractDate: $('#ContractDate').val(),
            PaymentDate: $('#PaymentDate').val(),
            DocumentNumber: $('#DocumentNumber').val(),
            AccountPeriodEnd: $('#AccountPeriodEnd').val(),
            DocumentNumber: $('#DocumentNumber').val(),
            ProcessingDateFrom: $('#ProcessingDateFrom').val(),
            ProcessingDateTo: $('#ProcessingDateTo ').val()
        };

        var grid = $('#GridSearch').val();

        var url;
        var selectedSearchValid = $("#searchValid").val();
        if (selectedSearchValid.length == 0) {
            url = $("#documentGridSearch").data('request-url');

            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify({ grid: grid, page: newPage, isAjaxCall: true, filterHouseHolding: false, filterUnapproved: false, filterApproved: filterApproved }),
                dataType: 'html',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                },
                success: function (data) {
                    $('#documentData').empty();
                    $('#documentData').html(data);

                    dialoginitialize();
                    dialoginitializeStatus();
                },
                async: true,
                processData: false
            });
        } else {
            url = $("#documentSearch").data('request-url');

            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify({ SearchViewModel: SearchViewModel, page: newPage, isAjaxCall: true }),
                dataType: 'html',
                contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                },
                success: function (data) {
                    $('#documentData').empty();
                    $('#documentData').html(data);

                    dialoginitialize();
                    dialoginitializeStatus();
                },
                async: true,
                processData: false
            });
        }
        return false;
    });

    $(function () {
        $('#SelectedSubDocId').change(function () {
            var selectedSubDocType = $(this).val();
            if (selectedSubDocType != null && selectedSubDocType != '') {
                showExtraFields();
            }
        });
    });

    //MArquee
    $('.marquee').marquee({
        speed: 15000
    });

    //Placeholder fix
    $('input, textarea').placeholder();

});
