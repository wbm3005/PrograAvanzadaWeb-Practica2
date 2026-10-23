// Comportamiento común del layout: sidebar de Spark, alertas y envío de formularios.
(function () {
    'use strict';

    const SEGUNDOS_ALERTA = 5;

    function iniciarSidebar() {
        const sidebar = document.getElementById('sidebar');
        const botonMovil = document.getElementById('sidebar-toggle');
        const botonEscritorio = document.getElementById('desktop-sidebar-toggle');

        if (sidebar && botonMovil) {
            const fondo = document.createElement('div');
            fondo.className = 'sidebar-overlay';
            document.body.appendChild(fondo);

            const mostrar = (visible) => {
                sidebar.classList.toggle('show', visible);
                fondo.classList.toggle('show', visible);
            };

            botonMovil.addEventListener('click', () => mostrar(!sidebar.classList.contains('show')));
            fondo.addEventListener('click', () => mostrar(false));
        }

        if (botonEscritorio) {
            botonEscritorio.addEventListener('click', () => {
                const minimizado = document.body.classList.toggle('sidebar-minimized');
                const icono = botonEscritorio.querySelector('i');
                if (icono) {
                    icono.className = minimizado ? 'bi bi-chevron-bar-right' : 'bi bi-chevron-bar-left';
                }
                botonEscritorio.setAttribute('aria-label', minimizado ? 'Expandir menú' : 'Minimizar menú');
            });
        }
    }

    function cerrarAlerta(alerta) {
        alerta.classList.add('alerta-cerrando');
        setTimeout(() => alerta.remove(), 300);
    }

    function iniciarAlertas() {
        document.querySelectorAll('.alert-custom-close').forEach((boton) => {
            boton.addEventListener('click', () => {
                const alerta = boton.closest('.alert-custom');
                if (alerta) {
                    cerrarAlerta(alerta);
                }
            });
        });

        document.querySelectorAll('.alert-custom[data-autocierre]').forEach((alerta) => {
            setTimeout(() => cerrarAlerta(alerta), SEGUNDOS_ALERTA * 1000);
        });
    }

    // Se escucha en document: si jquery-validation cancela el envío por datos inválidos
    // detiene la propagación, el evento no llega aquí y el botón no se bloquea.
    function iniciarBloqueoEnvio() {
        document.addEventListener('submit', (evento) => {
            if (evento.defaultPrevented) {
                return;
            }

            evento.target.querySelectorAll('button[type="submit"]').forEach((boton) => {
                boton.disabled = true;
                boton.setAttribute('aria-busy', 'true');
            });
        });

        // Al volver con "Atrás" el navegador puede restaurar la página con el botón aún deshabilitado.
        window.addEventListener('pageshow', () => {
            document.querySelectorAll('button[type="submit"][aria-busy]').forEach((boton) => {
                boton.disabled = false;
                boton.removeAttribute('aria-busy');
            });
        });
    }

    document.addEventListener('DOMContentLoaded', () => {
        iniciarSidebar();
        iniciarAlertas();
        iniciarBloqueoEnvio();
    });
})();
