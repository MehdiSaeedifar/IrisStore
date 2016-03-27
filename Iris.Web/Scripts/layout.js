$('#menuNav a').each(function () {
    // and test its normalized href against the url pathname regexp
    if (this.href === window.location.href) {
        $(this).parent('li').addClass('active');
    }
});

$('#newestProductsCarousel,topViewProductsCarousel,topProductsCarousel').carousel({
    pause: true, // init without autoplay (optional)
    interval: false, // do not autoplay after sliding (optional)
    wrap: true // do not loop
});


$('.product-widget [data-toggle="tooltip"]').tooltip({
    container: "body",
    delay: { "show": 500, "hide": 10 },
    template: '<div class="tooltip product-widget" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>'
});

$.widget("custom.catcomplete", $.ui.autocomplete, {
    _create: function () {
        this._super();
        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
    },
    _renderMenu: function (ul, items) {

        var that = this,
            currentCategory = "";
        $.each(items, function (index, item) {
            var li;

            if (item.category != currentCategory) {
                ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                currentCategory = item.category;
            }

            li = that._renderItemData(ul, item);

            if (item.category) {
                if (item.image != null) {
                    li.html("<a><img src='" + item.image + "' />" + item.label + "</a>");
                } else {
                    li.attr("aria-label", item.label);
                }
            }

        });
    }
});

$(function () {

    $("#searchInput").catcomplete({
        delay: 300,
        source: autocompleteSource,
        minLength: 2,
        select: function (event, ui) {
            window.location = ui.item.url;
        },
        position: { my: "right top", at: "right bottom", collision: "none" }
    });

    $("#frmSearch form").submit(function (event) {
        event.preventDefault();
        var $this = $(this);

        var url = $this.attr('action') + "#/page/all/" + $this.find("input").val();

        $this.attr('action', url);

        $this[0].submit();
    });

});