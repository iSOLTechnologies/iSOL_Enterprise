var StatusCode_;
var Guid;
var attch = [];
const Tus = Uppy.Tus;
const ProgressBar = Uppy.ProgressBar;
const StatusBar = Uppy.StatusBar;
const FileInput = Uppy.FileInput;
const Informer = Uppy.Informer;

$(document).ready(function () {
   // initUppy5();
    this.table = null;
    var self = this;
    var RowDataPrint = null;
    var rowdata;
    $('#DocumentDate').attr('disabled', true);
    $('#UserId').attr('disabled', true);
    $('#UserId').val($('#_UserId').val());
    $('#DepartmentCode').attr('disabled', true);
    $('#DepartmentCode').val($('#_DepartmentCode').val());
    // GetAll();
    var PageGuid = $('#PageGuid').val();
    Guid = $('#Guid').val();



    $(document).on('click', '#btnSave', function () {
        $(this).addClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        $(this).attr('disabled', true);
        CheckFeedbacks_AddTickets();
       
    })

    $(document).on('click', '#btnSaveBack', function () {

        $(this).addClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        $(this).attr('disabled', true);
        CheckFeedbacks_AddTickets();
        
    })




    $(document).on('click', '#btnCancel', function () {
        ClearForm();
        //window.location.href = '/TicketingSystem/Index?Source=9245fe4ad402451cb9ed9c1a042474821101202105224455';
    })

    $(document).on('click', '#btnBack', function () {
        window.history.back();
      ///  window.location.href = '/TicketingSystem/Index?Source=9245fe4ad402451cb9ed9c1a042474821101202105224455';
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


    $(document).on('click', '.SpanURL', function () {

        var val = $(this).attr('Link');
        var win = window.open(val, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            alert('Please allow popups for this website');
        }
    });


    $("#DocumentDate").val(moment().format("DD/MM/YYYY").toString());

    var username = $('#Name').val();

    if (Guid != null && Guid != '' && Guid != undefined) {

        $('#AttachDiv').removeAttr('hidden');
        $('#SpanAttched').removeAttr('hidden');
        $('#heading').text('Edit Ticket');
        $('#SpanBtnSave').text('Save & Back');
        $('#btnCancel').text('Cancel & Back');
        $('#breadcrumPageName').text('Edit Ticket');
        //$('#btnSave').attr('disabled', true);
        //$('#btnCancel').attr('disabled', true);
        GetData();
    }

    $(document).on('change', '#kt_uppy_5', function () {

        GetBase64(attch, 'kt_uppy_5');
      

    });
});

function CheckFeedbacks_AddTickets() {

    return $.ajax({
        url: '/TicketingSystem/GetUnFeedbackBackTickets',
        dataType: 'json',
        data: { UserId: $('#_UserId').val() },
        type: 'Get',
        //cache: false,
        //async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        debugger
        if (result.data.length > 0) {

            toastr.warning('Kindly provide Feedback on your previous Tickets');
            generateFeedbackFields(result.data);
            loadJavascript();
            $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');

            $('#btnSave').attr('disabled', false);
        }
        else {
            Validation();
        }
    });

}

function AddTicket() {
    return $.ajax({
        url: '/TicketingSystem/Add',
        dataType: 'json',
        data: GetFormData(),
        type: 'POST',
        //cache: false,
        //async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        debugger
        if (result.isInserted) {

            toastr.success(result.message);
            ClearForm();
            $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#btnSave').attr('disabled', false);

           
        }
        if (result.isError) {
            toastr.error(result.message);
            $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#btnSave').attr('disabled', false);

        }
       
    });
    //var data = AjaxCall("TicketingSystem", "Add", "Post", GetFormData());
    //if (data.isInserted) {
    //    debugger

    //    ClearForm();
    //}

}

function EditTicket() {
  
    return $.ajax({
        url: '/TicketingSystem/Edit',
        dataType: 'json',
        data: GetFormData(),
        type: 'POST',
        //cache: false,
        //async: false,
        success: function (result) {

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        debugger
        if (result.isInserted) {

            toastr.success(result.message);
            ClearForm();
            setTimeout(function () {
                $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            }, 500);
            $('#btnSave').attr('disabled', false);
            setTimeout(function () {
                window.history.back();
            }, 800);
           

        }
        if (result.isError) {
            toastr.error(result.message);
            $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
            $('#btnSave').attr('disabled', false);

        }

    });
    //var data = AjaxCall("TicketingSystem", "Edit", "Post", GetFormData());
    //if (data.isInserted) {
    //    debugger
    //    ClearForm();
    //    window.location.href = '/TicketingSystem/Index?Source=' + $('#PageGuid').val();
    //}

}

function ClearForm() {
    ResetFields_O('F_' + $('#PageGuid').val());
    $("#DocumentDate").val(moment().format("DD/MM/YYYY").toString());
    $("#UserId").val($('#_UserId').val());
    $("#DepartmentCode").val($('#_DepartmentCode').val());
}

function GetFormData() {

    var formData = {
        DocumentDate: moment($('#DocumentDate').val(), "DD/MM/YYYY").format('YYYY-MM-DD HH:mm:ss'),
        TicketCode: $('#TicCode').val(),
        Guid: $('#Guid').val(),
        Project: $('#Task').val(),
        UserId: $('#UserId').val(),
        // DepartmentCode: $('#DepartmentCode option:selected').val(),
        Description: $('#Description').val(),
        StatusCode: StatusCode_,
        ListAttachments: []
    };

    var file = document.getElementById('kt_uppy_5');

    for (var i = 0; i < file.files.length; i++) {

        var Attachments = {
            FileBase64: attch[i],
            FileName: file.files[i].name
        };
        formData.ListAttachments.push(Attachments);
    }

    return formData;
}

function Validation() {
    xflag = [];
    isvalid = null

    toastr.remove();
    xflag.push(mandatoryValidate('#DocumentDate', 'Date'));
    xflag.push(mandatoryValidate('#UserId', 'User'));

    xflag.push(mandatoryValidate('#DepartmentCode option:selected', 'Department '));

    xflag.push(mandatoryValidate('#Task', 'Task'));

    xflag.push(mandatoryValidate('#Description', 'Description'));

    for (var i = 0; i < xflag.length; i++) {
        if (xflag[i] == true) {
            isvalid = false;
        }
    }
    if (isvalid != false) {
        debugger
        if (Guid != null && Guid != '' && Guid != undefined) {
            EditTicket();
        } else {
            AddTicket();

        }


    } else {
        setTimeout(
            function () {

                $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                $('#btnSave').attr('disabled', false);

            }, 800);
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
        debugger
        console.log(result.data);
        //  Get(result.data, 'F_' + $('#PageGuid').val());
        if (result.data != null) {
           
            PopulateData(result.data);
        
        }
       
        return result;
    });

    //var data = AjaxCall("TicketingSystem", "GetData", "Post", GetFormData());
    //Get(data,'F_PageGuid');
}

function PopulateData(result) {
    console.log(result);
    IsNullDate(result.documentDate, 'DocumentDate');
    $('#TicCode').val(result.ticketCode);
    $('#Task').val(result.project);
    $('#UserId').val(result.userId);
    $('#Name').val(result.name);
    $('#Description').val(result.description);
    StatusCode_ = result.statusCode;
    var pageid = 'F_' + $('#PageGuid').val();
    if (StatusCode_ != 'PND') {
        DisableFields(pageid);
        $('#btnSave').attr('disabled', true);
        $('#btnCancel').attr('disabled', true);

    }
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

function initUppy5() {
    // Uppy variables
    // For more info refer: https://uppy.io/
    var elemId = 'kt_uppy_5';
    var id = '#' + elemId;
    var $statusBar = $(id + ' .kt-uppy__status');
    var $uploadedList = $(id + ' .kt-uppy__list');
    var timeout;

    var uppyMin = Uppy.Core({
        debug: true,
        autoProceed: true,
        showProgressDetails: true,
        restrictions: {
            maxFileSize: 1000000, // 1mb
            maxNumberOfFiles: 5,
            minNumberOfFiles: 1
        }
    });

    uppyMin.use(FileInput, { target: id + ' .kt-uppy__wrapper', pretty: false });
    uppyMin.use(Informer, { target: id + ' .kt-uppy__informer' });

    // demo file upload server
    uppyMin.use(Tus, { endpoint: 'https://master.tus.io/files/' });
    uppyMin.use(StatusBar, {
        target: id + ' .kt-uppy__status',
        hideUploadButton: true,
        hideAfterFinish: false
    });

    $(id + ' .uppy-FileInput-input').addClass('kt-uppy__input-control').attr('id', elemId + '_input_control');
    $(id + ' .uppy-FileInput-container').append('<label class="kt-uppy__input-label btn btn-label-brand btn-bold btn-font-sm" for="' + (elemId + '_input_control') + '">Attach files</label>');

    var $fileLabel = $(id + ' .kt-uppy__input-label');

    uppyMin.on('upload', function (data) {
        $fileLabel.text("Uploading...");
        $statusBar.addClass('kt-uppy__status--ongoing');
        $statusBar.removeClass('kt-uppy__status--hidden');
        clearTimeout(timeout);
    });

    uppyMin.on('complete', function (file) {
        $.each(file.successful, function (index, value) {
            var sizeLabel = "bytes";
            var filesize = value.size;
            if (filesize > 1024) {
                filesize = filesize / 1024;
                sizeLabel = "kb";

                if (filesize > 1024) {
                    filesize = filesize / 1024;
                    sizeLabel = "MB";
                }
            }
            var uploadListHtml = '<div class="kt-uppy__list-item" data-id="' + value.id + '"><div class="kt-uppy__list-label">' + value.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="kt-uppy__list-remove" data-id="' + value.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
            $uploadedList.append(uploadListHtml);
        });

        $fileLabel.text("Add more files");

        $statusBar.addClass('kt-uppy__status--hidden');
        $statusBar.removeClass('kt-uppy__status--ongoing');
    
    });

    $(document).on('click', '#kt_uppy_5 .kt-uppy__list .kt-uppy__list-remove', function () {
        debugger
       
        var itemId = $(this).attr('data-id');
        uppyMin.removeFile(itemId);
        console.log(attch);
        $('#kt_uppy_5 .kt-uppy__list-item[data-id="' + itemId + '"').remove();
    });
}



