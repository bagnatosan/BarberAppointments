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