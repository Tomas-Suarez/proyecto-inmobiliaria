function mostrarModalConfirmacion(titulo, mensaje, formId) {
    const modalElement = document.getElementById('modalConfirmacion');
    if (!modalElement) return;

    modalElement.querySelector('.modal-title').textContent = titulo;
    modalElement.querySelector('.modal-body p').textContent = mensaje;

    const btnConfirmar = modalElement.querySelector('#btnConfirmarModal');
    btnConfirmar.onclick = function () {
        document.getElementById(formId).submit();
    }

    const modal = new bootstrap.Modal(modalElement);
    modal.show();
}
