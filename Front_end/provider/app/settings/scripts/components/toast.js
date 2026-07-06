/**
 * components/toast.js
 * ─────────────────────────────────────────────────────────
 * Lightweight toast notification system.
 * Usage: Toast.show('Saved!', 'success')
 */

import AppConfig from '../core/config.js';

const Toast = (() => {
  let container;

  function getContainer() {
    if (!container) {
      container = document.createElement('div');
      container.id = 'toast-container';
      Object.assign(container.style, {
        position:  'fixed',
        top:       '5rem',
        right:     '1.5rem',
        zIndex:    '9999',
        display:   'flex',
        flexDirection: 'column',
        gap:       '0.75rem',
        pointerEvents: 'none',
      });
      document.body.appendChild(container);
    }
    return container;
  }

  const TYPES = {
    success: { bg: '#006947', icon: 'check_circle' },
    error:   { bg: '#b31b25', icon: 'error'        },
    info:    { bg: '#0050d4', icon: 'info'          },
    warning: { bg: '#6f5900', icon: 'warning'       },
  };

  /**
   * Show a toast notification.
   * @param {string} message
   * @param {'success'|'error'|'info'|'warning'} type
   * @param {number} duration - ms before auto-dismiss
   */
  function show(message, type = 'info', duration = AppConfig.TOAST_DURATION_MS) {
    const cfg  = TYPES[type] || TYPES.info;
    const wrap = getContainer();

    const toast = document.createElement('div');
    toast.setAttribute('role', 'alert');
    Object.assign(toast.style, {
      display:       'flex',
      alignItems:    'center',
      gap:           '0.75rem',
      padding:       '0.75rem 1.25rem',
      background:    cfg.bg,
      color:         'white',
      borderRadius:  '9999px',
      fontSize:      '0.875rem',
      fontWeight:    '600',
      boxShadow:     '0 8px 24px rgba(0,0,0,0.2)',
      pointerEvents: 'all',
      transform:     'translateX(6rem)',
      opacity:       '0',
      transition:    'transform 0.3s cubic-bezier(0.34,1.56,0.64,1), opacity 0.3s ease',
    });

    toast.innerHTML = `
      <span class="material-symbols-outlined" style="font-size:1.25rem;">${cfg.icon}</span>
      <span>${message}</span>
    `;

    wrap.appendChild(toast);

    // Animate in
    requestAnimationFrame(() => {
      requestAnimationFrame(() => {
        toast.style.transform = 'translateX(0)';
        toast.style.opacity   = '1';
      });
    });

    // Auto-dismiss
    setTimeout(() => {
      toast.style.transform = 'translateX(6rem)';
      toast.style.opacity   = '0';
      toast.addEventListener('transitionend', () => toast.remove(), { once: true });
    }, duration);
  }

  return { show };
})();

export default Toast;
