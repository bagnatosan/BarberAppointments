var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
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
const maxDate = new Date();
maxDate.setDate(maxDate.getDate() + 14);
flatpickr("#calendar-inline", {
    inline: true,
    minDate: "today",
    maxDate: maxDate,
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
function LoadAvailableSlots(hairdresserId, date) {
    return __awaiter(this, void 0, void 0, function* () {
        const slotsList = document.getElementById('slots-list');
        if (!slotsList)
            return;
        try {
            const response = yield fetch(`/Appointment/GetAvailableSlots?hairdresserId=${hairdresserId}&date=${date}`);
            const data = yield response.json();
            slotsList.innerHTML = '';
            data.forEach((time) => {
                const now = new Date(); //js con new Date te da la fecha y hora actual
                const slotDateTime = new Date(`${date}T${time}`);
                let button = document.createElement('button');
                button.className = 'btn btn-hour mt-2';
                button.innerText = time;
                if (slotDateTime > now) {
                    slotsList.appendChild(button);
                }
                button.addEventListener('click', (event) => __awaiter(this, void 0, void 0, function* () {
                    let buttonsReset = document.querySelectorAll('.btn-hour');
                    buttonsReset.forEach(el => {
                        el.classList.remove('selected');
                    });
                    button.classList.add('selected');
                    ChooseRecurrence(time, hairdresserId, date);
                }));
            });
        }
        catch (error) {
            console.error('Error al cargar los horarios disponibles:', error);
        }
    });
}
function ChooseRecurrence(selectedTime, hairdresserId, date) {
    return __awaiter(this, void 0, void 0, function* () {
        const showRecurrenceForm = document.getElementById('step-recurrence');
        if (showRecurrenceForm) {
            showRecurrenceForm.style.display = 'flex';
        }
        const btnSingleBooking = document.getElementById('btn-single-booking');
        if (btnSingleBooking) {
            btnSingleBooking.addEventListener('click', (event) => __awaiter(this, void 0, void 0, function* () {
                if (confirm("¿Estás seguro de reservar este turno?")) {
                    const insertPost = yield fetch(`/Appointment/Insert`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            SelectedHairdresserId: hairdresserId,
                            SelectedHaircutId: selectedHaircutInput.value,
                            SelectedDate: date,
                            SelectedTime: selectedTime
                        })
                    });
                    if (insertPost.ok) {
                        alert("El turno se ha registrado correctamente");
                        window.location.href = '/';
                    }
                    else {
                        const badRequest = yield insertPost.text();
                        alert(badRequest);
                    }
                }
            }));
        }
    });
}
/*
* async function LoadAvailableSlots(hairdresserId: string, date: string) {
    const slotsList = document.getElementById('slots-list');
    if (!slotsList) return;

    try {
        const response = await fetch(`/Appointment/GetAvailableSlots?hairdresserId=${hairdresserId}&date=${date}`);
        const data = await response.json();

        slotsList.innerHTML = '';
        data.forEach((time: string) => {
            const now = new Date(); //js con new Date te da la fecha y hora actual
            const slotDateTime = new Date (`${date}T${time}`);
            let button = document.createElement('button');
            button.className = 'btn btn-hour mt-2';
            button.innerText = time;
            
            if(slotDateTime > now )
            {
                slotsList.appendChild(button);
            }

            button.addEventListener('click', async (event) => {
                if(confirm("¿Estás seguro de reservar este turno?")){

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
            })
        });

    } catch (error) {
        console.error('Error al cargar los horarios disponibles:', error);
    }
}
* */
//# sourceMappingURL=appointment.js.map