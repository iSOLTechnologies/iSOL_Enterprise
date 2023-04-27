//$(document).ready(function () {


//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');

//    $('#BtnSave').click(function () {

//        validation();
//    });
//    $('#BtnDeleteVehicleDevice').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_VehicleDevice/DeleteVehicleDevice",
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
//            url: "/_VehicleDevice/AddVehicleDevice",
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
//            equipmentno: $('#equipmentno').val(),
//            deviceno: $('#deviceno').val(),
//            identificationno: $('#identificationno').val(),
//            supplier: $('#supplier').val(),
//            installationdate: $('#installationdate').val(),
//            isactive: $('#chkactive123').prop('checked')
//        };
//        return formData;
//    }

   
    
//    function clearFields() {

//        var formData = {
//            equipmentno: $('#equipmentno').val(null),
//            deviceno: $('#deviceno').val(null),
//            identificationno: $('#identificationno').val(null),
//            supplier: $('#supplier').val(null),
//            isactive: $('#chkactive123').prop('checked', false),
//            installationdate:$('#installationdate').val(null)
//        };
//        return formData;
//    }

    
   


//    function validation() {
//        
//        toastr.remove();
//        var date = ($('#installationdate').val());
//        var formattedDate = moment().format('YYYY-MM-DD');
//        var letters = /^[0-9a-zA-Z  ]+$/;
        

//        if ($('#equipmentno').val() == "" || $('#equipmentno').val() == null) {
            
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Equipment no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#equipmentno').val().match(letters)) {
//            }
//            else {
              
//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in equipment no');
//                    }
//                }, 1);
//            }
//        }
        
//        if ($('#equipmentno').val().length > 5) {

            
//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Equipment no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        if ($('#deviceno').val() == "" || $('#deviceno').val() == null) {
           
//            var flag3 = false;
//            setInterval(function () {
//                if (!flag3) {
//                    flag3 = true;//store this to compare later
//                    toastr.warning("Device no is mendatory");
//                }
//            }, 1);

//        }
//        else {
//            if ($('#deviceno').val().match(letters)) {
//            }
//            else {
               
//                var flag4 = false;
//                setInterval(function () {
//                    if (!flag4) {
//                        flag4 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in device no');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#deviceno').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("Device no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        if ($('#identificationno').val() == "" || $('#identificationno').val() == null) {
           
//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Identification no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#identificationno').val().match(letters)) {
//            }
//            else {
               
//                var flag6 = false;
//                setInterval(function () {
//                    if (!flag6) {
//                        flag6 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in identification no');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#identificationno').val().length > 5) {


//            var flag12 = false;
//            setInterval(function () {
//                if (!flag12) {
//                    flag12 = true;//store this to compare later
//                    toastr.warning("Identification no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        if ($('#supplier').val() == "" || $('#supplier').val() == null) {
            
//            var flag7 = false;
//            setInterval(function () {
//                if (!flag7) {
//                    flag7 = true;//store this to compare later
//                    toastr.warning("Supplier is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#supplier').val().match(letters)) {
//            }
//            else {
               
//                var flag8 = false;
//                setInterval(function () {
//                    if (!flag8) {
//                        flag8 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in supplier');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#supplier').val().length > 25) {


//            var flag13 = false;
//            setInterval(function () {
//                if (!flag13) {
//                    flag13 = true;//store this to compare later
//                    toastr.warning("Supplier must be less than 26 digits");
//                }
//            }, 1);
//        }
//        if (date <= formattedDate ) {


//        }
//        else {
//            var flag9 = false;
//            setInterval(function () {
//                if (!flag9) {
//                    flag9 = true;//store this to compare later
//                    toastr.warning("Date should be vaild");
//                }
//            }, 1);
//        }
        
//        if ($('#installationdate').val() == "" || $('#installationdate').val() == null) {
          
//            var flag10 = false;
//            setInterval(function () {
//                if (!flag10) {
//                    flag10 = true;//store this to compare later
//                    toastr.warning("Installation date is mendatory");
//                }
//            }, 1);
//        }

//        if ($('#equipmentno').val() != "" && $('#equipmentno').val().length < 6 && $('#equipmentno').val().match(letters) && $('#deviceno').val() != "" && $('#deviceno').val().match(letters) && $('#deviceno').val().length < 6 && $('#identificationno').val() != "" && $('#identificationno').val().match(letters) && $('#identificationno').val().length < 6 && $('#supplier').val() != "" && $('#supplier').val().match(letters) && $('#supplier').val().length < 26 && $('#installationdate').val() != "" && date <= formattedDate )  {
           
//            AddGroup();
//        }
//    }

//});