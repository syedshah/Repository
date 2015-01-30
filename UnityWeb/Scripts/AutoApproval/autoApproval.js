function autoApproval() {
    func = function () {
    };
};

(function ($) {

    (function() {
        $(document).ready(function() {
            var manCoId = $('#SelectManCo').val();
            if (manCoId != undefined && manCoId != "") {
              autoApproval.autoApprovals(manCoId);
            }
        });
    })();
    
    $(document).ready(function () {
        $("#SelectManCo").change(function () {
            $("#manCoDocumentApprovals").empty();
            var manCoId = $(this).val();
            autoApproval.autoApprovals(manCoId);
        });
    });
    
    autoApproval.autoApprovals = function (manCoId) {
        var ajaxObject = {
            data: { manCoId: manCoId },
            url: utility.getBaseUrl() + "/" + "AutoApproval/AutoApprovals",
            type: "GET",
            dataType: "html",
            async: true,
            success: function (data, texStatus, jqXHR) {
                $("#manCoDocumentApprovals").html(data);
                autoApproval.docTypeSelection();
            },
            error: function (jqXHR, texStatus, errorThrown) {
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    autoApproval.docTypeSelection = function () {
        $(document).ready(function () {
            $(function () {
                $('#DocTypeId').change(function () {
                    var selectedDocType = $(this).val();
                    if (selectedDocType != null && selectedDocType != '') {
                      $.getJSON(utility.getBaseUrl() + '/search/subdoctype', { docTypeId: selectedDocType }, function (subDocTypes) {
                            var subDocTypesSelect = $('#SubDocTypeId');
                            subDocTypesSelect.empty();
                            subDocTypesSelect.append($('<option/>', {
                                value: "",
                                text: "SUB DOC TYPE"
                            }));
                            subDocTypesSelect.append($('<option/>', {
                              value: "-1",
                              text: "All"
                            }));
                            $.each(subDocTypes, function (index, code) {
                                subDocTypesSelect.append($('<option/>', {
                                    value: code.Id,
                                    text: code.Code
                                }));
                            });
                        });
                    }
                });
            });

        });
    };  
    
})(jQuery);
 











































































































































