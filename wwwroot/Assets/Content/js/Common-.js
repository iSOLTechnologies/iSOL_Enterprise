function loadPartialView(Controller, ViewName, viewArray, requestMethod, ActionURL, params) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/' + Controller + '/' + ViewName,
            type: requestMethod,
            success: function (result) {
                $('#detailPartialView').html(result)
                for (var i = 0; i < viewArray.length; i++) {
                    if (viewArray[i].viewName === ViewName) {
                        $('#' + ViewName).addClass('kt-widget__item--active');
                    } else {
                        $('#' + viewArray[i].viewName).removeClass('kt-widget__item--active');
                    }
                }
            },
            error: function (error) {
                reject(error)
            },
        }).done(function () {
            var data = AjaxCall(Controller, ActionURL, requestMethod, params)
            data = $.parseJSON(data);
            resolve(data);
            debugger
        });
    })

}

function GenNumRange(Dest) {
    debugger
    return $.ajax({
        url: "/common/norange",
        dataType: 'json',
        type: "GET",
        cache: false,
        async: false,
        data: { pageid: Dest },
        success: function (resp) {
        }
    }).done(function (resp) {
        debugger
        return resp.NewNumber;
    });
}

function NumRange(Dest) {
    debugger
    var res = GenNumRange(Dest).done();
    if (res.responseJSON.IsInvalidKey || res.responseJSON.IsTokenExpired) {
        toastr.error(res.responseJSON.msg);
        //setInterval(window.location.href = "/Login/Signout", 5000);
        setTimeout(function () {
            window.location.href = "/Login/Signout"
        }, 2000);
    }
    if (res.responseJSON.isError) {
        toastr.error(res.responseJSON.msg);
    }
    return res.responseJSON.NewNumber;
}

function shortDesc(string, stringLength) {
    length_temp = stringLength;
    var temp_ret = string.length > length_temp ? (string.substring(0, length_temp) + "...") : (string);
    return temp_ret;
}


function CheckExistance(input, value, tblName) {


    $.ajax({
        url: "/common/CheckExistance",
        type: "GET",
        dataType: 'json',
        data: {
            input: input,
            value: value,
            tblName: tblName
        },
        success: function (result) {

            if ($('#' + input).val().length > 0) {

                if (result) {
                    $('#' + input).addClass('is-invalid');
                }
                else {
                    $('#' + input).addClass('is-valid');
                }
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}


function Delete(tblName, Key, Value, Dest) {

    $.ajax({
        url: "/common/Delete",
        type: "GET",
        dataType: 'json',
        data: {
            Key: Key,
            Value: Value,
            tblName: tblName,
            Dest: Dest
        },
        success: function (result) {
            if (result.IsInvalidKey || result.IsTokenExpired) {
                toastr.error(result.msg);
                setTimeout(function () {
                    window.location.href = "/Login/Signout"
                }, 2000);
            }
            if (result.isUnAuthorized) {
                toastr.error(result.msg);
            }
            if (result.isError) {
                toastr.error(result.msg);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}

function GetBase64(array, ElementId) {

    var file = document.getElementById(ElementId);

    for (var i = 0; i < file.files.length; i++) {

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
    debugger
    var res = AjaxCallReturn(controller, action, type, param).done(function (data) {

        return data;
    });
    debugger
    if (res.responseJSON.IsInvalidKey || res.responseJSON.IsTokenExpired) {
        toastr.error(res.responseJSON.msg);
        setTimeout(function () {
            window.location.href = "/Login/Signout"
        }, 2000);
    }
    if (res.responseJSON.isUnAuthorized) {
        toastr.error(res.responseJSON.msg);
    }
    if (res.responseJSON.isError) {
        toastr.error(res.responseJSON.msg);
    }
    return res.responseJSON;
}


function FormSetter(result, list) {

    //////


    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {

        }
        result.data[list[i]]
    }
}

function GetViewData(data, divElement) {

    if (data != null || data != undefined) {
        var div = document.getElementsByClassName(divElement);

        $(div).find('p,span,div,img').each(function (index, item) {//input,select,img,.kt-avatar__holder,span,

            var elemen = $(this)[0].localName;
            var Type = $(this).attr('type');
            var id = $(this).attr('id');
            if ($(this)[0].localName == 'div') {
                var name = $(this).attr('id');
                var sadas = data[name];
                $(this).html(data[name]);
            }

            if ($(this)[0].localName == 'span') {
                var name = $(this).attr('id');
                var sadas = data[name];
                $(this).html(data[name]);
            }

            if ($(this)[0].localName == 'p') {
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

function Get(data, divElement) {

    if (data != null || data != undefined) {
        var div = document.getElementById(divElement);

        $(div).find('input, select, img, .kt-avatar__holder').each(function (index, item) {//input,select,img,.kt-avatar__holder,span,

            var elemen = $(this)[0].localName;
            var Type = $(this).attr('type');
            var id = $(this).attr('id');

            if (Type == 'text') {
                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).val(data[name]);
            }
            if (Type == 'hidden') {
                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).val(data[name]);
            }

            if (Type == 'password') {
                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).val(data[name]);

            }
            if (Type == 'checkbox') {

                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).prop('checked', data[name]);

            }
            if ($(this)[0].localName == 'div') {

                var id = $(this).attr('id');
                var sadas = data[id];
                $(this).attr('style', 'background-image: url("../' + data[id] + '")');

            }
            if ($(this)[0].localName == 'select') {

                var id;
                var name = $(this).attr('name');
                var sadas = data[name];
                if (name != data[name]) {
                    var id = $(this).attr('id');
                    $(this).val(data[id]);
                } else {
                    $(this).val(data[name]);
                }
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


//function GetMultiples(data) {

//    //if (data != null || data != undefined) {

//    //    for (var i = 0; i < data.length; i++) {
//    //        var count = 0;
//    //        var Array1 = [];
//    //        var json = '';
//    //        json += '{'
//    //        
//    //        for (var key in data[i]) {
//    //            var dasd = key.length;
//    //            //var formdata = {

//    //            if (count == 0) {
//    //                json += [key] + ': ' + '"' + data[i][key] + '"';

//    //            } else {
//    //                json += ', ' + [key] + ': ' + '"' + data[i][key] + '"';
//    //            }

//    //            // }

//    //            //Array1.push(json);
//    //            count++;
//    //        }
//    //        //if (i == data.length - 2) {
//    //        //    json += '},';

//    //        //}
//    //        //else {
//    //        json += "}";
//    //        //}


//    //        Array.push(json);

//    //        Array1 = [];
//    //    }
//    //}
//    return data;
//}

function DisableFields(Element) {

    var div = document.getElementById(Element);
    $(div).find('input,textarea,select,checkbox').each(function () {

        $(this).attr('disabled', true);
    });
}


function EnableFields(Element) {

    var div = document.getElementById(Element);
    $(div).find('input,textarea,select,checkbox').each(function () {

        $(this).removeAttr('disabled');
    });
}

function ResetForm(Element) {

    var div = document.getElementById(Element);
    $(div).find('input,textarea,select,checkbox').each(function () {
        $(this).val('');
    });
}

function UnBindClick(element, Istrue) {
    var self = this;
    if (Istrue == true) {

        $(element).attr('disabled', true);
        $(element).bind('click', function () { return false; });
    }
    if (Istrue == false) {

        $(element).removeAttr('disabled');
        $(element).unbind('click');
    }
}

function GatherDataGeneric() {
    var div = document.getElementById('ProfleMst');
    var txt;

    var json = '';
    json += '{'
    $(div).find('input,select,img').each(function (index, item) {

        var asdas = index;
        var Type = $(this).attr('type');

        if (index == 0) {

            if (Type == 'text') {
                var name = $(this).attr('name');
                var val = $(this).val();
                json += [name] + ': ' + '"' + val + '"';

            } if (Type == 'checkbox') {

                var name = $(this).attr('name');
                var ischeck = $(this).prop('checked');
                json += [name] + ': ' + '"' + ischeck + '"';
            } if (Type == 'file') {

                //var name = $(this).attr('name');
                //var base64 = $(this).attr('style', 'backgroud-image');
                //json += [name] + ': ' + '"' + base64 + '"';
            }
            if ($(this)[0].localName == 'select') {

                var name = $(this).attr('name');
                var select = $(this).val();
                json += [name] + ': ' + '"' + select + '"';
            }

        } else {

            if (Type == 'text') {
                var name = $(this).attr('name');
                var val = $(this).val();
                json += ', ' + [name] + ': ' + '"' + val + '"';

            } if (Type == 'checkbox') {

                var name = $(this).attr('name');
                var ischeck = $(this).prop('checked');
                json += ', ' + [name] + ': ' + '"' + ischeck + '"';
            } if (Type == 'file') {

                //var name = $(this).attr('name');
                //var base64 = $(this).attr('style','backgroud-image');
                //json += ', ' + [name] + ': ' + '"' + base64 + '"';
            }
            if ($(this)[0].localName == 'select') {

                var name = $(this).attr('name');
                var select = $(this).val();
                json += ', ' + [name] + ': ' + '"' + select + '"';
            }
        }
    });
    json += "}";

    return json;
}


function CheckPreActivities(PageId) {
    $.ajax({
        url: "/common/CheckPreActivities",
        type: "GET",
        dataType: 'json',
        data: { pageId: PageId },
        async: false,
        success: function (result) {

            var data = $.parseJSON(result);

            $('#create').hide();
            UnBindClick('#BtnEdit', true);
            UnBindClick('#BtnDelete', true);
            //$('#BtnDelete').click(function () { return false;})
            //$('#BtnDelete').attr('disabled', true);

            UnBindClick('.BtnDetail', true);
            for (var i = 0; i < data.length; i++) {
                if (data[i].activityid == 'C') {
                    $('#create').show();
                }
                if (data[i].activityid == 'D') {
                    UnBindClick('#BtnDelete', false);
                }
                if (data[i].activityid == 'E') {

                    UnBindClick('#BtnEdit', false);
                }
                if (data[i].activityid == 'V') {
                    UnBindClick('.BtnDetail', false);
                }

            }


        }, error: function (jqXhr, textStatus, errorMessage) {

            var data = errorMessage;
        }
    });
}


function CheckPostActivities(PageId, Activity) {
    debugger
    var a = null;
    $.ajax({
        url: "/common/CheckPostActivities",
        type: "GET",
        dataType: 'json',
        data: { pageId: PageId, Activity: Activity },
        async: false,
        success: function (resp) {

            if (resp.isError) {
                toastr.error(resp.msg);
                a = false;

            } else if (resp.isSuccess) {

                a = true;

            } else {
                toastr.error(resp.msg)
                a = false;
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {

        }
    });

    return a;
}

function CheckPostActivitiesView(PageId, Activity) {
    debugger
    if (!CheckPostActivities(PageId, Activity)) {
        return;
    }
}