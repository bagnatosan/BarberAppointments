"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
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
const turnoCards = document.querySelectorAll('.turno-card');
turnoCards.forEach((tarjeta) => {
    const cuadroCancelar = tarjeta.querySelector('.cancelar-confirmacion');
    const btnSi = tarjeta.querySelector('.btn-confirmar-si');
    const btnNo = tarjeta.querySelector('.btn-confirmar-no');
    const date = tarjeta.dataset['fecha'];
    const userId = tarjeta.dataset['userid'];
    tarjeta.addEventListener('click', () => {
        if (cuadroCancelar) {
            cuadroCancelar.style.display = 'block';
        }
    });
    if (btnNo && cuadroCancelar) {
        btnNo.addEventListener('click', (e) => {
            e.stopPropagation(); // frena burbujeo
            cuadroCancelar.style.display = 'none';
        });
    }
    if (btnSi) {
        btnSi.addEventListener('click', (e) => {
            e.stopPropagation();
            CancelAppointment(date || '', userId || '');
        });
    }
});
function CancelAppointment(date, userId) {
    return __awaiter(this, void 0, void 0, function* () {
        const response = yield fetch(`/Appointment/CancelAppointment?date=${date}&userId=${userId}`, {
            method: 'POST',
        });
        const data = yield response.json();
        if (response.ok) {
            alert(data.message);
            window.location.reload();
        }
        else {
            alert(data.message);
        }
    });
}
//# sourceMappingURL=index.js.map