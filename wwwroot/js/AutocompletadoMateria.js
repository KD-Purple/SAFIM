$(document).ready(function () {

    // Configurar el autocompletado para el campo clavemateria
    $("#clavemateria").autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                url: '/materias/obtenermaterias/clave',
                type: "POST",
                dataType: "json",
                data: {
                    clavemateria: request.term,
                },
                success: function (data) {
                    response(data);
                },
            });
        },
        select: function (event, ui) {
            $("#clavemateria").val(ui.item.clave);
            $("#nombremateria").val(ui.item.materia);
            return false;
        }
    });

    // Configurar el autocompletado para el campo nombremateria
    $("#nombremateria").autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                url: '/materias/obtenermaterias/nombre',
                type: "POST",
                dataType: "json",
                data: {
                    nombremateria: request.term,
                },
                success: function (data) {
                    response(data);
                },
            });
        },
        select: function (event, ui) {
            $("#clavemateria").val(ui.item.clave);
            $("#nombremateria").val(ui.item.materia);
            return false;
        }
    });
});