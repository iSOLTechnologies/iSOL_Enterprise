import { Session } from "inspector";

$(document).ready(function () {
    $("#1").addClass('kt-menu__item--open');
    $("#119").addClass('kt-menu__item--open');

    $("#BtnInsert").click(function () {
        clearFields();
        AddGroup();
        $.get('@Url.Action("detailProfile","_Vehicle")', {}, function (response) {
            $("#detailPartialView").html(response);
        });
    });
    function AddGroup() {
       
        console.log(FieldsData());
        $.ajax({
            url: "/_TransporterProfile/Add",
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
        //;
        //var url = document.getElementById("useridimage").style.backgroundImage;

        //var usermst = {
        //    employeeno: $('#Username').val(),
        //    email: $('#Email').val(),
        //    password: $('#Password').val(),
        //    fk_retypemst_reftype: 'TA',
        //    fk_retypemst_reftype: $('#transporterno').val(),
        //    username: $('#username').val(),
        //    useridimage: url.substring(4, url.length - 1),
        //}
        //var transportProfile = { 
        //    //Profile
        //    fk_client_clientno: Session["client"],
        //    fk_cocode_cocodeno: Session["cocode"],
        //    transporterno: $('#transporterno').val(),
        //    transportername: $('#transportername').val(),
        //    transportercompanyname: $('#transportercompanyname').val(),
        //    fk_transporttype_vendtypeno: $('#fk_transporttype_vendtypeno  option:selected').val(),
        //    fk_transportgrp_vendgrpno: $('#fk_transportgrp_vendgrpno  option:selected').val(),
        //    fk_membershiphdr_membershipno: $('#fk_membershiphdr_membershipno option:selected').val(),
        //    fk_language_langno: $('#fk_language_langno option:selected').val(),
        //    fk_currency_currencyno: $('#fk_currency_currencyno option:selected').val(),
        //    openingtime: $('#openingtime').val(),
        //    closingtime: $('#closingtime').val(),
        //    paytermno: $('#paytermno option:selected').val(),
        //    paymethodno: $('#paymethodno option:selected').val(),
        //    paydayno: $('#paydayno').val(),
        //    taxgroupno: $('#taxgroupno').val(),
        //    priceincludetax: $('#priceincludetax').val(),

        //    //Address
        //    //addresstype: $('#addresstype').val(),
        //    //validfrom: $('#validfrom').val(),
        //    //validto: $('#validto').val(),
        //    //openingtime: $('#openingtime').val(),
        //    //closingtime: $('#closingtime').val(),
        //    //emergencynumber: $('#emergencynumber').val(),
        //    //contactperson: $('#contactperson').val(),
        //    //contactpersonnumbe: $('#contactpersonnumbe').val(),
        //    //longitude: $('#longitude').val(),
        //    //latitude: $('#latitude').val(),
        //    //address1: $('#address1').val(),
        //    //address2: $('#address2').val(),
        //    //address3: $('#address3').val(),
        //    //street: $('#street').val(),
        //    //countryid: $('#countryid').val(),
        //    //cityname: $('#cityname').val(),
        //    //zone: $('#zone').val(),
        //    //region: $('#region').val(),
        //    //pobox: $('#pobox').val(),
        //    //phone1: $('#phone1').val(),
        //    //phone2: $('#phone2').val(),
        //    //phone3: $('#phone3').val(),
        //    //mobile1: $('#mobile1').val(),
        //    //mobile2: $('#mobile2').val(),
        //    //mobile3: $('#mobile3').val(),
        //    //fax1: $('#fax1').val(),
        //    //fax2: $('#fax2').val(),
        //    //fax3: $('#fax3').val(),
        //    //email1: $('#email1').val(),
        //    //email2: $('#email2').val(),
        //    //email3: $('#email3').val(),
        //    //website: $('#website').val(),
        //};
        //var _data = { userProfile: usermst, transportProfile = transportProfile }

        return "";
    }


    function clearFields() {

        var formData = {

            employeeno: $('#Username').val(null),
            email: $('#Email').val(null),
            password: $('#Password').val(null),
            fk_retypemst_reftype: $('#transporterno').val(null),
            username: $('#username').val(null),
            transportername: $('#transportername').val(null),
            transportercompanyname: $('#transportercompanyname').val(null),
            fk_transporttype_vendtypeno: $('#fk_transporttype_vendtypeno  option:selected').val(null),
            fk_transportgrp_vendgrpno: $('#fk_transportgrp_vendgrpno  option:selected').val(null),
            fk_membershiphdr_membershipno: $('#fk_membershiphdr_membershipno option:selected').val(null),
            fk_language_langno: $('#fk_language_langno option:selected').val(null),
            fk_currency_currencyno: $('#fk_currency_currencyno option:selected').val(null),
            openingtime: $('#openingtime').val(null),
            closingtime: $('#closingtime').val(null),
            paytermno: $('#paytermno option:selected').val(null),
            paymethodno: $('#paymethodno option:selected').val(null),
            paydayno: $('#paydayno').val(null),
            taxgroupno: $('#taxgroupno').val(null),
            priceincludetax: $('#priceincludetax').val(null),
            validfrom: $('#validfrom').val(null),
            validto: $('#validto').val(),
            openingtime: $('#openingtime').val(null),
            closingtime: $('#closingtime').val(null),
            emergencynumber: $('#emergencynumber').val(null),
            contactperson: $('#contactperson').val(null),
            contactpersonnumbe: $('#contactpersonnumbe').val(null),
            longitude: $('#longitude').val(null),
            latitude: $('#latitude').val(null),
            address1: $('#address1').val(null),
            address2: $('#address2').val(null),
            address3: $('#address3').val(null),
            street: $('#street').val(null),
            countryid: $('#countryid').val(null),
            cityname: $('#cityname').val(null),
            zone: $('#zone').val(null),
            region: $('#region').val(null),
            pobox: $('#pobox').val(null),
            phone1: $('#phone1').val(null),
            phone2: $('#phone2').val(null),
            phone3: $('#phone3').val(null),
            mobile1: $('#mobile1').val(null),
            mobile2: $('#mobile2').val(null),
            mobile3: $('#mobile3').val(null),
            fax1: $('#fax1').val(null),
            fax2: $('#fax2').val(null),
            fax3: $('#fax3').val(null),
            email1: $('#email1').val(null),
            email2: $('#email2').val(null),
            email3: $('#email3').val(null),
            website: $('#website').val(null)

        };
        return formData;
    }

})

