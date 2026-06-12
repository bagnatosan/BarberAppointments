// Captura de eventos 1
const barberSelect = document.getElementById('selected-barber-id') as HTMLInputElement;
const stepService = document.getElementById('step-service') as HTMLElement;
const selectedHaircutInput = document.getElementById('selected-haircut-id') as HTMLInputElement;
const stepCalendar = document.getElementById('step-calendar') as HTMLElement;
const hoursContainer = document.getElementById('hours-container') as HTMLElement;
const btnBarber = document.querySelectorAll('.btn-barber') as NodeListOf<HTMLButtonElement>;
const btnService = document.querySelectorAll('.btn-service') as NodeListOf<HTMLButtonElement>;
const showRecurrenceForm = document.getElementById('step-recurrence');
const recurrenceOptions = document.getElementById('recurrence-options') as HTMLDivElement;
const btnSingleBooking = document.getElementById('btn-single-booking') as HTMLButtonElement;
const btnRecurrent = document.getElementById('btn-toggle-recurrent') as HTMLButtonElement;
const btnWeekly = document.getElementById('btn-recurrence-1') as HTMLButtonElement;
const btnBiWeekly = document.getElementById('btn-recurrence-2') as HTMLButtonElement;
const btnConfirmRecurrent = document.getElementById('btn-confirm-recurrent') as HTMLButtonElement;
const bookingOptionsSeparator = document.getElementById('booking-options-separator') as HTMLDivElement;

let selectedDate: string = '';
let selectedTime: string = '';
let weeklyBool = false;
let biWeeklyBool = false;

// Escucha del click en los barberos 2
btnBarber.forEach(el => {
    el.addEventListener('click', () => {
        ResetButtons(1);
        barberSelect.value = el.getAttribute('data-id') || '';
        
        // Mostrar el paso de servicios
        if (stepService) {
            stepService.style.display = 'block';
        }
        stepService.scrollIntoView({
            behavior: 'smooth',
            block: 'center',
        })
        
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
        ResetButtons(2);
        selectedHaircutInput.value = el.getAttribute('data-id') || '';
        
        // Mostrar el calendario (Paso 3)
        if (stepCalendar) {
            stepCalendar.style.display = 'flex';
        }
        
        stepCalendar.scrollIntoView({
            behavior: 'smooth',
            block: 'center',
        })
        
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
        ResetButtons(3);
        const hairdresserId = barberSelect.value;
        
        selectedDate = dateStr;

        if (hairdresserId && dateStr) {
            const step3Label = document.getElementById('step-3-label');
            if (step3Label) {
                step3Label.style.display = 'block';
            }
            hoursContainer.style.display = 'block'; 
            if (hoursContainer) {
                hoursContainer.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
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

                    if (showRecurrenceForm) {
                        showRecurrenceForm.style.display = 'flex';
                        showRecurrenceForm.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    }
                    
                })
            });
    
        } catch (error) {
            console.error('Error al cargar los horarios disponibles:', error);
        }
    }

    //Turno individual

    if (btnSingleBooking) {
        btnSingleBooking.addEventListener('click', async (event) => {
            if(!selectedTime || !selectedDate)
            {
                alert('Por favor seleccione un dia y horario');
                return;
            }

            // Visual feedback
            if (btnRecurrent) btnRecurrent.classList.remove('selected');
            btnSingleBooking.classList.add('selected');
            
            // Ocultar opciones de recurrencia si estaban abiertas
            if (recurrenceOptions) recurrenceOptions.style.display = 'none';
            
            // Quitar clase selected a los botones de recurrencia
            if (btnWeekly) btnWeekly.classList.remove('selected');
            if (btnBiWeekly) btnBiWeekly.classList.remove('selected');
            weeklyBool = false;
            biWeeklyBool = false;

            if (bookingOptionsSeparator) bookingOptionsSeparator.style.display = 'block';

            if (btnConfirmRecurrent) {
                btnConfirmRecurrent.innerText = 'Confirmar Turno Único';
                btnConfirmRecurrent.style.display = 'block';
                btnConfirmRecurrent.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        });
    } 


    
    //Turnos fijos

    if (btnRecurrent) {
        btnRecurrent.addEventListener('click', async (event) => {
            // Visual feedback
            if (btnSingleBooking) btnSingleBooking.classList.remove('selected');
            btnRecurrent.classList.add('selected');

            if (btnConfirmRecurrent) {
                btnConfirmRecurrent.innerText = 'Confirmar Turno Recurrente';
                btnConfirmRecurrent.style.display = 'none';
            }
            
            const weeklyStatus = document.getElementById('weekly-status') as HTMLElement;
            const biWeeklyStatus = document.getElementById('biweekly-status') as HTMLElement;

            const url = `/Appointment/ChechRecurrence?SelectedHairdresserId=${barberSelect.value}&SelectedDate=${selectedDate}&SelectedTime=${selectedTime}`;

            const response = await fetch(url, {
                method: 'GET'
            });
            
            const data = await response.json();
            const availableWeekly = data.weeklyAvailable;
            const availableBiWeekly = data.biweeklyAvailable;
            
            //Boton semanal
            
            if(availableWeekly)
            {
                btnWeekly.disabled = false;
                
                weeklyStatus.classList.remove('status-available', 'status-occupied');
                
                weeklyStatus.innerText = 'Disponible';
                weeklyStatus.classList.add('status-available');
            }
            
            else
            {
                btnWeekly.disabled = true;

                weeklyStatus.classList.remove('status-available', 'status-occupied');

                weeklyStatus.innerText = 'No Disponible';
                weeklyStatus.classList.add('status-occupied');
            }
            
            if(availableBiWeekly)
            {
                btnBiWeekly.disabled = false;
                biWeeklyStatus.classList.remove('status-available', 'status-occupied');

                biWeeklyStatus.innerText = 'Disponible';
                biWeeklyStatus.classList.add('status-available');
            }
            
            else
            {
                btnBiWeekly.disabled = true;
                biWeeklyStatus.classList.remove('status-available', 'status-occupied');

                biWeeklyStatus.innerText = 'No Disponible';
                biWeeklyStatus.classList.add('status-occupied');
            }
            
            if (bookingOptionsSeparator) bookingOptionsSeparator.style.display = 'block';
            recurrenceOptions.style.display = 'flex';
            if (recurrenceOptions) {
                recurrenceOptions.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
            
        })
    }
    
    if (btnWeekly) {
        btnWeekly.addEventListener('click', async (event) => {
            //reseteo
            weeklyBool = false;
            biWeeklyBool = false;
            
            weeklyBool = true;
            if (btnConfirmRecurrent) {
                btnConfirmRecurrent.innerText = 'Confirmar Turno Recurrente';
                btnConfirmRecurrent.style.display = 'block';
                btnConfirmRecurrent.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }

            if (btnBiWeekly) btnBiWeekly.classList.remove('selected');
            btnWeekly.classList.add('selected');
        })
    }

    if (btnBiWeekly) {
        btnBiWeekly.addEventListener('click', async (event) => {
            weeklyBool = false;
            biWeeklyBool = false;
            
            biWeeklyBool = true;
            if (btnConfirmRecurrent) {
                btnConfirmRecurrent.innerText = 'Confirmar Turno Recurrente';
                btnConfirmRecurrent.style.display = 'block';
                btnConfirmRecurrent.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }

            if (btnWeekly) btnWeekly.classList.remove('selected');
            btnBiWeekly.classList.add('selected');
        })
    }

    if (btnConfirmRecurrent) {
        btnConfirmRecurrent.addEventListener('click', async (event) => {
            const insertPost = await fetch(`/Appointment/Insert`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    SelectedHairdresserId: barberSelect.value,
                    SelectedHaircutId: selectedHaircutInput.value,
                    SelectedDate: selectedDate,
                    SelectedTime: selectedTime,
                    Weekly: weeklyBool,
                    BiWeekly: biWeeklyBool
                })
            });


            if (insertPost.ok) {
                (weeklyBool || biWeeklyBool) ? alert("Turno recurrente registrado correctamente") : alert("El turno se ha registrado correctamente");
                window.location.href = '/';
            } else {
                const badRequest = await insertPost.text();
                alert(badRequest);
            }
        })
    }

function ResetButtons (step: number) {
        if (step == 1)
        {
            if(stepService) stepService.style.display = 'none';           //Elegir Corte (paso 2)
            if(stepCalendar) stepCalendar.style.display = 'none';        //Calendario (paso 3)
            if(hoursContainer) hoursContainer.style.display = 'none';      //Horarios disponibles (paso 4)
            if(showRecurrenceForm) showRecurrenceForm.style.display = 'none';//Boton unico (paso4)
            if(recurrenceOptions) recurrenceOptions.style.display = 'none';//Boton Semanal/Quincenal (paso 4) 
            if(bookingOptionsSeparator) bookingOptionsSeparator.style.display = 'none';
            
            selectedDate = '';              //Paso 3
            selectedTime = '';              //Paso 4
            selectedHaircutInput.value = ''; //Paso 2
            weeklyBool = false;
            biWeeklyBool = false;

            if (btnSingleBooking) btnSingleBooking.classList.remove('selected');
            if (btnRecurrent) btnRecurrent.classList.remove('selected');
            if (btnConfirmRecurrent) btnConfirmRecurrent.style.display = 'none';

            document.querySelectorAll('.btn-hour').forEach
            (btn => btn.classList.remove('selected'));  //Paso 4

            document.querySelectorAll('.btn-recurrence-opt').forEach
            (btn => btn.classList.remove('selected')); //Paso 4

            document.querySelectorAll('.btn-service').forEach( 
                btn => btn.classList.remove('selected')); //Paso 2
        }
        
        else if(step == 2)
        {
            if(stepCalendar) stepCalendar.style.display = 'none';        //Calendario (paso 3)
            if(hoursContainer) hoursContainer.style.display = 'none';      //Horarios disponibles (paso 4)
            if(showRecurrenceForm) showRecurrenceForm.style.display = 'none';//Boton unico (paso4)
            if(recurrenceOptions) recurrenceOptions.style.display = 'none';//Boton Semanal/Quincenal (paso 4) 
            if(bookingOptionsSeparator) bookingOptionsSeparator.style.display = 'none';

            selectedDate = '';              //Paso 3
            selectedTime = '';              //Paso 4
            weeklyBool = false;
            biWeeklyBool = false;

            if (btnSingleBooking) btnSingleBooking.classList.remove('selected');
            if (btnRecurrent) btnRecurrent.classList.remove('selected');
            if (btnConfirmRecurrent) btnConfirmRecurrent.style.display = 'none';

            document.querySelectorAll('.btn-hour').forEach
            (btn => btn.classList.remove('selected'));  //Paso 4

            document.querySelectorAll('.btn-recurrence-opt').forEach
            (btn => btn.classList.remove('selected')); //Paso 4

        }
        
        else if(step == 3)
        {
            if(hoursContainer) hoursContainer.style.display = 'none';      //Horarios disponibles (paso 4)
            if(showRecurrenceForm) showRecurrenceForm.style.display = 'none';//Boton unico (paso4)
            if(recurrenceOptions) recurrenceOptions.style.display = 'none';//Boton Semanal/Quincenal (paso 4) 
            if(bookingOptionsSeparator) bookingOptionsSeparator.style.display = 'none';

            selectedTime = '';              //Paso 4
            weeklyBool = false;
            biWeeklyBool = false;

            if (btnSingleBooking) btnSingleBooking.classList.remove('selected');
            if (btnRecurrent) btnRecurrent.classList.remove('selected');
            if (btnConfirmRecurrent) btnConfirmRecurrent.style.display = 'none';

            document.querySelectorAll('.btn-hour').forEach
            (btn => btn.classList.remove('selected'));  //Paso 4

            document.querySelectorAll('.btn-recurrence-opt').forEach
            (btn => btn.classList.remove('selected')); //Paso 4

        }
}