$(document).ready(function () {
    $("#matricula").autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                url: '/asesor/obtenerasesores',
                type: "POST",
                dataType: "json",
                data: {
                    matricula: request.term,
                },
                success: function (data) {
                    response(data);
                },
            });
        },
        select: function (event, ui) {
            $("#matricula").val(ui.item.label);
            $("#nombre").text(ui.item.nombre);
            $("#correo").text(ui.item.correo);
            $("#carrera").text(ui.item.carrera);
            $("#semestre").text(ui.item.semestre);
            $("#carrera").val(ui.item.carrera);
            return false;
        },
    });
})