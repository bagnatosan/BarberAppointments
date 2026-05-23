// // Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Usamos querySelector para agarrar la clase .a-agendar
const btn = document.getElementById('btnAgendar') as HTMLButtonElement | null;

if (btn) {
    btn.addEventListener('click', function (e: MouseEvent) {
        // La animación de pulso
        btn.classList.remove('pulse');
        void btn.offsetWidth; // Truco para resetear la animación
        btn.classList.add('pulse');

        // Quitamos la clase cuando termina para que se pueda repetir
        btn.addEventListener('animationend', () => {
            btn.classList.remove('pulse');
        }, { once: true });

        // No hace falta poner e.preventDefault() porque al ser target="_blank" 
        // la página de la barbería se queda abierta y la animación se ve igual.
    });
}

// User dropdown toggle
document.addEventListener('click', function (e: MouseEvent) {
    const toggle = document.querySelector('.user-menu-toggle') as HTMLElement | null;
    const menu = document.getElementById('UserDropdown') as HTMLElement | null;
    const target = e.target as Node; // Casteamos el target a Node para poder usar .contains()

    if (toggle && menu && target) {
        if (toggle.contains(target)) {
            menu.classList.toggle('show');
        } else if (!menu.contains(target)) {
            menu.classList.remove('show');
        }
    }
});

//agendar turno

//Captura de eventos 1
const barberSelect = document.getElementById('barber-select') as HTMLSelectElement;
const stepCalendar = document.getElementById('step-calendar') as HTMLDivElement;
const hoursContainer = document.getElementById('hours-container') as HTMLDivElement;
const calendarInline = document.getElementById('calendar-inline') as HTMLDivElement;

declare var flatpickr: any;

//Escucha del select 2
barberSelect.addEventListener('change' , () => {
    if(barberSelect.value === '')
    {
        stepCalendar.style.display = 'flex';
    }
    else
    {
        stepCalendar.style.display = 'none';
        hoursContainer.style.display = 'none';
    }
});

//Inicializacion de flatpickr 3

flatpickr("#Calendar-inline" , {
    inline: true,
    minDate: "today",
    locale: {
        firstDayOfWeek: 1,
    },
    onChange: function(selectedDates: Date[], dateStr: string) {
        const hairdresserId = barberSelect.value;
        
        if (hairdresserId && dateStr) {
            hoursContainer.style.display = 'block';
            LoadAvailableSlots(hairdresserId, dateStr);
        }
    }
})


async function LoadAvailableSlots(hairdresserId: string, date: string) : Promise<void> {
    const slotsList = document.getElementById('slots-list') as HTMLDivElement;
   
    if (!slotsList) return;
    
    try {
        const response = await fetch(`/Appointment/GetAvailableSlots?hairdresserId=${hairdresserId}&date=${date}`);
        const data = await response.json();
        slotsList.innerHTML = '';
    }
    catch (error) {
        console.error('Error al cargar los horarios disponibles:', error);
    }
    
}