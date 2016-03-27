
(function (Iris) {
    $.fn.productInfoTab = function (options) {
        var defaults = {

        };

        var settings = $.extend(defaults, options);

        $("#Price").number(true, 0);

        $("#Categories").select2({
            tags: true,
            dir: "rtl"
        }).on('change', function () {
            $(this).trigger('blur');
        });;

        $("#Tags").select2({
            tags: true,
            dir: "rtl"
        }).on('change', function () {
            $(this).trigger('blur');
        });;;

        CKEDITOR.replace('Description', {
            filebrowserBrowseUrl: Iris.roxyFileManPath,
            filebrowserImageBrowseUrl: Iris.roxyFileManPath + '?type=image',
            removeDialogTabs: 'link:upload;image:upload',
            height: "270px"
        });
    }


    $.validator.setDefaults({
        ignore: "",
        showErrors: function (errorMap, errorList) {
            this.defaultShowErrors();
            //اگر المانی معتبر است نیاز به نمایش پاپ اور ندارد
            $("." + this.settings.validClass).popover("destroy");
            //افزودن پاپ اورها
            for (var i = 0; i < errorList.length; i++) {
                var error = errorList[i];

                if ($(error.element).is("select")) {
                    $(error.element).siblings("span:first").find("span.select2-selection").popover({ placement: 'left', trigger: "focus hover" })
                        //.attr("data-original-title", "خطای اعتبارسنجی")
                        .attr("data-content", error.message);
                } else {
                    $(error.element).popover({ placement: 'left', trigger: "focus hover click" })
                        //.attr("data-original-title", "خطای اعتبارسنجی")
                        .attr("data-content", error.message);
                }
            }
        },
        // همانند قبل برای رنگی کردن کل ردیف در صورت عدم اعتبار سنجی و برعکس
        highlight: function (element, errorClass, validClass) {
            if (element.type === 'radio') {
                this.findByName(element.name).addClass(errorClass).removeClass(validClass);
            } else if ($(element).hasClass("js-example-responsive")) {
                $(element).siblings("span:first").find("span.select2-selection").addClass(errorClass).removeClass(validClass);
                $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            } else {
                $(element).addClass(errorClass).removeClass(validClass);
                $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            }
            $(element).trigger('highlited');
        },
        unhighlight: function (element, errorClass, validClass) {
            if (element.type === 'radio') {
                this.findByName(element.name).removeClass(errorClass).addClass(validClass);
            }
            if ($(element).hasClass("js-example-responsive")) {
                $(element).siblings("span:first").find("span.select2-selection").removeClass(errorClass).addClass(validClass);
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
            } else {
                $(element).removeClass(errorClass).addClass(validClass);
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
            }
            $(element).trigger('unhighlited');
        }
    });

    //برای حالت پست بک از سرور عمل می‌کند
    $(function () {
        $('form').each(function () {
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('has-error');
                }
            });
        });
    });


})(Iris);