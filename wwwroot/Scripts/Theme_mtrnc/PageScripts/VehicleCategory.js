//$(document).ready(function () {


//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');

//    $('#BtnSave').click(function () {

//        validation();
//    });
//    $('#BtnDeleteVehicleCategory').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_VehicleCategory/DeleteVehicleCategory",
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
//            url: "/_VehicleCategory/AddVehicleCategory",
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
//              var formData = {


//            objectcategoryno: $('#objectcategoryno').val(),
//            objectcategoryltxt: $('#objectcategoryltxt').val(),
//            isactive: $('#chkactive123').prop('checked')
//        };
//        return formData;
//    }

//    function clearFields() {

//        var formData = {
//            objectcategoryno: $('#objectcategoryno').val(null),
//            objectcategoryltxt: $('#objectcategoryltxt').val(null),
//            isactive: $('#chkactive123').prop('checked',false)
//        };
//        return formData;
//    }

//    function validation() {
//       toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;
//        if ($('#objectcategoryno').val() == "" || $('#objectcategoryno').val() == null) {
         
            
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Category no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#objectcategoryno').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in category no');
//            }
//        }

//        if ($('#objectcategoryno').val().length > 5) {

           
//            var flag1 = false;
//            setInterval(function () {
//                if (!flag1) {
//                    flag1 = true;//store this to compare later
//                    toastr.warning("Category no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        if ($('#objectcategoryltxt').val() == "" || $('#objectcategoryltxt').val() == null) {
           
//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Category is mendatory");
//                }
//            }, 1);

//        }
//        else {
//            if ($('#objectcategoryltxt').val().match(letters)) {
//            }
//            else {
//                toastr.warning('Please type alphanumeric characters only in category');
//            }
//        }
//        if ($('#objectcategoryltxt').val().length > 25) {


//            var flag3 = false;
//            setInterval(function () {
//                if (!flag3) {
//                    flag3 = true;//store this to compare later
//                    toastr.warning("Category must be less than 26 digits");
//                }
//            }, 1);
//        }
//        if ($('#objectcategoryno').val() != "" && $('#objectcategoryno').val().length < 6 && $('#objectcategoryno').val().match(letters) && $('#objectcategoryltxt').val() != "" && $('#objectcategoryltxt').val().match(letters) && $('#objectcategoryltxt').val().length < 26) {
//            AddGroup();
//        }
//    }

//});