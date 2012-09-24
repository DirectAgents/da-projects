var loader = (function ($, host) {
    return {
        loadTemplate: function (path) {
            var tmplLoader = $.get(path)
                    .success(function (result) {
                        $("body").append(result);
                    })
                    .error(function (result) {
                        alert("Error Loading Template");
                    })
            tmplLoader.complete(function () {
                $(host).trigger("TemplateLoaded", [path]);
            });
        }
    };
})(jQuery, document);