function GetListValueType() {

    return $.ajax({
        url: '/LabTemplate/GetListValueType',
        dataType: 'json',
        type: 'Get',
        async: false,
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        result = result.data;
        console.log(result)

        var html = '';
        for (var i = 0; i < result.length; i++) {
            $('#ValueTypeCode').append('<option value="' + result[i].valueTypeCode + '">' + result[i].valueTypeDescription + ' </option>');
            $('#LengthValueType').append('<option value="' + result[i].valueTypeCode + '">' + result[i].valueTypeDescription + ' </option>');
            $('#WidthValueType').append('<option value="' + result[i].valueTypeCode + '">' + result[i].valueTypeDescription + ' </option>');
            html += '<option value="' + result[i].valueTypeCode + '">' + result[i].valueTypeDescription + ' </option>';
            ListValueType = html;
        }


    });
}

function GetListDurationType() {

    return $.ajax({
        url: '/LabTemplate/GetListDurationType',
        dataType: 'json',
        type: 'Get',
        async: false,
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        result = result.data;
        console.log(result)
        var html = '';
        for (var i = 0; i < result.length; i++) {
            $('#DurationTypeCode').append('<option value="' + result[i].durationTypeCode + '">' + result[i].durationTypeDescription + ' </option>');
            html += '<option value="' + result[i].durationTypeCode + '">' + result[i].durationTypeDescription + ' </option>';
            ListDurationType = html;
        }


    });
}

function GetListValueMethod() {

    return $.ajax({
        url: '/LabTemplate/GetListValueMethod',
        dataType: 'json',
        type: 'Get',
        async: false,
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        result = result.data;
        console.log(result)
        var html = '';
        for (var i = 0; i < result.length; i++) {
            $('#ValueMethodCode').append('<option value="' + result[i].methodCode + '">' + result[i].methodDescription + ' </option>');
            html += '<option value="' + result[i].methodCode + '">' + result[i].methodDescription + ' </option>';
            ListValueMethod = html;
        }


    });
}

function GetLisParameters() {

    return $.ajax({
        url: '/LabTemplate/GetListParameters',
        dataType: 'json',
        type: 'Get',
        async: false,
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        result = result.data;
        console.log(result)
        var html = '';
        for (var i = 0; i < result.length; i++) {
            $('#ParamCode').append('<option value="' + result[i].paramCode + '">' + result[i].paramName + ' </option>');
            html += '<option value="' + result[i].paramCode + '">' + result[i].paramName + ' </option>';
            ListParameter = html;
        }


    });
}

function GetLisRmCodes() {

    return $.ajax({
        url: '/LabTemplate/GetListRmCodes',
        dataType: 'json',
        type: 'Get',
        async: false,
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        result = result.data;
        console.log(result)
        var html = '';
        for (var i = 0; i < result.length; i++) {
            $('#RmCode').append('<option value="' + result[i].itemCode + '">' + result[i].itemName + ' </option>');
            html += '<option value="' + result[i].itemCode + '">' + result[i].itemName + ' </option>';
            ListItemCodes = html;
        }


    });
}

function ValidateData(Id) {

    if ($("#" + Id).val() == null || $("#" + Id).val() == '' || $("#" + Id).val() == undefined) {
        $("#" + Id).addClass('is-invalid');
        return false;
    }
    else {
        $("#" + Id).removeClass('is-invalid');
        return true;
    }

}

$(document).on('keypress', '#QcMethod,#MinValue,#MaxValue,#Desc1,#Desc2,#Duration,#IncrementalDuation,#Counter', function (e) {

    $(this).removeClass('is-invalid');
});

function ValidateListData(obj) {
    debugger
    var val = obj.val();
    if (val == null || val == '' || val == undefined) {
        $(obj).addClass('is-invalid');
        return false;
    }
    else {

        $(obj).removeClass('is-invalid');
        return true;
    }

}




function ValidateForm() {
    debugger
    var isValid = true;
    var array = [];

    array.push(ValidateData('DocDate'));
    array.push(ValidateData('DocNum'));
    array.push(ValidateData('EffectiveDate'));
    array.push(ValidateData('ItemCode option:selected'));

    $('#ListParameters .itm').each(function (index, item) {
        var dt1 = $(this).closest('#ListParameters .itm').find("#ParamCode");
        var dt2 = $(this).closest('#ListParameters .itm').find("#QcMethod");
        var dt3 = $(this).closest('#ListParameters .itm').find("#ValueTypeCode");
        var dt4 = $(this).closest('#ListParameters .itm').find("#StandardValue");
        var dt5 = $(this).closest('#ListParameters .itm').find("#MinValue");
        var MaxValue = $(this).closest('#ListParameters .itm').find("#MaxValue");
        var dt7 = $(this).closest('#ListParameters .itm').find("#ValueMethodCode");
        var dt8 = $(this).closest('#ListParameters .itm').find("#Desc1");
        var dt9 = $(this).closest('#ListParameters .itm').find("#Desc2");
        var dt10 = $(this).closest('#ListParameters .itm').find("#DurationTypeCode");
        var dt11 = $(this).closest('#ListParameters .itm').find("#Duration");
        var dt12 = $(this).closest('#ListParameters .itm').find("#Counter");
        //  var dt13 = $(this).closest('#ListParameters .itm').find("#IncrementalDuation");

        array.push(ValidateListData(dt1));
        array.push(ValidateListData(dt2));
        array.push(ValidateListData(dt3));

        if (dt3.val() != '' && dt3.val() != null && dt3.val() != undefined) {
            if (dt3.val() == 'PM') {
                array.push(ValidateListData(dt4));
                array.push(ValidateListData(dt5));
                array.push(ValidateListData(MaxValue));
            }
            else if (dt3.val() == 'RN') {
                array.push(ValidateListData(dt5));
                array.push(ValidateListData(MaxValue));
            } else if (dt3.val() == 'DS' || dt3.val() == 'GT' || dt3.val() == 'LT') {
                array.push(ValidateListData(dt4));
            }

        } else {
            array.push(ValidateListData(dt4));
            array.push(ValidateListData(dt5));
            array.push(ValidateListData(MaxValue));
        }



        isValid = ValidateListData(dt7);
        if (dt7.val() != '' && dt7.val() != null && dt7.val() != undefined) {
            if (dt7.val() != 'SN') {
                array.push(ValidateListData(dt8));
                array.push(ValidateListData(dt9));
            }

        }
        else {
            array.push(ValidateListData(dt8));
            array.push(ValidateListData(dt9));
        }

        //isValid = ValidateListData(dt8);
        //isValid = ValidateListData(dt9);
        array.push(ValidateListData(dt10));
        if (dt10.val() != '' && dt10.val() != null && dt10.val() != undefined) {
            if (dt10.val() != 'ONT') {
                array.push(ValidateListData(dt11));
                array.push(ValidateListData(dt12));
                //  isValid = ValidateListData(dt13);
            }

        }
        else {
            array.push(ValidateListData(dt11));
            array.push(ValidateListData(dt12));
            // isValid = ValidateListData(dt13);
        }

    });

    $('#ListParameters2 .itm').each(function (index, item) {
        var RmCode = $(this).closest('#ListParameters .itm').find("#RmCode");
        var LengthValueType = $(this).closest('#ListParameters .itm').find("#LengthValueType");
        var StandardLength = $(this).closest('#ListParameters .itm').find("#StandardLength");
        var MinLength = $(this).closest('#ListParameters .itm').find("#MinLength");
        var MaxLength = $(this).closest('#ListParameters .itm').find("#MaxLength");
        var WidthValueType = $(this).closest('#ListParameters .itm').find("#WidthValueType");
        var StandardWidth = $(this).closest('#ListParameters .itm').find("#StandardWidth");
        var MinWidth = $(this).closest('#ListParameters .itm').find("#MinWidth");
        var MaxWidth = $(this).closest('#ListParameters .itm').find("#MaxWidth");

    });

    for (var i = 0; i < array.length; i++) {
        if (array[i] == false) {
            isValid = false;
        }
    }



    if (isValid != false) {
        if (Guid != undefined && Guid != '' && Guid != null && isDuplicate == false) {
            Edit();
        }
        else {
            Add();
        }

    }
    else {

        setTimeout(
            function () {

                $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                $('#btnSave').attr('disabled', false);

            }, 500);
    }
}

function Add() {

    return $.ajax({
        url: '/LabTemplate/Add',
        dataType: 'json',
        type: 'POST',
        data: FormData(),
        success: function (result) {
            console.log(result);
            if (result.isInserted) {
                toastr.success(result.message);

                setTimeout(
                    function () {

                        $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                        $('#btnSave').attr('disabled', false);

                    }, 800);
                setTimeout(function () {
                    window.history.back();
                }, 800);


            }

            if (result.isError) {
                toastr.error(result.message);
                $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                $('#btnSave').attr('disabled', false);

            }

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {


    });
}

function Edit() {

    return $.ajax({
        url: '/LabTemplate/Edit',
        dataType: 'json',
        type: 'POST',
        data: FormData(),
        success: function (result) {
            console.log(result);
            if (result.isInserted) {
                toastr.success(result.message);

                setTimeout(
                    function () {

                        $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                        $('#btnSave').attr('disabled', false);

                    }, 800);
                setTimeout(function () {
                    window.history.back();
                }, 800);


            }

            if (result.isError) {
                toastr.error(result.message);
                $('#btnSave').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                $('#btnSave').attr('disabled', false);

            }



        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {


    });
}

function PopulateData() {

    return $.ajax({
        url: '/LabTemplate/GetData',
        dataType: 'json',
        type: 'Get',
        data: { Guid: $("#Guid").val() },
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {
        debugger
        result = result.data;
        console.log(result)

        $('#DocDate').val(moment(result.docDate).format("YYYY-MM-DD"));
        $('#EffectiveDate').val(moment(result.effectiveDate).format("YYYY-MM-DD"));
        //$('#ItemCode').val(result.itemCode);
        $('#DocNum').val(result.docNum);

        var SelectData = [];
        SelectData.push(result.itemCode);
        $('#ItemCode').val(SelectData);
        $('#ItemCode').change();

        for (var i = 1; i < result.listLabTemplateDetails.length; i++) {
            $('#ListBtnAddd').click();
        }




        $('#ListParameters .itm').each(function (index, item) {
            $(this).closest('#ListParameters .itm').find("#ParamCode").val(result.listLabTemplateDetails[index].paramCode);
            $(this).closest('#ListParameters .itm').find("#QcMethod").val(result.listLabTemplateDetails[index].qcMethod);
            $(this).closest('#ListParameters .itm').find("#ValueTypeCode").val(result.listLabTemplateDetails[index].valueTypeCode).trigger('change');
            $(this).closest('#ListParameters .itm').find('#ValueTypeCode').trigger('change');
            $(this).closest('#ListParameters .itm').find("#StandardValue").val(result.listLabTemplateDetails[index].standardValue);
            $(this).closest('#ListParameters .itm').find("#MinValue").val(result.listLabTemplateDetails[index].minValue);
            $(this).closest('#ListParameters .itm').find("#MaxValue").val(result.listLabTemplateDetails[index].maxValue);
            $(this).closest('#ListParameters .itm').find("#ValueMethodCode").val(result.listLabTemplateDetails[index].valueMethodCode).trigger('change');
            $(this).closest('#ListParameters .itm').find("#Desc1").val(result.listLabTemplateDetails[index].desc1);
            $(this).closest('#ListParameters .itm').find("#Desc2").val(result.listLabTemplateDetails[index].desc2);
            $(this).closest('#ListParameters .itm').find("#DurationTypeCode").val(result.listLabTemplateDetails[index].durationTypeCode).trigger('change');
            $(this).closest('#ListParameters .itm').find("#Duration").val(result.listLabTemplateDetails[index].duration);
            $(this).closest('#ListParameters .itm').find("#Counter").val(result.listLabTemplateDetails[index].paramCount);
        });

        for (var i = 1; i < result.listLabTemplateRmDetails.length; i++) {
            $('#ListBtnAddRM').click();
        }

        $('#ListParameters2 .itm').each(function (index, item) {
            $(this).closest('#ListParameters2 .itm').find("#RmCode").val(result.listLabTemplateRmDetails[index].itemCode);
            $(this).closest('#ListParameters2 .itm').find("#LengthValueType").val(result.listLabTemplateRmDetails[index].lengthValueType).trigger('change');;
            $(this).closest('#ListParameters2 .itm').find("#StandardLength").val(result.listLabTemplateRmDetails[index].standardLength)
            $(this).closest('#ListParameters2 .itm').find("#MinLength").val(result.listLabTemplateRmDetails[index].minLength)
            $(this).closest('#ListParameters2 .itm').find("#MaxLength").val(result.listLabTemplateRmDetails[index].maxLength);
            $(this).closest('#ListParameters2 .itm').find("#WidthValueType").val(result.listLabTemplateRmDetails[index].widthValueType).trigger('change');
            $(this).closest('#ListParameters2 .itm').find("#StandardWidth").val(result.listLabTemplateRmDetails[index].standardWidth);
            $(this).closest('#ListParameters2 .itm').find("#MinWidth").val(result.listLabTemplateRmDetails[index].minWidth)
            $(this).closest('#ListParameters2 .itm').find("#MaxWidth").val(result.listLabTemplateRmDetails[index].maxWidth);
        });

    });
}

function DocNumOnLoad() {

    return $.ajax({
        url: '/LabTemplate/GetUpdateDocumentNumberOnLoad',
        dataType: 'json',
        type: 'Get',
        success: function (result) {


        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }

    }).done(function (result) {

        result = result.data;
        console.log(result)
        $('#DocNum').val(result);


    });
}