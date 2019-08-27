$(function () {
    $("#AccountSource").change(function () {
        var selectedItemValue = $(this).val();

        $("#AccountDes").html('');
        $('#AccountSource option').each(function () {
            var value = $(this).attr('value');
            if ($(this).attr('value') != selectedItemValue) {
                $("#AccountDes").append($('<option></option>').val(value).html(value));
            }
        });

    });

    $("#ipNumber").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });


    $('#sendbutton').attr('disabled',true);
    $('#ipNumber').keyup(function(){
        if($(this).val().length !=0)
            $('#sendbutton').attr('disabled', false);            
        else
            $('#sendbutton').attr('disabled',true);
    })


    $('#sendbuttonLogin').attr('disabled', true);
    $('#ipNumber').keyup(function () {
        if ($(this).val().length == 6)
            $('#sendbuttonLogin').attr('disabled', false);
        else
            $('#sendbuttonLogin').attr('disabled', true);
    })
});


