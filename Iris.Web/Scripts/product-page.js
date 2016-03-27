$('.product-images-gallery').magnificPopup({
    type: 'image',
    delegate: 'a',
    gallery: {
        enabled: true
    },
    ainClass: 'mfp-with-zoom', // this class is for CSS animation below
    zoom: {
        enabled: true, // By default it's false, so don't forget to enable it

        duration: 300, // duration of the effect, in milliseconds
        easing: 'ease-in-out', // CSS transition easing function

        // The "opener" function should return the element from which popup will be zoomed in
        // and to which popup will be scaled down
        // By defailt it looks for an image tag:
        opener: function (openerElement) {
            // openerElement is the element on which popup was initialized, in this case its <a> tag
            // you don't need to add "opener" option if this code matches your needs, it's defailt one.
            return openerElement.is('img') ? openerElement : openerElement.find('img');
        }
    }
});

$('#productImagesCarousel').carousel({
    pause: true, // init without autoplay (optional)
    interval: false, // do not autoplay after sliding (optional)
    wrap: true // do not loop
});

$('#similarProductsCarousel').carousel({
    pause: true, // init without autoplay (optional)
    interval: false, // do not autoplay after sliding (optional)
    wrap: true // do not loop
});

Highcharts.setOptions({
    lang: {
        thousandsSep: ','
    }
});

$('#chartTabLink').on('shown.bs.tab', function (e) {

    $('#chart').highcharts({
        title: {
            text: 'نمودار تغییرات قیمت',
            x: -20 //center
        },
        subtitle: {
            text: '',
            x: -20
        },
        xAxis: {
            categories: dateVals
        },
        yAxis: {
            title: {
                text: 'قیمت (تومان)',
                useHTML: true
            },
            plotLines: [
                {
                    value: 0,
                    width: 1,
                    color: '#808080'
                }
            ],
            labels: {
                format: '{value:,.0f}'
            },

        },
        tooltip: {
            //formatter: function () {
            //    return '<div dir="rtl"><p>' + this.x + '</p><p>' + this.y + '</p> ' +
            //        '</div>';
            //},
            valueSuffix: " تومان ",
            useHTML: true
        },
        legend: {
            layout: 'horizontal',
            align: 'right',
            verticalAlign: 'bottom',
            borderWidth: 0,
            rtl: true,
            useHTML: true
        },
        series: [
            {
                name: productName,
                data: pricesVal
            }
        ],
        credits: false
    });

});

$(".rating").rating();


$('input[id^=rate]').on('rating.change', function (event, value, caption) {
    event.preventDefault();
    var rateElement = $(this).attr('id');

    toastr.options = {
        "positionClass": "toast-top-center",
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000"
    }

    $(this).StarRating({
        postInfoUrl: postInfoUrl,
        errorHandler: function () {
            $('#' + rateElement).rating('reset');

            toastr.warning('کاربر گرامی مشکلی در ثبت امتیاز شما بوجود آمده');
        },
        completeHandler: function () {

            toastr.success('کاربر گرامی امتیاز شما با موفقیت ثبت شد.از توجه شما متشکریم');

        },
        onlyOneTimeHandler: function () {
            $('#' + rateElement).rating('reset');

            toastr.error('کاربر گرامی شما فقط قادر هستید یکبار به هر کالا امتیاز دهید.با تشکر');
        },
        authorizationHandler: function () {
            $('#' + rateElement).rating('reset');
        }
    });
});