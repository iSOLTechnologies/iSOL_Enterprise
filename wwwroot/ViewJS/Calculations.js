

function GetPrice() {

    $("#Total").val(0);
    var totalBeforeDiscount = 0;
    var RoundingValue = 0;
    var Discount = parseFloat($("#Discount").val() / 100);
    $('#ListParameters .itm').each(function (index, item) {
        var dt1 = $(this).closest('#ListParameters .itm').find("#TtlPrc");
        if (dt1.val() != null)
            totalBeforeDiscount = Number(totalBeforeDiscount) + Number(dt1.val());
    })
    if (totalBeforeDiscount != 0)
        $("#TotalBeforeDiscount").val(totalBeforeDiscount);

    if ($('#RoundingChkBox').is(":checked")) {


        RoundingValue = parseFloat($('#Rounding').val());

    }
    if (Discount != "" && RoundingValue != "") {

        $("#Total").val(totalBeforeDiscount - ((totalBeforeDiscount + RoundingValue) * Discount));

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

}

$(document).on('keyup', '#QTY,#UPrc,#DicPrc,#Rounding,#Discount', function (e) {

    var Qty = $(this).closest('#ListParameters .itm').find('#QTY');
    var UPrc = $(this).closest('#ListParameters .itm').find('#UPrc');
    var DicPrc = $(this).closest('#ListParameters .itm').find('#DicPrc');
    var TtlPrc = $(this).closest('#ListParameters .itm').find('#TtlPrc');
    var VatGroup = $(this).closest('#ListParameters .itm').find('#VatGroup');


    if ($(DicPrc).val() == "") {
        var Onepercent = parseFloat($(TtlPrc).val() / 100);
        var Tax = parseFloat(Onepercent * $(VatGroup).val())
        if (($(QTY).val() != null && $(UPrc).val() != null) || ($(QTY).val() != "" && $(UPrc).val() != "")) {
            $(TtlPrc).val((((parseFloat($(Qty).val()) * parseFloat($(UPrc).val())) + parseFloat(Tax)).toFixed(2)));
            GetPrice();
            //document.getElementById("TtlPrc").value = parseFloat(TtlPrc).toFixed(2);
            //console.log(TtlPrc)
        }
    }

    if (DicPrc != "") {
        $(TtlPrc).val((parseFloat($(Qty).val()) * parseFloat($(UPrc).val())).toFixed(2));
        var Onepercent = parseFloat($(TtlPrc).val() / 100);
        var discountPrice = parseFloat(Onepercent * $(DicPrc).val());
        var Tax = parseFloat(Onepercent * $(VatGroup).val())
        console.log("discountPrice:", discountPrice)
        if (($(QTY).val() != null && $(UPrc).val() != null) || ($(QTY).val() != "" && $(UPrc).val() != "")) {
            $(TtlPrc).val((((parseFloat($(Qty).val()) * parseFloat($(UPrc).val())) - (parseFloat(discountPrice))) + (Tax)).toFixed(2));
            GetPrice();
            //document.getElementById("TtlPrc").value = parseFloat(TtlPrc).toFixed(2);
            //console.log(Tax)
        }
    }

});

$(document).on('click', '#RoundingChkBox', function (e) {

    if ($(this).is(":checked")) {
        $('#Rounding').removeAttr('readonly', 'readonly');
    }
    else {
        $('#Rounding').attr('readonly', 'readonly');
    }
});
