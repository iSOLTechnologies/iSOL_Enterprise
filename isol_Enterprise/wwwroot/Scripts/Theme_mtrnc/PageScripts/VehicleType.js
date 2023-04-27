//$(document).ready(function () {


//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');

//    $('#BtnSave').click(function () {

//        validation();
//    });
//    $('#BtnDeleteVehicleType').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_VehicleType/DeleteVehicleType",
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
//            url: "/_VehicleType/AddVehicle",
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
//            objecttypeno: $('#objecttypeno').val(),
//            objecttypeltxt: $('#objecttypeltxt').val(),
//            readinggroupno: $('#readinggroupno').val(),
//            readingidno: $('#readingidno').val(),
//            isactive: $('#chkactive123').prop('checked')
//        };
//        return formData;
//    }

//    function clearFields() {

//        var formData = {
//            objecttypeno: $('#objecttypeno').val(null),
//            objecttypeltxt: $('#objecttypeltxt').val(null),
//            readinggroupno: $('#readinggroupno').val(null),
//            readingidno: $('#readingidno').val(null),
//            isactive: $('#chkactive123').prop('checked', false)
//            //isactive: $('.chkactive1').prop('checked',false)
//        };
//        return formData;
//    }

//    function validation() {
// toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;
//        if ($('#objecttypeno').val() == "" || $('#objecttypeno').val() == null) {
           
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Vehicle type no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#objecttypeno').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in vehicle type no');
//            }
//        }
//        if ($('#objecttypeno').val().length > 5) {

            
//            var flag1 = false;
//            setInterval(function () {
//                if (!flag1) {
//                    flag1 = true;//store this to compare later
//                    toastr.warning("Vehicle type no must be less than 6 digits");
//                }
//            }, 1);
//        }

//        if ($('#objecttypeltxt').val() == "" || $('#objecttypeltxt').val() == null) {
         
//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Vehicle type is mendatory");
//                }
//            }, 1);

//        }
//        else {
//            if ($('#objecttypeltxt').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in vehicle type');
//            }
//        }
//        if ($('#objecttypeltxt').val().length > 25) {


//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Vehicle type must be less than 26 digits");
//                }
//            }, 1);
//        }
//        //if ($('#readinggroupno').val() == "" || $('#readinggroupno').val() == null) {
            
//        //    var flag3 = false;
//        //    setInterval(function () {
//        //        if (!flag3) {
//        //            flag3 = true;//store this to compare later
//        //            toastr.warning("Reading group no is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        //else {
//        //    if ($('#readinggroupno').val().match(letters)) {
//        //    }
//        //    else {
//        //        toastr.warning('Please type alphanumeric characters only in reading group no');
//        //    }
//        //}
//        //if ($('#readinggroupno').val().length > 5) {


//        //    var flag6 = false;
//        //    setInterval(function () {
//        //        if (!flag6) {
//        //            flag6 = true;//store this to compare later
//        //            toastr.warning("Reading group no must be less than 6 digits");
//        //        }
//        //    }, 1);
//        //}
//        if ($('#readingidno').val() == "" || $('#readingidno').val() == null) {
           
//            var flag4 = false;
//            setInterval(function () {
//                if (!flag4) {
//                    flag4 = true;//store this to compare later
//                    toastr.warning("Reading id no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#readingidno').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in reading id no');
//            }
//        }
//        if ($('#readingidno').val().length > 5) {


//            var flag7 = false;
//            setInterval(function () {
//                if (!flag7) {
//                    flag7 = true;//store this to compare later
//                    toastr.warning("Reading id no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        if ($('#objecttypeno').val() != "" && $('#objecttypeno').val().length < 6 && $('#objecttypeno').val().match(letters) && $('#objecttypeltxt').val() != "" && $('#objecttypeltxt').val().match(letters) && $('#objecttypeltxt').val().length < 26 && $('#readingidno').val() != "" && $('#readingidno').val().match(letters) && $('#readingidno').val().length < 6 ) {
//            AddGroup();
//        }
//    }

//});