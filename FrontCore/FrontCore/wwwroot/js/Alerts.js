window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operación Correcta", { timeOut: 10000 });
    }
    if (type === "error") {
        toastr.error(message, "Operación Fallida", { timeOut: 10000 });
    }
}

function MostrarModalConfirmacionBorrado() {
    $('#modalConfirmacionBorrado').modal('show');
}

function OcultarModalConfirmacionBorrado() {
    $('#modalConfirmacionBorrado').modal('hide');
}

