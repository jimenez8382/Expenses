(function ($) {
    $.fn.dataTableCustom = function (options) {
        // Create some defaults, extending them with any options that were provided
        var settingsOptions = $.extend({
            'DeleteUrl': '',
            'EditUrl': '',
            'IdEdited': '',
        }, options);

        var dataTable = this;
        init();
        function init() {
            $('#Confirm').modal();
            var oTable = $('#' + dataTable).dataTable({
                "bServerSide": true,
                "paging": true,
                "iDisplayLength": 10,
                "aLengthMenu": [[10, 15, 25, 35, 50, 100, -1], [10, 15, 25, 35, 50, 100, "All"]],
                "sAjaxSource": "Expenses/AjaxHandler",
                "bProcessing": true,
                columnDefs: [
                { type: 'currency', targets: 2 }
                ],
                "aoColumns": [
                            null,
                            { "sType": "date" },
                            { "sType": "currency" },
                            null
                ],
                "rowCallback": function (row, data, index) {
                    $(row).attr('id', data[3]);
                    var onclick = 'onclick="document.getElementById(' + "'md1_YesBtn'" + ').setAttribute(' + "'data-Id'" + ',' + data[3] + ');"'
                    var Edit = settingsOptions.EditUrl + "/" + data[3];
                    var Delete = settingsOptions.DeleteUrl + "/" + data[3];
                    $('td:eq(3)', row).html('<a href="' + Edit + '"><i class="Small material-icons">mode_edit</i></a>' +
                        '<a class="delete"' + onclick + '   data-Id=' + data[3] + ' href="#Confirm"><i class="Small material-icons">delete</i></a>');
                },
                "initComplete": function (settings, json) {
                    var Changed = settingsOptions.IdEdited;
                    if (Changed != '') {
                        $("#" + Changed).addClass("ItemChanged").delay(2000).queue(function () {
                            $(this).removeClass("ItemChanged").dequeue();
                        });
                    }
                }
            });

            $('#md1_YesBtn').click(function () {
                var resourceUrl = settingsOptions.DeleteUrl;
                $('#Confirm').modal('close');
                window.location = resourceUrl + "/" + $(this).attr("data-id");
            });
        }
    };
})(jQuery);