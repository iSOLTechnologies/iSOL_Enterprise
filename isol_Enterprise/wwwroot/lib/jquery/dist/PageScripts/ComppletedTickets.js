$(document).ready(function () {
    this.table = null;
    var self = this;
    var RowDataPrint = null; 
    var rowdata;
   // GetAll();
    var Guid = $('#PageGuid').val();

    AllListViews(Guid);

    $(document).on('click', '#T_' + Guid + ' tr', function (e) {
        rowdata = null;
        RowDataPrint = null;
        debugger

        var selectedRow = $(this).closest('tr');
        var rowData = LookTable.row(selectedRow).data();
        var SelectedVal = rowData['guid'];
        //$('#' + Guid + ' tr').removeClass('Mainrow_selected');
        var asd = '#T_' + Guid + ' tr';
        $('#T_' + Guid + ' tr').removeClass("Mainrow_selected");
        $(selectedRow).addClass('Mainrow_selected');
        $('.OperationBtns').attr("data-id", SelectedVal);

        RowDataPrint = rowData;
    });
   

});

$(document).on('click', '.OperationBtns', function (e) {
    debugger

    var data = $(this).attr('data', 'Operation');
    var Operation = data.context.dataset.operation;

    //if (!CheckGlobalRoles('is' + Operation)) { return; }

    $(this).removeAttr('data', 'Operation');
    debugger

    var rowData = data.context.dataset;
    //var SelectedRow = $(this).data();
    if (rowData.id == null || rowData.id == '' || rowData.id == undefined) {
        if (Operation != "Create") { toastr.info("Please Select Any Row For " + data.context.innerText); return; }
    }
    /// if (Operation != "Print") { $('._tabNewBtn > a').text(data.context.innerText); UnBindClick('._tabNewBtn', false); }

    if (Operation == "Create") {

        window.location.href = '/TicketingSystem/AddTicket?Source=' + $('#PageGuid').val();


    }

    if (Operation == "Edit") {

        window.location.href = '/TicketingSystem/AddTicket?Source=' + $('#PageGuid').val() + '&Id=' + rowData.id;



    }

    if (Operation == "Delete") {

        rowdata = rowData.id;
        $("#con-delete-IN").modal('show');
        $("#con-delete-IN").modal('show');
    }
});

function SetForm(Activity) {
    SetFields($('#PageId').val(), Activity);
};
