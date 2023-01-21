//$(document).ready(function () {

//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');
//    clearFields();
//    $('#BtnSave').click(function () {
//        validation();

//    });
//    $('#BtnDeleteTransportType').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/transporttype/DeleteTransportType",
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
//            url: "/transporttype/AddTransportType",
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

//    function clearFields() {

//        var formData = {
//            vendtypeno: $('#vendtypeno').val(null),
//            //vendtypestxt: $('#vendtypestxt').val(null),
//            vendtypeltxt: $('#vendtypeltxt').val(null),
//            langno: $('#langno').val(null),
//            isactive: $('#chkactive123').prop('checked',false)
//        };
//        return formData;
//    }
//    function FieldsData() {
//        var formData = {
//            vendtypeno: $('#vendtypeno').val(),
//            //vendtypestxt: $('#vendtypestxt').val(),
//            vendtypeltxt: $('#vendtypeltxt').val(),
//            langno: $('#langno Option:Selected').val(),
//            isactive: $('#chkactive123').prop('checked')
//        };

//        return formData;
//    }


//    function validation() {
//        toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;
//        if ($('#vendtypeno').val() == "" || $('#vendtypeno').val() == null) {
            
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Transport type no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#vendtypeno').val().match(letters)) {
//            }
//            else {
                
//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in transporter type no');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#vendtypeno').val().length > 5) {

            
//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Transporter type no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        //if ($('#vendtypestxt').val() == "" || $('#vendtypestxt').val() == null) {

//        //    var flag5 = false;
//        //    setInterval(function () {
//        //        if (!flag5) {
//        //            flag5 = true;//store this to compare later
//        //            toastr.warning("Transporter type is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        //else {
//        //    if ($('#vendtypestxt').val().match(letters)) {
//        //    }
//        //    else {

//        //        var flag6 = false;
//        //        setInterval(function () {
//        //            if (!flag6) {
//        //                flag6 = true;//store this to compare later
//        //                toastr.warning('Please type alphanumeric characters only in transporter type');
//        //            }
//        //        }, 1);
//        //    }
//        //}


//        if ($('#vendtypeltxt').val() == "" || $('#vendtypeltxt').val() == null) {
           
//            var flag3 = false;
//            setInterval(function () {
//                if (!flag3) {
//                    flag3 = true;//store this to compare later
//                    toastr.warning("Description is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#vendtypeltxt').val().match(letters)) {
//            }
//            else {
                
//                var flag4 = false;
//                setInterval(function () {
//                    if (!flag4) {
//                        flag4 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in transporter type descripton');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#vendtypeltxt').val().length > 50) {


//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Transporter description must be less than 51 digits");
//                }
//            }, 1);
//        }
//        //if ($('#langno option:selected').val() == "" || $('#langno option:selected').val() == null) {

//        //    var flag6 = false;
//        //    setInterval(function () {
//        //        if (!flag6) {
//        //            flag6 = true;//store this to compare later
//        //            toastr.warning("Langno method is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        if ($('#vendtypeno').val() != "" && $('#vendtypeno').val().length < 6 && $('#vendtypeno').val().match(letters) && $('#vendtypeltxt').val() != "" && $('#vendtypeltxt').val().length < 51  && $('#vendtypeltxt').val().match(letters)) {
//            AddGroup();
//        }
//    }
    


//    $('#showtoast').click(function () {

//        toastr.success("Transport Type has been placed!");
//        toastr.info("Transport Type has been placed!");
        
//        toastr.error("Transport Type has been placed!");
//    });
//});