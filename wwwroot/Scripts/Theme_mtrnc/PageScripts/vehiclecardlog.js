$(document).ready(function () {


    $("#1").addClass('kt-menu__item--open');
    $("#10").addClass('kt-menu__item--open');

    $('#BtnSave').click(function (){
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
            isactive: $('#isactive').prop('checked',false)
        };
        return formData;
    }

    function validation()
    {
 toastr.remove();
        var letters = /^[0-9a-zA-Z  ]+$/;
        
        if ($('#cardno').val() == "" || $('#cardno').val() == null) {
         
            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Vehicle card no is Mendatory");
                }
            }, 1);
        }
        else {
            if ($('#cardno').val().match(letters)) {
            }
            else {
               
                var flag1 = false;
                setInterval(function () {
                    if (!flag1) {
                        flag1 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in vehicle card no');
                    }
                }, 1);
            }
        }
        if ($('#cardno').val().length > 5) {


            var flag2 = false;
            setInterval(function () {
                if (!flag2) {
                    flag2 = true;//store this to compare later
                    toastr.warning("Vehicle card no must be less than 6 digits");
                }
            }, 1);
        }

        if ($('#cardstxt').val() == "" || $('#cardstxt').val() == null) {

            var flag3 = false;
            setInterval(function () {
                if (!flag3) {
                    flag3 = true;//store this to compare later
                    toastr.warning("Card is Mendatory");
                }
            }, 1);
        }
        else {
            if ($('#cardstxt').val().match(letters)) {
            }
            else {
                
                var flag4 = false;
                setInterval(function () {
                    if (!flag4) {
                        flag4 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in card');
                    }
                }, 1);
            }
        }
        if ($('#cardstxt').val().length > 25) {


            var flag5 = false;
            setInterval(function () {
                if (!flag5) {
                    flag5 = true;//store this to compare later
                    toastr.warning("Card must be less than 26 digits");
                }
            }, 1);
        }
        if ($('#authority').val() == "" || $('#authority').val() == null) {
           
            var flag6 = false;
            setInterval(function () {
                if (!flag6) {
                    flag6 = true;//store this to compare later
                    toastr.warning("Authority is mendatory");
                }
            }, 1);

        }
        else {
            if ($('#authority').val().match(letters)) {
            }
            else {
                
                var flag7 = false;
                setInterval(function () {
                    if (!flag7) {
                        flag7 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in authority');
                    }
                }, 1);
            }
        }
        if ($('#authority').val().length > 25) {


            var flag8 = false;
            setInterval(function () {
                if (!flag8) {
                    flag8 = true;//store this to compare later
                    toastr.warning("Authority must be less than 26 digits");
                }
            }, 1);
        }
        if ($('#controller').val() == "" || $('#controller').val() == null) {
            
            var flag9 = false;
            setInterval(function () {
                if (!flag9) {
                    flag9 = true;//store this to compare later
                    toastr.warning("Controller Name is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#controller').val().match(letters)) {
            }
            else {
                
                var flag10 = false;
                setInterval(function () {
                    if (!flag10) {
                        flag10 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in controller');
                    }
                }, 1);

            }
        }
        if ($('#controller').val().length > 25) {


            var flag11 = false;
            setInterval(function () {
                if (!flag11) {
                    flag11 = true;//store this to compare later
                    toastr.warning("Controller must be less than 26 digits");
                }
            }, 1);
        }
        if ($('#issuedate').val() == "" || $('#issuedate').val() == null) {
           
            var flag12 = false;
            setInterval(function () {
                if (!flag12) {
                    flag12 = true;//store this to compare later
                    toastr.warning("issue Date is mendatory");
                }
            }, 1);
        }

        if ($('#expirydate').val() == "" || $('#expirydate').val() == null) {
           
            var flag13 = false;
            setInterval(function () {
                if (!flag13) {
                    flag13 = true;//store this to compare later
                    toastr.warning("expirydate date is mendatory");
                }
            }, 1);
        }
        if ($('#expirydate').val() == "" || $('#expirydate').val() == null || $('#issuedate').val() == "" || $('#issuedate').val() == null) {

        }
        else {
            if ($('#expirydate').val() <= $('#issuedate').val()) {
                var flag14 = false;
                setInterval(function () {
                    if (!flag14) {
                        flag14 = true;//store this to compare later
                        toastr.warning("Expiry date should not be equal or less than issue date");
                    }
                }, 1);
            }
        }
        if ($('#remarks').val() == "" || $('#remarks').val() == null) {
            
            var flag15 = false;
            setInterval(function () {
                if (!flag15) {
                    flag15 = true;//store this to compare later
                    toastr.warning("Remarks is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#remarks').val().match(letters)) {
            }
            else {
                
                var flag16 = false;
                setInterval(function () {
                    if (!flag16) {
                        flag16 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in remarks');
                    }
                }, 1);
            }
        }
        if ($('#remarks').val().length > 25) {


            var flag17 = false;
            setInterval(function () {
                if (!flag17) {
                    flag17 = true;//store this to compare later
                    toastr.warning("Remarks must be less than 26 digits");
                }
            }, 1);
        }
        if ($('#fk_documentdardtype_cardtypeno').val() == "" || $('#fk_documentdardtype_cardtypeno').val() == null) {

            var flag18 = false;
            setInterval(function () {
                if (!flag18) {
                    flag18 = true;//store this to compare later
                    toastr.warning("Card type no is mendatory");
                }
            }, 1);
        }
        if ($('#fk_vehicleprofile_equipmentno').val() == "" || $('#fk_vehicleprofile_equipmentno').val() == null) {

            var flag19 = false;
            setInterval(function () {
                if (!flag19) {
                    flag19 = true;//store this to compare later
                    toastr.warning("Vehicle profile equipment no is mendatory");
                }
            }, 1);
        }

        if ($('#cardno').val() !== "" && $('#cardno').val().match(letters) && $('#cardno').val().length < 6 && $('#cardstxt').val() !== "" && $('#cardstxt').val().match(letters) && $('#cardstxt').val().length < 26 && $('#authority').val() !== "" && $('#authority').val().match(letters) && $('#authority').val().length < 26 && $('#controller').val() !== "" && $('#controller').val().match(letters) && $('#controller').val().length < 26 && $('#remarks').val() !== "" && $('#remarks').val().match(letters) && $('#remarks').val().length < 26 && $('#issuedate').val() !== "" && $('#expirydate').val() !== "" && $('#issuedate').val() < $('#expirydate').val()) {
           AddGroup();
        }
        //if ($('#cardno').val() != "" && $('#cardno').val().length < 6 && $('#cardno').val().match(letters) && ('#cardstxt').val() != "" && $('#cardstxt').val().length < 26 && $('#cardstxt').val().match(letters) && $('#authority').val() != "" && $('#authority').val().match(letters) && $('#authority').val().length < 26 && $('#controller').val() != "" && $('#controller').val().length < 26 && $('#controller').val().match(letters) && $('#issuedate').val() != "" && $('#expirydate').val() != "" && $('#remarks').val() != "" && $('#remarks').val().match(letters) && $('#remarks').val().length < 26 && $('#issuedate').val() < $('#expirydate').val()) {
        //    // AddGroup();
        //    toastr.success("Vehicle profile equipment no is mendatory");
        //}
    }

});