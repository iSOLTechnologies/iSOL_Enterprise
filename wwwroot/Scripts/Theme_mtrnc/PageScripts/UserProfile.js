$(document).ready(function () {

    $("#1").addClass('kt-menu__item--open');
    $("#10").addClass('kt-menu__item--open');
    clearFields();
    $('#BtnSave').click(function () {
        validation();

    });
    $('#BtnDeleteAdministrator').click(function () {
        var id = $("#hdid").val();
        $.ajax({
            url: "/_Administrator/DeleteAdministrator",
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
            url: "/_Administrator/AddAdministrator",
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
            userno: $('#userno').val(null),
            userfullname: $('#userfullname').val(null),
            dateofbirth: $('#dateofbirth').val(null),
            contactno: $('#contactno').val(null),
            phoneno: $('#phoneno').val(null),
            contactperson: $('#contactperson').val(null),
            contactpersoncontactno: $('#contactpersoncontactno').val(null),
            isactive: $('#chkactive123').prop('checked', false),
            appstatus: $('#appstatus').prop('checked', false)
        };
        return formData;
    }
    function FieldsData() {
        var formData = {

            userno: $('#userno').val(),
            userfullname: $('#userfullname').val(),
            dateofbirth: $('#dateofbirth').val(),
            contactno: $('#contactno').val(),
            phoneno: $('#phoneno').val(),
            contactperson: $('#contactperson').val(),
            contactpersoncontactno: $('#contactpersoncontactno').val(),
            isactive: $('#chkactive123').prop('checked'),
            appstatus: $('#appstatus').prop('checked'),
            langno: $('#langno Option:Selected').val(),
            religion: $('#religion Option:Selected').val()
        };

        return formData;
    }
    function validation() {
        if ($('#userno').val() == "" || $('#userno').val() == null) {
            toastr.warning("userno no is mendatory");
        }

        if ($('#userno').val().length > 5) {

            toastr.warning("userno must be less than 6 digits");
        }
        if ($('#userfullname').val() == "" || $('#userfullname').val() == null) {
            toastr.warning("userfullname is mendatory");

        }
        if ($('#dateofbirth').val() == "" || $('#dateofbirth').val() == null) {
            toastr.warning("dateofbirth is mendatory");

        }
        if ($('#contactno').val() == "" || $('#contactno').val() == null) {
            toastr.warning("contactno is mendatory");

        }
        if ($('#contactperson').val() == "" || $('#contactperson').val() == null) {
            toastr.warning("contactperson is mendatory");

        }
        if ($('#contactpersoncontactno').val() == "" || $('#contactpersoncontactno').val() == null) {
            toastr.warning("contactpersoncontactno is mendatory");

        }
        if ($('#userno').val() != "" && $('#userno').val().length < 6 && $('#userfullname').val() != "" && $('#dateofbirth').val() != "" && $('#contactno').val() != "" && $('#contactperson').val() != "" && $('#contactpersoncontactno').val() != "" && $('#langno Option:Selected').val() != "") {
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