$(document).ready(function () {

    $("#1").addClass('kt-menu__item--open');
    $("#10").addClass('kt-menu__item--open');

    $('#BtnSave').click(function () {
        
        AddGroup();
    });

    getActivityType();

    function getActivityType() {
        $.ajax({
            url: "/transportgrp/getAll",
            dataType: 'json',
            type: "GET",
            success: function (result) {
                var html = '<table class="kt-datatable__table" id="html_table" width="100%" style="display: block;">'
                html += '<thead class="kt-datatable__head"> ';
                html += '<tr class="kt-datatable__row" style="left: 0px;"> ';
                html += '<th data-field="Order ID" class="kt-datatable__cell kt-datatable__cell--sort"><span style="width: 141px;">Group No</span></th>';
                html += '<th data-field="Car Make" class="kt-datatable__cell kt-datatable__cell--sort"><span style="width: 141px;">Group</span></th> ';
                html += '<th data-field="Car Model" class="kt-datatable__cell kt-datatable__cell--sort"><span style="width: 141px;">Payment Term</span></th> ';
                html += '<th data-field="Color" class="kt-datatable__cell kt-datatable__cell--sort"><span style="width: 141px;">Tax Group</span></th> ';
                html += '</tr>';
                html += '</thead>';
                html += '<tbody style="" class="kt-datatable__body">';

                for (var i = 0; i < result.data.length; i++) {
                    html += '<tr  data-row="' + i + '" class="kt-datatable__row">';
                    html += '<td class="kt-datatable__cell"><span style="width: 141px;">' + result.data[i].vendgrpno + '</span></td> ';
                    html += '<td class="kt-datatable__cell"><span style="width: 141px;">' + result.data[i].vendgrpstxt + '</span></td> ';
                    html += '<td class="kt-datatable__cell"><span style="width: 141px;">' + result.data[i].fk_payterm_paytermno + '</span></td> ';
                    html += '<td class="kt-datatable__cell"><span style="width: 141px;">' + result.data[i].taxgrpno + '</span></td> ';
                    html += '</tr> ';
                }
                html += '</tbody></table> ';
                document.getElementById('tbl').innerHTML = html;


            },
            error: function (jqXhr, textStatus, errorMessage) {
                console.log(errorMessage);
            }
        });
    }
    

    function AddGroup() {
        $.ajax({
            url: "/ResourceGroup/Add",
            dataType: 'json',
            type: "POST",
            data: FieldsData(),
            success: function (result) {
                console.log(result.msg);
                console.log(result.isInserted);
            },
            error: function (jqXhr, textStatus, errorMessage) {
                console.log(errorMessage);
            }
        });
    }


    function FieldsData() {
        var formData = {
            drivergrpno: $('#DriverGroupNo').val(),
            driverltxt: $('#DriverDescription').val(),
            drivergrpstxt: $('#DriverName').val(), 
            fk_religionmst_religionno: $('#Religion option:selected').val(),
            taxgrpno: $('#TaxGroup').val(),
        };
        return formData;
    }

});