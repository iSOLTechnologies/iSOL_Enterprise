
$(document).ready(function () {

    $('#kt_aside_toggler').click(function () {
        if ($('#kt_aside_toggler').hasClass('kt-aside__brand-aside-toggler--active')) {
            $('.searchBarShaheer').css('display', 'none')
        } else {
            $('.searchBarShaheer').css('display', 'block')
        }
    })

    activateNavigationPages()

    function activateNavigationPages() {
        debugger
       
        let element = $('#' + pageId);
     //   element = document.getElementById(pageId); 
        let elementId = $('#' + pageId).attr('id')


        element.parents('li').parents('li').addClass('kt-menu__item--open')
        element.parents('li').addClass('kt-menu__item--open')

        if (pageId == elementId) {
            element.css('background-color', '#f8f8fb')
            element.find('span').css('color', '#5d78ff')
        }
        debugger
        let module = $('#' + pageId).parents('li').parents('li').children('a').children('span').html()
        let submodule = $('#' + pageId).parents('li').children('a').children('span').html()
        let page = $('#' + pageId).children('a').children('span').html()

        var BreadCrum = '<h3 class="kt-subheader__title">';
        BreadCrum += submodule;
        BreadCrum += '<span class="kt-subheader__separator kt-subheader__separator--v"></span>';
        BreadCrum += '<div class="kt-subheader__group" id="kt_subheader_search">';
        BreadCrum += '<span class="kt-subheader__desc" id="kt_subheader_total">';
        BreadCrum += page;
        BreadCrum += '</span>';
        BreadCrum += '</div>';

        $('.kt-subheader__main').html(BreadCrum);
    }

})

//Loader
var myLoader;

function Loader() {
    myLoader = setTimeout(showPage, 5000);
}

function showPage() {
    document.getElementById("loader").style.display = "none";
    document.getElementById("myDiv").style.display = "block";
}

//for Short Text...
function ShortTxt(field) {
    var transportername;
    if (field.length > 10) {
        //transportername = result.data[i].transportername.slice(0, -10) + '...';
        transportername = field.substring(0, 10) + '...'
    } else {
        transportername = field;
    }
    return transportername;
}


function GetNavId(id, ParentNav) {

    sessionStorage.setItem('act', id);
    sessionStorage.setItem('ParentNav', ParentNav);

}


function Active() {


    var ac = sessionStorage.getItem('act');
    var ParentNav = sessionStorage.getItem('ParentNav');
    document.getElementById(ac).classList.add('kt-menu__item--open');
    document.getElementById(ParentNav).classList.add('kt-menu__item--open');
}

function NotifHtml(uid, msg, date, Caseclass, iconClr) {

    var html = '<a id="UpdateNotifStatus" class="' + Caseclass + '" data-uid="' + uid + '">';
    html += '   <div class="kt-notification__item-icon">';
    html += '     <i class="flaticon2-line-chart kt-font-' + iconClr + '"></i>';
    html += '   </div>';
    html += '   <div class="kt-notification__item-details">';
    html += '       <div class="kt-notification__item-title">' + msg + '</div>';
    html += '       <div class="kt-notification__item-time">' + date + ' </div>';
    html += '   </div>';
    html += '</a>';

    return html;
}


function UpdateNotifStatus(id) {
    $.ajax({
        url: "/Home/UpdateNotifStatus",
        dataType: 'json',
        type: "POST",
        data: { id: id },
        success: function (result) {

            if (result.isSuccess) {

                window.location.href = "/_Driver/index?Data=" + id;
            }
            else {
                toastr.error(result.msg);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}

$(document).ready(function myfunction() {


    //var hub = $.connection.myHub;

    // Declare a function on the hub hub so the server can invoke it
    //hub.client.displayStatus = function () {

    //    getData();
    //};


    // Start the connection
    //$.connection.hub.start();
    //getData();

    function getData() {
        $.ajax({
            url: '/Home/GetNotification',
            type: 'GET',
            datatype: 'json',
            //data: { id: id },
            success: function (data) {

                data = $.parseJSON(data);
                if (data.length > 0) {
                    var count;
                    $('#Notif').empty();
                    console.log(data);
                    for (var i = 0; i < 10; i++) {

                        //document.getElementById('Notif').innerHTML += NotifHtml(data[i].Uid, '/_Driver/DetailView?driverno=' + data[i].Uid, data[i].Message, data[i].CreatedOn);

                        if (data[i].IsRead == false) {
                            document.getElementById('Notif').innerHTML += NotifHtml(data[i].Uid, data[i].Message, data[i].CreatedOn, 'kt-notification__item to', 'success');
                        }
                        if (data[i].IsRead == true) {
                            count = i + 1;
                            document.getElementById('Notif').innerHTML += NotifHtml(data[i].Uid, data[i].Message, data[i].CreatedOn + ' (Unread)', 'kt-notification__item to ActiveNotif', 'warning');
                        }
                    }
                    $('#NotifCount').text(count);
                    //    divNotificaion.empty();
                    //    //// set Count
                    //    //$('#ntCnt').text(data.length);

                    //    /////Make notification html
                    //    //for (var i = 0; i < data.length; i++) {
                    //    //    var notification = '<div class="alert alert-danger alert-dismissable">' +
                    //    //        '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
                    //    //        + data[i].Message +
                    //    //        '</div>'
                    //    //    divNotificaion.append(notification);
                    //    //}

                }
            }, error: function (r, e, w) {

            }
        });
    }


    $(document).on('click', '#UpdateNotifStatus', function (e) {
        var SelectedNotif = $(this).data("uid");
        console.log(SelectedNotif);
        UpdateNotifStatus(SelectedNotif);
    });



    $('.accc').addClass('kt-menu__item--open');

    if (sessionStorage.getItem("act") != '' && sessionStorage.getItem("act") != null) {
        Active();
    }

    var val = 0;
    $('.ArrrowCssi').click(function () {
        SearchToggle();
    });
    $('.ArrrowCssa').click(function () {
        SearchToggle();
    });

    function SearchToggle() {
        if (val == 0) {

            $('.ArrrowCssi').removeClass('fas fa-angle-down');
            $('.ArrrowCssi').addClass('fas fa-angle-up');
            val = 1;
        }
        else {
            $('.ArrrowCssi').removeClass('fas fa-angle-up');
            $('.ArrrowCssi').addClass('fas fa-angle-down');
            val = 0;
        }
    }

    $("#tbSearchNav").on("keyup", function () {
        debugger
        var value = $(this).val().toLowerCase();
        $(".kt-menu__item").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            //console.clear()
            //console.log(this)
            $(this).addClass('kt-menu__item--open');
            if ($("#tbSearchNav").val() == "" || $("#tbSearchNav").val() == null) {
                $(this).removeClass('kt-menu__item--open');

            }
        });
    });
    $('#closeButtonSearch_Shaheer').click(function () {
        $("#tbSearchNav").val('')
        var value = $(this).val().toLowerCase();
        $(".kt-menu__item").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)

            $(this).addClass('kt-menu__item--open');
            if ($("#tbSearchNav").val() == "" || $("#tbSearchNav").val() == null) {
                $(this).removeClass('kt-menu__item--open');

            }
        });
    })

});

toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

//$('.2ndtimepicker').timepicker({
//    minuteStep: 1,
//    showSeconds: true,
//    showMeridian: false,
//    snapToStep: true
//});
$('.2ndtimepicker').timepicker();

$('#next').click(function () {
    var $next = $('.progress ul li.current').removeClass('current').addClass('complete').next('li');
    if ($next.length) {
        $next.removeClass('complete').addClass('current');
        //console.log('Prev');
    } else {
        $(".progress ul li:first").removeClass('complete').addClass('current');
        if (".progress ul li:last") {
            $('.progress ul li').removeClass('current').removeClass('complete').removeAttr('class');
            $(".progress ul li:first").addClass('current');
        }
        //console.log('Next');
    }
});
$('#prev').click(function () {
    var $prev = $('.progress ul li.current').removeClass('current').removeClass('complete').removeAttr('class').prev('li');
    if ($prev.length) {
        $prev.removeClass('complete').addClass('current');
        //console.log('Prev');
    } else {
        $(".progress ul li:first").removeClass('complete').addClass('current');
        $(".progress ul li:last").removeClass('current').removeClass('complete').removeAttr('class');
        //console.log('Prev');
    }
});