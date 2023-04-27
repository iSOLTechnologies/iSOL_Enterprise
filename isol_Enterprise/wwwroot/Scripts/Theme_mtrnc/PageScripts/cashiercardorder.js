$(document).ready(function () {


    $("#1").addClass('kt-menu__item--open');
    $("#10").addClass('kt-menu__item--open');

    $('#BtnSave').click(function () {
        validation();
    });
    $('#BtnDeleteVehicleDevice').click(function () {
        var id = $("#hdid").val();
        $.ajax({
            url: "/_VehicleDevice/DeleteVehicleDevice",
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
            url: "/_vehiclecardlog/AddVehicleDevice",
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
            cardno: $('#cardno').val(),
            cardstxt: $('#cardstxt').val(),
            fk_documentdardtype_cardtypeno: $('#fk_documentdardtype_cardtypeno').val(),
            fk_vehicleprofile_equipmentno: $('#fk_vehicleprofile_equipmentno').val(),
            authority: $('#authority').val(),
            controller: $('#controller').val(),
            expirydate: $('#expirydate').val(),
            remarks: $('#remarks').val(),
            isactive: $('#isactive').prop('checked')
        };
        return formData;
    }

    function clearFields() {

        var formData = {
            cardno: $('#cardno').val(null),
            cardstxt: $('#cardstxt').val(null),
            fk_documentdardtype_cardtypeno: $('#fk_documentdardtype_cardtypeno').val(null),
            fk_vehicleprofile_equipmentno: $('#fk_vehicleprofile_equipmentno').val(null),
            authority: $('#authority').val(null),
            controller: $('#controller').val(null),
            expirydate: $('#expirydate').val(null),
            remarks: $('#remarks').val(null),
            isactive: $('#isactive').prop('checked', false)
        };
        return formData;
    }

    function validation() {
        if ($('#cardno').val() == "" || $('#authority').val() == null) {
            toastr.warning("Card Number is mendatory");
        }

        if ($('#cardstxt').val().length > 25) {

            toastr.warning("card Name must be less than 6 digits");
        }
        if ($('#authority').val() == "" || $('#authority').val() == null) {
            toastr.warning("Authority name is mendatory");

        }
        if ($('#controller').val() == "" || $('#controller').val() == null) {
            toastr.warning("controller Name is mendatory");
        }
        if ($('#issuedate').val() == "" || $('#issuedate').val() == null) {
            toastr.warning("issue Date is mendatory");
        }
        if ($('#expirydate').val() == "" || $('#expirydate').val() == null) {
            toastr.warning("expirydate date is mendatory");
        }
        if ($('#remarks').val() == "" || $('#remarks').val() == null) {
            toastr.warning("remarks is mendatory");
        }
        //if ($('#remarks').val() == "" || $('#remarks').val() == null) {
        //    toastr.warning("device number is mendatory");
        //}

        if ($('#cardno').val() != "" && $('#cardstxt').val().length < 26 && $('#authority').val() != "" && $('#controller').val() != "" && $('#issuedate').val() != "" && $('#expirydate').val() != "" && $('#remarks').val() != "" && $('#deviceno').val() != "") {
            AddGroup();
        }
    }

});