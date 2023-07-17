// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

        // https://www.codexworld.com/add-remove-input-fields-dynamically-using-jquery/

// https://www.codexworld.com/add-remove-input-fields-dynamically-using-jquery/
$(document).ready(function () {
    // alert("test");
    var wrapper = $(".speakers"); //Input field wrapper

    var x = 1;
    if ($("#speaker_count").val() != undefined) {
        x = $("#speaker_count").val();
        //alert(x);
    }

    //var x = @{ ViewData["Speaker_Count"] };
    //var x = 1;
    // remove speaker
    $(wrapper).on('click', '.remove_speaker', function (e) {
        e.preventDefault();
    $(this).parent('div').remove();
            });

    // add new speaker
    $("#add_speaker").click(function (e) {
        // alert("click");
        e.preventDefault();
        var newSpeaker = '<div>Speaker<input type="text" name="Speaker[' + x + ']" class="form-control" />Subject<input type="text" name="Subject[' + x + ']" class="form-control" /><a href="javascript:void(0);" class="remove_speaker">Remove Speaker ❌</a></div>';
        x++;
        $(wrapper).append(newSpeaker);
    });
});
