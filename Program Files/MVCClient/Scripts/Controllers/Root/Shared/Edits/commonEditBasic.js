define([], (function () {

    $(document).ready(function () {
        if ($("#submitButton") != undefined)
            $("#submitButton").click(function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Save);
            });

        if ($("#closedSubmitButton") != undefined)
            $("#closedSubmitButton").click(function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Closed);
            });
        //closedSubmitButton_NMVN: the TOTALLY same with closedSubmitButton. IT JUST IS ANOTHER BUTTON ON THE SAME VIEW
        if ($("#closedSubmitButton_NMVN") != undefined)
            $("#closedSubmitButton_NMVN").click(function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Closed);
            });


        if ($("#submitCreateWizard") != undefined)
            $("#submitCreateWizard").click(function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Popup);
            });

        
    });

}));
