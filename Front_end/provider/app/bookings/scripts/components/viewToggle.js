/**
 * viewToggle.js — Switch between list and calendar views
 * The Radiant Marketplace — Provider Hub
 */

const ViewToggle = (() => {
  const VIEWS = ['list', 'calendar'];
  let currentView = 'list';
  let buttons = [];

  /** @param {string} view */
  function setView(view) {
    if (!VIEWS.includes(view)) return;
    currentView = view;

    buttons.forEach(btn => {
      const isActive = btn.dataset.view === view;
      btn.classList.toggle('is-active', isActive);
      btn.setAttribute('aria-selected', String(isActive));
    });

    // Emit a custom event so controllers can react
    document.dispatchEvent(new CustomEvent('viewchange', { detail: { view } }));
  }

  return {
    init() {
      buttons = Array.from(document.querySelectorAll('.view-toggle__btn'));
      if (!buttons.length) return;

      buttons.forEach(btn => {
        btn.addEventListener('click', () => setView(btn.dataset.view));
      });

      // Set initial active state
      setView(currentView);
    },

    getView() { return currentView; },
    setView,
  };
})();

export default ViewToggle;
