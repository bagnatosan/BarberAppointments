"use strict";
//desplegable usuario
const btnAgendar = document.getElementById('btnAgendar');
if (btnAgendar) {
    btnAgendar.addEventListener('click', function () {
        // La animación de pulso
        btnAgendar.classList.remove('pulse');
        void btnAgendar.offsetWidth; // Truco para resetear la animación
        btnAgendar.classList.add('pulse');
        // Quitamos la clase cuando termina para que se pueda repetir
        btnAgendar.addEventListener('animationend', () => {
            btnAgendar.classList.remove('pulse');
        }, { once: true });
    });
}
//cancelar turno
const turnoCard = document.querySelector('.turno-card');
const cuadroCancelar = document.querySelector('.cancelar-confirmacion');
const btnSi = document.querySelector('.btn-confirmar-si');
const btnNo = document.querySelector('.btn-confirmar-no');
turnoCard.addEventListener('click', () => {
    cuadroCancelar.style.display = 'block';
    btnNo.addEventListener('click', () => {
        cuadroCancelar.style.display = 'none';
    });
});
