$(document).ready(function () {

    $("#1").addClass('kt-menu__item--open');
    $("#10").addClass('kt-menu__item--open');
    clearFields();
    $('#BtnSave').click(function () {
        validation();

    });

    $('#BtnDeleteMembershipCard').click(function () {
        var id = $("#hdid").val();
        $.ajax({
            url: "/_MembershipCard/DeleteMembershipcard",
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
            url: "/_MembershipCard/AddMembershipCard",
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

    function clearFields() {

        var formData = {
            membershipno: $('#membershipno').val(null),
            fk_costemementprofile_costelementno: $('#fk_costemementprofile_costelementno').val(null),
            fk_membershiptype_membershiptype: $('#fk_membershiptype_membershiptype').val(null),
            isbasedongrossvalue: $('#isbasedongrossvalue').prop('checked', false),
            basedonvalue: $('#basedonvalue').prop('checked', false),
            basedonfield: $('#basedonfield').prop('checked', false),
            isactive: $('#chkactive123').prop('checked', false)
        };
        return formData;
    }
    function FieldsData() {
        var formData = {
            membershipno: $('#membershipno').val(),
            fk_costemementprofile_costelementno: $('#fk_costemementprofile_costelementno Option:Selected').val(),
            fk_membershiptype_membershiptype: $('#fk_membershiptype_membershiptype Option:Selected').val(),
            isbasedongrossvalue: $('#isbasedongrossvalue').prop('checked'),
            basedonvalue: $('#basedonvalue').prop('checked'),
            basedonfield: $('#basedonfield').prop('checked'),
            isactive: $('#chkactive123').prop('checked')
        };

        return formData;
    }
    function validation()
    {
        var letters = /^[0-9a-zA-Z  ]+$/;
        toastr.remove();
        if ($('#membershipno').val() == "" || $('#membershipno').val() == null) {
            
            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Membership no is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#membershipno').val().match(letters)) {
            }
            else {
                toastr.warning('Please type alphanumeric characters only in membership no');
            }
        }

        if ($('#membershipno').val().length > 5) {

            
            var flag1 = false;
            setInterval(function () {
                if (!flag1) {
                    flag1 = true;//store this to compare later
                    toastr.warning("Membershipno must be less than 6 digits");
                }
            }, 1);
        }
        if ($('#fk_costemementprofile_costelementno Option:Selected').val() == "" || $('#fk_costemementprofile_costelementno Option:Selected').val() == null) {
            
            var flag2 = false;
            setInterval(function () {
                if (!flag2) {
                    flag2 = true;//store this to compare later
                    toastr.warning("Cost element no is mendatory");
                }
            }, 1);

        }
        if ($('#fk_membershiptype_membershiptype Option:Selected').val() == "" || $('#fk_membershiptype_membershiptype Option:Selected').val() == null) {
            
            var flag3 = false;
            setInterval(function () {
                if (!flag3) {
                    flag3 = true;//store this to compare later
                    toastr.warning("Membership type is mendatory");
                }
            }, 1);

        }
        if ($('#membershipno').val() != "" && $('#membershipno').val().length < 6 && $('#membershipno').val().match(letters) && $('#fk_costemementprofile_costelementno Option:Selected').val() != "" && $('#fk_membershiptype_membershiptype Option:Selected').val() != "") {
            AddGroup();
        }
    }



    $('#showtoast').click(function () {

        toastr.success("Transport Type has been placed!");
        toastr.info("Transport Type has been placed!");
        cc
        toastr.error("Transport Type has been placed!");
    });
});