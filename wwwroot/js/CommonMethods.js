﻿
$(document).on('change keyup', '#UPrc,#DicPrc', function (e) {

    var quantityField = $(this).closest('#ListParameters .itm').find('#DicPrc');
    var quantityValue = $(this).closest('#ListParameters .itm').find('#DicPrc').val();

    if (quantityValue == "") {
        quantityField.val("");
    } else if (quantityValue < 0) {
        quantityField.val("");
        toastr.warning("Discount can't be less than 0");
    } else if (quantityValue > 100) {
        quantityField.val(100);
        toastr.warning("Discount can't be greater than 100");
    }
});

$(document).on('change keyup', '#Discount', function (e) {

    if ($(this).val() > 100) {
        $(this).val(100);
        toastr.warning("Discount can't be greater than 100");
    }
    else if ($(this).val() < 0) {
        $(this).val("");
        toastr.warning("Discount can't be less than 0");
    }
})


$(document).on('change keyup', '#Warehouse', function (e) {
    let ItemCodeField = $(this).closest('#ListParameters .itm').find('#ItemCode');
    let ItemCodeValue = ItemCodeField.val();
    let WarehouseField = $(this).closest('#ListParameters .itm').find('#Warehouse');
    let WarehouseValue = WarehouseField.val();
    let onHand = $(this).closest('#ListParameters .itm').find('#onHand');

    $(this).closest('#ListParameters .itm').find("#QTY").val(0);
    //console.log("ItemCodeValue", ItemCodeValue);
    //console.log("WarehouseValue", WarehouseValue.toString());
    $.get("Common/GetSelectedWareHouseData", { ItemCode: ItemCodeValue, WhsCode: WarehouseValue }, function (data) {

        onHand.val(data);

    });
});




function GetWareHouseQty(ItemCodeValue, WarehouseValue) {

    var a = 0;

    return $.ajax({
        url: 'Common/GetSelectedWareHouseData',
        type: 'GET',
        dataType: 'json',
        async : false,
        data: { ItemCode: ItemCodeValue, WhsCode: WarehouseValue },
        success: function (result) {
            return result;
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });

    //$.get("Common/GetSelectedWareHouseData", { ItemCode: ItemCodeValue, WhsCode: WarehouseValue }, function (data) {

    //    console.log("Ware" + data);
    //    return data;

    //});

}




function GetWareHouseData() {
    $.get("Delivery/GetWareHouseData", function (data) {


        $("#Warehouse").html("");

        $.each(data, function () {

            $("#Warehouse").append($('<option>', {
                value: this.whscode,
                text: this.whsname

            }));


            WareHouseData += "<option  value='" + this.whscode + "'>" + this.whsname + "</option>";
        });

    });
}


function InitializeWareHouseData() {

    $.get("Delivery/GetWareHouseData", function (data) {




        $.each(data, function () {




            WareHouseData += "<option  value='" + this.whscode + "'>" + this.whsname + "</option>";
        });

    });
}



function GetPrice() {


    let FooterTax = $('#FooterTax').val();
    console.log("FooterTax", FooterTax);

    $("#Total").val(0);
    var totalBeforeDiscount = 0;
    var RoundingValue = 0;
    var Discount = parseFloat($("#Discount").val() / 100);
    $('#ListParameters .itm').each(function (index, item) {
        var dt1 = $(this).closest('#ListParameters .itm').find("#TtlPrc");
        if (dt1.val() != null)
            totalBeforeDiscount = Number(totalBeforeDiscount) + Number(dt1.val());
    });
    if (totalBeforeDiscount != 0)
        $("#TotalBeforeDiscount").val(totalBeforeDiscount);

    if ($('#RoundingChkBox').is(":checked") && $('#Rounding').val() != "") {

        RoundingValue = parseFloat($('#Rounding').val());

    }
    if (Discount != "" && RoundingValue != "") {

        $("#Total").val((totalBeforeDiscount + RoundingValue) - ((totalBeforeDiscount + RoundingValue) * Discount));

    }
    else if (Discount == "" && RoundingValue != "") {
        $("#Total").val((totalBeforeDiscount + RoundingValue));
    }
    else if (Discount != "" && RoundingValue == "") {

        $("#Total").val(parseFloat(totalBeforeDiscount - (totalBeforeDiscount * Discount)));
    }
    else if (Discount == "" && RoundingValue == "") {

        $("#Total").val(totalBeforeDiscount);
    }

    if (FooterTax != null && FooterTax != "" && FooterTax != undefined) {
        console.log("A", $("#Total").val());
        let OnePercent = parseFloat($("#Total").val()) / 100;
        let TaxAmount = parseFloat(OnePercent) * parseFloat(FooterTax);
        $("#Total").val((parseFloat($("#Total").val()) + parseFloat(TaxAmount)).toFixed(2)); 
    }
}



function ValidateData(element) {

    let isValid = [];

    $('#' + element + ' input,' + '#' + element + ' select, ' + '#' + element + ' textarea').each(function (index, data) {

        if (!($(this).hasClass("NotReq"))) {
            if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '') {
                if ($(this).val() == null || $(this).val() == '' || $(this).val() == undefined) {
                    $(this).addClass('is-invalid');
                    isValid.push(false);
                }
                else {
                    $(this).removeClass('is-invalid');
                    isValid.push(true);
                }

            }
        }
    });

    if (isValid.includes(false))
        return false;
    else
        return true;

}

function ValidateListData(element) {
    let isValid = [];

    $('#' + element).each(function (index, item) {

        $(this).find('input , select , textarea').each(function (index, data) {

            if (!($(this).hasClass("NotReq"))) {
                if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '') {
                    if ($(this).val() == null || $(this).val() == '' || $(this).val() == undefined) {
                        $(this).addClass('is-invalid');
                        isValid.push(false);
                    }
                    else {
                        $(this).removeClass('is-invalid');
                        isValid.push(true);
                    }
                }
            }
        });

    });


    if (isValid.includes(false))
        return false;
    else
        return true;
}


function getJsonObj(element) {

    var jsonobj = new Object();



    $('#' + element + ' input,' + '#' + element + ' select, ' + '#' + element + ' textarea').each(function (index, data) {

        if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '')
            jsonobj[$(this).attr("id")] = $(this).val();

    });
    return jsonobj;
}

function getJsonObjList(element) {
    let listObj = [];

    $('#' + element).each(function (index, item) {
        var jsonobj = new Object();
        $(this).find('input , select , textarea').each(function (index, data) {

            if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '')
                jsonobj[$(this).attr("id")] = $(this).val();

        });

        listObj.push(jsonobj);



    })
    return listObj;
}
