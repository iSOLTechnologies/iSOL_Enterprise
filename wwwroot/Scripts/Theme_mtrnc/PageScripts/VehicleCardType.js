//$(document).ready(function () {


//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');

//    $('#BtnSave').click(function () {

//        validation();
//    });

//    $('#BtnDeleteVehicleCardType').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_VehicleCardType/DeleteVehicleCardType",
//            dataType: 'json',
//            type: "POST",
//            data: { id: id },
//            success: function (result) {

//                if (result.isInserted) {
//                    $("#myModal3").modal("hide");
//                    clearFields();
//                    toastr.success(result.msg);
//                    window.history.back();
//                }
//                else {
//                    toastr.error(result.isError);
//                }
//            },
//            error: function (jqXhr, textStatus, errorMessage) {
//                console.log(errorMessage);
//            }

//        })
//    });
//    function AddGroup() {
//        $.ajax({
//            url: "/_VehicleCardType/AddVehiclecardtype",
//            dataType: 'json',
//            type: "POST",
//            data: FieldsData(),
//            success: function (result) {

//                if (result.isInserted) {

//                    clearFields();
//                    toastr.success(result.msg);
//                }
//                else {
//                    toastr.error(result.isError);
//                }
//            },
//            error: function (jqXhr, textStatus, errorMessage) {
//                console.log(errorMessage);
//            }
//        });
//    }


//    function FieldsData() {
        
//        var formData = {
//            accountype: $('#accountype').val(),
//            cardtypeno: $('#cardtypeno').val(),
//            cardtypeltxt: $('#cardtypeltxt').val(),
//            triggerdays: $('#triggerdays').val(),
//            langno: $('#langno Option:Selected').val(),
//            allowmultiple: $('#allowmultiple').prop('checked'),
//            isactive: $('#chkactive123').prop('checked')
//        };
//        return formData;
//    }

//    function clearFields() {

//        var formData = {
//            accountype: $('#accountype').val(null),
//            cardtypeno: $('#cardtypeno').val(null),
//            cardtypeltxt: $('#cardtypeltxt').val(null),
//            triggerdays: $('#triggerdays').val(null),
//            langno: $('#langno').val(null),
//            allowmultiple: $('#allowmultiple').prop('checked', false),
//            isactive: $('#chkactive123').prop('checked', false)
//        };
//        return formData;
//    }

//    function validation() {
//  toastr.remove();
//        var letters = /^[0-9a-zA-Z ]+$/;
//        if ($('#cardtypeno').val() == "" || $('#cardtypeno').val() == null) {
          
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Card type no no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#cardtypeno').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in card type');
//            }
//        }
//        if ($('#cardtypeno').val().length > 5) {
            
//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Card type no must be less than 6 digits");
//                }
//            }, 1);
//        }

//        //if ($('#accountype').val() == "" || $('#accountype').val() == null) {
            
//        //    var flag1 = false;
//        //    setInterval(function () {
//        //        if (!flag1) {
//        //            flag1 = true;//store this to compare later
//        //            toastr.warning("Account type is mendatory");
//        //        }
//        //    }, 1);
           
//        //}
//        //else {
//        //    if ($('#accountype').val().match(letters)) {
//        //    }
//        //    else {
//        //        toastr.warning('Please type alphanumeric characters only in account type');
//        //    }
//        //}
//        if ($('#cardtypeltxt').val() == "" || $('#cardtypeltxt').val() == null) {
//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Card type is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#cardtypeltxt').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in card type');
//            }
//        }
//        if ($('#triggerdays').val() == "" || $('#triggerdays').val() == null) {
//            var flag3 = false;
//            setInterval(function () {
//                if (!flag3) {
//                    flag3 = true;//store this to compare later
//                    toastr.warning("Trigger days no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#triggerdays').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type numeric characters only in trigger days');
//            }
//        }
//        //if ($('#langno Option:Selected').val() == "" || $('#langno Option:Selected').val() == null) {
//        //    var flag4 = false;
//        //    setInterval(function () {
//        //        if (!flag4) {
//        //            flag4 = true;//store this to compare later
//        //            toastr.warning("Language type is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        //if ($('#accountype').val().length > 5) {

//        //    var flag5 = false;
//        //    setInterval(function () {
//        //        if (!flag5) {
//        //            flag5 = true;//store this to compare later
//        //            toastr.warning("Account type no must be less than 6 digits");
//        //        }
//        //    }, 1);
//        //}
//        if ($('#cardtypeltxt').val().length > 50) {


//            var flag6 = false;
//            setInterval(function () {
//                if (!flag6) {
//                    flag6 = true;//store this to compare later
//                    toastr.warning("Card type must be less than 51 digits");
//                }
//            }, 1);
//        }
//        if ($('#triggerdays').val().length > 11) {

//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Trigger days no must be less than 12 digits");
//                }
//            }, 1);
//        }
//        if ($('#cardtypeno').val() != "" && $('#cardtypeno').val().length < 6 && $('#cardtypeno').val().match(letters) && $('#cardtypeltxt').val() != "" && $('#cardtypeltxt').val().match(letters) && $('#cardtypeltxt').val().length < 51 && $('#triggerdays').val() != "" && $('#triggerdays').val().match(letters) && $('#triggerdays').val().length < 12 ) {
//            AddGroup();
//        }
//    }

//});