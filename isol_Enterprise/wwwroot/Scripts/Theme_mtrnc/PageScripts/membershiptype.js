//$(document).ready(function () {


//    $("#1").addClass('kt-menu__item--open');
//    $("#13").addClass('kt-menu__item--open');

//    clearFields();
//    $('#BtnSave').click(function () {
//        validation();
//    });
//    $('#BtnDeleteMembershipType').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_Membershiptype/DeleteMembershipType",
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
//            url: "/_Membershiptype/AddGroup",
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
//        //
//        var formData = {
//            membershiptype: $('#membershiptype').val(),
//            //memebershipstxt: $('#memebershipstxt').val(),
//            memebershipltxt: $('#memebershipltxt').val(),
//            langno: $('#langno Option:Selected').val(),
//            isactive: $('#chkactive123').prop('checked')
//        };

//        return formData;
//    }

//    function clearFields() {
//        var formData = {
//            membershiptype: $('#membershiptype').val(null),
//            //memebershipstxt: $('#memebershipstxt').val(null),
//            memebershipltxt: $('#memebershipltxt').val(null),
//            langno: $('#langno').val(null),
//            isactive: $('#chkactive123').prop('checked', false)
//        };
//        return formData;
//    }


//    function validation() {
//        toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;
//        if ($('#membershiptype').val() == "" || $('#membershiptype').val() == null) {
            
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Membershiptype no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#membershiptype').val().match(letters)) {
//            }
//            else {
                
//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in membershiptype no');
//                    }
//                }, 1);
//            }
//        }

//        if ($('#membershiptype').val().length > 5) {

            
//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Membership type no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        //if ($('#memebershipstxt').val() == "" || $('#memebershipstxt').val() == null) {
            
//        //    var flag3 = false;
//        //    setInterval(function () {
//        //        if (!flag3) {
//        //            flag3 = true;//store this to compare later
//        //            toastr.warning("memebershipstxt type is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        //else {
//        //    if ($('#memebershipstxt').val().match(letters)) {
//        //    }
//        //    else {
                
//        //        var flag4 = false;
//        //        setInterval(function () {
//        //            if (!flag4) {
//        //                flag4 = true;//store this to compare later
//        //                toastr.warning('Please type alphanumeric characters only in memebershipstxt type');
//        //            }
//        //        }, 1);
//        //    }
//        //}
//        if ($('#memebershipltxt').val() == "" || $('#memebershipltxt').val() == null) {
            
//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Description is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#memebershipltxt').val().match(letters)) {
//            }
//            else {
               
//                var flag6 = false;
//                setInterval(function () {
//                    if (!flag6) {
//                        flag6 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in description');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#memebershipltxt').val().length > 50) {


//            var flag8 = false;
//            setInterval(function () {
//                if (!flag8) {
//                    flag8 = true;//store this to compare later
//                    toastr.warning("Description must be less than 51 digits");
//                }
//            }, 1);
//        }
//        //if ($('#langno option:selected').val() == "" || $('#langno option:selected').val() == null) {

//        //    var flag7 = false;
//        //    setInterval(function () {
//        //        if (!flag7) {
//        //            flag7 = true;//store this to compare later
//        //            toastr.warning("Langno method is mendatory");
//        //        }
//        //    }, 1);
//        //}
//        if ($('#membershiptype').val() != "" && $('#membershiptype').val().length < 6 && $('#membershiptype').val().match(letters) && $('#memebershipltxt').val() != "" && $('#memebershipltxt').val().match(letters) && $('#memebershipltxt').val().length < 51) {
//            AddGroup();
//        }
//    }

//});