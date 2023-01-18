//$(document).ready(function () {

//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');

//    $('#BtnSave').click(function () {

//        validation();
//    });

//    $('#BtnDeleteMake').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_make/DeleteMake",
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
//            url: "/_make/AddMake",
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


//            makeno: $('#makeno').val(),
//            //makestxt: $('#makestxt').val(),
//            makeltxt: $('#makeltxt').val(),
//            fk_language_langno: $('#fk_language_langno Option:Selected').val(),
//            isactive: $('#chkactive123').prop('checked')
//            //isactive: $('.chkactive1').prop('checked')
//            //stchk: $('#stchk').val(),
//        };
//        return formData;
//    }

//    function clearFields() {

//        var formData = {
//            makeno: $('#makeno').val(null),
//            //makestxt: $('#makestxt').val(null),
//            makeltxt: $('#makeltxt').val(null),
//            fk_language_langno: $('#fk_language_langno').val(null),
//            isactive: $('#chkactive123').prop('checked', false)
//            //isactive: $('.chkactive1').prop('checked',false)
//        };
//        return formData;
//    }

//    function validation() {
//        toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;
//        if ($('#makeno').val() == "" || $('#makeno').val() == null) {
            
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Make no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#makeno').val().match(letters)) {
//            }
//            else {
//                var flag1 = false;
//                setInterval(function () {
//                    if (!flag1) {
//                        flag1 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in make no');

//                    }
//                }, 1);
//            }
//        }

//        if ($('#makeno').val().length > 5) {

//            var flag2 = false;
//            setInterval(function () {
//                if (!flag2) {
//                    flag2 = true;//store this to compare later
//                    toastr.warning("Make no must be less than 6 digits");

//                }
//            }, 1);
//        }
//        //if ($('#makestxt').val() == "" || $('#makestxt').val() == null) {
           
//        //    var flag3 = false;
//        //    setInterval(function () {
//        //        if (!flag3) {
//        //            flag3 = true;//store this to compare later
//        //            toastr.warning("make is mendatory");
//        //        }
//        //    }, 1);

//        //}
//        //else {
//        //    if ($('#makestxt').val().match(letters)) {
//        //    }
//        //    else {
                
//        //        var flag4 = false;
//        //        setInterval(function () {
//        //            if (!flag4) {
//        //                flag4 = true;//store this to compare later
//        //                toastr.warning('Please type alphanumeric characters only in make');
//        //            }
//        //        }, 1);
//        //    }
//        //}
//        if ($('#makeltxt').val() == "" || $('#makeltxt').val() == null) {
           
//            var flag5 = false;
//            setInterval(function () {
//                if (!flag5) {
//                    flag5 = true;//store this to compare later
//                    toastr.warning("Description is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#makeltxt').val().match(letters)) {
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
//        if ($('#makeltxt').val().length > 50) {


//            var flag8 = false;
//            setInterval(function () {
//                if (!flag8) {
//                    flag8 = true;//store this to compare later
//                    toastr.warning("Make description must be less than 51 digits");
//                }
//            }, 1);
//        }
//        if ($('#makeno').val() != "" && $('#makeno').val().length < 6 && $('#makeno').val().match(letters) && $('#makeltxt').val() != "" && $('#makeltxt').val().match(letters) && $('#makeltxt').val().length < 51 ) {
//            AddGroup();
//        }
//    }



//});