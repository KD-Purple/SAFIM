$(document).ready(function () {
    autoComplete($("#matricula"));

    $("#agregar").on("click", function () {

        var contador = $(".reporteRow").length;
        var clonar = $(".reporteRow:last").clone().prop("id", "linea" + (contador));

        clonar.closest('.row').find('.matricula').val('');
        clonar.closest('.row').find('#carrera').val('');
        clonar.closest('.row').find('#grupo').val('');
        clonar.closest('.row').find('#temas').val('');
        clonar.closest('.row').find('#comentario').val('');

        autoComplete(clonar.find(".matricula"));

        clonar.insertAfter(".reporteRow:last");
        $("<hr>").insertBefore(clonar);
    });

    $("#remover").on("click", function () {
        var contador = $(".reporteRow").length - 1;

        if (contador > 0) {
            $("#linea" + contador).remove();
        }
    });
})

function autoComplete(t) {
    t.autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                url: '/asesor/obteneralumnos/todos',
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
            $(this).val(ui.item.label);
            $(".nombre").text(ui.item.nombre);
            $(this).closest('.row').find('#carrera').val(ui.item.carrera);
            return false;
        },
    });
}