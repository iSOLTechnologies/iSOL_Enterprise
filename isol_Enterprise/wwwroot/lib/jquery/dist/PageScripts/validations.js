var letters = /^[0-9a-zA-Z \.\r\n]+$/;
var name = '[\w!”$ %&’() * +,/;[\]\\^_`{|}~]*';
var num = /^[0-9 ]+$/;
var float = /^\-?([0-9]+(\.[0-9]+)?|Infinity)$/;

var decimalPositveValues = /^[+]?([0-9]+(?:[\.][0-9]{0,4})?|\.[0-9]{1,2})$/;

function decimal(id, message) {
    
    if ($(id).val() == "" || $(id).val() == null) {
    }
    else {

        if ($(id).val().match(decimalPositveValues)) {
        }
        else {


            toastr.warning('Please type Decimals and Positive Values only in ' + message);
            return true;
        }
    }
    return false;
}

function D_decimal(id, message,regex,regexName) {

    if ($(id).val() == "" || $(id).val() == null) {
    }
    else {

        if ($(id).val().match(regex)) {
        }
        else {


            toastr.warning('Please type ' + regexName + ' only in ' + message);
            return true;
        }
    }
    return false;
}

function mandatoryValidate(id, message) {

    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    return false;
}
function mandatoryAlphaValidateWithDeshAndDot(id, message) {
    debugger
    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    else {
        if ($(id).val().match(name)) {
        }
        else {
            toastr.warning('Please type alphanumeric characters only in ' + message);
            return true;
        }
    }
    return false;
}
function mandatoryWithAlphaValidate(id, message) {
    debugger
    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    else {
        if ($(id).val().match(letters)) {
        }
        else {


            toastr.warning('Please type alphanumeric characters only in ' + message);
            return true;
        }
    }
    return false;
}



function limitValidate(id, length, message) {
    if ($(id).val() == "" || $(id).val() == null) {

    }
    else {

        if ($(id).val().length < length) {
        }
        else {

            toastr.warning('Character Length must be less than' + length + ' in ' + message);
            return true;
        }
    }
    return false;
}

function alphaValidate(id, message) {

    if ($(id).val() == "" || $(id).val() == null) {
    }
    else {

        if ($(id).val().match(letters)) {
        }
        else {


            toastr.warning('Please type alphanumeric characters only in ' + message);
            return true;
        }
    }
    return false;
}

function D_alphaValidate(id, message, regex, regexName) {
    debugger
    if ($(id).val() == "" || $(id).val() == null) {
    }
    else {

        if ($(id).val().match(regex)) {
        }
        else {


            toastr.warning('Please type ' + regexName + ' characters only in ' + message);
            return true;
        }
    }
    return false;
}


function intValidate(id, message) {

    if ($(id).val() == "" || $(id).val() == null) {
    }
    else {

        if ($(id).val().match(num)) {
        }
        else {


            toastr.warning('Please type Numeric characters only in ' + message);
            return true;
        }
    }
    return false;
}



function mandatoryWithIntValidate(id, message) {

    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    else {
        if ($(id).val().match(num)) {
        }
        else {


            toastr.warning('Please type Numbers only in ' + message);
            return true;
        }
    }
    return false;
}

function floatValidation(id, message) {
    if ($(id).val() == "" || $(id).val() == null) {

    }
    else {

        if ($(id).val().match(float)) {
        }
        else {

            toastr.warning('Please type Decimal only in ' + message);
            return true;
        }
    }
    return false;
}

function passwordChecking(password, repassword) {
    if ($(password).val() != "" || $(password).val() != null && $(repassword).val() != "" || $(repassword).val() != null) {
        if ($(password).val().match($(repassword).val())) {
            return false;
        }
        else {
            toastr.warning('Password does not match');
            return true;
        }

    }
}

//function checkTwoTimes(time1, time2) {
//    var t1 = $("#" + time1).val();
//    var t2 = $("#" + time2).val();
//    if (t1 != null && t1 != undefined && t1 != '' && t2 != null && t2 != undefined && t2 != '') {
//        if (Date.parse(t1) && Date.parse(t2)) {
//            if (Date.parse(t1) == Date.parse(t2)) {

//                toastr.warning('Start & End Time must be different');
//                return false;
//            } else if (Date.parse(t1) >= Date.parse(t2)) {
//                toastr.warning('Start Time is must be less than End Time');
//                return false;
//            } else {
//                return true;
//            }
//        } else {
//            toastr.warning("validation name must be in corect format.");
//            return false;
//        }
//}

function checkfunc(date1, date2, validation) {


    var dt1 = $("#" + date1).val();
    var dt2 = $("#" + date2).val();
    if (validation == "validexp") {

        if (dt1 != null && dt1 != undefined && dt1 != '' && dt2 != null && dt2 != undefined && dt2 != '') {
            if (Date.parse(dt1) && Date.parse(dt2)) {
                if (Date.parse(dt1) == Date.parse(dt2)) {

                    toastr.warning('Issued & Expiry Date must be different');
                    return false;
                } else if (Date.parse(dt1) >= Date.parse(dt2)) {
                    toastr.warning('Expiry Date is must be less than Issued Date');
                    return false;
                } else {
                    return true;
                }
            } else {
                toastr.warning("validation name must be in corect format.");
                return false;
            }
        }
    } else if (validation == 'age') {
        if (dt1 != null && dt1 != undefined && dt1 != '') {
            if (Date.parse(dt1)) {
                var cdate = new Date().getFullYear();
                var dbyear = new Date(dt1);
                var age = cdate - dbyear.getFullYear();
                if (age < 18) {
                    toastr.warning("Age Must be Above '18'");
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        } else {
            return false;
        }

    } else {
        return false;
    }
    return true;
}

function getnorange(pageid, type, group) {

    $.ajax({
        url: "/common/norange",
        dataType: 'json',
        type: "GET",
        data: { pageid: pageid, type: type, group: group },
        success: function (result) {

            sessionStorage.setItem('norange', result.data);
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}

function NoRange() {
    return getnorange(norange);
}

function passwordChecking(password, repassword) {
    if ($(password).val() != "" || $(password).val() != null && $(repassword).val() != "" || $(repassword).val() != null) {
        if ($(password).val().match($(repassword).val())) {
            return false;
        }
        else {
            toastr.warning('Password does not match');
            return true;
        }

    }
}