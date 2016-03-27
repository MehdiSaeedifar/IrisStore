(function (Iris) {
    
    $.fn.priceTab = function (options) {

        var defaults = {
            addPriceBtnId: "btnAddPrice",
            tblPriceId: "tblProductPrices",
            currentDate: null,
            productId: null
        };

        var settings = $.extend(defaults, options);

        var $tablePrice = $("#" + settings.tblPriceId);

        var bindDataPicker = function ($element) {

            $element.on("click", function () {
                PersianDatePicker.Show($(this)[0], settings.currentDate);
            });
        };

        var bindPriceNumber = function ($element) {

            $element.number(true, 0);
        };

        $tablePrice.find("tr button[name='removeItem']").each(function (item) {

            var $button = $(this);

            $button.confirmation({
                placement: "right",
                title: "آیا از حذف قیمت مورد نظر اطمینان دارید؟",
                btnCancelLabel: 'نه',
                btnOkLabel: 'آره',
                container: 'body',
                onConfirm: function (event, element) {

                    $button.closest("tr").remove();
                }
            });

        });

        $tablePrice.find("a[data-price]").editable({
            title: 'قیمت را وارد نمایید:',
            display: function (value, sourceData) {
                $(this).html(Iris.formatCurrency(value));
            }
        }).on('save', function (e, params) {
            $(this).closest("tr").children("input.priceValHidden").val(params.newValue);
        }).on('shown', function (e, editable) {

            setTimeout(function () {
                bindPriceNumber(editable.input.$input);
            }, 50);

        });

        $tablePrice.find('a[data-date]').editable({
            title: 'تاریخ را وارد نمایید',
            display: function (value, sourceData) {

                $(this).html(value);
            }
        }).on('save', function (e, params) {

            $(this).closest("tr").children("input.dateValHidden").val(params.newValue);

        }).on('shown', function (e, editable) {

            setTimeout(function () {
                bindDataPicker(editable.input.$input);
            }, 100);

        });

        var addPrice = function () {

            var gId = Iris.createGUID();

            var rowTemplate = '<tr><td><input type="text" name="price" class="form-control" style="width: 200px;display: inline;" /></td><td><input type="text" name="date" class="form-control" style="width: 200px;display: inline;" /></td><td><button name="btnEdit" type="button" class="btn btn-sm btn-info" style="margin-left:5px;"><i class="fa fa-edit"></i></button><button type="button" class="btn btn-sm btn-danger" name="btnRemove"><i class="fa fa-trash-o"></i></button></td></tr>';


            $tablePrice.find("tbody").prepend(rowTemplate);

            var $addedRow = $tablePrice.find("tbody tr:first");

            var $dateInput = $addedRow.find("input[name='date']");
            var $priceInput = $addedRow.find("input[name='price']");

            bindDataPicker($dateInput);
            bindPriceNumber($priceInput);

            $addedRow.find("button[name='btnRemove']").on("click", function () {

                $(this).closest("tr").remove();
            });

            $addedRow.find("button[name='btnEdit']").on("click", function () {

                var $this = $(this);
                var $currentRow = $this.closest("tr");

                var priceVal = $currentRow.find("input[name='price']").val();
                var dateVal = $currentRow.find("input[name='date']").val();

                if (priceVal === "" || dateVal === "") {

                    toastr.error('لطفا فیلدها را  با اطلاعات صحیح پر کنید', '', {
                        positionClass: "toast-top-center"
                    });

                    return;
                }

                var priceRowTemplate = '<input id="Prices_Index" name="Prices.Index" type="hidden" value="' + gId + '"><input class="priceValHidden" id="Prices_' + gId + '__Price" name="Prices[' + gId + '].Price" type="hidden" value="' + priceVal + '"><input class="dateValHidden" id="Prices_' + gId + '__Date" name="Prices[' + gId + '].Date" type="hidden" value="' + dateVal + '"><input  id="Prices_' + gId + '__ProductId" name="Prices[' + gId + '].ProductId" type="hidden" value="' + settings.productId + '"><td><a data-price="" class="editable editable-click">' + Iris.formatCurrency(priceVal) + '</a><span>تومان</span></td><td><a data-date="" class="editable editable-click">' + dateVal + '</a></td><td><button type="button" class="btn btn-sm btn-danger"><i class="fa fa-trash-o"></i></button></td>';

                $currentRow.html(priceRowTemplate);

            });
        }

        $("#" + settings.addPriceBtnId).on("click", function () {
            addPrice();
        });

        return this;
    };


})(Iris)