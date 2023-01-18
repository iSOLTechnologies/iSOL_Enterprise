
var StatusCode_;
var Guid;
$(document).ready(function () {
    this.table = null;
    var self = this;
    var RowDataPrint = null;
    var rowdata;

    //  $('#DocumentDate').attr('disabled', true);

    $('#DaysAgreed').attr('disabled', true);
    $('#DaysTaken').attr('disabled', true);
    $('#DepartmentCode').attr('disabled', true);

    //$('#DepartmentCode').val($('#_DepartmentCode').val());
    // GetAll();

    var UserRoleCode = $('#UserRoleCode').val();

    if (UserRoleCode == 'SAD') {
        $('#DocumentDate').attr('disabled', true);
        $('#ComplaintCategoryCode').attr('disabled', true);
        $('#AssignTo').attr('disabled', true);
        $('#StartDate').attr('disabled', true);
        $('#UserId').attr('disabled', true);
        $('#Task').attr('disabled', true);
        $('#Description').attr('disabled', true);
    }

    var PageGuid = $('#PageGuid').val();

    Guid = $('#Guid').val();



    $(document).on('click', '#btnSave_A', function () {
        $(this).addClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        $(this).attr('disabled', true);
        Valiation_A();
    })


    $(document).on('click', '#btnSaveBack', function () {
        $(this).addClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        $(this).attr('disabled', true);
        if (Valiation_A()) {
           // window.history.back();
           // window.location.href = '/TicketingSystem/Index_A?Source=9245fe4ad502471cb9ed9d1a042474821101202105424456';
        }
    })

    $(document).on('change', '#UserId', function () {
        
        if ($('#UserId').val() != '' && $('#UserId').val() != null && $('#UserId').val() != undefined) {
            GetDepartment();
        }
    })

    $(document).on('change', '#StartDate', function () {
        
        $('#SendEmailtoAssignee').prop('checked', true);
        if ($('#StartDate').val() != '' && $('#StartDate').val() != null && $('#StartDate').val() != undefined && $('#ExpectedCompletionDate').val() != '' && $('#ExpectedCompletionDate').val() != null && $('#ExpectedCompletionDate').val() != undefined) {
            $('#DaysAgreed').val(DateDiff('StartDate', 'ExpectedCompletionDate'));
        }
    })

    $(document).on('change', '#ExpectedCompletionDate', function () {
        if ($('#ExpectedCompletionDate').val() != '' && $('#ExpectedCompletionDate').val() != null && $('#ExpectedCompletionDate').val() != undefined && $('#StartDate').val() != '' && $('#StartDate').val() != null && $('#StartDate').val() != undefined) {
            $('#DaysAgreed').val(DateDiff('StartDate', 'ExpectedCompletionDate'));
        }

    })

    $(document).on('change', '#ActualCompletionDate', function () {
        if ($('#ActualCompletionDate').val() != '' && $('#ActualCompletionDate').val() != null && $('#ActualCompletionDate').val() != undefined) {
            $('#DaysTaken').val(DateDiff('StartDate', 'ActualCompletionDate'));
        }

    })


    $(document).on('click', '#btnCancel', function () {
        ClearForm();
        window.location.href = '/TicketingSystem/Index_A?Source=9245fe4ad502471cb9ed9d1a042474821101202105424456';
    })

    $(document).on('click', '#btnBack', function () {
        window.history.back();
        //window.location.href = '/TicketingSystem/Index_A?Source=9245fe4ad502471cb9ed9d1a042474821101202105424456';
    })

    $('#DocumentDate').datepicker({
        "format": 'dd/mm/yyyy',
        "autoclose": true
    });

    $('#StartDate').datepicker({
        "format": 'dd/mm/yyyy',
        "autoclose": true
    });

    $('#ExpectedCompletionDate').datepicker({
        "format": 'dd/mm/yyyy',
        "autoclose": true
    });

    $('#ActualCompletionDate').datepicker({
        "format": 'dd/mm/yyyy',
        "autoclose": true
    });

    $("#DocumentDate").val(moment().format("DD/MM/YYYY").toString());

    if (Guid != null && Guid != '' && Guid != undefined) {
      
        $('#btnSave_A').hide();
        $('#heading').text('Edit Ticket');
        $('#SpanBtnSave').text('Save & Back');
        $('#btnCancel').text('Cancel & Back');
        $('#breadcrumPageName').text('Edit Ticket');

        GetData();
    }

    var UserRoleCode = $('#UserRoleCode').val();

    if (UserRoleCode == 'SAD') {
        $('#DocumentDate').attr('disabled', true);
        $('#ComplaintCategoryCode').attr('disabled', true);
        $('#AssignTo').attr('disabled', true);
        $('#StartDate').attr('disabled', true);
        $('#UserId').attr('disabled', true);
        $('#Task').attr('disabled', true);
        $('#Description').attr('disabled', true);
    }

    $('#SendEmailtoAssignee').prop('checked', false);


    $(document).on('change', '#UploadPurchaseRequest', function () {


        GetBase64(attch, 'UploadPurchaseRequest')

    });

    $(document).on('click', '.SpanURL', function () {

        var val = $(this).attr('Link');
     var win =   window.open(val, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            alert('Please allow popups for this website');
        }
    });
});


function AddTicket_A() {
    return $.ajax({
        url: '/TicketingSystem/Add_A',
        dataType: 'json',
        data: GetFormData_A(),
        type: 'POST',
        //cache: false,
        //async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        if (result.isInserted) {

            toastr.success(result.message);
            ClearForm();
            setTimeout(function () {
                $('#btnSave_A').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                $('#btnSave_A').attr('disabled', false);
            }, 500);
          
           

        }
        if (result.isError) {
            toastr.error(result.message);
            $('#btnSave_A').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#btnSave_A').attr('disabled', false);

        }

        });


    //var data = AjaxCall("TicketingSystem", "Add_A", "Post", GetFormData_A());
    //if (data.isInserted) {
    //    ClearForm();

    //    return true;
    //}
    //else {

    //    return false;
    //}

}

function EditTicket_A() {
    debugger
    return $.ajax({
        url: '/TicketingSystem/Edit_A',
        dataType: 'json',
        data: GetFormData_A(),
        type: 'POST',
        //cache: false,
        //async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        
        if (result.isInserted) {

            toastr.success(result.message);
            ClearForm();
            setTimeout(function () {
                $('#btnSaveBack').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            }, 500);
            $('#btnSaveBack').attr('disabled', false);
            setTimeout(function () {
                window.history.back();
            }, 800);


        }
        if (result.isError) {
            toastr.error(result.message);
            $('#btnSaveBack').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#btnSaveBack').attr('disabled', false);

        }

    });
    //var data = AjaxCall("TicketingSystem", "Edit_A", "Post", GetFormData_A());
    //if (data.isInserted) {
    //    ClearForm();

    //    return true;
    //}
    //else {

    //    return false;
    //}
}

function ClearForm() {
    ResetFields_O('F_' + $('#PageGuid').val());
    $("#DocumentDate").val(moment().format("DD/MM/YYYY").toString());
    $("#Name").val($('#sessionName').val());
}

function Valiation_A() {
    xflag = [];
    isvalid = null

    toastr.remove();
    xflag.push(mandatoryValidate('#DocumentDate', 'Date'));
    xflag.push(mandatoryWithAlphaValidate('#UserId', 'User'));

    xflag.push(mandatoryValidate('#DepartmentCode option:selected', 'Department '));

    xflag.push(mandatoryValidate('#Task', 'Task'));

    xflag.push(mandatoryValidate('#Description', 'Description'));

    xflag.push(mandatoryValidate('#StartDate', 'Start Date'));
    //  xflag.push(mandatoryValidate('#ExpectedCompletionDate', 'Expected Completion Date'));

    xflag.push(mandatoryValidate('#DaysTaken', 'Days Taken'));
    xflag.push(floatValidation('#DaysTaken', 'Days Taken'));
    xflag.push(floatValidation('#DaysAgreed', 'Days Agreed'));
    xflag.push(mandatoryValidate('#StatusCode', 'Status Code'));

    if ($('#StatusCode option:selected').val() == 'ING') {
        xflag.push(mandatoryValidate('#ExpectedCompletionDate', 'Expected Completion Date'));
    }

    if ($('#StatusCode option:selected').val() == 'COM') {
        xflag.push(mandatoryValidate('#ActualCompletionDate', 'Actual Completion Date'));
    }
    for (var i = 0; i < xflag.length; i++) {
        if (xflag[i] == true) {
            isvalid = false;
        }
    }
    if (isvalid != false) {
       
        if (Guid != null && Guid != '' && Guid != undefined) {

            EditTicket_A();

        } else {

            AddTicket_A();

        }

    }
    else {
        setTimeout(function () {
            $('#btnSave_A').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#btnSave_A').attr('disabled', false);
        }, 500);
    }
}

function GetData() {
    return $.ajax({
        url: '/TicketingSystem/GetData',
        dataType: 'json',
        data: { Guid: $('#Guid').val() },
        type: 'Get',
        cache: false,
        async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        //  Get(result.data, 'F_' + $('#PageGuid').val());
        PopulateData(result.data);
        return result;
    });

    //var data = AjaxCall("TicketingSystem", "GetData", "Post", GetFormData());
    //Get(data,'F_PageGuid');
}

function GetDepartment() {
    
    return $.ajax({
        url: '/TicketingSystem/GetDepartmentCode',
        dataType: 'json',
        data: { UserId: $('#UserId option:selected').val() },
        type: 'Get',
        cache: false,
        async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        
        $('#DepartmentCode').val(result.data);
        //  Get(result.data, 'F_' + $('#PageGuid').val());
        //  PopulateData(result.data);
        return result;
    });

    //var data = AjaxCall("TicketingSystem", "GetData", "Post", GetFormData());
    //Get(data,'F_PageGuid');
}

function GetData_A() {
    return $.ajax({
        url: '/TicketingSystem/GetData',
        dataType: 'json',
        data: { Guid: $('#Guid').val() },
        type: 'Get',
        cache: false,
        async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        //  Get(result.data, 'F_' + $('#PageGuid').val());
        PopulateData(result.data);
        return result;
    });

    //var data = AjaxCall("TicketingSystem", "GetData", "Post", GetFormData());
    //Get(data,'F_PageGuid');
}
    
function PopulateData(result) {
    
    IsNullDate(result.documentDate, 'DocumentDate');
    $('#Task').val(result.project);
    $('#UserId').val(result.userId);
    $('#DepartmentCode').val(result.departmentCode);
    $('#Description').val(result.description);
    StatusCode_ = result.statusCode;
    $('#ComplaintCategoryCode').val(result.complaintCategoryCode);
    IsNullDate(result.expectedCompletionDate, 'ExpectedCompletionDate');
    IsNullDate(result.actualCompletionDate, 'ActualCompletionDate');
    IsNullDate(result.startDate, 'StartDate');
    // $('#ExpectedCompletionDate').val(IsNullDate(result.expectedCompletionDate));
    // $('#ActualCompletionDate').val(moment(result.actualCompletionDate).format("DD/MM/YYYY"));
    $('#DaysTaken').val(result.daysTaken);
    $('#DaysAgreed').val(result.daysAgreed);
    $('#StatusCode').val(result.statusCode);
    $('#AssignTo').val(result.assignTo);
    $('#Remarks').val(result.remarks);
    var html = '';
    
    if (result.listAttachments.length > 0 && result.listAttachments != null) {

        html += '<div id="" class="kt-section__content kt-section__content--border kt-section__content--fit">';
        html += ' <ul id="" class="kt-nav kt-nav--bold kt-nav--md-space kt-nav--v3" role="tablist">';
        for (var i = 0; i < result.listAttachments.length; i++) {
            html += '  <li class="kt-nav__item">';
            html += '  <a class="kt-nav__link" href="#" role="tab" data-toggle="kt-tooltip" title="" data-placement="right" data-original-title="This feature is coming soon!">';
            html += '     <span class="kt-nav__link-icon">';
            html += ' <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">';
            html += '  <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">';
            html += '  <path d="M5.85714286,2 L13.7364114,2 C14.0910962,2 14.4343066,2.12568431 14.7051108,2.35473959 L19.4686994,6.3839416 C19.8056532,6.66894833 20,7.08787823 20,7.52920201 L20,20.0833333 C20,21.8738751 19.9795521,22 18.1428571,22 L5.85714286,22 C4.02044787,22 4,21.8738751 4,20.0833333 L4,3.91666667 C4,2.12612489 4.02044787,2 5.85714286,2 Z" fill="#000000" fill-rule="nonzero" opacity="0.3" />';
            html += '  <rect fill="#000000" opacity="0.3" transform="translate(8.984240, 12.127098) rotate(-45.000000) translate(-8.984240, -12.127098) " x="7.41281179" y="10.5556689" width="3.14285714" height="3.14285714" rx="0.75" />';
            html += '  <rect fill="#000000" opacity="0.3" transform="translate(15.269955, 12.127098) rotate(-45.000000) translate(-15.269955, -12.127098) " x="13.6985261" y="10.5556689" width="3.14285714" height="3.14285714" rx="0.75" />';
            html += '  <rect fill="#000000" transform="translate(12.127098, 15.269955) rotate(-45.000000) translate(-12.127098, -15.269955) " x="10.5556689" y="13.6985261" width="3.14285714" height="3.14285714" rx="0.75" />';
            html += '  <rect fill="#000000" transform="translate(12.127098, 8.984240) rotate(-45.000000) translate(-12.127098, -8.984240) " x="10.5556689" y="7.41281179" width="3.14285714" height="3.14285714" rx="0.75" />';
            html += '  </g>';
            html += ' </svg>';
            html += '    </span>';
            html += '       <span  Link="/' + result.listAttachments[i].path + '"  class="SpanURL kt-nav__link-text">' + result.listAttachments[i].fileName + '</span>';
            //html += '      <iframe id="my_iframe" style="display:none;"></iframe>';
            html += '      </a>  </li>';
        }
        html += '</ul>';
        html += '</div>';

    }
    else {
        html += 'None';
    }
    $('#AttachmentDiv').html(html);

}

function generateIcon(ext) {
    var html = '';
    if (ext == 'xlx' || ext == 'xlsx') {

        html += ' <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">';
        html += '  <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">';
        html += '  <path d="M5.85714286,2 L13.7364114,2 C14.0910962,2 14.4343066,2.12568431 14.7051108,2.35473959 L19.4686994,6.3839416 C19.8056532,6.66894833 20,7.08787823 20,7.52920201 L20,20.0833333 C20,21.8738751 19.9795521,22 18.1428571,22 L5.85714286,22 C4.02044787,22 4,21.8738751 4,20.0833333 L4,3.91666667 C4,2.12612489 4.02044787,2 5.85714286,2 Z" fill="#000000" fill-rule="nonzero" opacity="0.3" />';
        html += '  <rect fill="#000000" opacity="0.3" transform="translate(8.984240, 12.127098) rotate(-45.000000) translate(-8.984240, -12.127098) " x="7.41281179" y="10.5556689" width="3.14285714" height="3.14285714" rx="0.75" />';
        html += '  <rect fill="#000000" opacity="0.3" transform="translate(15.269955, 12.127098) rotate(-45.000000) translate(-15.269955, -12.127098) " x="13.6985261" y="10.5556689" width="3.14285714" height="3.14285714" rx="0.75" />';
        html += '  <rect fill="#000000" transform="translate(12.127098, 15.269955) rotate(-45.000000) translate(-12.127098, -15.269955) " x="10.5556689" y="13.6985261" width="3.14285714" height="3.14285714" rx="0.75" />';
        html += '  <rect fill="#000000" transform="translate(12.127098, 8.984240) rotate(-45.000000) translate(-12.127098, -8.984240) " x="10.5556689" y="7.41281179" width="3.14285714" height="3.14285714" rx="0.75" />';
        html += '  </g>';
        html += ' </svg>';
    }

    return html;
}

var attch = [];
function GetFormData_A() {

    var formData = {
        DocumentDate: moment($('#DocumentDate').val(), "DD/MM/YYYY").format('YYYY-MM-DD HH:mm:ss'),
        Guid: $('#Guid').val(),
        Project: $('#Task').val(),
        // DepartmentCode: $('#DepartmentCode option:selected').val(),
        ComplaintCategoryCode: $('#ComplaintCategoryCode option:selected').val(),
        Description: $('#Description').val(),
        StatusCode: $('#StatusCode option:selected').val(),
        StartDate: moment($('#StartDate').val(), "DD/MM/YYYY").format('YYYY-MM-DD HH:mm:ss'),
        ExpectedCompletionDate: moment($('#ExpectedCompletionDate').val(), "DD/MM/YYYY").format('YYYY-MM-DD HH:mm:ss'),
        ActualCompletionDate: moment($('#ActualCompletionDate').val(), "DD/MM/YYYY").format('YYYY-MM-DD HH:mm:ss'),
        DaysTaken: $('#DaysTaken').val(),
        DaysAgreed: $('#DaysAgreed').val(),
        SendEmail: $('#SendEmail').prop('checked'),
        SendEmailtoAssignee: $('#SendEmailtoAssignee').prop('checked'),
        UserId: $('#UserId option:selected').val(),
        TicketCode: $('#Guid').val(),
        AssignTo: $('#AssignTo option:selected').val(),
        Remarks: $('#Remarks').val(),
        ListAttachments: []
    };

    //var file = document.getElementById('UploadPurchaseRequest');
    //var dsa = attch.length;

    //for (var i = 0; i < file.files.length; i++) {

    //    var Attachments = {
    //        FileBase64: attch[i],
    //        FileName: file.files[i].name
    //    };
    //    formData.ListAttachments.push(Attachments);
    //}




    return formData;
}
