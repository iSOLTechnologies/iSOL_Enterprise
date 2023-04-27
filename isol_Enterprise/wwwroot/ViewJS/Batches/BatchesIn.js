var AddedBatches = [];
var ObjBatchCode;
var ObjDistNum;
var linenum = 0;
$(document).on('click', '#ListbtnSelectBatch', function () {
    //debugger
    let selectedRow = $(this).closest('tr');

    let selectedWareHouse = selectedRow.find("#Warehouse").val();
    let selectedItemCode = selectedRow.find("#ItemCode").val();
    let QTY = selectedRow.find("#QTY").val();
    let OldQty = selectedRow.find("#OldQty");
    let btnClickWork = false;
 
    ObjBatchCode = $(this).closest('tr').find("#ListbtnSelectBatch");
  

    if ((selectedWareHouse == null || selectedWareHouse == "" || selectedWareHouse == undefined) || (selectedItemCode == null || selectedItemCode == "" || selectedItemCode == undefined)) {
        toastr.error("Select Item & WareHouse");
    }
    else
    {



        if (OldQty.val() != "" && OldQty.val() != undefined && OldQty.val() != null && QTY != "" && QTY != undefined && QTY != null) {

           
            if (parseFloat(QTY) != parseFloat(OldQty.val()))
            {
                btnClickWork = true;
            }

        }
        else if (QTY != "" && QTY != undefined && QTY != null) {
            btnClickWork = true;
        }
        else {
            toastr.warning("Quantity can't be 0");
            btnClickWork = false;
        }

        

    }
    if (btnClickWork) {
        linenum = $(ObjBatchCode).closest('tr').index();       
        if (AddedBatches.length > 0) {
            ClearAddedBatches(selectedItemCode, selectedWareHouse);

        }
        $(ObjBatchCode).closest('#ListParameters .itm').find('#ListbtnSelectBatch span i').removeClass("batch_is_valid");

        AddDataInRowsFromDocumentTable(selectedRow);
        $('#myBatchAddmodal').modal('show');
    }


});

$(document).on('click', "#btnSearchBatch", function () {



    let selectedWareHouse = $("#adWarehouse").val();
    let selectedItemCode = $("#adItemno").val();
    ObjDistNum = $(this).closest('tr').find("#DistNumber");
    if ((selectedWareHouse == null || selectedWareHouse == "" || selectedWareHouse == undefined) || (selectedItemCode == null || selectedItemCode == "" || selectedItemCode == undefined)) {
        toastr.error("Select Item & WareHouse");
    }
    else {
        generatebatchSearchTable(selectedItemCode, selectedWareHouse);
        $('#myBatchSearchmodal').modal('show');

    }
});

function generatebatchSearchTable(itemno, whsno) {
    CopydataTable = $('#myBatchSearchtable').DataTable({
        responsive: true,
        searching: true,
        ordering: false,
        ordering: false,
        "pageLength": 5,
        "ajax": {
            url: 'GoodReceipt/GetBatches',
            data: { itemno: itemno, whsno: whsno }
        },
        lengthMenu: [[5, 7], [5, 7]],
        layout: {
            scroll: false,
            footer: false,
        },
        // column sorting
        sortable: true,
        pagination: true,
        destroy: true,
        //   async: false,
        search: {
            input: $('#generalSearch'),
        },
        columns: [

            {
                data: 'sno', title: '', render: function (data, type, row) {
                    return data;
                }
            },
            {
                data: 'distNumber', title: 'Batch Number', render: function (data, type, row) {
                    return data;
                }
            },
        ],
    });
    FilterCopydataTable();
}

$(document).on('dblclick', '#myBatchSearchtable tbody tr', function (e) {
    //debugger
    var LookTable = $('#myBatchSearchtable').DataTable();

    var selectedRow = $(this).closest('tr');
    var rowData = LookTable.row(selectedRow).data();
    console.log(rowData);
    $('#myBatchSearchtable tr').removeClass("Mainrow_selected");
    $(selectedRow).addClass('Mainrow_selected');


    $(ObjDistNum).closest('#ListParameters6 .itm').find("#DistNumber").val(rowData['distNumber']);

    $('#myBatchSearchmodal').modal('hide');

});

function ClearAddedBatches(selectedItemCode, selectedWareHouse) {

    $.each(AddedBatches, function (index, item) {

        $.each(item, function (index, item) {
            
            if (this.itemno == selectedItemCode && this.whseno == selectedWareHouse && this.linenum == linenum) {
                
                AddedBatches.splice(index, 1);
            }
        })
    });
}

function UpdateTotalNeeded() {

    let totalAdded = 0;
    let Quantity = Number($("#adQuantity").val());
    let i = 0;
    $('#ListParameters6 .itm').each(function (index, item) {

        $(this).find('#BQuantity').each(function (index, data) {
            totalAdded = totalAdded + Number($(this).val());
        });
        i += 1;
    });
    $("#adTotalNeeded").val(Quantity - totalAdded);
    $("#adTotalSelected").val(totalAdded);
    $("#adTotalBacthes").val((i));

}

function AddDataInRowsFromDocumentTable(selectedRow) {
    $("#adItemno").val(selectedRow.find("#ItemCode").val());
    $("#adItemDesc").val(selectedRow.find("#ItemName").val());
    $("#adWarehouse").val(selectedRow.find("#Warehouse").val());
    $("#adQuantity").val(selectedRow.find("#QTY").val());
    $("#adTotalNeeded").val(selectedRow.find("#QTY").val());
    $("#adTotalSelected").val(0);
    $("#adTotalBacthes").val(0);
    $("#adDirection").val('In');
}

$(document).on('keyup', '#BQuantity', function (e) {

    if ($("#adQuantity").val() != $("#adTotalNeeded").val()) {
        let qty = $(this).val();
        if (qty.length > 0) {
            qty = qty.substring(0, (qty.length - 1));
            console.log("qty", qty);
            $("#adTotalNeeded").val(Number(qty) + Number($("#adTotalNeeded").val()));
        }

    }
    if (Number($(this).val()) > Number($("#adTotalNeeded").val())) {
        toastr.warning("You can't add more than needed quanity !");
        $(this).val(0);
        UpdateTotalNeeded();
    }
    else {
        UpdateTotalNeeded();

        $(this).closest('tr').find("#ExpDate").val(new Date().toJSON().slice(0, 10));
    }
});

$(document).on('click', '#btnAddbatchTableOk', function (e) {
    let listObj = [];
    if (Number($("#adTotalNeeded").val()) == 0) {


        $('#ListParameters6 .itm').each(function (index, item) {

            if ($(this).find("#BQuantity").val() != null && $(this).find("#BQuantity").val() != '' && $(this).find("#BQuantity").val() != undefined && $(this).find("#BQuantity").val() != "") {


                var jsonobj = new Object();
                $(this).find('input , select , textarea').each(function (index, data) {

                    if ($(this).attr("id") != undefined || $(this).attr("id") != "" || $(this).attr("id") != '')
                        jsonobj[$(this).attr("id")] = $(this).val();

                });
                jsonobj['itemno'] = $("#adItemno").val();
                jsonobj['whseno'] = $("#adWarehouse").val();
                jsonobj['linenum'] = linenum;               
                listObj.push(jsonobj);
            }


        });
        AddedBatches.push(listObj);

        console.log(AddedBatches);

        $('#myBatchAddmodal').modal('hide');
        const its = document.querySelectorAll('#ListParameters6 .itm');
        for (const i of its) {
            i.remove();

        }
        append_addBatchTablerow();
        $(ObjBatchCode).closest('#ListParameters .itm').find('#ListbtnSelectBatch span i').addClass("batch_is_valid");
        $(ObjBatchCode).closest('#ListParameters .itm').find('#ListbtnSelectBatch span i').removeClass("batch_not_valid");
    }
    else {
        toastr.warning("Added Quantity is less than required quantity!");
        $(ObjBatchCode).closest('#ListParameters .itm').find('#ListbtnSelectBatch span i').removeClass("batch_is_valid");
    }

});

function append_addBatchTablerow() {
    var html = ` <tr class="rowItemDetail align-items-center itm">

                                                                <td>
                                                                                  <div class="input-group">
                                                                                    <input id="DistNumber" class="form-control form-control-sm">
                                                                                       <div id="btnSearchBatch" class="input-group-append">
                                                                                           <span class="input-group-text">
                                                                                               <i class="la la-search"></i>
                                                                                           </span>
                                                                                       </div>
                                                                                   </div>
                                                                               <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                            </td>
                                                                <td>
                                                                 <input id="BQuantity" class="form-control form-control-sm">
                                                               <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                            </td>
                                                            <td>
                                                                 <input id="ExpDate" type="date" class="d form-control form-control-sm" placeholder="Valid until">
                                                                 <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                            </td> 
                                                            
                                                        </tr>`;
    $('#MyyyyBatchAddTable tbody').append(html);
}
$(document).on('click', '#ListBtnDeleteRM', function () {
    let selectedRow = $(this).closest('tr');
    let selectedWareHouse = selectedRow.find("#Warehouse").val();
    let selectedItemCode = selectedRow.find("#ItemCode").val();

    if (!((selectedWareHouse == null || selectedWareHouse == "" || selectedWareHouse == undefined) || (selectedItemCode == null || selectedItemCode == "" || selectedItemCode == undefined))) {

        if (AddedBatches.length > 0) {
            ClearAddedBatches(selectedItemCode, selectedWareHouse);

        }
    }
    $(this).closest('tr').remove();
})