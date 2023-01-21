$(document).ready(function () {

    $("#1").addClass('kt-menu__item--open');
    $("#10").addClass('kt-menu__item--open');

    $('#BtnSave').click(function () {

        validation();
    });
    $('#BtnDeleteCostElementProfile').click(function () {
        var id = $("#hdid").val();
        $.ajax({
            url: "/_CostElementProfile/DeleteCostElementProfile",
            dataType: 'json',
            type: "POST",
            data: { id: id },
            success: function (result) {

                if (result.isInserted) {
                    $("#myModal3").modal("hide");
                    clearFields();
                    toastr.success(result.msg);
                    window.history.back();
                }
                else {
                    toastr.error(result.isError);
                }
            },
            error: function (jqXhr, textStatus, errorMessage) {
                console.log(errorMessage);
            }

        })
    });

    function AddGroup() {
        $.ajax({
            url: "/_CostElementProfile/AddCostElementProfile",
            dataType: 'json',
            type: "POST",
            data: FieldsData(),
            success: function (result) {

                if (result.isInserted) {

                    clearFields();
                    toastr.success(result.msg);
                }
                else {
                    toastr.error(result.isError);
                }
            },
            error: function (jqXhr, textStatus, errorMessage) {
                console.log(errorMessage);
            }
        });
    }


    function FieldsData() {
        var formData = {
            costelementno: $('#costelementno').val(),
            //costelementstxt: $('#costelementstxt').val(),
            costelementltxt: $('#costelementltxt').val(),
            fk_accounttype_accounttype: $('#fk_accounttype_accounttype Option:Selected').val(),
            costelementdirection: $('#costelementdirection').val(),
            costelementvalcondition: $('#costelementvalcondition').val(),
            costelementvalbasedon: $('#costelementvalbasedon').val(),
            langno: $('#langno Option:Selected').val(),
            isactive: $('#chkactive123').prop('checked'),
            isbasedontotalamount: $('#isbasedontotalamount').prop('checked')
        };
        return formData;
    }

    function clearFields() {

        var formData = {
            costelementno: $('#costelementno').val(null),
            //costelementstxt: $('#costelementstxt').val(null),
            costelementltxt: $('#costelementltxt').val(null),
            fk_accounttype_accounttype: $('#fk_accounttype_accounttype').val(null),
            langno: $('#langno').val(null),
            costelementdirection: $('#costelementdirection').val(null),
            costelementvalcondition: $('#costelementvalcondition').val(null),
            costelementvalbasedon: $('#costelementvalbasedon').val(null),
            isactive: $('#chkactive123').prop('checked', false),
            isbasedontotalamount: $('#isbasedontotalamount').prop('checked', false)
        };
        return formData;
    }

    function validation() {
  toastr.remove();
        var letters = /^[0-9a-zA-Z  ]+$/;

        if ($('#costelementno').val() == "" || $('#costelementno').val() == null) {
           
            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Cost element no is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#costelementno').val().match(letters)) {
            }
            else {
                toastr.warning('Please type alphanumeric characters only in cost element');
            }
        }
        if ($('#costelementno').val().length > 5 ) {

            
            var flag1 = false;
            setInterval(function () {
                if (!flag1) {
                    flag1 = true;//store this to compare later
                    toastr.warning("Cost element no must be less than 6 digits");
                }
            }, 1);
        }
        //if ($('#costelementstxt').val() == "" || $('#costelementstxt').val() == null) {
            
        //    var flag2 = false;
        //    setInterval(function () {
        //        if (!flag2) {
        //            flag2 = true;//store this to compare later
        //            toastr.warning("cost element is mendatory");
        //        }
        //    }, 1);

        //}
        //else {
        //    if ($('#costelementstxt').val().match(letters)) {
        //    }
        //    else {
        //        toastr.warning('Please type alphanumeric characters only in cost element');
        //    }
        //}
        if ($('#costelementltxt').val() == "" || $('#costelementltxt').val() == null) {
           
            var flag3 = false;
            setInterval(function () {
                if (!flag3) {
                    flag3 = true;//store this to compare later
                    toastr.warning("Description is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#costelementltxt').val().match(letters)) {
            }
            else {
                toastr.warning('Please type alphanumeric characters only in description');
            }
        }
        if ($('#fk_accounttype_accounttype Option:Selected').val() == "" || $('#fk_accounttype_accounttype Option:Selected').val() == null) {
          
            var flag4 = false;
            setInterval(function () {
                if (!flag4) {
                    flag4 = true;//store this to compare later
                    toastr.warning("Account type is mendatory");
                }
            }, 1);
        }
        if ($('#costelementdirection').val() == "" || $('#costelementdirection').val() == null) {
            
            var flag5 = false;
            setInterval(function () {
                if (!flag5) {
                    flag5 = true;//store this to compare later
                    toastr.warning("Cost element direction is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#costelementdirection').val().match(letters)) {
            }
            else {
                toastr.warning('Please type alphanumeric characters only in account type');
            }
        }
        if ($('#costelementvalcondition').val() == "" || $('#costelementvalcondition').val() == null) {
            
            var flag6 = false;
            setInterval(function () {
                if (!flag6) {
                    flag6 = true;//store this to compare later
                    toastr.warning("Cost element val condition is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#costelementvalcondition').val().match(letters)) {
            }
            else {
                toastr.warning('Please type alphanumeric characters only in cost element');
            }
        }
        if ($('#costelementvalbasedon').val() == "" || $('#costelementvalbasedon').val() == null) {
           
            var flag7 = false;
            setInterval(function () {
                if (!flag7) {
                    flag7 = true;//store this to compare later
                    toastr.warning("Cost element val based on is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#costelementvalbasedon').val().match(letters)) {
            }
            else {
                toastr.warning('Please type alphanumeric characters only in cost element val based');
            }
        }
        if ($('#langno Option:Selected').val() == "" || $('#langno Option:Selected').val() == null) {
           
            var flag8 = false;
            setInterval(function () {
                if (!flag8) {
                    flag8 = true;//store this to compare later
                    toastr.warning("Language type is mendatory");
                }
            }, 1);
        }
        if ($('#costelementltxt').val().length > 50) {


            var flag9 = false;
            setInterval(function () {
                if (!flag9) {
                    flag9 = true;//store this to compare later
                    toastr.warning("Description must be less than 51 digits");
                }
            }, 1);
        }
        if ($('#costelementdirection').val().length > 100) {


            var flag10 = false;
            setInterval(function () {
                if (!flag10) {
                    flag10 = true;//store this to compare later
                    toastr.warning("Cost element direction must be less than 101 digits");
                }
            }, 1);
        }
        if ($('#costelementvalcondition').val().length > 100) {


            var flag11= false;
            setInterval(function () {
                if (!flag11) {
                    flag11 = true;//store this to compare later
                    toastr.warning("Cost element val condition must be less than 101 digits");
                }
            }, 1);
        }
        if ($('#costelementvalbasedon').val().length > 100) {


            var flag12 = false;
            setInterval(function () {
                if (!flag12) {
                    flag12 = true;//store this to compare later
                    toastr.warning("Cost element val based on must be less than 101 digits");
                }
            }, 1);
        }
        if ($('#costelementno').val() != "" && $('#costelementno').val().length < 6 && $('#costelementno').val().match(letters) && $('#costelementltxt').val() != "" && $('#costelementltxt').val().length < 51 && $('#costelementltxt').val().match(letters) && $('#fk_accounttype_accounttype Option:Selected').val() != "" && $('#costelementdirection').val() != "" && $('#costelementdirection').val().length < 101 && $('#costelementdirection').val().match(letters) && $('#costelementvalcondition').val() != "" && $('#costelementvalcondition').val().length < 101 && $('#costelementvalcondition').val().match(letters) && $('#costelementvalbasedon').val() != "" && $('#costelementvalbasedon').val().length < 101 && $('#costelementvalbasedon').val().match(letters)  && $('#langno Option:Selected').val() != "") {
            AddGroup();
        }
    }

});