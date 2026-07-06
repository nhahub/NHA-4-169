/**
 * BAYTACK ADMIN — Loading Page Script
 *
 * Handles:
 * - Animated particles
 * - Progress bar simulation
 * - finishLoading(url) → fades out then navigates
 * - navigateWithLoader(url) → can be called from ANY page
 *   to show the loading screen before transitioning
 */

/* ── Particles ── */
const PARTICLE_COLORS = ['#0050d4', '#006947', '#7b9cff', '#69f6b8'];
const particleContainer = document.getElementById('adminParticles');

if (particleContainer) {
  for (let i = 0; i < 20; i++) {
    const p = document.createElement('div');
    p.className = 'admin-loader-particle';
    const size = Math.random() * 5 + 2;
    p.style.cssText = `
      width:${size}px;
      height:${size}px;
      left:${Math.random() * 100}%;
      bottom:0;
      background:${PARTICLE_COLORS[Math.floor(Math.random() * PARTICLE_COLORS.length)]};
      animation-duration:${3 + Math.random() * 4}s;
      animation-delay:${Math.random() * 3}s;
    `;
    particleContainer.appendChild(p);
  }
}

/* ── Progress Bar Simulation ── */
const progressFill = document.getElementById('adminProgressFill');
let _progress  = 0;
let _intervalId = null;

if (progressFill) {
  _intervalId = setInterval(() => {
    _progress = Math.min(_progress + Math.random() * 12, 90);
    progressFill.style.width = `${_progress}%`;
  }, 200);
}

/* ── Finish & Redirect ── */
/**
 * Complete the progress bar animation then navigate to a URL.
 * @param {string} redirectUrl  Target page (default: analytics.html)
 */
export function finishLoading(redirectUrl = 'admin/analytics.html') {
  if (_intervalId) clearInterval(_intervalId);

  if (progressFill) progressFill.style.width = '100%';

  setTimeout(() => {
    const wrapper = document.getElementById('adminLoaderWrapper');
    if (wrapper) wrapper.classList.add('is-fading');

    setTimeout(() => {
      window.location.href = redirectUrl;
    }, 500);
  }, 400);
}

/* ── Navigate With Loader (called from other pages) ── */
/**
 * Store the target URL and go to the loading page.
 * The loading page reads the target from sessionStorage and redirects.
 * @param {string} targetUrl
 */
export function navigateWithLoader(targetUrl) {
  sessionStorage.setItem('ek_admin_redirect_target', targetUrl);
  window.location.href = 'loading.html';
}

/* ── Auto-redirect if target is stored ── */
const storedTarget = sessionStorage.getItem('ek_admin_redirect_target');
if (storedTarget) {
  sessionStorage.removeItem('ek_admin_redirect_target');
  finishLoading(storedTarget);
} else {
  // Default: go to analytics after loading finishes naturally
  setTimeout(() => finishLoading('admin/analytics.html'), 2800);
}
