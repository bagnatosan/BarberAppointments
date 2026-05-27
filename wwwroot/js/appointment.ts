//Captura de eventos 1
const barberSelect = document.getElementById('barber-select') as HTMLSelectElement;
const stepCalendar = document.getElementById('step-calendar') as HTMLElement;
const hoursContainer = document.getElementById('hours-container') as HTMLElement;

//Escucha del select 2
barberSelect.addEventListener('change', () => {
    if (barberSelect.value !== '') {
        stepCalendar.style.display = 'flex';
    } else {
        stepCalendar.style.display = 'none';
        hoursContainer.style.display = 'none';
        const step3Label = document.getElementById('step-3-label');
        if (step3Label) {
            step3Label.style.display = 'none';
        }
    }
});


//Inicializacion de flatpickr 3
declare var flatpickr: any;
flatpickr("#calendar-inline", {
    inline: true,
    minDate: "today",
    locale: "es", // Aquí establecemos el idioma español
    onChange: function (selectedDates: Date[], dateStr: string) {
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



async function LoadAvailableSlots(hairdresserId: string, date: string) {
    const slotsList = document.getElementById('slots-list');
    if (!slotsList) return;

    try {
        const response = await fetch(`/Appointment/GetAvailableSlots?hairdresserId=${hairdresserId}&date=${date}`);
        const data = await response.json();

        slotsList.innerHTML = '';
        data.forEach((time: string) => {
            let button = document.createElement('button');
            button.className = 'btn btn-hour mt-2';
            button.innerText = time;
            slotsList.appendChild(button);

            button.addEventListener('click', async (event) => {
                if(confirm("¿Estás seguro de reservar este turno?")){

                    const insertPost = await fetch(`/Appointment/Insert`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            SelectedHairdresserId: hairdresserId,
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
            })
        });

    } catch (error) {
        console.error('Error al cargar los horarios disponibles:', error);
    }
}