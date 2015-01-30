(function(u) {
  var SearchModel = function () {

    var self = this;

    self.SelectedDocId = ko.observable("");
    self.SelectedSubDocId = ko.observable("");
  }
  u.SearchModel = SearchModel;
}(window.Unity));