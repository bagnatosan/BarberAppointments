"use strict";
// Captura de eventos 1
const barberSelect = document.getElementById('selected-barber-id');
const stepService = document.getElementById('step-service');
const selectedHaircutInput = document.getElementById('selected-haircut-id');
const stepCalendar = document.getElementById('step-calendar');
const hoursContainer = document.getElementById('hours-container');
const btnBarber = document.querySelectorAll('.btn-barber');
const btnService = document.querySelectorAll('.btn-service');
// Escucha del click en los barberos 2
btnBarber.forEach(el => {
    el.addEventListener('click', () => {
        barberSelect.value = el.getAttribute('data-id') || '';
        // Mostrar el paso de servicios
        if (stepService) {
            stepService.style.display = 'block';
        }
        // Resetear selección visual de barberos
        btnBarber.forEach(e => {
            e.classList.remove('selected');
        });
        el.classList.add('selected');
    });
});
// Escucha del click en los servicios 2.5
btnService.forEach(el => {
    el.addEventListener('click', () => {
        selectedHaircutInput.value = el.getAttribute('data-id') || '';
        // Mostrar el calendario (Paso 3)
        if (stepCalendar) {
            stepCalendar.style.display = 'flex';
        }
        // Resetear selección visual de servicios
        btnService.forEach(e => {
            e.classList.remove('selected');
        });
        el.classList.add('selected');
    });
});
flatpickr("#calendar-inline", {
    inline: true,
    minDate: "today",
    locale: "es",
    onChange: function (selectedDates, dateStr) {
        const hairdresserId = barberSelect.value;
        if (hairdresserId && dateStr) {
            const step3Label = document.getElementById('step-3-label');
            if (step3Label) {
                step3Label.style.display = 'block';
            }
            hoursContainer.style.display = 'block';
            LoadAvailableSlots(hairdresserId, dateStr);
        }
    }
});
async function LoadAvailableSlots(hairdresserId, date) {
    const slotsList = document.getElementById('slots-list');
    if (!slotsList)
        return;
    try {
        const response = await fetch(`/Appointment/GetAvailableSlots?hairdresserId=${hairdresserId}&date=${date}`);
        const data = await response.json();
        slotsList.innerHTML = '';
        data.forEach((time) => {
            let button = document.createElement('button');
            button.className = 'btn btn-hour mt-2';
            button.innerText = time;
            slotsList.appendChild(button);
            button.addEventListener('click', async (event) => {
                if (confirm("¿Estás seguro de reservar este turno?")) {
                    const insertPost = await fetch(`/Appointment/Insert`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            SelectedHairdresserId: hairdresserId,
                            SelectedHaircutId: selectedHaircutInput.value,
                            SelectedDate: date,
                            SelectedTime: time
                        })
                    });
                    if (insertPost.ok) {
                        alert("El turno se ha registrado correctamente");
                        window.location.href = '/';
                    }
                    else {
                        const badRequest = await insertPost.text();
                        alert(badRequest);
                    }
                }
            });
        });
    }
    catch (error) {
        console.error('Error al cargar los horarios disponibles:', error);
    }
}
