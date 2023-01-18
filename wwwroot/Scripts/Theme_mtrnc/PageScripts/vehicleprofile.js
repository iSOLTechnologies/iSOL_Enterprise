var cardtop = 0;
var Authoritytop = 40;
var Controllertop = 80;
var IssueDatetop = 120;


$(document).ready(function () {

   
    $("#1").addClass('kt-menu__item--open');
    $("#110").addClass('kt-menu__item--open');

    clearFields();
    $('#BtnSave').click(function () {
        validation();

    });


    //TrPositioning();


    $(document).on('click', '#btnDeleteRow', function () {
        $(this).closest('tr').remove();
    })

    $(document).on('click', '#btnNewRow', function () {
        AddRow();
        //TrPositioning();
    })
    

    function TrPositioning() {
        
        $('#card').css({
            "position": "absolute",
            "top": "0px",
            "left": "11px"
        });
        $('#Authority').css({
            "position": "absolute",
            "top": "40px",
            "left": "11px"
        });
        $('#Controller').css({
            "position": "absolute",
            "top": "80px",
            "left": "11px"
        });
        $('#IssueDate').css({
            "position": "absolute",
            "top": "120px",
            "left": "11px"
        });
        $('#ExpiryDate').css({
            "position": "absolute",
            "top": "12px",
            "left": "398px"
        });
        $('#Remarks').css({
            "position": "absolute",
            "top": "66px",
            "left": "402px"
        });
        $('#Description').css({
            "position": "absolute",
            "top": "120px",
            "left": "397px"
        });
        
    }


    function AddRow() {

        var clone = $('#template tbody').clone();
        var html = clone.removeClass('hide').html();
        $('#tblItemDetails tbody').append(html);
        
    }


    function insert() {
        $.ajax({
            url: "/vehicleprofile/insert",
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
            vendgrpno: $('#TransporterGroupNO').val(null),
            vendgrpstxt: $('#TransporterGroup').val(null),
            vendgrpltxt: $('#TransporterGroupDescripton').val(null),
            fk_payterm_paytermno: $('#PaymentTerm option:selected').val(),
            fk_paymethod_paymethodno: $('#PaymentMethod option:selected').val(),
            taxgrpno: $('#TaxGroup').val(null),
            isactive: $('#chkactive123').prop('checked', false)
        };
        return formData;
    }


    function validation() {
        toastr.remove();
        var letters = /^[0-9a-zA-Z]+$/;

        if ($('#vehicleType').val() == "" || $('#vehicleType').val() == null) {

            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Vehicle Type is mendatory");
                }
            }, 1);
        }

        if ($('#vehicleCategory').val() == "" || $('#vehicleCategory').val() == null) {

            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Vehicle Category is mendatory");
                }
            }, 1);
        }
        if ($('#VehicleSubCategory').val() == "" || $('#VehicleSubCategory').val() == null) {

            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Vehicle Sub Category is mendatory");
                }
            }, 1);
        }

        if ($('#vehicleno').val() == "" || $('#vehicleno').val() == null) {

            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Vehicle No is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#vehicleno').val().match(letters)) {
            }
            else {

                var flag1 = false;
                setInterval(function () {
                    if (!flag1) {
                        flag1 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in Vehicle No');
                    }
                }, 1);
            }
        }
        if ($('#vehicleno').val().length > 5) {


            var flag2 = false;
            setInterval(function () {
                if (!flag2) {
                    flag2 = true;//store this to compare later
                    toastr.warning("Vehicle No must be less than 6 digits");
                }
            }, 1);
        }
        if ($('#Identification').val() == "" || $('#Identification').val() == null) {

            var flag3 = false;
            setInterval(function () {
                if (!flag3) {
                    flag3 = true;//store this to compare later
                    toastr.warning("Identification is mendatory");
                }
            }, 1);

        }
        else {
            if ($('#Identification').val().match(letters)) {
            }
            else {

                var flag4 = false;
                setInterval(function () {
                    if (!flag4) {
                        flag4 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in Identification');
                    }
                }, 1);
            }
        }
        if ($('#Identification').val().length > 5) {


            var flag2 = false;
            setInterval(function () {
                if (!flag2) {
                    flag2 = true;//store this to compare later
                    toastr.warning("Identification must be less than 6 digits");
                }
            }, 1);
        }

        if ($('#Description').val() == "" || $('#Description').val() == null) {

            var flag3 = false;
            setInterval(function () {
                if (!flag3) {
                    flag3 = true;//store this to compare later
                    toastr.warning("Description is mendatory");
                }
            }, 1);

        }
        else {
            if ($('#Description').val().match(letters)) {
            }
            else {

                var flag4 = false;
                setInterval(function () {
                    if (!flag4) {
                        flag4 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in Description');
                    }
                }, 1);
            }
        }
        if ($('#Description').val().length > 5) {


            var flag2 = false;
            setInterval(function () {
                if (!flag2) {
                    flag2 = true;//store this to compare later
                    toastr.warning("Description length is 25 Characters");
                }
            }, 1);
        }

        if ($('#ownerType').val() == "" || $('#ownerType').val() == null) {

            var flag = false;
            setInterval(function () {
                if (!flag) {
                    flag = true;//store this to compare later
                    toastr.warning("Owner Type is mendatory");
                }
            }, 1);
        }

        if ($('#TransporterGroupDescripton').val() == "" || $('#TransporterGroupDescripton').val() == null) {

            var flag5 = false;
            setInterval(function () {
                if (!flag5) {
                    flag5 = true;//store this to compare later
                    toastr.warning("Transporter Group Descripton is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#TransporterGroupDescripton').val().match(letters)) {
            }
            else {

                var flag6 = false;
                setInterval(function () {
                    if (!flag6) {
                        flag6 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in Transporter Group Descripton');
                    }
                }, 1);
            }
        }

        if ($('#PaymentTerm option:selected').val() == "" || $('#PaymentTerm option:selected').val() == null) {

            var flag7 = false;
            setInterval(function () {
                if (!flag7) {
                    flag7 = true;//store this to compare later
                    toastr.warning("Payterm is mendatory");
                }
            }, 1);
        }
        if ($('#PaymentMethod option:selected').val() == "" || $('#PaymentMethod option:selected').val() == null) {

            var flag8 = false;
            setInterval(function () {
                if (!flag8) {
                    flag8 = true;//store this to compare later
                    toastr.warning("Payment Method is mendatory");
                }
            }, 1);
        }
        if ($('#TaxGroup').val() == "" || $('#TaxGroup').val() == null) {

            var flag9 = false;
            setInterval(function () {
                if (!flag9) {
                    flag9 = true;//store this to compare later
                    toastr.warning("Tax Group is mendatory");
                }
            }, 1);
        }
        else {
            if ($('#TaxGroup').val().match(letters)) {
            }
            else {

                var flag10 = false;
                setInterval(function () {
                    if (!flag10) {
                        flag10 = true;//store this to compare later
                        toastr.warning('Please type alphanumeric characters only in Tax Group');
                    }
                }, 1);
            }
        }

        if ($('#TransporterGroupNO').val() != "" && $('#TransporterGroupNO').val().length < 6 && $('#TransporterGroupNO').val().match(letters) && $('#TransporterGroup').val() != "" && $('#TransporterGroup').val().match(letters) && $('#PaymentTerm option:selected').val() != "" && $('#PaymentMethod option:selected').val() != "" && $('#TaxGroup').val() != "" && $('#TaxGroup').val().match(letters)) {
            insert();
        }
    }

    function FieldsData() {
        var formData = {
            vendgrpno: $('#TransporterGroupNO').val(),
            vendgrpstxt: $('#TransporterGroup').val(),
            vendgrpltxt: $('#TransporterGroupDescripton').val(),
            fk_payterm_paytermno: $('#PaymentTerm Option:Selected').val(),
            fk_paymethod_paymethodno: $('#PaymentMethod option:selected').val(),
            taxgrpno: $('#TaxGroup').val(),
            langno: $('#langno Option:Selected').val(),
            isactive: $('#chkactive123').prop('checked')
        };

        return formData;
    }
    $('#showtoast').click(function () {

        toastr.success("New order has been placed!");
        toastr.info("New order has been placed!");
        cc
        toastr.error("New order has been placed!");
    });
});


