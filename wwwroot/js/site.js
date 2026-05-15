// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Usamos querySelector para agarrar la clase .a-agendar
const btn = document.getElementById('btnAgendar');

if (btn) {
    btn.addEventListener('click', function (e) {
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
document.addEventListener('click', function (e) {
    const toggle = document.querySelector('.user-menu-toggle');
    const menu = document.getElementById('UserDropdown');
    
    if (toggle && menu) {
        if (toggle.contains(e.target)) {
            menu.classList.toggle('show');
        } else if (!menu.contains(e.target)) {
            menu.classList.remove('show');
        }
    }
});

// wwwroot/js/agendar.js
document.addEventListener("DOMContentLoaded", function () {

    // Simulación de datos que vendrán del back-end
    const hours = ["09:00", "10:00", "11:00", "14:00", "15:00", "16:00"];

    const calendarEl = document.getElementById("calendar-inline");

    // Solo ejecutamos si el elemento existe en la página actual
    if (calendarEl) {
        flatpickr(calendarEl, {
            inline: true,
            minDate: "today",
            // Localización al español (opcional)
            locale: {
                firstDayOfWeek: 1,
                weekdays: {
                    shorthand: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                    longhand: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                },
                months: {
                    shorthand: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    longhand: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                },
            },
            onChange: function (selectedDates, dateStr) {
                const hoursContainer = document.getElementById("hours-container");
                const list = document.getElementById("slots-list");

                if (hoursContainer && list) {
                    hoursContainer.style.display = "block";
                    list.innerHTML = "";

                    hours.forEach(h => {
                        const btn = document.createElement("button");
                        btn.className = "btn btn-hour";
                        btn.type = "button"; // Evita que el formulario se envíe solo
                        btn.innerText = h;
                        btn.onclick = () => {
                            console.log("Día: " + dateStr + " - Hora: " + h);
                            // Aquí podrías guardar la selección en inputs ocultos
                        };
                        list.appendChild(btn);
                    });
                }
            }
        });
    }
});
