$(document).on("keydown", "input", function () {
    $(this).removeClass('is-invalid');

    let MaxChar = $(this).attr("char");
    console.log(MaxChar);

    if (MaxChar != "" && MaxChar != undefined && MaxChar != null) {

        if (this.value.length > MaxChar - 1) {
            this.value = this.value.slice(0, MaxChar - 1);
            toastr.warning("Character number is greater then Allowed");
        }
    }

});
function ValidateData(element) {

    let isValid = [];

    $('#' + element + ' input,' + '#' + element + ' select, ' + '#' + element + ' textarea').each(function (index, data) {

        if (!($(this).hasClass("NotReq"))) {

                if ($(this).val() == null || $(this).val() == '' || $(this).val() == undefined) {
                    $(this).addClass('is-invalid');
                    isValid.push(false);
                }
                else {
                    $(this).removeClass('is-invalid');
                    isValid.push(true);
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
                
                    if ($(this).val() == null || $(this).val() == '' || $(this).val() == undefined) {
                        $(this).addClass('is-invalid');
                        isValid.push(false);
                    }
                    else {
                        $(this).removeClass('is-invalid');
                        isValid.push(true);
                    }
                
            }
        });

    });


    if (isValid.includes(false))
        return false;
    else
        return true;
}

function ValidateBatches(element) {
   
    let isValid = [];
    let triggerBatch = false;
    $('#' + element).each(function (index, item) {



        if (!($(this).find('#ListbtnSelectBatch').hasClass("d-none"))) {

            let itemno = $(this).find('#ItemCode');
            let isbatchSelected = $(this).find('#ListbtnSelectBatch span i').hasClass("batch_is_valid");

            if (isbatchSelected) {

                isValid.push(true);
                $(this).find('#ListbtnSelectBatch span i').removeClass("batch_not_valid");
            }
            else {
                isValid.push(false);
                $(this).find('#ListbtnSelectBatch span i').addClass("batch_not_valid");
                if (!triggerBatch) {
                    $(this).find('#ListbtnSelectBatch').trigger('click');
                    triggerBatch = true;
                }
            }
        }


    });
    if (isValid.includes(false))
        return false;
    else
        return true;
}


function getJsonObj(element) {

    var jsonobj = new Object();



    $('#' + element + ' input,' + '#' + element + ' select, ' + '#' + element + ' textarea').each(function (index, data) {

        if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '') {
            if ($(this).is(':checkbox') || $(this).is(':radio'))
                jsonobj[$(this).attr("id")] = $(this).is(':checked') == true ? 'Y' : 'N';
            else
                jsonobj[$(this).attr("id")] = $(this).val();
        }
    });
    return jsonobj;
}
function getJsonObjKeyName(element) {

    var jsonobj = new Object();



    $('#' + element + ' input,' + '#' + element + ' select, ' + '#' + element + ' textarea').each(function (index, data) {

        if ($(this).attr("name") != undefined || $(this).attr("name") != "" || $(this).attr("name") != '')
        {
            
            if ($(this).is(':checkbox'))
                jsonobj[$(this).attr("name")] = $(this).is(':checked') == true ? 'Y' : 'N';
            else if ($(this).is(':radio'))
                jsonobj[$(this).attr("name")] = $('input[name="' + $(this).attr("name") +'"]:checked').val();
            else
            jsonobj[$(this).attr("name")] = $(this).val();
        }
        else if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '')
            jsonobj[$(this).attr("id")] = $(this).val();
    });

    return jsonobj;
}

function getJsonObjList(element) {
    let listObj = [];
    console.log(element);

    $('#' + element).each(function () {

        
        var jsonobj = new Object();
        $(this).find('input , select , textarea').each(function (index, data) {

            console.log($(this).val());
            if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '') { 

                if ($(this).is(':checkbox') || $(this).is(':radio'))
                    jsonobj[$(this).attr("id")] = $(this).is(':checked') == true ? 'Y' : 'N';
                else
                    jsonobj[$(this).attr("id")] = $(this).val();
            }

        });

        listObj.push(jsonobj);



    });
    return listObj;
}