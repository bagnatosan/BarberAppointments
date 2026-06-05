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
    const cuadroCancelar = tarjeta.querySelector('.cancelar-confirmacion') as HTMLDivElement;
    const btnSi = tarjeta.querySelector('.btn-confirmar-si') as HTMLButtonElement;
    const btnNo = tarjeta.querySelector('.btn-confirmar-no') as HTMLButtonElement;

    const date = tarjeta.dataset['fecha'];
    const userId = tarjeta.dataset['userid'];

    tarjeta.addEventListener('click', () => {
        if (cuadroCancelar) {
            cuadroCancelar.style.display = 'block';
        }
    });

    if (btnNo && cuadroCancelar) {
        btnNo.addEventListener('click', (e) => {
            e.stopPropagation();        // frena burbujeo
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

async function CancelAppointment(date: string, userId: string) {
    const response = await fetch(`/Appointment/CancelAppointment?date=${date}&userId=${userId}`, {
        method: 'POST',
    });
    const data = await response.json();

    if (response.ok) {
        alert(data.message);
        window.location.reload();
    } else {
        alert(data.message);
    }
}