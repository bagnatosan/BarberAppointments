//desplegable usuario
const btnAgendar = document.getElementById('btnAgendar') as HTMLButtonElement; 

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

const turnoCards = document.querySelectorAll('.turno-card') as NodeListOf<HTMLDivElement>;

turnoCards.forEach((tarjeta) => {
    const cuadroCancelarNormal = tarjeta.querySelector('.normal-confirm') as HTMLDivElement;
    const btnSiNormal = cuadroCancelarNormal?.querySelector('.btn-confirmar-si') as HTMLButtonElement;
    const btnNoNormal = cuadroCancelarNormal?.querySelector('.btn-confirmar-no') as HTMLButtonElement;
    
    const recurrentForm = tarjeta.querySelector('.recurrent-confirm') as HTMLDivElement;
    const btnConfirmOneAppointment = tarjeta.querySelector('.btn-solo-este') as HTMLButtonElement;
    const btnConfirmRecurrentAppointments = tarjeta.querySelector('.btn-todos-fijos') as HTMLButtonElement;
    const btnVolverRecurrent = recurrentForm?.querySelector('.btn-confirmar-no') as HTMLButtonElement;

    const date = tarjeta.dataset['fecha'];
    const userId = tarjeta.dataset['userid'];
    const dataRecurrent = tarjeta.dataset['recurrent']?.toLowerCase();
    
    let deleteRecurrent: boolean;

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
            e.stopPropagation();        // frena burbujeo
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

async function CancelAppointment(date: string, userId: string, deleteRecurrent: boolean) {
    const response = await fetch(`/Appointment/CancelAppointment?date=${date}&userId=${userId}
    &cancelRecurrent=${deleteRecurrent}`, {
        method: 'POST',
    });
    const data = await response.json();

    if (response.ok) {
        deleteRecurrent ? alert("Turno recurrente eliminado satisfactoriamente") : alert(data.message);
        window.location.reload();
    } else {
        deleteRecurrent ? alert("No se pudo eliminar turno recurrente satisfactoriamente") :  alert(data.message);
    }
}

function ListenerBtnConfirmation(btn: HTMLButtonElement, userId: string, date: string, deleteRecurrent: boolean) {
    btn.addEventListener('click', (e) => {
        e.stopPropagation();
        CancelAppointment(date, userId, deleteRecurrent);
    });
}
