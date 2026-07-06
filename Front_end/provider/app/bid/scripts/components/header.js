/**
 * BayTack — Component: Header
 * Manages active nav link highlighting and scroll-shadow behavior.
 */

import { qs, qsa } from '../core/helpers.js';

export function initHeader() {
  _setActiveNavLink();
  _bindScrollShadow();
  _bindBottomNav();
}

/** Mark the nav link matching the current path as active. */
function _setActiveNavLink() {
  const path = window.location.pathname;

  // Desktop nav
  qsa('.header__nav-link').forEach(link => {
    link.classList.toggle(
      'header__nav-link--active',
      link.getAttribute('href') === path
    );
  });

  // Mobile bottom nav
  qsa('.bottom-nav__item').forEach(item => {
    item.classList.toggle(
      'bottom-nav__item--active',
      item.getAttribute('href') === path
    );
    if (item.getAttribute('href') === path) {
      item.setAttribute('aria-current', 'page');
    }
  });
}

/** Add / remove a stronger shadow class when user scrolls down. */
function _bindScrollShadow() {
  const header = document.getElementById('site-header');
  if (!header) return;

  const onScroll = () => {
    header.classList.toggle('header--scrolled', window.scrollY > 8);
  };

  window.addEventListener('scroll', onScroll, { passive: true });
  onScroll(); // run once on init
}

/** Sync bottom-nav active item on click (SPA-style, no full reload). */
function _bindBottomNav() {
  const nav = document.getElementById('bottom-nav');
  if (!nav) return;

  nav.addEventListener('click', (e) => {
    const item = e.target.closest('.bottom-nav__item');
    if (!item) return;

    qsa('.bottom-nav__item', nav).forEach(i => {
      i.classList.remove('bottom-nav__item--active');
      i.removeAttribute('aria-current');
    });

    item.classList.add('bottom-nav__item--active');
    item.setAttribute('aria-current', 'page');
  });
}
