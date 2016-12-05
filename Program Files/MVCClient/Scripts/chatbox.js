$(document).ready(function () {
    if ($("#windowchat").data("kendoWindow") != undefined) {
        $(".k-i-custom_restore").parent().attr("href", "javascript:void(0)");
        $(".k-i-custom_minimize").parent().attr("href", "javascript:void(0)");
        chatbox_position(true)
        $('#windowchat').parent().addClass("myWindow");//Use this class myWindow, in conjunction with the following css: .myWindow  .k-window-titlebar, .myWindow    .k-window-content{background-color: #0A7A06} ===> the style will affect only the particular myWindow widget instance                
        $("#windowchat").data("kendoWindow").open();
    }

    $(".k-i-custom_minimize").on("click", function (e) {
        chatbox_position(false);
    });

    $(".k-i-custom_restore").on("click", function (e) {
        chatbox_position(true);
    });
});

function chatbox_position(isMinimize) {
    if (isMinimize == false) {
        $("#windowchat").data("kendoWindow").restore();
        //$("#windowchat").data("kendoWindow").wrapper.css({ top: screen.availHeight - 335, left: 1 });
        $("#windowchat").data("kendoWindow").wrapper.css({ top: window.innerHeight - 257, left: 1 });
        $(".k-i-custom_minimize").parent().css("display", "none");
        $(".k-i-custom_restore").parent().css("display", "inline-block");
    }
    else {
        $("#windowchat").data("kendoWindow").minimize();
        //$("#windowchat").closest(".k-window").css({ top: screen.availHeight - 100, left: 1 });
        $("#windowchat").closest(".k-window").css({ top: window.innerHeight - 30, left: 1 });
        $(".k-i-custom_restore").parent().css("display", "none");
        $(".k-i-custom_minimize").parent().css("display", "inline-block");
    }
}

$(window).resize(function () {
    chatbox_position(true);
});


$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {

        var textArea = document.getElementById("discussion");
        // Add the message to the page.
        if (htmlEncode(message) != "") {
            textArea.value = htmlEncode(name) + ': ' + htmlEncode(message) + '\n' + textArea.value;
        }
    };    

    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#message').on('keyup', function (e) {
            if (e.which == 13) {
                d = new Date();
                var userName2 = userName + '(' + d.getHours() + ':' + d.getMinutes() + ')';
                $('#displayname').val(userName2);
                //e.preventDefault();
                chat.server.send($('#displayname').val(), $('#message').val());
                $('#message').val('').focus();
            }
        });
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}