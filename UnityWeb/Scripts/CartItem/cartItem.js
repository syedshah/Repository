function cartItem() {
    func = function () {
    };
};

(function ($) {

  var docSelected, docIds;
  
  cartItem.exportBasket = function () {
    var i = 0;
    var cartId = $(".cartId").val();
    if (cartId.length >= 1) {
      cartItem.exportBasketItems(cartId);
    }
  };
  
  cartItem.exportBasketItems = function (cartId) {
    var ajaxObject = {
      data: { cartId: cartId },
      url: utility.getBaseUrl() + "/" + "CartItem/ExportBasketToZip",
      type: "POST",
      dataType: "json",
      async: true,
      traditional: true,
      success: function (data, texStatus, jqXHR) {
        window.location = utility.getBaseUrl() + "/" + '/CartItem/Download?file=' + data.file;
      },
      error: function (jqXHR, texStatus, errorThrown) {
        alert("There was an error.");
      }
    };

    utility.sendAjaxRequest(ajaxObject);
  };

    cartItem.exportSelectedDocuments = function () {
        var i = 0;
        var documentIds = new Array();
        docSelected.each(function () {
            var checked = $(this).prop('checked');
            if (checked == true) {             
                documentIds.push(docIds[i].value);
            }
            i++;
        });
        if (documentIds.length >= 1) {
            cartItem.export(documentIds);
        }  
    };

    cartItem.export = function (documentIds) {
        var ajaxObject = {
            data: { documentIds: documentIds },
            url: utility.getBaseUrl() + "/" + "CartItem/ExportDocumentsToZip",
            type: "POST",
            dataType: "json",
            async: true,
            traditional: true,
            success: function (data, texStatus, jqXHR) {
              window.location = utility.getBaseUrl() + "/" + '/CartItem/Download?file=' + data.file;
            },
            error: function (jqXHR, texStatus, errorThrown) {
                alert("There was an error.");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };
    
    var initialize = function () {
        $(document).ready(function () {
            docSelected = $(".docSelected");
            docIds = $(".docId");
        });
    };

    (function () {
        initialize();
    })();

})(jQuery);