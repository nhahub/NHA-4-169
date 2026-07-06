// ============================================
// PROVIDER HUB — Modal UI Component
// File: /scripts/components/modal.js
// ============================================

import { qs, on, addClass, removeClass } from '../core/helpers.js';

/**
 * Generic Modal — create, show, hide, destroy
 *
 * Usage:
 *   import Modal from '../components/modal.js';
 *   const m = Modal.create({ title: 'My Modal', content: '<p>Hello</p>' });
 *   m.show();
 */

const Modal = (() => {

  const _buildModal = ({ id = 'phModal', title = '', content = '', footer = '' }) => {
    const wrapper = document.createElement('div');
    wrapper.id = id;
    wrapper.setAttribute('role', 'dialog');
    wrapper.setAttribute('aria-modal', 'true');
    wrapper.setAttribute('aria-labelledby', `${id}-title`);
    wrapper.style.cssText = `
      position: fixed; inset: 0; z-index: var(--z-modal, 100);
      display: flex; align-items: center; justify-content: center;
      padding: 1rem;
      background: rgba(0,0,0,0.3);
      opacity: 0; pointer-events: none;
      transition: opacity 0.3s ease;
    `;

    wrapper.innerHTML = `
      <div class="modal__panel" style="
        background: var(--color-surface-lowest);
        border-radius: var(--radius-lg);
        padding: var(--space-8);
        max-width: 32rem;
        width: 100%;
        box-shadow: 0 24px 64px rgba(0,0,0,0.15);
        transform: translateY(16px);
        transition: transform 0.3s ease;
      ">
        <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:var(--space-6);">
          <h3 id="${id}-title" style="font-family:var(--font-headline);font-size:var(--text-xl);font-weight:700;">${title}</h3>
          <button class="modal__close-btn" aria-label="Close modal" style="
            background:none; border:none; cursor:pointer;
            color:var(--color-on-surface-variant);
            padding:var(--space-2); border-radius:var(--radius-full);
            display:flex; align-items:center;
            transition: background-color 0.15s ease;
          ">
            <span class="material-symbols-outlined">close</span>
          </button>
        </div>
        <div class="modal__content">${content}</div>
        ${footer ? `<div class="modal__footer" style="margin-top:var(--space-6);">${footer}</div>` : ''}
      </div>
    `;

    return wrapper;
  };

  /**
   * Create a modal instance
   * @param {{ id?, title, content, footer? }} options
   * @returns {{ show, hide, destroy, el }}
   */
  const create = (options = {}) => {
    const el = _buildModal(options);
    document.body.appendChild(el);

    const panel = qs('.modal__panel', el);
    const closeBtn = qs('.modal__close-btn', el);

    const show = () => {
      el.style.pointerEvents = 'auto';
      requestAnimationFrame(() => {
        el.style.opacity = '1';
        panel.style.transform = 'translateY(0)';
      });
      document.body.style.overflow = 'hidden';
      // Focus trap
      closeBtn?.focus();
    };

    const hide = () => {
      el.style.opacity = '0';
      panel.style.transform = 'translateY(16px)';
      setTimeout(() => { el.style.pointerEvents = 'none'; }, 300);
      document.body.style.overflow = '';
    };

    const destroy = () => {
      hide();
      setTimeout(() => el.remove(), 300);
    };

    // Close on backdrop click
    on(el, 'click', (e) => { if (e.target === el) hide(); });

    // Close button
    on(closeBtn, 'click', hide);

    // Close on Escape
    on(document, 'keydown', (e) => { if (e.key === 'Escape') hide(); });

    return { show, hide, destroy, el };
  };

  return { create };
})();

export default Modal;
