Iris = {
    formatCurrency: function (number) {
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    },
    cloneCanvas: function (oldCanvas) {

        var newCanvas = document.createElement('canvas');
        var context = newCanvas.getContext('2d');

        newCanvas.width = oldCanvas.width;
        newCanvas.height = oldCanvas.height;

        context.drawImage(oldCanvas, 0, 0);

        return newCanvas;
    },
    createGUID: function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    },
    roxyFileManPath: null

}


var actionMessage = $("input#message").val();

if (actionMessage !== "") {
    toastr.success('', actionMessage, {
        "positionClass": "toast-top-center",
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000"
    });

    $("input#message").val('');
}
