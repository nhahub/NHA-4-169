// ============================================
// PROVIDER HUB — Reviews Page Controller
// File: /scripts/controllers/reviewsController.js
// ============================================

import ReviewService    from '../services/reviewService.js';
import { qs, qsAll, on, addClass, removeClass, debounce, starsHTML, sanitize } from '../core/helpers.js';
import Config           from '../core/config.js';

const ReviewsController = (() => {

  // ─── State ────────────────────────────────────
  let state = {
    filter:       'all',
    page:         1,
    allLoaded:    false,
    isLoading:    false,
    searchQuery:  '',
  };

  // ─── DOM refs ─────────────────────────────────
  let reviewsFeed   = null;
  let loadMoreBtn   = null;
  let filterBtns    = null;
  let searchInput   = null;
  let statsGrid     = null;

  // ─── Render helpers ───────────────────────────

  /** Build a single review card HTML string */
  const _buildReviewCard = (review) => {
    const isHighlighted = review.rating === 5;
    const stars         = starsHTML(review.rating);

    return `
      <article
        class="review-card ${isHighlighted ? 'review-card--highlighted' : ''}"
        data-review-id="${sanitize(review.id)}"
        aria-label="Review by ${sanitize(review.author)}"
      >
        <img
          class="review-avatar"
          src="${sanitize(review.avatarUrl)}"
          alt="${sanitize(review.author)} avatar"
          loading="lazy"
        />
        <div style="flex:1; min-width:0;">
          <div class="review-header">
            <div>
              <h3 class="review-author">${sanitize(review.author)}</h3>
              <div class="review-meta">
                <div class="review-stars" aria-label="${review.rating} out of 5 stars">
                  ${stars}
                </div>
                ${review.verified ? `<span class="review-badge">Verified Order</span>` : ''}
              </div>
            </div>
            <time class="review-timestamp" datetime="${new Date(review.createdAt).toISOString()}">
              ${_formatTime(review.createdAt)}
            </time>
          </div>

          <p class="review-text">"${sanitize(review.text)}"</p>

          <div class="review-service-strip">
            <div class="review-service-info">
              <span class="material-symbols-outlined" aria-hidden="true">${sanitize(review.serviceIcon)}</span>
              <div>
                <p class="review-service-info__label">Service</p>
                <p class="review-service-info__value">${sanitize(review.serviceName)}</p>
              </div>
            </div>
            <div class="review-divider" aria-hidden="true"></div>
            <div>
              <p class="review-service-info__label">Order ID</p>
              <p class="review-service-info__value">${sanitize(review.orderId)}</p>
            </div>
          </div>
        </div>
      </article>
    `;
  };

  const _formatTime = (date) => {
    const diff  = Date.now() - new Date(date).getTime();
    const hours = Math.floor(diff / 3600000);
    const days  = Math.floor(hours / 24);
    if (hours < 1)  return 'Just now';
    if (hours < 24) return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    if (days < 7)   return `${days} day${days > 1 ? 's' : ''} ago`;
    return new Date(date).toLocaleDateString('en-EG', { day: 'numeric', month: 'short' });
  };

  /** Render stats into the summary section */
  const _renderStats = (stats) => {
    const avgEl    = qs('[data-stat="average"]');
    const totalEl  = qs('[data-stat="total"]');
    const satEl    = qs('[data-stat="satisfaction"]');
    const fiveEl   = qs('[data-stat="five-star"]');
    const weekEl   = qs('[data-stat="weekly"]');
    const barPos   = qs('[data-bar="positive"]');
    const barNeu   = qs('[data-bar="neutral"]');
    const barNeg   = qs('[data-bar="negative"]');

    if (avgEl)   avgEl.textContent   = stats.averageRating.toFixed(1);
    if (totalEl) totalEl.textContent = stats.totalReviews.toLocaleString();
    if (satEl)   satEl.textContent   = `${stats.satisfactionPct}%`;
    if (fiveEl)  fiveEl.textContent  = stats.fiveStarCount.toLocaleString();
    if (weekEl)  weekEl.textContent  = stats.weeklyCount;

    // Update satisfaction bar
    const posWidth = stats.satisfactionPct;
    const neuWidth = Math.max(0, 100 - posWidth - 5);
    const negWidth = Math.max(0, 100 - posWidth - neuWidth);
    if (barPos) barPos.style.width = `${posWidth}%`;
    if (barNeu) barNeu.style.width = `${neuWidth}%`;
    if (barNeg) barNeg.style.width = `${negWidth}%`;
  };

  // ─── Load reviews ─────────────────────────────

  const _setLoading = (loading) => {
    state.isLoading = loading;
    if (loadMoreBtn) {
      loadMoreBtn.disabled     = loading;
      loadMoreBtn.textContent  = loading
        ? 'Loading...'
        : `Load ${Config.REVIEWS_PER_PAGE} More Reviews`;
    }
  };

  const loadReviews = async (replace = false) => {
    if (state.isLoading || state.allLoaded) return;
    _setLoading(true);

    try {
      // Use mock data; swap to: ReviewService.getReviews(...)
      const { reviews, total } = ReviewService.getMockReviews();

      if (replace && reviewsFeed) reviewsFeed.innerHTML = '';

      const fragment = document.createDocumentFragment();
      reviews.forEach(review => {
        const template = document.createElement('template');
        template.innerHTML = _buildReviewCard(review).trim();
        fragment.appendChild(template.content.firstChild);
      });
      reviewsFeed?.appendChild(fragment);

      // Check if all loaded
      const loadedCount = reviewsFeed?.children.length ?? 0;
      if (loadedCount >= total) {
        state.allLoaded = true;
        if (loadMoreBtn) loadMoreBtn.style.display = 'none';
      }

    } catch (err) {
      console.error('[ReviewsController] Failed to load reviews:', err);
      _showError('Could not load reviews. Please try again.');
    } finally {
      _setLoading(false);
    }
  };

  const _showError = (message) => {
    if (!reviewsFeed) return;
    const el = document.createElement('p');
    el.style.cssText = 'text-align:center; color:var(--color-error); padding:2rem;';
    el.textContent = message;
    reviewsFeed.appendChild(el);
  };

  // ─── Filter ───────────────────────────────────

  const _applyFilter = (filter) => {
    if (state.filter === filter) return;
    state.filter    = filter;
    state.page      = 1;
    state.allLoaded = false;

    // Update button UI
    filterBtns?.forEach(btn => {
      const isActive = btn.dataset.filter === filter;
      btn.classList.toggle('btn--gold',    isActive);
      btn.classList.toggle('btn--surface', !isActive);
    });

    loadReviews(true);
  };

  // ─── Search ───────────────────────────────────

  const _handleSearch = debounce((query) => {
    state.searchQuery = query.trim().toLowerCase();
    if (!reviewsFeed) return;

    reviewsFeed.querySelectorAll('.review-card').forEach(card => {
      const text = card.textContent.toLowerCase();
      card.style.display = (!state.searchQuery || text.includes(state.searchQuery)) ? '' : 'none';
    });
  }, 250);

  // ─── Export ───────────────────────────────────

  const _handleExport = async () => {
    try {
      const blob = await ReviewService.exportCsv(state.filter);
      const url  = URL.createObjectURL(blob);
      const a    = Object.assign(document.createElement('a'), {
        href:     url,
        download: `reviews-${state.filter}-${Date.now()}.csv`,
      });
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    } catch {
      // In dev, just log — in production show a toast
      console.warn('[ReviewsController] Export not available in dev mode');
    }
  };

  // ─── Init ─────────────────────────────────────

  const init = async () => {
    reviewsFeed  = qs('#reviewsFeed');
    loadMoreBtn  = qs('#loadMoreBtn');
    filterBtns   = qsAll('[data-filter]');
    searchInput  = qs('#reviewSearchInput');

    // Load initial data
    await Promise.all([
      loadReviews(true),
      (async () => {
        const stats = ReviewService.getMockStats();
        _renderStats(stats);
      })(),
    ]);

    // Filter buttons
    filterBtns?.forEach(btn => {
      on(btn, 'click', () => _applyFilter(btn.dataset.filter));
    });

    // Load more
    on(loadMoreBtn, 'click', () => {
      state.page++;
      loadReviews(false);
    });

    // Search (header input)
    on(searchInput, 'input', (e) => _handleSearch(e.target.value));

    // Export
    const exportBtn = qs('#exportBtn');
    on(exportBtn, 'click', _handleExport);
  };

  return { init };
})();

export default ReviewsController;
