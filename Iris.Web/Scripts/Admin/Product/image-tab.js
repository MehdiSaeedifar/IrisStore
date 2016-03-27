(function () {

    $.fn.imageTab = function (options) {
        
        var defaults = {
            fileUploadId: null,
            uploadUrl: null,
            tblFilesId: null,
            uploadedFilesId: null
        };

        var settings = $.extend(defaults, options);

        var $fileUpload = $("#" + settings.fileUploadId).fileupload({
            url: settings.uploadUrl,
            formData: function (form) {
                return form.find("input[type='file']").serializeArray();
            },
            autoUpload: true,
            maxFileSize: 99900000,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
        });

        $("#" + settings.tblFilesId + " tbody").sortable({
            items: '> tr',
            revert: true,
            forcePlaceholderSize: true,
            placeholder: 'ui-state-highlight',
            cursor: "move",
            start: function (event, ui) {
                // Build a placeholder cell that spans all the cells in the row
                var cellCount = 0;
                $('td, th', ui.helper).each(function () {
                    // For each TD or TH try and get it's colspan attribute, and add that or 1 to the total
                    var colspan = 1;
                    var colspanAttr = $(this).attr('colspan');
                    if (colspanAttr > 1) {
                        colspan = colspanAttr;
                    }
                    cellCount += colspan;
                });

                // Add the placeholder UI - note that this is the item's content, so TD rather thanTR
                ui.placeholder.html('<td colspan="' + cellCount + '">&nbsp;</td>');
            }

        });


        var imagesDataInput = $("input#" + settings.uploadedFilesId).val();

        if (imagesDataInput != undefined) {
            var imagesData = $.parseJSON(imagesDataInput);
            $fileUpload.fileupload('option', 'done')
                .call($fileUpload[0], $.Event('done'), { result: { files: imagesData } });
        }

        return this;
    }

})();