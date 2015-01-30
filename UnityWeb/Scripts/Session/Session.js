var session = function () {

};

(function ($) {

  var sessionTimeOutWarning, ajaxCallCount, ajaxCallHandler, timerHandler, timeOut, timeOutInSeconds, timeWarnig, timer, theTime;
  var sessionReset = function() {
    ajaxCallCount = ajaxCallCount + 1;
    if (ajaxCallCount == (timeWarnig + 1)) { //time expires          
      window.location.href = utility.getBaseUrl() + '/Session/Expired'; //redirect to session out
    } else if (ajaxCallCount == timeWarnig) {
      $('#sessionExpiryModal').modal('show'); //$("#sessiontimeout-warnig").dialog('open');
      
      $('button#continue').click(function (e) {
        $('#sessionExpiryModal').modal('hide');
          });
      timerClock();
    }
    $.ajax({ url: utility.getBaseUrl() + "/Session/SessionReset", type: "GET", cache: false, async: false });

    ajaxCallHandler = setTimeout(sessionReset, 60000);
  };

  var timerClock = function() {
    timer = timer - 1;
    theTime.html(timer);
    timerHandler = setTimeout(timerClock, 1000);
  };

  session.resetHandlers = function () {
    ajaxCallCount = 0;
    timer = 60;
    clearTimeout(ajaxCallHandler);
    clearTimeout(timerHandler);
    ajaxCallHandler = setTimeout(sessionReset, 60000);
    closeDialog();
  };

  var closeDialog = function() {
    sessionTimeOutWarning.dialog('close');
  };

  //Initialization private function
  var initialize = function () {
    $(document).ready(function () {
      sessionTimeOutWarning = $('#sessionExpiryModal');
      theTime = $('#the_time');
      ajaxCallCount = 0;
      timeOut = 10; //minutes
      timeOutInSeconds = 10 * 60000; //120000
      timeWarnig = 9; //1 min before timeout
      timer = 60;
      ajaxCallHandler = setTimeout(sessionReset, 60000);
    });
  };

  //Triggers the Initialization function
  (function () {
    initialize();
  })();

})(jQuery);


