(function (Iris) {
    
    $.fn.discountTab = function (options) {

        var defaults = {
            addDiscountBtnId: "btnAddDiscount",
            tblDiscountId: "tblProductDiscounts",
            startDate: null,
            endDate: null,
            productId: null
        };

        var settings = $.extend(defaults, options);

        var $tableDiscount = $("#" + settings.tblDiscountId);

        var bindStartDataPicker = function ($element) {

            $element.on("click", function () {
                PersianDatePicker.Show($(this)[0], settings.startDate);
            });
        };

        var bindEndDataPicker = function ($element) {

            $element.on("click", function () {
                PersianDatePicker.Show($(this)[0], settings.endDate);
            });
        };

        var bindDiscountNumber = function ($element) {

            $element.number(true, 0);
        };

        $tableDiscount.find("tr button[name='removeItem']").each(function (item) {

            var $button = $(this);

            $button.confirmation({
                placement: "right",
                title: "آیا از حذف تخفیف مورد نظر اطمینان دارید؟",
                btnCancelLabel: 'نه',
                btnOkLabel: 'آره',
                container: 'body',
                onConfirm: function (event, element) {

                    $button.closest("tr").remove();
                }
            });

        });

        $tableDiscount.find("a[data-discount]").editable({
            title: 'تخفیف را وارد نمایید:',
            display: function (value, sourceData) {
                $(this).html(Iris.formatCurrency(value));
            }
        }).on('save', function (e, params) {
            $(this).closest("tr").children("input.discountValHidden").val(params.newValue);
        }).on('shown', function (e, editable) {

            setTimeout(function () {
                bindPriceNumber(editable.input.$input);
            }, 50);

        });

        $tableDiscount.find('a[data-startdate]').editable({
            title: 'تاریخ شروع را وارد نمایید',
            display: function (value, sourceData) {

                $(this).html(value);
            }
        }).on('save', function (e, params) {

            $(this).closest("tr").children("input.startdateValHidden").val(params.newValue);

        }).on('shown', function (e, editable) {

            setTimeout(function () {
                bindStartDataPicker(editable.input.$input);
            }, 100);

            });

        $tableDiscount.find('a[data-enddate]').editable({
            title: 'تاریخ پایان را وارد نمایید',
            display: function (value, sourceData) {

                $(this).html(value);
            }
        }).on('save', function (e, params) {

            $(this).closest("tr").children("input.enddateValHidden").val(params.newValue);

        }).on('shown', function (e, editable) {

            setTimeout(function () {
                bindEndDataPicker(editable.input.$input);
            }, 100);

        });

        var addDiscount = function () {

            var gId = Iris.createGUID();

            var rowTemplate = '<tr><td><input type="text" name="discount" class="form-control" style="width: 121px;display: inline;" /></td><td><input type="text" name="startdate" class="form-control" style="width: 121px;display: inline;" /></td><td><input type="text" name="enddate" class="form-control" style="width: 121px;display: inline;" /></td><td><button name="btnEdit" type="button" class="btn btn-sm btn-info" style="margin-left:5px;"><i class="fa fa-edit"></i></button><button type="button" class="btn btn-sm btn-danger" name="btnRemove"><i class="fa fa-trash-o"></i></button></td></tr>';


            $tableDiscount.find("tbody").prepend(rowTemplate);

            var $addedRow = $tableDiscount.find("tbody tr:first");

            var $startdateInput = $addedRow.find("input[name='startdate']");
            var $enddateInput = $addedRow.find("input[name='enddate']");
            var $discountInput = $addedRow.find("input[name='discount']");

            bindStartDataPicker($startdateInput);
            bindEndDataPicker($enddateInput);
            bindDiscountNumber($discountInput);

            $addedRow.find("button[name='btnRemove']").on("click", function () {

                $(this).closest("tr").remove();
            });

            $addedRow.find("button[name='btnEdit']").on("click", function () {

                var $this = $(this);
                var $currentRow = $this.closest("tr");

                var discountVal = $currentRow.find("input[name='discount']").val();
                var startdateVal = $currentRow.find("input[name='startdate']").val();
                var enddateVal = $currentRow.find("input[name='enddate']").val();


                if (discountVal === "" || startdateVal === "" || enddateVal === "") {

                    toastr.error('لطفا فیلدها را  با اطلاعات صحیح پر کنید', '', {
                        positionClass: "toast-top-center"
                    });

                    return;
                }

                var discountRowTemplate = '<input id="Discounts_Index" name="Discounts.Index" type="hidden" value="' + gId + '"><input class="discountValHidden" id="Discounts_' + gId + '__Discount" name="Discounts[' + gId + '].Discount" type="hidden" value="' + discountVal + '"><input class="startdateValHidden" id="Discounts_' + gId + '__StartDate" name="Discounts[' + gId + '].StartDate" type="hidden" value="' + startdateVal + '"><input class="enddateValHidden" id="Discounts_' + gId + '__EndDate" name="Discounts[' + gId + '].EndDate" type="hidden" value="' + enddateVal + '"><input  id="Discounts_' + gId + '__ProductId" name="Discounts[' + gId + '].ProductId" type="hidden" value="' + settings.productId + '"><td><a data-discount="" class="editable editable-click">' + Iris.formatCurrency(discountVal) + '</a><span>درصد</span></td><td><a data-startdate="" class="editable editable-click">' + startdateVal + '</a></td><td><a data-enddate="" class="editable editable-click">' + enddateVal + '</a></td><td><button type="button" class="btn btn-sm btn-danger"><i class="fa fa-trash-o"></i></button></td>';

                $currentRow.html(discountRowTemplate);

            });
        }

        $("#" + settings.addDiscountBtnId).on("click", function () {
            addDiscount();
        });

        return this;
    };


})(Iris)