$(document).ready(function () {
    GetTransporters();

});
function GetTransporters() {

    $.ajax({
        url: "/_ActiveDriverLog/getAll",
        type: "GET",
        dataType: 'json',
        success: function (result) {
            console.log(result);
            for (var i = 0; i < result.data.length; i++) {
                //if (i > 4) {
                    document.getElementById('Main').innerHTML += DivHtml(result.data[i].useridimage, result.data[i].username, result.data[i].email, result.data[i].employeeno, result.data[i].fk_retypemst_reftype, result.data[i].rowuid);
                //}
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}

function DivHtml(image, DriverNo, DriverName, para, Vehicle, Phone, CurrentLocation, cocode, link) {
    var html = '<div class="col-xl-3 Card1">';
    html += '<div class="kt-portlet kt-portlet--height-fluid">';
    html += '    <div class="kt-portlet__head kt-portlet__head--noborder">';
    html += '        <div class="kt-portlet__head-label">';
    html += '            <h3 class="kt-portlet__head-title"></h3> </div>';
    html += '    </div>';
    html += '    <div class="kt-portlet__body">';
    html += '        <div class="kt-widget kt-widget--user-profile-2">';
    html += '            <div class="kt-widget__head">';
    html += '                <div class="kt-widget__media"> <img class="kt-widget__img kt-hidden-" src="' + image + '" alt="image"> </div>';
    html += '                <div class="kt-widget__info"> <a href="#" id="TransporterName" class="kt-widget__username coName0"> ' + DriverName + ' </a> <span class="kt-widget__desc coName0">' + DriverNo + ' </span> </div>';
    html += '            </div>';
    html += '            <div class="kt-widget__body">';
    html += '                <div id="para" class="kt-widget__section coName0"> ' + para + ' </div>';
    html += '                <div class="kt-widget__item">';
    html += '                    <div class="kt-widget__contact"> <span class="kt-widget__label">Vehicle:</span> <a id="Vehicle" href="#" class="kt-widget__data coName0">' + Vehicle + '</a> </div>';
    html += '                    <div class="kt-widget__contact"> <span class="kt-widget__label">Phone:</span>';
    html += '                        <a id="Phone" href="#" class="kt-widget__data" coName0>' + Phone + ' </a>';
    html += '                    </div>';
    html += '                    <div class="kt-widget__contact"> <span class="kt-widget__label">Location:</span> <span id="Location" class="kt-widget__data coName0">' + CurrentLocation + '</span> </div>';
    html += '                </div>';
    html += '            </div>';
    html += '            <div class="kt-widget__footer">';
    html += '                  <a href="' + link + '" class="btn btn-label-danger btn-lg btn-upper">Detail</a>';
    //html += '                <button id="BtnDetail" type="button" class="btn btn-label-danger btn-lg btn-upper"></button>';
    html += '            </div>';
    html += '        </div>';
    html += '    </div>';
    html += '</div>';
    html += '</div>';

    return html;
}

