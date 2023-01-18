//$(document).ready(function () {

//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');
//    clearFields();
//    $('#BtnSave').click(function () {
//      validation();
//       // AddGroup();
//    });

//    function AddGroup() {
        
//             $.ajax({
//            url: "/unitmst/AddGroup",
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

//    function EditGroup() {
//        $.ajax({
//            url: "/unitmst/AddGroup",
//            dataType: 'json',
//            type: "POST",
//            data: FieldsData(),
//            success: function (result) {

//                clearFields();
//                console.log(result.msg);
//                console.log(result.isInserted);
//                console.log(result.isError);
//            },
//            error: function (jqXhr, textStatus, errorMessage) {
//                console.log(errorMessage);
//            }
//        });
//    }

//    //$('#BtnSave').validate(){
//    //            rules: {
//    //        'vendgrpno': {
//    //            required: true,
//    //            maxlength: 5 
//    //        },

//    //        'vendgrpstxt': {
//    //            required: true,
//    //        }
//    //    },
//    //    messages: {
//    //        'vendgrpno': {
//    //            vendgrpno:  'Please enter valid Group No',
//    //            maxlength: 'Your password must be at least 6 characters long'

//    //        },

//    //        'vendgrpstxt': {
//    //            required: 'Please Enter Group',

//    //        }
//    //    }

//    //});  

//    function clearFields() {
//        var formData = {
//            unitno: $('#unitno').val(null),
//            unitltxt: $('#unitltxt').val(null),
//            symbol: $('#symbol').val(null),
//            //paytermno: $('#paytermno').val(null),
//            //paymentmethodno: $('#paymentmethodno').val(null),
//            unitclass: $('#unitclass').val(null),
//            systemunitno: $('#systemunitno').prop('checked', false),
//            decimalprecision: $('#decimalprecision').val(null),
//            isactive: $('#isactive').prop('checked', false)
//        };
//        return formData;
//    }

//    function validation()
    
//    {
//        toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;

//        if ($('#unitno').val() == "" || $('#unitno').val() == null) {
            
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("unit no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#unitno').val().match(letters)) {
//            }
//            else {
                
//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in unit no');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#unitno').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("unit no must be less than 6 digits");
//                }
//            }, 1);
//        }

//        if ($('#unitltxt').val() == "" || $('#unitltxt').val() == null) {

//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("unitLtxt is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#unitltxt').val().match(letters)) {
//            }
//            else {

//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in unitltxt');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#unitltxt').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("unitltxt must be less than 6 digits");
//                }
//            }, 1);
//        }



//        if ($('#symbol').val() == "" || $('#symbol').val() == null) {

//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("symbol is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#symbol').val().match(letters)) {
//            }
//            else {

//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in symbol');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#symbol').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("symbol must be less than 6 digits");
//                }
//            }, 1);
//        }





//        if ($('#unitclass').val() == "" || $('#unitclass').val() == null) {

//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("unitclass is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#unitclass').val().match(letters)) {
//            }
//            else {

//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in unitclass');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#unitclass').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("unitclass must be less than 6 digits");
//                }
//            }, 1);
//        }





//        if ($('#decimalprecision').val() == "" || $('#decimalprecision').val() == null) {

//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("decimalprecision is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#decimalprecision').val().match(letters)) {
//            }
//            else {

//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in decimalprecision');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#decimalprecision').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("decimalprecision must be less than 6 digits");
//                }
//            }, 1);
//        }


        
//        if ($('#unitno').val() != "" && $('#unitno').val().length < 6 && $('#unitno').val().match(letters) && $('#unitltxt').val() != "" && $('#unitltxt').val().length < 6 && $('#unitltxt').val().match(letters) && $('#symbol').val() != "" && $('#symbol').val().match(letters) && $('#symbol').val().length < 6 && $('#unitclass').val() != "" && $('#unitclass').val().match(letters) && $('#unitclass').val().length < 6 && $('#decimalprecision').val() != "" && $('#decimalprecision').val().match(letters) && $('#decimalprecision').val().length < 6) {
//            AddGroup();
//         // toastr.warning("true");
//        }
//}
    

//     function FieldsData() {
        
//        var formData = {
//            unitno: $('#unitno').val(),
//            unitltxt: $('#unitltxt').val(),
//            symbol: $('#symbol').val(),
//            //paytermno: $('#paytermno').val(null),
//            //paymentmethodno: $('#paymentmethodno').val(null),
//            unitclass: $('#unitclass').val(),
//            systemunitno: $('#systemunitno').prop('checked'),
//            decimalprecision: $('#decimalprecision').val(),
//            isactive: $('#isactive').prop('checked')
//        };

//        return formData;
//    }


//    $('#showtoast').click(function () {

//        toastr.success("New order has been placed!");
//        toastr.info("New order has been placed!");
//        toastr.error("New order has been placed!");
//    });

//    $('#BtnDeleteTransportGroup').click(function () {
   
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/unitmst/DeleteTransportGroup",
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
//});

