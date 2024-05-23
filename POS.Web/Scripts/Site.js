function startLoader() {
    //start loader
    $('body').loadingModal({
        position: 'auto',
        text: '',
        color: '#fff',
        opacity: '0.7',
        backgroundColor: 'rgb(0,0,0)',
        animation: 'doubleBounce'
    });
}

function stopLoader() {
    //stop loader
    $('body').loadingModal('hide');
    $('body').loadingModal('destroy');
}

$(function () {

    $("#btn-add,#btn-addSecond").click(function (e) {
        e.preventDefault();
        var URL = $(this).attr('href');
        var caption = $(this).data('caption');
        startLoader();

        $.ajax({
            url: URL,
            type: "GET",
        }).done(function (result) {
            $("#customModal-title").html(caption);
            $("#customModal-body").html(result);
            $("#customModal").modal({
                backdrop: 'static',
                keyboard: false,
                autoOpen: false,
                draggable: false,
                resizable: false,
            });
            stopLoader();

            // Summernote
            //$('.textarea').summernote();
            //$('.dropify').dropify();

            $('select').selectpicker();
        });
    });

    $("#bootstrap-data-table-export").on("click", ".btn-view , .btn-edit", function (e) {
        e.preventDefault();
        var id = $(this).parents(".dgrow").data("id");
        var URL = $(this).attr('href');
        var caption = $(this).data('caption');

        startLoader();
        $.ajax({
            url: URL + id,
            type: "GET",
        }).done(function (result) {
            $("#customModal-title").html(caption);
            $("#customModal-body").html(result);
            $("#customModal").modal({
                backdrop: 'static',
                keyboard: false,
                autoOpen: false,
                draggable: false,
                resizable: false,
            });

            stopLoader();

            $('select').selectpicker();
        });
    });


    $("#bootstrap-data-table-export").on("click", ".btn-delete", function (e) {
        e.preventDefault();
        var id = $(this).parents(".dgrow").data("id");
        var URL = $(this).attr('href');
        alertify.confirm("Are you sure you want to remove", function (e, str) {
            if (e) {
                startLoader();
                $.ajax
                    ({
                        url: URL + id,
                        contentType: 'application/json; charset=utf-8',
                        type: "POST",
                        data: JSON.stringify({ id: id }),
                        datatype: "Json",
                    }).done(function (res) {
                        console.log(res);
                        if (res == "Error" || res == "") {
                            alertify.error("Something went wrong");
                        }
                        else if (res == "Success") {
                            //debugger
                            alertify.success("Delete successfully");
                            location.reload();
                        }
                        stopLoader();
                    });
            }
            else {
                // user clicked "cancel"
            }
        }, "Default Value");
    });

    $("#bootstrap-data-table-export").DataTable();

    RefreshCart();
    //// Summernote
    //$('.textarea').summernote();

    //$('.dropify').dropify();

});

function AddItem(itemId, qty, price, itemName, taxAmount, taxPercentage, currentTableId, departmentId) {

    var modelData = {
        ItemId: itemId,
        ItemName: itemName,
        Quantity: qty,
        Price: price,
        TaxAmount: taxAmount,
        TaxPercentage: taxPercentage,
        TableId: currentTableId,
        DepartmentId: departmentId,
    };

    $.ajax
        ({
            url: "/Home/AddItemIntoCart/",
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify(modelData),
            datatype: "Json",
            success: function (t) {
                //console.log(t);
                alertify.success(itemName + " Add Into Cart Successfully");
                RefreshCart();
                $("#ItemRemarksModal-title").html(itemName + " Remarks");
                $("#ItemRemarksModal").modal();

                $("#itemID").val(itemId);
            },
            error: function () {
                $("#errorMessage").text(itemName + " Item not found ");
                alertify.error("Something went wrong");
            }
        });
}

function MinusQuantity(id, name, currentTableId) {
    debugger
    startLoader();
    $.ajax
        ({
            url: '/Home/MinusQuantity/',
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify({ id: id, tableId: currentTableId })
        }).done(function (res) {
            console.log(res);
            if (res == "Error" || res == "") {
                alertify.error("Something went wrong");
            }
            else if (res == "Success") {
                alertify.success(name + " Minus Item successfully");
                RefreshCart();
            }

            stopLoader();
        });
}

//function PointQuantity(id, name, currentTableId) {
//    debugger
//    startLoader();
//    $.ajax
//        ({
//            url: '/Home/PointQuantity/',
//            contentType: 'application/json; charset=utf-8',
//            type: "POST",
//            data: JSON.stringify({ id: id, tableId: currentTableId })
//        }).done(function (res) {
//            console.log(res);
//            if (res == "Error" || res == "") {
//                alertify.error("Something went wrong");
//            }
//            else if (res == "Success") {
//                alertify.success(name + " 0.5 Item successfully");
//                RefreshCart();
//            }

//            stopLoader();
//        });
//}

function PointQuantity(id, name, currentTableId) {
    startLoader();
    $.ajax
        ({
            url: '/Home/PlusQuantity/',
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify({ id: id, tableId: currentTableId })
        }).done(function (res) {
            console.log(res);
            if (res == "Error" || res == "") {
                alertify.error("Something went wrong");
            }
            else if (res == "Success") {
                alertify.success(name + " Plus Item successfully");
                RefreshCart();
            }

            stopLoader();
        });
}

function PlusQuantity(id, name, currentTableId) {
    startLoader();
    $.ajax
        ({
            url: '/Home/PlusQuantity/',
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify({ id: id, tableId: currentTableId })
        }).done(function (res) {
            console.log(res);
            if (res == "Error" || res == "") {
                alertify.error("Something went wrong");
            }
            else if (res == "Success") {
                alertify.success(name + " Plus Item successfully");
                RefreshCart();
            }

            stopLoader();
        });
}

function UpdateQuantity(id, qty) {
    startLoader();
    $.ajax
        ({
            url: '/Home/UpdateItemQty/',
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify({ id: id, qty: qty })
        }).done(function (res) {
            console.log(res);
            if (res == "Error" || res == "") {
                alertify.error("Something went wrong");
            }
            else if (res == "Success") {
                alertify.success("Update Quantity successfully");
                RefreshCart();
            }

            stopLoader();
        });
}

function RemoveItem(id, name, currentTableId) {

    startLoader();
    $.ajax
        ({
            url: '/Home/RemoveItemFromCart/' + id,
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify({ id: id, tableId: currentTableId })
        }).done(function (res) {
            console.log(res);
            if (res == "Error" || res == "") {
                alertify.error("Something went wrong");
            }
            else if (res == "Success") {
                alertify.success(name + " Remove Item From Cart ");
                RefreshCart();
            }
            stopLoader();
        });
}

function VoidItem(id, name, currentTableId) {
    alertify.confirm("Are you sure you want to void " + name + " Item", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/VoidItemFromCart/' + id,
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    data: JSON.stringify({ id: id, tableId: currentTableId })
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong");
                    }
                    else if (res == "Success") {
                        alertify.success(name + " Void Item From Cart ");
                        RefreshCart();
                    }
                    stopLoader();
                });

        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");
}

function ComplimentaryItem(id, name, currentTableId) {
    alertify.confirm("Are you sure you want to complimentary " + name + " Item", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/ComplimentaryItemFromCart/' + id,
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    data: JSON.stringify({ id: id, tableId: currentTableId })
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong");
                    }
                    else if (res == "Success") {
                        alertify.success(name + " Complimentary Item From Cart ");
                        RefreshCart();
                    }
                    stopLoader();
                });

        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");


}

function ReleaseVoidItem(id, name, currentTableId) {
    alertify.confirm("Are you sure you want to Undo void " + name + " Item", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/ReleaseVoidItemFromCart/' + id,
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    data: JSON.stringify({ id: id, tableId: currentTableId })
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong");
                    }
                    else if (res == "Success") {
                        alertify.success(name + " Void Item  has been undo ");
                        RefreshCart();
                    }
                    stopLoader();
                });

        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");
}

function ReleaseComplimentaryItem(id, name, currentTableId) {
    alertify.confirm("Are you sure you want to Undo complimentary " + name + " Item", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/ReleaseComplimentaryItemFromCart/' + id,
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    data: JSON.stringify({ id: id, tableId: currentTableId })
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong");
                    }
                    else if (res == "Success") {
                        alertify.success(name + " Complimentary Item has been undo ");
                        RefreshCart();
                    }
                    stopLoader();
                });

        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");


}

/// --------------------------------------------------------------------------------------------------------------
function Order_Void_Or_Complimentary(OrderId, IsVoid, IsComplimentary, IsPayment,IsUpdateMode, Caption, CheckMessage) {
    alertify.confirm(CheckMessage, function (e, str) {
        if (e) {
            debugger
            var modelData = {
                Id: OrderId,
                IsVoid: IsVoid,
                IsComplimentary: IsComplimentary,
                IsPayment: IsPayment,
                IsUpdateMode: IsUpdateMode,
            };

            startLoader();
            $.ajax
                ({
                    url: '/Home/Order_Void_Or_Complimentary/',
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    data: JSON.stringify(modelData)
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong");
                    }
                    else if (res == "Success") {

                        $("#orderReasonModal").modal({
                            backdrop: 'static',
                            keyboard: false,
                            autoOpen: false,
                            draggable: false,
                            resizable: false,
                        });
                        $("#orderID").val(OrderId);

                        //alertify.success(Caption);
                        //location.href ="/Home/AllOrders"
                    }
                    stopLoader();
                });

        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");
}

function RefreshCart() {
    $.ajax(
        {
            url: "/Home/CartItems/",
            type: "Get"
        }
    ).done(function (result) {
        $("#LoadCartItems").html(result);
    });
}

function HoldTable(id, name) {

    alertify.confirm("Are you sure you want to hold this table", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: "/Home/IsHoldTable/" + id,
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    datatype: "Json",
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong");
                    }
                    else if (res == "Success") {
                        //debugger
                        alertify.success("Successfully Hold This Table");
                        window.location.href = "/Home/POS/" + id;
                    }
                    stopLoader();
                });
        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");

}

function CustomModal(URL, caption) {
    startLoader();
    $.ajax({
        url: URL,
        type: "GET",
    }).done(function (result) {
        $("#customModal-title").html(caption);
        $("#customModal-body").html(result);
        $("#customModal").modal({
            backdrop: 'static',
            keyboard: false,
            autoOpen: false,
            draggable: false,
            resizable: false,
        });

        $('select').selectpicker();

        stopLoader();
    });
}

function AddOrder(tip, service, discount, currentTableId) {
    $.ajax
        ({
            url: "/Home/CreateTicket/",
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: JSON.stringify({ Tip: tip, Service: service, Discount: discount, TableId: currentTableId }),
            datatype: "JSON",
            success: function (t) {
                //console.log(t);
                alertify.success("Add Into Order Successfully");
                RefreshCart();
            },
            error: function () {
                alertify.error("Something went wrong");
            }
        });


}

function GetPrint(URL, id) {

    startLoader();
    $.ajax({
        url: URL + id,
        type: "POST",
    }).done(function (res) {
        console.log(res);
        alertify.success("Print Report");
        stopLoader();
    });
}

function GetReportView(URL, id) {
    startLoader();
    $.ajax({
        url: URL + id,
        type: "POST",
    }).done(function (res) {
        console.log(res);
        if (res == "Error" || res == "") {
            alertify.error("Something went wrong");
        }
        else if (res == "Success") {
            alertify.success("Generate Report");
            window.open('/RptViewer/MainViewer.aspx', '_blank');
        }
        stopLoader();
    });
}

function ShopOpen() {
    alertify.confirm("Are you sure you want to open this shop", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/User/OpenShop/',
                    type: "POST",
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong when try to open shop");
                    }
                    else if (res == "Success") {
                        alertify.success("The Shop has been open successfully");
                        location.reload();
                    }
                    stopLoader();
                });
        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");

}

function ShopClose() {
    alertify.confirm("Are you sure you want to close this shop", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/User/CloseShop/',
                    type: "POST",
                }).done(function (res) {
                    console.log(res);
                    if (res == "Error" || res == "") {
                        alertify.error("Something went wrong when try to close shop");
                    }
                    else if (res == "Success") {
                        alertify.success("The Shop has been close successfully");
                        location.href = "/Account/Logout";
                    }
                    stopLoader();
                });
        }
        else {
            // user clicked "cancel"
        }
    }, "Default Value");

}

function GetInvoice(id) {
    debugger
    alertify.confirm("Are you sure you want to print this invoice", function (e, str) {
        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/PrintInvoice/'+ id,
                    type: "GET",
                }).done(function (result) {
                    console.log(result);
                    $("#InvoiceFormat").html(result);
                    //printdiv('InvoiceFormat')
                    printDiv01("InvoiceFormat");
                });
            stopLoader();
        }
        else {
            window.location.href = "/Home/Index";
        }
    }, "Default Value");
}

function GetPrintBill(invoiceNo) {
    debugger;
    alertify.confirm("Are you sure you want to print this invoice", function (e) {

        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Report/PrintBill',
                    type: "POST",
                    data: { invoiceNo: invoiceNo },
                }).done(function (result) {
                    if (result == "Error" || result == "") {
                        alertify.error("Something went wrong with printing");
                    }
                    else if (result == "Success") {
                        alertify.success("CR Print successfully");
                    }
                });
            stopLoader();
        }
        else {

        }
    }, "Default Value");
}


function PrintElem(elem) {
    debugger;
    alert("ok");

    var mywindow = window.open('', 'PRINT', 'height=400,width=600');

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + document.title + '</h1>');
    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}

function printdiv(printpage) {
    var headstr = "<html><head><title></title></head><body>";
    var footstr = "</body>";
    var newstr = document.all.item(printpage).innerHTML;
    var oldstr = document.body.innerHTML;
    document.body.innerHTML = headstr + newstr + footstr;
    window.print();
    document.body.innerHTML = oldstr;
    location.reload();
    return false;
}


function printDiv01(DivId) {
    debugger
    var divContents = document.getElementById(DivId).innerHTML;
    var a = window.open('', 'PRINT', 'height=400, width=600');
    a.document.write('<html>');
    a.document.write('<body >');
    a.document.write(divContents);
    a.document.write('</body></html>');

    a.document.close();
    a.focus();

    a.print();
    //a.close()
    window.location.href = "/Home/Index";
    alertify.success(" Print successfully");
}


function GetPrintToAllDepartments(orderId) {
    debugger;
    alertify.confirm("Are you sure you want to print to all departments", function (e) {

        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/PrintAllOrderDepartments',
                    type: "POST",
                    data: { id: orderId },
                }).done(function (result) {
                    if (result == "Error" || result == "") {
                        alertify.error("Something went wrong with printing");
                    }
                    else if (result == "Success") {
                        alertify.success("All Department Print successfully");
                    }
                });
            stopLoader();
        }
        else {

        }
    }, "Default Value");
}

function GetPrintToKitchenDepartment(orderId) {
    debugger;
    alertify.confirm("Are you sure you want to print to Kitchen Department", function (e) {

        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/PrintKitchenOrder',
                    type: "POST",
                    data: { id: orderId },
                }).done(function (result) {
                    if (result == "Error" || result == "") {
                        alertify.error("Something went wrong with printing");
                    }
                    else if (result == "Success") {
                        alertify.success("Kitchen Print successfully");
                    }
                });
            stopLoader();
        }
        else {

        }
    }, "Default Value");
}

function GetPrintToBarDepartment(orderId) {
    debugger;
    alertify.confirm("Are you sure you want to print to Bar Department", function (e) {

        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/PrintBarOrder',
                    type: "POST",
                    data: { id: orderId },
                }).done(function (result) {
                    if (result == "Error" || result == "") {
                        alertify.error("Something went wrong with printing");
                    }
                    else if (result == "Success") {
                        alertify.success("Kitchen Print successfully");
                    }
                });
            stopLoader();
        }
        else {

        }
    }, "Default Value");
}

function GetPrintToDessertDepartment(orderId) {
    debugger;
    alertify.confirm("Are you sure you want to print to Dessert Department", function (e) {

        if (e) {
            startLoader();
            $.ajax
                ({
                    url: '/Home/PrintDessertOrder',
                    type: "POST",
                    data: { id: orderId },
                }).done(function (result) {
                    if (result == "Error" || result == "") {
                        alertify.error("Something went wrong with printing");
                    }
                    else if (result == "Success") {
                        alertify.success("Kitchen Print successfully");
                    }
                });
            stopLoader();
        }
        else {

        }
    }, "Default Value");
}