// Captura de eventos 1
const barberSelect = document.getElementById('selected-barber-id') as HTMLInputElement;
const stepService = document.getElementById('step-service') as HTMLElement;
const selectedHaircutInput = document.getElementById('selected-haircut-id') as HTMLInputElement;
const stepCalendar = document.getElementById('step-calendar') as HTMLElement;
const hoursContainer = document.getElementById('hours-container') as HTMLElement;
const btnBarber = document.querySelectorAll('.btn-barber') as NodeListOf<HTMLButtonElement>;
const btnService = document.querySelectorAll('.btn-service') as NodeListOf<HTMLButtonElement>;
let selectedDate: string = '';
let selectedTime: string = '';

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

// Inicialización de flatpickr 3
declare var flatpickr: any;
const maxDate = new Date();
maxDate.setDate(maxDate.getDate() + 14);

flatpickr("#calendar-inline", {
    inline: true,
    minDate: "today",
    maxDate: maxDate,
    locale: "es",
    onChange: function (selectedDates: Date[], dateStr: string) {
        const hairdresserId = barberSelect.value;
        
        selectedDate = dateStr;

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
                    let buttonsReset = document.querySelectorAll('.btn-hour') as NodeListOf<HTMLButtonElement>;
                    buttonsReset.forEach(el => {
                        el.classList.remove('selected');    
                    })
                    button.classList.add('selected');
                    
                    selectedTime = time;

                    const showRecurrenceForm = document.getElementById('step-recurrence');
                    if (showRecurrenceForm) {
                        showRecurrenceForm.style.display = 'flex';
                    }
                    
                })
            });
    
        } catch (error) {
            console.error('Error al cargar los horarios disponibles:', error);
        }
    }

    //Turno individual

    const btnSingleBooking = document.getElementById('btn-single-booking');

    if (btnSingleBooking) {
        btnSingleBooking.addEventListener('click', async (event) => {
            
            if(!selectedTime || !selectedDate)
            {
                alert('Por favor seleccione un dia y horario');
                return;
            }

            if (confirm("¿Estás seguro de reservar este turno?")) {

                const insertPost = await fetch(`/Appointment/Insert`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        SelectedHairdresserId: barberSelect.value,
                        SelectedHaircutId: selectedHaircutInput.value,
                        SelectedDate: selectedDate,
                        SelectedTime: selectedTime
                    })
                });

                if (insertPost.ok) {
                    alert("El turno se ha registrado correctamente");
                    window.location.href = '/';
                } else {
                    const badRequest = await insertPost.text();
                    alert(badRequest);  
                }
            }
        });
    } 


    
    //Turnos fijos

    const btnRecurrent = document.getElementById('btn-toggle-recurrent') as HTMLButtonElement;
    
    if (btnRecurrent) {
        btnRecurrent.addEventListener('click', async (event) => {
            const recurrenceOptions = document.getElementById('recurrence-options') as HTMLDivElement;
            
            recurrenceOptions.style.display = 'flex';


            const btnRecurrenceWeekly = document.getElementById('btn-recurrence-1') as HTMLButtonElement;
            const btnRecurrenceBiWeekly = document.getElementById('btn-recurrence-2') as HTMLButtonElement;
            
            
            //Logica de turnos por semana
            
            
        })
    }