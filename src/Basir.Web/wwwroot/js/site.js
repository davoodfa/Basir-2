(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initLoadingOverlay();
        initPasswordToggles();
        initAlerts();
        initToastTriggers();
    });

    function initLoadingOverlay() {
        var forms = document.querySelectorAll('form');
        var overlay = document.getElementById('loading-overlay');
        if (!overlay) return;

        forms.forEach(function (form) {
            form.addEventListener('submit', function () {
                if (form.querySelector('[type="submit"]') && !form.dataset.noLoading) {
                    overlay.classList.remove('d-none');
                }
            });
        });

        window.addEventListener('beforeunload', function () {
            overlay.classList.remove('d-none');
        });
    }

    function initPasswordToggles() {
        document.querySelectorAll('[data-pw-toggle]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var input = document.getElementById(this.getAttribute('data-pw-toggle'));
                if (!input) return;
                var icon = this.querySelector('i');
                if (input.type === 'password') {
                    input.type = 'text';
                    if (icon) { icon.className = 'bi bi-eye-slash'; }
                } else {
                    input.type = 'password';
                    if (icon) { icon.className = 'bi bi-eye'; }
                }
            });
        });
    }

    function initAlerts() {
        document.querySelectorAll('.alert:not(.alert-permanent)').forEach(function (alert) {
            setTimeout(function () {
                var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                bsAlert.close();
            }, 5000);
        });
    }

    function initToastTriggers() {
        var toastEls = document.querySelectorAll('[data-toast]');
        var container = document.getElementById('toast-container');
        if (!container) return;

        toastEls.forEach(function (trigger) {
            var message = trigger.getAttribute('data-toast');
            var type = trigger.getAttribute('data-toast-type') || 'success';
            showToast(message, type);
        });
    }

    window.showToast = function (message, type) {
        type = type || 'success';
        var container = document.getElementById('toast-container');
        if (!container) return;

        var iconMap = {
            success: 'bi-check-circle-fill',
            danger: 'bi-x-circle-fill',
            warning: 'bi-exclamation-triangle-fill',
            info: 'bi-info-circle-fill'
        };
        var icon = iconMap[type] || iconMap.info;

        var wrapper = document.createElement('div');
        wrapper.innerHTML = '<div class="toast align-items-center border-0" role="alert" aria-live="assertive" aria-atomic="true">' +
            '<div class="d-flex">' +
            '<div class="toast-body bg-' + type + ' text-white rounded w-100">' +
            '<i class="bi ' + icon + '"></i> ' + escapeHtml(message) +
            '</div>' +
            '<button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>' +
            '</div>' +
            '</div>';
        var toastEl = wrapper.firstElementChild;
        container.appendChild(toastEl);

        var toast = new bootstrap.Toast(toastEl, { delay: 4000 });
        toast.show();

        toastEl.addEventListener('hidden.bs.toast', function () {
            toastEl.remove();
        });
    };

    function escapeHtml(str) {
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(str));
        return div.innerHTML;
    }

    window.addFormLoadingIndicator = function (formSelector) {
        var form = document.querySelector(formSelector);
        if (!form) return;
        form.addEventListener('submit', function () {
            var overlay = document.getElementById('loading-overlay');
            if (overlay) overlay.classList.remove('d-none');
        });
    };
})();
