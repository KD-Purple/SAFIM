
$(document).ready(function () {

    $.ajax({
        url: "/asesor/reportespendientes",
        contentType: "application/json",
        dataType: "json",
        type: "POST",
        success: function (data) {
            $("#reportes").html("Reportes <strong><span class='alert alert-danger font-weight-bold' style='padding:5px' >" + data + "</span>");
        }
    })


});