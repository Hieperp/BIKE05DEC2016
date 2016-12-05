//BEGIN Validation

$(document).ready(function () {
    if ($("#isValid").val() == 1) {
        $("#div-alert").css("display", "block");
    }
});


$("form").submit(function (event) {
    if (!$(this).valid()) {
        $("#div-alert").css("display", "block");
    }
    else {
        $("#div-alert").css("display", "none");       
        return true;        
    }
});


function alert_click() {
    $(".validation-summary-errors").toggle("slow", "swing");
}

//END Validation



