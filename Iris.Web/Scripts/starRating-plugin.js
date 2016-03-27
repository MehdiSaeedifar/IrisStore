// <![CDATA[
(function ($) {
    $.fn.StarRating = function (options) {
        var defaults = {
            postInfoUrl: '/',
            sectionType: 'Product',
            loginUrl: '/Customer/Login',
            errorHandler: null,
            completeHandler: null,
            onlyOneTimeHandler: null,
            authorizationHandler: function () { window.location = options.loginUrl; }
        };
        var options = $.extend(defaults, options);

        return this.each(function () {
            var inputRate = $(this);
            var dataJsonArray = { id: inputRate.attr('id').replace('rate-', ''), value: inputRate.val(), sectionType: options.sectionType };

            $.ajax({
                type: "POST",
                url: options.postInfoUrl,
                data: JSON.stringify(dataJsonArray),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                complete: function (xhr, status) {
                    var data = xhr.responseText;
                    if (xhr.status == 403) {
                        options.authorizationHandler(this);
                    } else if (status === 'error' || !data) {
                        if (options.errorHandler)
                            options.errorHandler(this);
                    } else if (data == "nok") {
                        if (options.onlyOneTimeHandler)
                            options.onlyOneTimeHandler(this);
                    } else {
                        if (options.completeHandler)
                            options.completeHandler(this);
                    }
                }
            });
            return false;
        });
    };
})(jQuery);
// ]]>