// Integra jquery-validation (unobtrusive) con las clases de error de Bootstrap 5 y de Spark.
(function ($) {
    'use strict';

    if (!$ || !$.validator) {
        return;
    }

    const CLASES_ERROR = 'is-invalid is-invalid-custom';

    // unobtrusive no redefine highlight/unhighlight, así que estos valores por defecto se respetan.
    $.validator.setDefaults({
        highlight: (elemento) => $(elemento).addClass(CLASES_ERROR),
        unhighlight: (elemento) => $(elemento).removeClass(CLASES_ERROR + ' input-validation-error')
    });

    // Campos que el servidor devolvió como inválidos (re-render tras un POST).
    $(() => $('.input-validation-error').addClass(CLASES_ERROR));
})(window.jQuery);
