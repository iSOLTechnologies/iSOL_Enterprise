$(document).on('click', "#ListBtnAddWarehouse", function () {

    let html = `<tr class="rowItemDetail align-items-center itm">

                                                                                            <td>
                                                                                                <div class="input-group">
                                                                                                    <input id="WhsCode" class="form-control form-control-sm" readonly>
                                                                                                        <div id="WhsCodeSearch" class="input-group-append">
                                                                                                            <span class="input-group-text">
                                                                                                                <i class="la la-search"></i>
                                                                                                            </span>
                                                                                                        </div>
                                                                                                </div>
                                                                                                <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                                            </td>
                                                                                            <td>

                                                                                                <input type="text" readonly id="WhsName" class="form-control form-control-sm" placeholder="Warehouse Name" aria-describedby="basic-addon2">
                                                                                                <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                                            </td>
                                                                                            <td>
                                                                                                    <label class="checkbox checkbox-success justify-content-center">
                                                                                                    <input type="checkbox" id="Locked">
                                                                                                        <span class="border_grey"></span>&nbsp;
                                                                                                </label>
                                                                                                <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                                            </td>
                                                                                            <td>
                                                                                                <input type="number" id="MinStock" class="form-control form-control-sm" placeholder="0.0000" aria-describedby="basic-addon2">

                                                                                                <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                                            </td>
                                                                                            <td>
                                                                                                 <input type="number" id="MaxStock" class="form-control form-control-sm" placeholder="0.0000" aria-describedby="basic-addon2">
                                                                                                <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                                            </td>
                                                                                            <td>
                                                                                                <input type="number" id="MinOrder" class="form-control form-control-sm" placeholder="0.0000" aria-describedby="basic-addon2">
                                                                                                <div id="ErrorMsg" class="error invalid-feedback">This field is required.</div>
                                                                                            </td>
                                                                                            <td>
                                                                                                <a id = "ListBtnDelete" class="btn btn-bold btn-sm btn-light-danger">
                                                                                                    <i class="la la-trash"></i> X
                                                                                                </a>
                                                                                            </td>
                                                                                        </tr>`;
    $('#Warehouse_Table tbody').append(html);
});


$(document).on('click', "#WhsCodeSearch", function () {

    ObjWarehouseCode = $(this).closest('tr').find("#WhsCode");
    generateWareHouseListTable();
    $('#myWarehouseModal').modal('show');
});

function generateWareHouseListTable() {

    datatable = $('#MyWareHouseListTable').DataTable({
        responsive: true,
        searching: true,
        ordering: false,
        ordering: false,
        "ajax": {
            url: '/ItemMasterData/GetWareHouseList',

        },
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
        columns: [{
            data: 'whscode',
            title: 'Warehouse Code',
        }, {
            data: 'whsname',
            title: 'Warehouse Name'
        }

        ],
    });
}

$(document).on('dblclick', '#MyWareHouseListTable tbody tr', function (e) {
    //debugger
    var LookTable = $('#MyWareHouseListTable').DataTable();;
    var selectedRow = $(this).closest('tr');
    var rowData = LookTable.row(selectedRow).data();

    var isWhsChkd = WarehouseAlreadyChecked(rowData['whscode']);
    console.log(isWhsChkd);
    if (isWhsChkd)
    {
        toastr.warning("The selected warehouse already exits");
        
    }
    else
    {
        $(ObjWarehouseCode).closest('#ListParameters .itm').find("#WhsCode").val(rowData['whscode']).trigger('change');
        $(ObjWarehouseCode).closest('#ListParameters .itm').find("#WhsName").val(rowData['whsname']).trigger('change');
    }
    $('#myWarehouseModal').modal('hide');


});
function WarehouseAlreadyChecked(WhsCode) {

    let _WhsCode;
    let isValid = [];
    console.log(WhsCode);
    $('#ListParameters .itm').each(function () {

        _WhsCode = $(this).find('#WhsCode').val();

        if (WhsCode == _WhsCode) {
            console.log("same");
            isValid.push(true);
        }
        else {
            isValid.push(false);
        }
    });
    if (isValid.includes(true))
        return true;
    else
        return false;
}