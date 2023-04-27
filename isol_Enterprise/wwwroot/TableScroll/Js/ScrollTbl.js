$('#tblItemDetails').on('scroll', function () {
    $("#tblItemDetails > *").width($(this).width() + $(this).scrollLeft());
});