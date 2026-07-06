/**
 * availabilityToggle.js — On/off toggle for provider availability
 * The Radiant Marketplace — Provider Hub
 */

const AvailabilityToggle = (() => {
  let toggleEl;
  let isAvailable = true;

  function render() {
    if (!toggleEl) return;
    toggleEl.classList.toggle('is-off', !isAvailable);
    toggleEl.setAttribute('aria-checked', String(isAvailable));
  }

  function toggle() {
    isAvailable = !isAvailable;
    render();
    // TODO: persist to backend via userService.updateAvailability(isAvailable)
  }

  return {
    init() {
      toggleEl = document.querySelector('.toggle-switch');
      if (!toggleEl) return;

      toggleEl.setAttribute('role', 'switch');
      toggleEl.setAttribute('aria-checked', String(isAvailable));
      toggleEl.addEventListener('click', toggle);
    },

    setAvailability(value) {
      isAvailable = Boolean(value);
      render();
    },

    getAvailability() {
      return isAvailable;
    },
  };
})();

export default AvailabilityToggle;
