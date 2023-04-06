$(document).on('click', '#ListbtnSelectBatch', function () {
    //debugger
    let selectedRow = $(this).closest('tr');

    let selectedWareHouse = selectedRow.find("#Warehouse").val();
    let selectedItemCode = selectedRow.find("#ItemCode").val();

    ObjBatchCode = $(this).closest('tr').find("#ListbtnSelectBatch");
    $(ObjBatchCode).closest('#ListParameters .itm').find('#ListbtnSelectBatch span i').removeClass("batch_is_valid");

    if ((selectedWareHouse == null || selectedWareHouse == "" || selectedWareHouse == undefined) || (selectedItemCode == null || selectedItemCode == "" || selectedItemCode == undefined)) {
        toastr.error("Select Item & WareHouse");
    }
    else {

        if (SelectedBatches.length > 0) {
            ClearSelectedBatches(selectedItemCode, selectedWareHouse);

        }

        BatchSelectUrl = 'Delivery/GetBatchList';
        AddDataInRowsFromDocumentTable(selectedRow);
        $('#myBatchSelectmodal').modal('show');

        generateBatchSelectTable(selectedItemCode, selectedWareHouse);
    }
});

function ClearSelectedBatches(selectedItemCode, selectedWareHouse) {
    $.each(SelectedBatches, function (index, item) {

        $.each(item, function (index, item) {

            if (this.itemno == selectedItemCode && this.whseno == selectedWareHouse) {
                SelectedBatches.splice(index, 1);
            }
        })
    });
}

function AddDataInRowsFromDocumentTable(selectedRow) {
    $("#slItemno").val(selectedRow.find("#ItemCode").val());
    $("#slItemDesc").val(selectedRow.find("#ItemName").val());
    $("#slWarehouse").val(selectedRow.find("#Warehouse").val());
    $("#slQuantity").val(selectedRow.find("#QTY").val());
    $("#slTotalNeeded").val(selectedRow.find("#QTY").val());
    $("#slTotalSelected").val(0);
    $("#slTotalBacthes").val(0);
    $("#slDirection").val('Out');
}

$(document).on('click', '#ListBtnDeleteRM', function () {
    let selectedRow = $(this).closest('tr');
    let selectedWareHouse = selectedRow.find("#Warehouse").val();
    let selectedItemCode = selectedRow.find("#ItemCode").val();

    if (!((selectedWareHouse == null || selectedWareHouse == "" || selectedWareHouse == undefined) || (selectedItemCode == null || selectedItemCode == "" || selectedItemCode == undefined))) {

        if (SelectedBatches.length > 0) {
            ClearSelectedBatches(selectedItemCode, selectedWareHouse);

        }
    }
    $(this).closest('tr').remove();
})