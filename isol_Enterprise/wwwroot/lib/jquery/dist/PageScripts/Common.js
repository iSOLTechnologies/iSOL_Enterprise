var globalWarehouseCode;
var globalItemCode;
var AdvSrch;
var LookTable;
var allLookupByPage = [];

var SelectedIndex1;
$(document).ready(function () {
    UserRoles();
    GetAdvanceSearch();
    $(document).on('click', '#G_btnSave', function () {
        AddBatchData();
    })

    $('#B_BatchExpiry').datepicker({
        "format": 'dd/mm/yyyy',
        "autoclose": true
    });

    $('#G_BatchExpiry').datepicker({
        "format": 'dd/mm/yyyy',
        "autoclose": true
    });
});

function GetAdvanceSearch() {

    var Guid = $('#PageGuid').val();

    return $.ajax({
        url: '/Common/GetAdvanceSearch',
        dataType: 'json',
        data: { Guid: Guid },
        type: 'Post',
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        
        result = result.data;
        if (result != undefined && result != '' && result != null) {
            GenerateAdvanceSearch(result);
        }
     
        if (result.isError) {
            toastr.error(result.message);
            $('#BtnDelete').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#BtnDelete').attr('disabled', false);

        }
    });



}

function GenerateAdvanceSearch(result) {
    console.log(result);
    var html = '';

    html += '  <div class="kt-portlet kt-portlet--head-sm kt-portlet--collapse" style="border: solid 2px #5d78ff" data-ktportlet="true" id="kt_portlet_tools_1"> ';
    html += '      <div class="kt-portlet__head">';
    html += '         <div class="kt-portlet__head-label">';
    html += '           <h3 class="kt-portlet__head-title">';
    html += '              Advance Search';
    html += '           </h3>';
    html += '        </div>';
    html += '        <div class="kt-portlet__head-toolbar">';
    html += '            <div class="kt-portlet__head-group">';
    html += '               <a href="#" data-ktportlet-tool="toggle" class="btn btn-sm btn-icon btn-clean btn-icon-md" aria-describedby="tooltip_dnafllv8as"><i class="la la-angle-down"></i></a>';
    html += '           </div>';
    html += '         </div>';
    html += '     </div>';
    html += '     <!--begin::Form-->';
    html += '    <form class="kt-form kt-form--label-right" kt-hidden-height="339" style="display: none; overflow: hidden;">';
    html += '       <div class="kt-portlet__body">';
    html += '           <div class="row">';

    for (var i = 0; i < result.length; i++) {
        var AdvSrcId = result[i].searchColumn.replace(".", "_");
        html += '                <div class="col-lg-3">';
        html += '                   <div class="form-group">';
        html += '                       <label for="' + result[i].fieldText + '">' + result[i].fieldText + '</label>';
        html += '                       <input type="text" class="form-control" id="' + AdvSrcId + '" placeholder="' + result[i].fieldText + '">';
        html += '                    </div>';
        html += '                   </div>';
    }

    html += '                   </div>';
    html += '               </div>';
    html += '                  <div class="kt-portlet__foot">';
    html += '                    <div class="">';
    html += '                         <button type="button" id="btnAdvanceSearch" class="btn btn-brand">Search</button>';
    html += '                         <button type="reset" id="btnAdvanceClear" class="btn btn-secondary">Clear</button>';
    html += '                      </div>';
    html += '                 </div>';
    html += '      </form>';
    html += '              <!--end::Form-->';
    html += '   </div>';

    var Guid = $('#PageGuid').val();
    $('.kt-portlet__body').prepend(html);
    var portlet = new KTPortlet('kt_portlet_tools_1');
}




function UserRoles() {
    var url = window.location.pathname;
    url = url.includes("Dashboard");
    if (url == false) {
        var Guid = $('#PageGuid').val();
        var dd = {
            Guid: Guid
        };
        var data = AjaxCall('Common', 'GetUserPageActivity', 'GET', dd);
        data = data.data;
        console.log(data);
        var html = "";
        for (var i = 0; i < data.length; i++) {

            if (data[i].roleActivityCode == 'A') {
                html += '  <a href="#" id="BtnAdd" class="btn btn-brand btn-elevate btn-icon-sm">';
                html += '     <i class="la la-plus"></i> Add </a> ';
            }

            if (data[i].roleActivityCode == 'V') {
                html += '  <a href="#" id="BtnView" class="btn btn-brand btn-elevate btn-icon-sm">';
                html += '     <i class="la la-eye"></i> View </a> ';
            }

            if (data[i].roleActivityCode == 'E') {
                html += '   <a href="#" id="BtnEdit" class="btn btn-success btn-elevate btn-icon-sm"> ';
                html += '       <i class="la la-pencil-square"></i> Edit </a> ';
            }

            if (data[i].roleActivityCode == 'D') {
                html += '   <a href="#" id="BtnDelete" class="btn btn-danger btn-elevate btn-icon-sm"> ';
                html += '     <i class="la la-trash"></i> Delete </a>';
            }

            if (data[i].roleActivityCode == 'S') {
                html += '   <a href="#" id="btnSave" class="btn btn-brand btn-elevate btn-icon-sm"> ';
                html += '     <i class="la la-check"></i> Save </a> ';
                html += '                         &nbsp;';
                html += '   <a href="#" id="kt_quick_panel_close_btn" class="btn btn-secondary"> ';
                html += '     <i class="la la-close"></i> Cancel </a>';
            }
        }


        $('.kt-portlet__head-actions').append(html);
        //   $('.kt-portlet__head-toolbar').append(html);
    }

}

function DataPlacement(data, divElement) {

    if (data != null || data != undefined) {
        var div = document.getElementById(divElement);

        $(div).find('input, select, img, .kt-avatar__holder').each(function (index, item) {//input,select,img,.kt-avatar__holder,span,
             
            var elemen = $(this)[0].localName;
            var Type = $(this).attr('type');
            var id = $(this).attr('id');
            id = id[0].toLowerCase() + id.slice(1);
            if (Type == 'Text') {
                var name = $(this).attr('id');
                var sadas = data[id];
                $(this).val(data[id]);
            }
            if (Type == 'Date') {
                var name = $(this).attr('id');
                var sadas = data[id];
                $(this).val(moment(data[id]).format("YYYY-MM-DD"));
                // $(this).val(data[id]);
            }

            if (Type == 'Time') {
                debugger
                var name = $(this).attr('id');
                var sadas = data[id];
                var sads = moment(data[id],'HH:mm').format('HH:mm')
                $(this).val(moment(data[id], 'HH:mm').format('HH:mm'));                // $(this).val(data[id]);
            }

            if (Type == 'Number') {
                var name = $(this).attr('id');
                var sadas = data[id];
                $(this).val(data[id]);
            }
            if (Type == 'hidden') {
                var name = $(this).attr('id');
                var sadas = data[id];
                $(this).val(data[id]);
            }

            if (Type == 'password') {
                var name = $(this).attr('id');
                var sadas = data[id];
                $(this).val(data[id]);

            }
            if (Type == 'Checkbox') {

                var name = $(this).attr('id');
                var sadas = data[id];
                $(this).prop('checked', data[id]);

            }
            if ($(this)[0].localName == 'div') {

                var id = $(this).attr('id');
                var sadas = data[id];
                $(this).attr('style', 'background-image: url("../' + data[id] + '")');

            }
            if ($(this)[0].localName == 'select') {

                // var id;
                var name = $(this).attr('name');
                $(this).val(data[id]);
                //var sadas = data[id];
                //if (id != data[id]) {
                //   // var id = $(this).attr('id');

                //} else {
                //    $(this).val(data[name]);
                //}
            }
            if ($(this)[0].localName == 'span') {
                var name = $(this).attr('id');
                var sadas = data[name];
                $(this).html(data[name]);
            }
            if ($(this)[0].localName == 'img') {

                var id = $(this).attr('id');
                var sadas = data[id];
                $(this).attr('src', '../' + data[id]);

            }
        });
    }
}



function SetvaluesfromLookup(PageId, LookupCode) {

    var self = this;
    return $.ajax({
        url: "/Common/SetLookupDataOnFields",
        type: "GET",
        dataType: 'json',
        cache: false,
        async: false,
        data: { PageId: PageId, LookupCode: LookupCode },
        success: function (resp) {
        }
    }).done(function (resp) {

        return resp.data;
    });
}


function validation(PageId, Activity) {
    var res = getValidations(PageId, Activity).done(function (data) {

        return data;
    });

    var resTable = getTableValidations(PageId, Activity).done(function (data) {

        return data;
    });

    xflag = [];
    isvalid = null

    toastr.remove();

    if (res.responseJSON.data != null) {
        for (var i = 0; i < res.responseJSON.data.length; i++) {

            if (res.responseJSON.data[i].isMandatory == true) {
                xflag.push(mandatoryValidate('#' + res.responseJSON.data[i].fieldId, res.responseJSON.data[i].fieldName));
            }

            if (res.responseJSON.data[i].dataType == 'string') {
                xflag.push(D_alphaValidate('#' + res.responseJSON.data[i].fieldId, res.responseJSON.data[i].fieldName, res.responseJSON.data[i].regex, res.responseJSON.data[i].regexName));
                xflag.push(limitValidate('#' + res.responseJSON.data[i].fieldId, + res.responseJSON.data[i].fieldSize, res.responseJSON.data[i].fieldName));

            }

            if (res.responseJSON.data[i].dataType == 'decimal') {
                xflag.push(D_decimal('#' + res.responseJSON.data[i].fieldId, res.responseJSON.data[i].fieldName, res.responseJSON.data[i].regex, res.responseJSON.data[i].regexName));

            }

            if (res.responseJSON.data[i].dataType == 'dropdown' && res.responseJSON.data[i].isMandatory == true) {
                xflag.push(mandatoryValidate('#' + res.responseJSON.data[i].fieldId, res.responseJSON.data[i].fieldName));
            }
        }
    }



    if (resTable.responseJSON.data != null) {
        for (var i = 0; i < resTable.responseJSON.data.length; i++) {

            for (var j = 0; j < resTable.responseJSON.data[i].listPageRoleTableDetail.length; j++) {


                $('#' + resTable.responseJSON.data[i].tableCode + ' .rowItemDetail').each(function (index, item) {

                    if (resTable.responseJSON.data[i].listPageRoleTableDetail[j].dataType == 'string') {
                        xflag.push(D_alphaValidate('#' + resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldId, resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldName, resTable.responseJSON.data[i].listPageRoleTableDetail[j].regex, resTable.responseJSON.data[i].listPageRoleTableDetail[j].regexName));
                        xflag.push(limitValidate('#' + resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldId, + resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldSize, resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldName));

                    }
                    if (res.responseJSON.data[i].dataType == 'decimal') {
                        xflag.push(D_decimal('#' + resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldId, resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldName, resTable.responseJSON.data[i].listPageRoleTableDetail[j].regex, resTable.responseJSON.data[i].listPageRoleTableDetail[j].regexName));

                    }
                    if (res.responseJSON.data[i].dataType == 'dropdown' && res.responseJSON.data[i].isMandatory == true) {
                        xflag.push(mandatoryValidate('#' + resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldId, resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldName));
                    }


                    if (resTable.responseJSON.data[i].listPageRoleTableDetail[j].isMandatory == true) {
                        xflag.push(mandatoryValidate('#' + resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldId, resTable.responseJSON.data[i].listPageRoleTableDetail[j].fieldName));
                    }

                });



            }
        }
    }



    if (true) {

    }

    for (var i = 0; i < xflag.length; i++) {
        if (xflag[i] == true) {
            isvalid = false;
        }
    }

    if (isvalid != false) {
        // SCMApp.StartSpinner($(this));
        return true;
    } else {
        return false;
    }
}



function breadCrums(PageId) {

    $.ajax({
        url: "/Common/breadCrums",
        type: "Get",
        dataType: 'json',
        cache: false,
        data: { PageId: PageId },
        success: function (resp) {


            $('.page-breadcrumb').html('<li> <span>' + resp.moduleName + '</span> <i class="fa fa-chevron-right"></i></li> <li> <span>' + resp.subModuleName + '</span> <i class="fa fa-chevron-right"></i> </li ><li> <span>' + resp.pageName + '</span> </li>');
            $('.page-title').html(resp.pageName);

        }, error: function (jqXhr, textStatus, errorMessage) {
        }
    });
}


function UnBindClick(element, Istrue) {
    var self = this;
    if (Istrue == true) {

        $(element).hide();
        $(element).children().bind('click', function () { return false; });
    }
    if (Istrue == false) {

        $(element).show();
        $(element).children().unbind('click');
    }
}

function DisableFields(id) {

    var div = document.getElementById(id);
    $(div).find('input,textarea,select,checkbox').each(function () {

        $(this).attr('disabled', true);
    });
}

function ResetForm() {

    var div = document.getElementById('tab_New');
    $(div).find('input,textarea,select,checkbox').each(function () {
        $(this).val('');
    });
}

function EnableField() {

    var div = document.getElementById('tab_New');
    $(div).find('input,textarea,select,checkbox').each(function () {

        $(this).attr('disabled', false);
    });
}


function ResertForm2(formName) {
    var self = this;
    $(formName)[0].reset();
    $('#Id').val("");

    IsActive: $('#chkIsActive').prop("checked", false);
}

function ResetFormById(formId) {
    var self = this;
    var div = document.getElementById(formId);
    $(div).find('input,textarea,select,checkbox').each(function () {
        $(this).val('');
    });

  
    //$('#' + formName)[0].reset();
    //$('#Id').val("");

    IsActive: $('#chkIsActive').prop("checked", false);
}

function DisableFields_O() {

    var div = document.getElementById('tab_New');
    $(div).find('input[type=text],textarea,select,checkbox').each(function () {
        $(this).attr('disabled', true);

    });
    $('#tblItemDetails .rowItemDetail').each(function (index, item) {

        $(this).find("input").attr('disabled', true);
        $(this).find("select").attr('disabled', true);

    })
}


function DisableDetail_O() {

    $('#tblItemDetails .rowItemDetail').each(function (index, item) {

        $(this).find("input").attr('disabled', true);
        $(this).find("select").attr('disabled', true);

    })
}

function EnableFields_O() {
    var div = document.getElementById('tab_New');
    $(div).find('input,textarea,select,checkbox').each(function () {
        $(this).attr('disabled', false);
    });
    $('#tblItemDetails .rowItemDetail').each(function (index, item) {

        $(this).find("input").attr('disabled', false);
        $(this).find("select").attr('disabled', false);

    })
}

function ResetFields_O(formName) {
    $('#' + formName)[0].reset();
    var Element = document.getElementById(formName);
    $(Element).find('input,textarea,select,checkbox').each(function () {
        $(this).val('');
        $(this).change();
    });

    $('#tblItemDetails .rowItemDetail').each(function (index, item) {

        if (index == 0) {
            $(this).find("input").val('');
            $(this).find("select").val('');
        } else {
            $(this).closest('tr').remove();
        }
    })
}


function ResetDetails_O() {

    $('#tblItemDetails .rowItemDetail').each(function (index, item) {

        if (index == 0) {
            $(this).find("input").val('');
            $(this).find("select").val('');
        } else {
            $(this).closest('tr').remove();
        }
    })
}

function UnBindClick_O(Istrue) {
    var self = this;
    if (Istrue == true) {

        $('#btnSave').hide();
        $('#btnSave').children().bind('click', function () { return false; });
        $('#btnCancel').hide();
        $('#btnCancel').children().bind('click', function () { return false; });
    }
    if (Istrue == false) {

        $('#btnSave').show();
        $('#btnSave').children().unbind('click');
        $('#btnCancel').show();
        $('#btnCancel').children().unbind('click');
    }
}

function actionButtons() {
    var buttons = "<button data-operation='Edit' class='btn btn-xs yellow BtnEditTbl'> <i class='fa fa-edit'></i></button> <button data-operation='View' class='btn btn-xs yellow BtnViewTbl'> <i class='fa fa-eye'></i></button><button data-operation='Delete' class='btn btn-xs red BtnDeleteTbl'> <i class='fa fa-trash'></i></button><button data-operation='Print' class='btn btn-xs blue BtnPrintTbl'> <i class='fa fa-print'></i></button>";
    return buttons;
}

function GetBase64(array, ElementId) {

    //var uploadField = document.getElementById(ElementId);

    //uploadField.onchange = function () {
    //    if (file.files[0].size > 2097152) {
    //        alert("File is too big!");
    //        this.value = "";
    //    } else {

    //    }

    //};

    var file = document.getElementById(ElementId);

    for (var i = 0; i < file.files.length; i++) {

        if (file.files[i].size > 2097152) {
            alert("File is too big!");
            this.value = "";
            $('#' + ElementId).val('');

        } else {


            const reader = new FileReader();
            if (file) {
                reader.readAsDataURL(file.files[i]);
            }
            reader.addEventListener("load", function () {
                // convert image file to base64 string
                var sgcvx = reader.result;
                array.push(reader.result);
            }, false);
        }
    }
}


function Lookup(LookupCode, WhereClause) {

    var columns = [];
    var jasonData;
    $.ajax({
        url: "/Common/GetColumnNames",
        type: "GET",
        dataType: 'json',
        async: false,
        data: { LookupCode: LookupCode },
        success: function (result) {

            if (result.data != null) {

                $("#LookupThead").empty();

                $('#Lookuplbl').text(result.data[0].lookupName);
                $('#LookupType').text(result.data[0].isHeader);

                var keys = result.data[0].lookupData[0];
                var html = '';
                html += '<tr>';

                for (var key in keys) {
                    columns.push({ 'data': key });
                    html += '<th>' + key + '</th>';
                }
                columns.push({
                    'data': "Actions", "render": function (data, type, row) {
                        return "<button id='close' data-operation='Select' class='btn btn-xs yellow'> <i class='fa fa-check-circle'></i></button>";
                    }
                });

                html += '<th> Action </th></tr>';

                document.getElementById('LookupThead').innerHTML += html;
                GetLookUpFormData(result, columns, LookupCode, WhereClause);
            }
            else {
                toastr.error('Lookup not Define Database');
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
        }

    }).done(function (resp) {

        jasonData = resp.data[0].lookupData;
    });

}

function LoadLookup(LookupCode, WhereClause, columns) {

    if (WhereClause.length > 0) {
        WhereClause = JSON.stringify(WhereClause);
    }

    $('#LookupTbl').DataTable({
        "destroy": true,
        "async": false,
        "processing": true,
        "ordering": false,
        "serverSide": true,
        "paging": true,
        "pagingType": "full_numbers",
        "keys": true,
        "language": {
            "infoFiltered": ""
        },
        "ajax": {
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            "url": "/Common/GetLookupData",
            data: { LookupCode, WhereClause },
        },
        "columns": columns,

    });

}

function GetLookUpFormData(result, columns, LookupCode, WhereClause) {


    var ListWhereClause = [];
    var whereClauseData = [];
    var where = [];
    var whereCl = [];

    whereCl = result.data[0].whereClause.split(',');
    for (var i = 0; i < whereCl.length; i++) {
        where[i] = whereCl[i].split('@')[1];
    }

    for (var i = 0; i < where.length; i++) {
        var xxx = WhereClause.closest('.rowItemDetail').find('#' + where[i]);
        if ($(xxx).val() != '') {
            ListWhereClause[i] = $(xxx).val();
            var GoodRcvd = {
                Data: $(xxx).val(),
                Parameter: where[i]
            }
            whereClauseData.push(GoodRcvd);
        }

    }
    LoadLookup(LookupCode, whereClauseData, columns);

}


function SelectRow() {
    return "<button style='cursor:pointer' class='btn btn-xs white'> <i class='fa fa-check-circle'></i></button>";
}


function ReloadLookUpTable() {
    var tableData = {
        Guid: Guid,
        AdvSrch: AdvSrch
    }

    LookTable.ajax.url("/Common/GetLookupData?Guid=" + Guid + '&AdvSrch=' + AdvSrch).load();

}

function AllListViews(Guid) {


    var columns = [];
    // var data = [];
    var dataRow = [];
    var jasonData;
    $.ajax({
        url: "/Common/GetColumnNames",
        type: "GET",
        dataType: 'json',
        async: false,
        data: { Guid: Guid },
        success: function (result) {

            if (result.data != null) {
                 

                $("#LookupThead").empty();
                $('#LookupTbody').html('');

                var html = '';
                html += '<tr>';

                //html += '<th></th>';
                //columns.push({
                //    'data': render: function(data, type, row) {
                //        var abc = {
                //            field: 'RecordID',
                //            title: '#',
                //            sortable: 'asc',
                //            width: 30,
                //            type: 'number',
                //            selector: false,
                //            textAlign: 'center'
                //        }
                //    }

                //});
                for (var i = 0; i < result.data.length; i++) {

                    //   html += '<th>'+i+'</th>';


                    if (result.data[i].dataType == 'Date') {
                        columns.push({
                            'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1), render: function (data, type, row) { if ((data != undefined) && (data != "")) return moment(data).format("DD-MMM-YYYY"); else return "" }
                        });
                    }
                    else if (result.data[i].dataType == 'Action') {
                        columns.push({
                            'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1), render: function (data, type, row) {
                                if (data != undefined && data != "") {
                                    if (data == true) { return '<i style="color:#fff;font-size:25px" class="fas fa-hand-paper"></i>' }
                                    else if (data == false) { return false }
                                }
                                else { return '' }
                            }
                        });
                        
                    }
                    else
                        if (result.data[i].dataType == 'Datetime') {
                            columns.push({
                                'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1), render: function (data, type, row) { if ((data != undefined) && (data != "")) return moment(data).format("DD-MMM-YYYY HH:mm:ss"); else return "" }
                            });
                        } else
                            if (result.data[i].dataType == 'Priority') {
                                columns.push({
                                    'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1), render: function (data, type, row) {
                                        if (data != undefined && data != "") {
                                            // if (data == 'H') { return '<span class="kt-badge  kt-badge--danger kt-badge--inline kt-badge--pill">High</span>' }
                                            if (data == 'H') { return '<i class="Redflg fas fa-exclamation-triangle"></i>' }
                                            else if (data == 'M') { return '<i class="Yellowflg fas fa-exclamation-triangle"></i>' }
                                            else if (data == 'L') { return '<i class="Greenflg fas fa-exclamation-triangle"></i>' }
                                        } else { return '' }
                                    }
                                });
                            }
                            else
                                if (result.data[i].dataType == 'Tags') {
                                    columns.push({
                                        'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1), render: function (data, type, row) {
                                            debugger
                                            if (data != undefined && data != "") {
                                                datav = data.split(',');
                                                var html = '';
                                                for (var i = 0; i < datav.length; i++) {
                                                    html += '<span style="margin-right:2px;margin-bottom:2px" class="tagify__tag-text kt-badge  kt-badge--brand kt-badge--inline">' + datav[i] + '</span>';
                                                }

                                                return html;
                                            }
                                            else {
                                                return '';
                                            }
                                           
                                        }
                                    });
                                }
                                else
                                if (result.data[i].dataType == 'Status') {
                                    columns.push({
                                        'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1), render: function (data, type, row) {
                                            if (data == false) {
                                                return '<span class="kt-badge  kt-badge--danger kt-badge--inline kt-badge--pill">In Active</span>'
                                            }
                                            else if (data == true) {
                                                return '<span class="kt-badge  kt-badge--brand kt-badge--inline kt-badge--pill"> Active</span>'
                                            }
                                            else {
                                                return '<span class="kt-badge  kt-badge--warning kt-badge--inline kt-badge--pill">Un Assign</span>'
                                            }
                                        }
                                    });
                                }
                                else {
                                    columns.push({ 'data': result.data[i].columnName[0].toLowerCase() + result.data[i].columnName.slice(1) });
                                }
                    html += '<th>' + result.data[i].columnName + '</th>';
                }


                //columns.push({
                //    'data': "Actions", "render": function (data, type, row) {
                //        return "<button id='close' data-operation='Select' class='btn btn-xs yellow'> <i class='fa fa-check-circle'></i></button>";
                //    }
                //});

                //    html += '<th> Action </th></tr>';

                html += '</tr>';

                document.getElementById('LookupThead').innerHTML += html;
                // document.getElementById('LookupTFoot').innerHTML += html;
                ListViewData(Guid, columns);
            }
            else {
                toastr.error('Lookup not Define Database');
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
        }

    }).done(function (resp) {

        // jasonData = resp.data[0].lookupData;
    });

}

function ListViewData(Guid, columns) {
    LookTable = null
    var tableData = {
        Guid: Guid,
        AdvSrch: AdvSrch
    }

    LookTable = $('#T_' + Guid).DataTable({

        "processing": true,
        "autoWidth": false,
        "bSortable": true,
        "ordering": false,
        "serverSide": true,
        "paging": true,
        "responsive": true,
        "pagingType": "full_numbers",
        "keys": true,
        "language": {
            "infoFiltered": ""
        },
        "ajax": {
            "type" :'Get',
            "url": "/Common/GetLookupData",
            "data": tableData,
        },

        "columns": columns,
        "createdRow": function (row, data, dataIndex) {
            //console.log(data);
            if (data['stop Action'] == true) {
                $(row).css( { 'background-color':' #fd397a !important'},{ 'color': '#fff !important' });
            }
        }
    });

    columns = [];

}

$(document).on('click', '#btnAdvanceClear', function () {

    var Guid = $('#PageGuid').val();
    LookTable.ajax.url("/Common/GetLookupData?Guid=" + Guid + "&adn=" + '').load();
});

$(document).on('click', '#btnAdvanceSearch', function () {

    var Guid = $('#PageGuid').val();

    return $.ajax({
        url: '/Common/GetAdvanceSearch',
        dataType: 'json',
        data: { Guid: Guid },
        type: 'Post',
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        result = result.data;


        var json = ''; 
        json += '[';

        for (var i = 0; i < result.length; i++) {
            var daw = result[i].searchColumn.replace(".", "_");


            json += '{';
            json += '"SearchColumn" : "' + result[i].searchColumn + '",';
            json += '"Value" : "' + $('#' + daw).val() + '"';
            json += '}';
            json += ',';
        }


        json = json.slice(0, -2);
        json += '}';
        json += ']';

        LookTable.ajax.url("/Common/GetLookupData?Guid=" + Guid + "&adn=" + json).load();


        if (result.isError) {
            toastr.error(result.message);
            $('#BtnDelete').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#BtnDelete').attr('disabled', false);

        }
    });
})

function AjaxCallReturn(controller, action, type, param) {

    return $.ajax({
        url: '/' + controller + '/' + action,
        dataType: 'json',
        data: param,
        type: type,
        cache: false,
        async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        return result;
    });

}


function AjaxCall(controller, action, type, param) {

    var res = AjaxCallReturn(controller, action, type, param).done(function (data) {

        return data;
    });


    //if (res.responseJSON.IsInvalidKey || res.responseJSON.IsTokenExpired) {
    //    toastr.error(res.responseJSON.msg);
    //    setTimeout(function () {
    //        window.location.href = "/Login/Signout"
    //    }, 2000);
    //}

    if (res.responseJSON.isInserted) {

        toastr.success(res.responseJSON.message);
    }
    if (res.responseJSON.isError) {
        toastr.error(res.responseJSON.message);
    }
    return res.responseJSON;
}

function AjaxCallAsync(controller, action, type, param) {

    var res = AjaxCallAsycnReturn(controller, action, type, param).done(function (data) {

        return data;
    });

    if (!res.responseJSON) {
        toastr.warning('Server not replied');
        if (res.responseJSON.isInserted) {
            toastr.success(res.responseJSON.message);
        }
        if (res.responseJSON.isError) {
            toastr.error(res.responseJSON.message);
        }
    }

    return res.responseJSON;
}

function AjaxCallAsycnReturn(controller, action, type, param) {

    return $.ajax({
        url: '/' + controller + '/' + action,
        dataType: 'json',
        type: 'Get',
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        return result;
    });

}

function Get(data, divElement) {

    if (data != null || data != undefined) {
        var div = document.getElementById(divElement);

        $(div).find('input, select, img, .kt-avatar__holder').each(function (index, item) {//input,select,img,.kt-avatar__holder,span,

            var elemen = $(this)[0].localName;
            var Type = $(this).attr('type');
            var id = $(this).attr('id');
            var name = $(this).attr('name');
            name = name.substr(0, 1).toLowerCase() + name.substr(1);
            var sadas = data[name];

            if (Type == 'text') {
                //var name = $(this).attr('name');
                //var sadas = data[name];
                $(this).val(data[name]);
            }
            if (Type == 'hidden') {
                //var name = $(this).attr('name');
                //var sadas = data[name];
                $(this).val(data[name]);
            }

            if (Type == 'password') {
                //var name = $(this).attr('name');
                //var sadas = data[name];
                $(this).val(data[name]);

            }
            if (Type == 'checkbox') {

                //var name = $(this).attr('name');
                //var sadas = data[name];
                $(this).prop('checked', data[name]);

            }
            if ($(this)[0].localName == 'div') {

                var id = $(this).attr('id');
                var sadas = data[id];
                $(this).attr('style', 'background-image: url("../' + data[id] + '")');

            }
            if ($(this)[0].localName == 'select') {

                var id;
                //var name = $(this).attr('name');
                //var sadas = data[name];
                if (name != data[name]) {
                    var id = $(this).attr('id');
                    $(this).val(data[name]);
                } else {
                    $(this).val(data[name]);
                }
            }
            if ($(this)[0].localName == 'span') {
                var name = $(this).attr('id');
                //   var sadas = data[name];
                $(this).html(data[name]);
            }
            if ($(this)[0].localName == 'img') {

                var id = $(this).attr('id');
                // var sadas = data[id];
                $(this).attr('src', '../' + data[id]);

            }
        });
    }
}

function IsNullDate(data, id) {
    if ((data != undefined) && (data != "")) {
        // $('#' + id).val(moment(data).format("DD/MM/YYYY"));
        return $('#' + id).val(moment(data).format("DD/MM/YYYY"));
    }
    else {
        return "";
    }

}

function DateDiff(DateFrom, DateTo) {



    var start = moment($('#' + DateFrom).val(), "DD/MM/YYYY").format('YYYY-MM-DD')
    var end = moment($('#' + DateTo).val(), "DD/MM/YYYY").format('YYYY-MM-DD')
    //var start = $('#' + DateFrom).val();
    //var end = $('#' + DateTo).val();
    start = new Date(start);
    end = new Date(end);
    // end - start returns difference in milliseconds 
    var diff = new Date(end - start);

    // get days
    var days = diff / 1000 / 60 / 60 / 24;


    //var start = $('#' + DateFrom).val();
    //var end = $('#' + DateTo).val();
    //days = (end - start) / (1000 * 60 * 60 * 24);
    return (Math.round(days));
}
