function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


(function ($) {
    $.fn.SearchProducts = function (options) {
        var defaults = {
            resultDiv: '#resultDiv',
            progressDiv: '#progress',
            sourceUrl: '/',
            loginUrl: '/login',
            errorHandler: null,
            completeHandler: null,
            noMoreInfoHandler: null,
            priceRangeInputId: null,
            groupsListDivId: null,
            searchInputId: null,
            btnSearchId: null,
            showStockProductsOnlyId: null,
            pagerSortById: null,
            pagerSortOrderId: null,
            pageSizeId: null,
            paginationId: null,
            mainNonAjaxContentDivId: "#mainNonAjaxContent",
            paramName: "",
            pageName: "صفحه"
        };

        options = $.extend(defaults, options);

        var showProgress = function () {
            $(options.progressDiv).fadeIn();
        }

        var hideProgress = function () {
            $(options.progressDiv).fadeOut();
        }

        var clearArea = function () {
            $(options.moreInfoDiv).html("");
            $(options.mainNonAjaxContentDivId).html("");
            window.scrollTo(0, 0);
        }

        return this.each(function () {


            var title = document.title;

            var updatePath = function () {

                if (!$(options.pagerSortById).val()) {
                    return;
                }

                var selectedGroups = "";
                $(options.groupsListDivId + " input:checked").each(function () {
                    selectedGroups += $(this).val() + "-";
                });

                selectedGroups = selectedGroups.substr(0, selectedGroups.length - 1);


                var path = "#/page/" + (selectedGroups || "all") + "/" + ($(options.searchInputId).val() || "empty") + "/" + (currentPage) + "/" + $(options.pagerSortById).val() + "/" + $(options.pagerSortOrderId).val() + "/" + $(options.pageSizeId).val() + "/" + $(options.showStockProductsOnlyId).is(":checked") + "/" + $(options.priceRangeInputId).slider("values", 0) + "/" + $(options.priceRangeInputId).slider("values", 1);


                try {
                    history.pushState({}, "", path);
                } catch (ex) {
                    window.location.hash = path;
                }
                document.title = title + " / " + options.pageName + " " + (page);
            }

            var currentPage = 1;

            var submitData = function (pageNumber) {


                if (pageNumber == null) {
                    pageNumber = currentPage;
                } else {
                    currentPage = pageNumber;
                }

                showProgress();

                var pagerSortBy = $(options.pagerSortById).val();

                var pagerSortOrder = $(options.pagerSortOrderId).val();
                var pagerPageSize = $(options.pageSizeId).val();

                var showStockProductsOnly = $(options.showStockProductsOnlyId).is(':checked');


                var minPrice = $(options.priceRangeInputId).slider("values", 0);
                var maxPrice = $(options.priceRangeInputId).slider("values", 1);



                var searchTerm = $(options.searchInputId).val();

                var selectedGroups = [];
                $(options.groupsListDivId + " input:checked").each(function () {
                    selectedGroups.push($(this).val());
                });

                updatePath();

                if (minPrice == values[0] && maxPrice == values[values.length - 1]) {
                    minPrice = maxPrice = null;
                }

                $.ajax({
                    type: "POST",
                    url: options.sourceUrl,
                    data: JSON.stringify(
                        {
                            pageNumber: pageNumber,
                            pageSize: pagerPageSize,
                            sortBy: pagerSortBy,
                            sortOrder: pagerSortOrder,
                            showStockProductsOnly: showStockProductsOnly,
                            minPrice: minPrice,
                            maxPrice: maxPrice,
                            selectedCategories: selectedGroups,
                            searchTerm: searchTerm,
                            name: options.paramName
                        }
                    ),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function (xhr, status) {
                        hideProgress();
                        var data = xhr.responseText;

                        if (options.completeHandler)
                            options.completeHandler(appendEl, $boxes);



                        if (xhr.status == 403) {
                            window.location = options.loginUrl;
                        } else if (status === 'error' || !data) {
                            if (options.errorHandler)
                                options.errorHandler(this);
                        } else {
                            if (data == "no-more-info") {
                                if (options.noMoreInfoHandler)
                                    options.noMoreInfoHandler(this);
                            } else {
                                var $boxes = $(data);
                                var appendEl;
                                $(options.resultDiv).fadeOut(function () {
                                    appendEl = $(options.resultDiv).html(data);
                                    $(options.resultDiv).fadeIn();

                                    $(options.paginationId + " a").on("click", function (event) {
                                        event.preventDefault();
                                        var $this = $(this);
                                        var href = $this.attr("href");
                                        if (href === null || href === undefined)
                                            return;
                                        var pageNumber = getParameterByName(href, "page");

                                        $('html,body').animate({ scrollTop: 0 });

                                        submitData(pageNumber);
                                    });

                                });
                            }
                        }
                    }
                });
            }

            Path.map("#/page(/:groups)(/:searchTerm)(/:page)(/:sortby)(/:order)(/:pageSize)(/:showStockProductsOnly)(/:minPrice)(/:maxPrice)").to(function () {
                var sortBy = this.params['sortby'] || 'ViewNumber';
                var order = this.params['order'] || 'desc';
                var pageSize = this.params['pageSize'] || +$(options.pageSizeId).val();
                var searchTerm = (this.params['searchTerm'] === "empty") ? "" : this.params['searchTerm'];
                var minPrice = this.params['minPrice'];
                var maxPrice = this.params['maxPrice'];

                var showStockProductsOnly = this.params['showStockProductsOnly'];

                var urlPage = 1;

                if (isNumeric(this.params['page'])) {
                    urlPage = parseInt(this.params['page'], 10);
                }





                $(options.pagerSortById).val(sortBy);
                $(options.pagerSortOrderId).val(order);
                $(options.pageSizeId).val(pageSize);
                $(options.searchInputId).val(decodeURIComponent(searchTerm || ""));


                if (showStockProductsOnly == "true") {
                    $(options.showStockProductsOnlyId).attr('checked', 'checked');
                }

                var $slider = $(options.priceRangeInputId);
                if (isNumeric(minPrice) && isNumeric(maxPrice)) {
                    $slider.slider("values", 0, minPrice);
                    $slider.slider("values", 1, maxPrice);
                    $("#priceMinVal").html(formatCurrency($slider.slider('values', 0)));
                    $("#priceMaxVal").html(formatCurrency($slider.slider('values', 1)));
                }

                var groups = this.params['groups'] || 'all';

                var groupsList = [];
                groupsList = groups.split("-");

                $(options.groupsListDivId + " input[type='checkbox']").each(function () {
                    for (var i = 0; i < groupsList.length; i++) {
                        if (groupsList[i] === $(this).val()) {
                            $(this).attr('checked', 'checked');
                            break;
                        }
                    }
                });

                page = urlPage;

                submitData(page);
            });

            //Path.root("#/page/all/1/date/desc/24/false");

            if ($(options.pagerSortById).val()) {
                Path.listen();
            }

            $(options.pagerSortById + "," +
                options.pagerSortOrderId + "," +
                options.showStockProductsOnlyId + "," +
                options.groupsListDivId + " input[type='checkbox']").change(function () {
                    submitData();
                });


            $(options.pageSizeId).change(function () {
                submitData(1);
            });

            $(options.priceRangeInputId).on("slidechange", function (event, ui) {
                submitData();
            });

            $(options.btnSearchId).on("click", function (event) {
                submitData();
            });

            function getParameterByName(url, name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(url);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }




        });
    };
})(jQuery);


function formatCurrency(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

$(function () {



    $("#priceMinVal").html(formatCurrency(values[0]));
    $("#priceMaxVal").html(formatCurrency(values[values.length - 1]));

    var slider = $("#priceRange").slider({
        orientation: 'horizontal',
        range: true,
        isRTL: true,
        animate: false,
        min: values[0],
        max: values[values.length - 1],
        values: [values[0], values[values.length - 1]],
        slide: function (event, ui) {
            var includeLeft = event.keyCode != $.ui.keyCode.RIGHT;
            var includeRight = event.keyCode != $.ui.keyCode.LEFT;

            var value = findNearest(includeRight, includeLeft, ui.value);

            if (ui.value == ui.values[0]) {
                slider.slider('values', 0, value);
            } else {
                slider.slider('values', 1, value);
            }

            $("#priceMinVal").html(formatCurrency(slider.slider('values', 0)));
            $("#priceMaxVal").html(formatCurrency(slider.slider('values', 1)));

            return false;
        },
        change: function (event, ui) {

            //getHomeListings();
        }
    });

    function findNearest(includeLeft, includeRight, value) {
        var nearest = null;
        var diff = null;
        for (var i = 0; i < values.length; i++) {
            if ((includeLeft && values[i] <= value) || (includeRight && values[i] >= value)) {
                var newDiff = Math.abs(value - values[i]);
                if (diff == null || newDiff < diff) {
                    nearest = values[i];
                    diff = newDiff;
                }
            }
        }
        return nearest;
    }
});


$("#priceRange").on("slidecreate", function (event, ui) {
    $("#searchProductFrm").SearchProducts({
        resultDiv: '#resultDiv',
        progressDiv: '#progress',
        sourceUrl: sourceUrl,
        loginUrl: '/login',
        errorHandler: null,
        completeHandler: null,
        noMoreInfoHandler: null,
        priceRangeInputId: "#priceRange",
        groupsListDivId: "#groupsList",
        searchInputId: "#searchTerm",
        btnSearchId: "#btnSearchProdcut",
        showStockProductsOnlyId: "#AvailableProducts",
        pagerSortById: "#pagerSortBy",
        pagerSortOrderId: "#pagerSortOrder",
        pageSizeId: "#pagerPageSize",
        paginationId: "#paginationDiv",
        mainNonAjaxContentDivId: "#mainNonAjaxContent",
        paramName: "",
        pageName: "صفحه"
    });
});