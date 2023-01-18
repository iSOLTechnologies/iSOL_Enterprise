var letters = /^[0-9a-zA-Z ]+$/;
var email = /^[0-9a-zA-Z @.]+$/;
var num = /^[0-9]+$/;
var float = /^\-?([0-9]+(\.[0-9]+)?|Infinity)$/;

function mandatoryValidate(id, message) {

    
    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    return false;
}

function mandatoryvaldite2(id, message) {


    if ($(id).val() == "" || $(id).val() == null) {

        $(id).addClass('is-invalid');
        toastr.warning(message + " is mandatory");
        return true;
    }
    $(id).removeClass('is-invalid');
    $(id).addClass('is-valid');
    return false;
}

function mandatoryWithAlphaValidate (id, message) {
    
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

function validEmail(id, message) {

    var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    else {
        if ($(id).val().match(re)) {
        }
        else {


            toastr.warning('Please type valid email');
            return true;
        }
    }
    return false;
}

function emailValidate(id, message) {
    
    if ($(id).val() == "" || $(id).val() == null) {

        toastr.warning(message + " is mandatory");
        return true;
    }
    else {
        if ($(id).val().match(email)) {
        }
        else {


            toastr.warning('Please type alphanumeric characters only in ' + message);
            return true;
        }
    }
    return false;
}

function limitvalidate(id, length, message) {
    if ($(id).val().length > length) {


        toastr.warning(message + " must be in " + length + " digits");
        return true;
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

function mandatoryWithIntavlidate(id, message) {
    
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
        toastr.warning(message + " is mandatory");
        return true;
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


function checkfunc(date1, date2, validation) {
   

    var dt1 = $("#" + date1).val();
    var dt2 = $("#" + date2).val();
    if (validation == "validexp") {

        if (dt1 != null && dt1 != undefined && dt1 != '' && dt2 != null && dt2 != undefined && dt2 != '') {
            if (Date.parse(dt1) && Date.parse(dt2)) {
                if (Date.parse(dt1) == Date.parse(dt2)) {

                    toastr.warning('Issued & Expiry Date must be different');
                    return true;
                } else if (Date.parse(dt1) > Date.parse(dt2)) {
                    toastr.warning('Expiry Date is must be greater than Issued Date');
                    return true;
                } else {
                    return false;
                }
            } else {
                toastr.warning("validation name must be in corect format.");
                return true;
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
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        } else {
            return true;
        }

    } else {
        return true;
    }
    return true;
}

function getnorange(pageid, type, group) {
    
    $.ajax({
        url: "/common/norange",
        dataType: 'json',
        type: "GET",
        data: { pageid: pageid, type: type, group: group},
        success: function (result) {

            sessionStorage.setItem('norange',result.data.msg);
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}

function NoRange() {
   return getnorange(norange);
}


