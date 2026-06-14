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
    var _a;
    const cuadroCancelarNormal = tarjeta.querySelector('.normal-confirm');
    const btnSiNormal = cuadroCancelarNormal === null || cuadroCancelarNormal === void 0 ? void 0 : cuadroCancelarNormal.querySelector('.btn-confirmar-si');
    const btnNoNormal = cuadroCancelarNormal === null || cuadroCancelarNormal === void 0 ? void 0 : cuadroCancelarNormal.querySelector('.btn-confirmar-no');
    const recurrentForm = tarjeta.querySelector('.recurrent-confirm');
    const btnConfirmOneAppointment = tarjeta.querySelector('.btn-solo-este');
    const btnConfirmRecurrentAppointments = tarjeta.querySelector('.btn-todos-fijos');
    const btnVolverRecurrent = recurrentForm === null || recurrentForm === void 0 ? void 0 : recurrentForm.querySelector('.btn-confirmar-no');
    const date = tarjeta.dataset['fecha'];
    const userId = tarjeta.dataset['userid'];
    const dataRecurrent = (_a = tarjeta.dataset['recurrent']) === null || _a === void 0 ? void 0 : _a.toLowerCase();
    let deleteRecurrent;
    tarjeta.addEventListener('click', () => {
        if (cuadroCancelarNormal && dataRecurrent === "false") {
            cuadroCancelarNormal.style.display = 'block';
        }
        else if (recurrentForm && dataRecurrent === "true") {
            recurrentForm.style.display = 'block';
        }
    });
    if (btnNoNormal && cuadroCancelarNormal) {
        btnNoNormal.addEventListener('click', (e) => {
            e.stopPropagation(); // frena burbujeo
            cuadroCancelarNormal.style.display = 'none';
        });
    }
    if (btnVolverRecurrent && recurrentForm) {
        btnVolverRecurrent.addEventListener('click', (e) => {
            e.stopPropagation();
            recurrentForm.style.display = 'none';
        });
    }
    if (btnSiNormal) {
        ListenerBtnConfirmation(btnSiNormal, userId || '', date || '', false);
    }
    if (btnConfirmOneAppointment) {
        ListenerBtnConfirmation(btnConfirmOneAppointment, userId || '', date || '', false);
    }
    if (btnConfirmRecurrentAppointments) {
        ListenerBtnConfirmation(btnConfirmRecurrentAppointments, userId || '', date || '', true);
    }
});
function CancelAppointment(date, userId, deleteRecurrent) {
    return __awaiter(this, void 0, void 0, function* () {
        const response = yield fetch(`/Appointment/CancelAppointment?date=${date}&userId=${userId}
    &cancelRecurrent=${deleteRecurrent}`, {
            method: 'POST',
        });
        const data = yield response.json();
        if (response.ok) {
            deleteRecurrent ? alert("Turno recurrente eliminado satisfactoriamente") : alert(data.message);
            window.location.reload();
        }
        else {
            deleteRecurrent ? alert("No se pudo eliminar turno recurrente satisfactoriamente") : alert(data.message);
        }
    });
}
function ListenerBtnConfirmation(btn, userId, date, deleteRecurrent) {
    btn.addEventListener('click', (e) => {
        e.stopPropagation();
        CancelAppointment(date, userId, deleteRecurrent);
    });
}
//# sourceMappingURL=index.js.map