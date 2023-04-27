//$(document).ready(function () {

//    $("#1").addClass('kt-menu__item--open');
//    $("#10").addClass('kt-menu__item--open');

//    $('#BtnSave').click(function () {

//        validation();
//    });
//    $('#BtnDeleteDriverGroup').click(function () {
//        var id = $("#hdid").val();
//        $.ajax({
//            url: "/_DriverGroup/DeleteDriverGroup",
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
//            url: "/_DriverGroup/AddDriverGroup",
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
//        
//        var formData = {
//            drivergrpno: $('#drivergrpno').val(),
//            //drivergrpstxt: $('#drivergrpstxt').val(),
//            driverltxt: $('#driverltxt').val(),
//            fk_religionmst_religionno: $('#fk_religionmst_religionno Option:Selected').val(),
//            taxgrpno: $('#taxgrpno').val(), 
//            langno: $('#langno Option:Selected').val(),
//            isactive: $('#chkactive123').prop('checked')
//        };
//        return formData;
//    }

//    function clearFields() {

//        var formData = {
//            drivergrpno: $('#drivergrpno').val(null),
//            //drivergrpstxt: $('#drivergrpstxt').val(null),
//            driverltxt: $('#driverltxt').val(null),
//            fk_religionmst_religionno: $('#fk_religionmst_religionno').val(null),
//            taxgrpno: $('#taxgrpno').val(null),
//            langno: $('#langno').val(null),
//            isactive: $('#chkactive123').prop('checked',false)
//        };
//        return formData;
//    }

//    function validation() {
//  toastr.remove();
//        var letters = /^[0-9a-zA-Z  ]+$/;
//        if ($('#drivergrpno').val() == "" || $('#drivergrpno').val() == null) {
           
//            var flag = false;
//            setInterval(function () {
//                if (!flag) {
//                    flag = true;//store this to compare later
//                    toastr.warning("Driver group no is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#drivergrpno').val().match(letters)) {
//            }
//            else {

                
//                var flag7 = false;
//                setInterval(function () {
//                    if (!flag7) {
//                        flag7 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in driver group no');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#drivergrpno').val().length > 5) {

           
//            var flag1 = false;
//            setInterval(function () {
//                if (!flag1) {
//                    flag1 = true;//store this to compare later
//                    toastr.warning("Driver group no must be less than 6 digits");
//                }
//            }, 1);
//        }
//        //if ($('#drivergrpstxt').val() == "" || $('#drivergrpstxt').val() == null) {
           
//        //    var flag2 = false;
//        //    setInterval(function () {
//        //        if (!flag2) {
//        //            flag2 = true;//store this to compare later
//        //            toastr.warning("driver group is mendatory");
//        //        }
//        //    }, 1);

//        //}
//        //else {
//        //    if ($('#drivergrpstxt').val().match(letters)) {
//        //    }
//        //    else {
//        //        toastr.warning('Please type alphanumeric characters only in Driver Name');
//        //    }
//        //}
//        if ($('#driverltxt').val() == "" || $('#driverltxt').val() == null) {
            
//            var flag3 = false;
//            setInterval(function () {
//                if (!flag3) {
//                    flag3 = true;//store this to compare later
//                    toastr.warning("Description is mendatory");
//                }
//            }, 1);
//        }
//        else {
//            if ($('#driverltxt').val().match(letters)) {
//            }
//            else {
              
//                var flag8 = false;
//                setInterval(function () {
//                    if (!flag8) {
//                        flag8 = true;//store this to compare later
//                        toastr.warning('Please type alphanumeric characters only in driver description');
//                    }
//                }, 1);
//            }
//        }
//        if ($('#driverltxt').val().length > 50) {


//            var flag11 = false;
//            setInterval(function () {
//                if (!flag11) {
//                    flag11 = true;//store this to compare later
//                    toastr.warning("Driver description no must be less than 51 digits");
//                }
//            }, 1);
//        }
//        if ($('#fk_religionmst_religionno Option:Selected').val() == "" || $('#fk_religionmst_religionno Option:Selected').val() == null) {
            
//            var flag4 = false;
//            setInterval(function () {
//                if (!flag4) {
//                    flag4 = true;//store this to compare later
//                    toastr.warning("Religion  is mendatory");
//                }
//            }, 1);
//        }

//        //if ($('#langno Option:Selected').val() == "" || $('#langno Option:Selected').val() == null) {
           
//        //    var flag5 = false;
//        //    setInterval(function () {
//        //        if (!flag5) {
//        //            flag5 = true;//store this to compare later
//        //            toastr.warning("Language type is mendatory");
//        //        }
//        //    }, 1);
//        //}
       
//        if ($('#drivergrpno').val() != "" && $('#drivergrpno').val().length < 6 && $('#drivergrpno').val().match(letters) && $('#driverltxt').val() != "" && $('#driverltxt').val().match(letters) && $('#driverltxt').val().length < 51 && $('#fk_religionmst_religionno Option:Selected').val() != "" ) {
//            AddGroup();
//        }
//    }

//});