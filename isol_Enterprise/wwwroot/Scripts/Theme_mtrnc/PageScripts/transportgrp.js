//$(document).ready(function () {

//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');
//    clearFields();
//    $('#BtnSave').click(function () {
//        validation();

//    });





//    function AddGroup() {
//        $.ajax({
//            url: "/transportgrp/AddGroup",
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
//            url: "/transportgrp/AddGroup",
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

   

//    function clearFields() {

//        var formData = {
//            vendgrpno: $('#TransporterGroupNO').val(null),
//            vrpltxendgrpstxt: $('#TransporterGroup').val(null),
//            vendgt: $('#TransporterGroupDescripton').val(null),
//            //paytermno: $('#paytermno').val(null),
//            //paymentmethodno: $('#paymentmethodno').val(null),
//            langno: $('#langno').val(null),
//            taxgrpno: $('#TaxGroup').val(null),
//            isactive: $('#chkactive123').prop('checked', false)
//        };
//        return formData;
//    }
//    function validation() {
//        toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;

//        if ($('#TransporterGroupNO').val() == "" || $('#TransporterGroupNO').val() == null) {

//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Group no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#TransporterGroupNO').val().match(letters)) {
//            }
//            else {

//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in transporter group no');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#langno Option:Selected').val() == "" || $('#langno Option:Selected').val() == null) {

//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Lango no is mendatory");
//                }
//            }, 1);
//        }
//        if ($('#TransporterGroupNO').val().length > 5) {


//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Group no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        //if ($('#TransporterGroup').val() == "" || $('#TransporterGroup').val() == null) {

//        //    var flag3 = false;
//        //    setInterval(function () {
//        //        if (!flag3) {
//        //            flag3 = true;//store this to compare later
//        //            toastr.warning("Group is mendatory");
//        //        }
//        //    }, 1);

//        //}
//        //else {
//        //    if ($('#TransporterGroup').val().match(letters)) {
//        //    }
//        //    else {

//        //        var flag4 = false;
//        //        setInterval(function () {
//        //            if (!flag4) {
//        //                flag4 = true;//store this to compare later
//        //                toastr.warning('Please type alphanumeric characters only in Transporter Group');
//        //            }
//        //        }, 1);
//        //    }
//        //}

//        if ($('#TransporterGroupDescripton').val() == "" || $('#TransporterGroupDescripton').val() == null) {

//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Transporter group descripton is mendatory");
//                }
//            }, 1);
//        }

//        else {
//            if ($('#TransporterGroupDescripton').val().match(letters)) {
//            }
//            else {

//                var flag6 = false;
//                setInterval(function () {
//                    if (!flag6) {
//                        flag6 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in transporter group descripton');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#TransporterGroupDescripton').val().length > 50) {


//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Group description must be less than 51 digits");
//                }
//            }, 1);
//        }
//        //if ($('#PaymentTerm option:selected').val() == "" || $('#PaymentTerm option:selected').val() == null) {

//        //    var flag7 = false;
//        //    setInterval(function () {
//        //        if (!flag7) {
//        //            flag7 = true;//store this to compare later
//        //            toastr.warning("Payterm is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        //if ($('#PaymentMethod option:selected').val() == "" || $('#PaymentMethod option:selected').val() == null) {

//        //    var flag8 = false;
//        //    setInterval(function () {
//        //        if (!flag8) {
//        //            flag8 = true;//store this to compare later
//        //            toastr.warning("Payment Method is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        if ($('#langno option:selected').val() == "" || $('#langno option:selected').val() == null) {

//            var flag19 = false;
//            setInterval(function () {
//                if (!flag19) {
//                    flag19 = true;//store this to compare later
//                    toastr.warning("Langno method is mendatory");
//                }
//            }, 1);
//        }
//        if ($('#TaxGroup').val() == "" || $('#TaxGroup').val() == null) {

//            var flag9 = false;
//            setInterval(function () {
//                if (!flag9) {
//                    flag9 = true;//store this to compare later
//                    toastr.warning("Tax group is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#TaxGroup').val().match(letters)) {
//            }
//            else {

//                var flag10 = false;
//                setInterval(function () {
//                    if (!flag10) {
//                        flag10 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in tax group');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#TaxGroup').val().length > 5) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("Tax group must be less than 6 digits");
//                }
//            }, 1);
//        }

//        if ($('#TransporterGroupNO').val() != "" && $('#TransporterGroupNO').val().length < 6 && $('#TransporterGroupNO').val().match(letters) && $('#TransporterGroupDescripton').val() != "" && $('#TransporterGroupDescripton').val().length < 51 && $('#TransporterGroupDescripton').val().match(letters) && $('#langno option:selected').val() != "" && $('#TaxGroup').val() != "" && $('#TaxGroup').val().match(letters) && $('#TaxGroup').val().length < 6) {
//            AddGroup();
//        }
//    }

//    function FieldsData() {
//        var formData = {
//            vendgrpno: $('#TransporterGroupNO').val(),
//            //vendgrpstxt: $('#TransporterGroup').val(),
//            vendgrpltxt: $('#TransporterGroupDescripton').val(),
//            //fk_payterm_paytermno: $('#PaymentTerm Option:selected').val(),
//            //fk_paymethod_paymethodno: $('#PaymentMethod option:selected').val(),
//            langno: $('#langno Option:Selected').val(),
//            taxgrpno: $('#TaxGroup').val(),
//            isactive: $('#chkactive123').prop('checked')
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
//            url: "/transportgrp/DeleteTransportGroup",
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

