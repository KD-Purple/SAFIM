$(document).ready(function () {

    var dias = ["lunes", "martes", "miercoles", "jueves", "viernes"];
    dias.forEach(d =>
        $("#" + d).find("#inicio").keyup(function () {
            $(this).prop('required', ($(this).val() != '' || $("#" + d).find("#fin").val() != '') ? true : false);
            $("#" + d).find("#fin").prop('required', ($(this).val() != '' || $("#" + d).find("#fin").val() != '') ? true : false);
        })
    );

    dias.forEach(d =>
        $("#" + d).find("#fin").keyup(function () {
            $(this).prop('required', ($(this).val() != '' || $("#" + d).find("#inicio").val() != '') ? true : false);
            $("#" + d).find("#inicio").prop('required', ($(this).val() != '' || $("#" + d).find("#inicio").val() != '') ? true : false);
        })
    );

})