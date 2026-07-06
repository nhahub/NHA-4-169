// ============================================
// PROVIDER HUB — Reviews Service
// File: /scripts/services/reviewService.js
// ============================================

import Api from '../core/api.js';
import Config from '../core/config.js';

/**
 * @typedef {Object} Review
 * @property {string}  id
 * @property {string}  author
 * @property {string}  avatarUrl
 * @property {number}  rating        - 1–5
 * @property {string}  text
 * @property {string}  serviceName
 * @property {string}  orderId
 * @property {string}  serviceIcon   - Material Symbol name
 * @property {Date}    createdAt
 * @property {boolean} verified
 */

/**
 * @typedef {Object} ReviewStats
 * @property {number} averageRating
 * @property {number} totalReviews
 * @property {number} satisfactionPct
 * @property {number} fiveStarCount
 * @property {number} weeklyCount
 */

const ReviewService = {

  /**
   * Fetch paginated reviews
   * @param {{ filter?: string, page?: number, limit?: number }} params
   * @returns {Promise<{ reviews: Review[], total: number }>}
   */
  getReviews: async ({ filter = 'all', page = 1, limit = Config.REVIEWS_PER_PAGE } = {}) => {
    const params = new URLSearchParams({ filter, page, limit });
    return Api.get(`/provider/reviews?${params}`);
  },

  /**
   * Fetch review summary stats
   * @returns {Promise<ReviewStats>}
   */
  getStats: async () => {
    return Api.get('/provider/reviews/stats');
  },

  /**
   * Export reviews as CSV download
   * @param {string} filter
   * @returns {Promise<Blob>}
   */
  exportCsv: async (filter = 'all') => {
    const response = await fetch(
      `${(await import('../core/config.js')).default.API_BASE_URL}/provider/reviews/export?filter=${filter}`,
      { headers: { Accept: 'text/csv' } }
    );
    if (!response.ok) throw new Error('Export failed');
    return response.blob();
  },

  /**
   * Mark a review as responded
   * @param {string} reviewId
   * @param {string} responseText
   * @returns {Promise<Review>}
   */
  respondToReview: async (reviewId, responseText) => {
    return Api.post(`/provider/reviews/${reviewId}/respond`, { text: responseText });
  },

  // ——— MOCK DATA (for development / before API is ready) ———
  getMockReviews: () => {
    return {
      reviews: [
        {
          id: 'rev-001',
          author: 'Amira Mansour',
          avatarUrl: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDgESi7WdIKf-9yt6c85Ibw5fgbXMZFL1WF8YhxwnHzSpM4cs0nYIFz2OY3akrUKbTTZEFVDYpeI5kTa6XerUoVwtBWiidkxIzNJNrp8_YQeONjpJkE6HEakxDHJ0D4avxbnGXqWKE-LatTCYr_KlOiozCQYXNWx9O1Ci70boZByuLdRGReGvddQH55-J199fD6dTFSzzm0031XDgUAmVmyAbabVcKr370axVu-DbIkRC8WGCi8BKnSDe2vcl4cWcpr5ZEQNlR710w',
          rating: 5,
          text: 'The deep cleaning service was absolutely phenomenal. Everything from the tile grout to the balcony windows was spotless. The provider was extremely polite and punctual. Highly recommended for busy Cairo apartments!',
          serviceName: 'Full Deep Clean',
          orderId: '#RD-9821',
          serviceIcon: 'cleaning_services',
          createdAt: new Date(Date.now() - 2 * 3600000),
          verified: true,
        },
        {
          id: 'rev-002',
          author: 'Khaled El-Sayed',
          avatarUrl: 'https://lh3.googleusercontent.com/aida-public/AB6AXuBxzwF7jcLtMAhhSXdotAmFMpbL_10PyeSbTBOrsGFEMH6jjEH1sRo_gqV5NzI5ZGnd2cETCybYYV3tRyof78Rgprhrl9iPV_PxQtDQuQOLw3D3LQ9aJOpps_e9B3Avzhnl2BvKf6yF8WArVBw7KsGa7WCG1ZvrhOPshRsnc2mLywGVC1LvVbDKndRndV80L-1zLuG4V81aNplVDMesMw2FhADmomDo6RzmU0xuu68DcGn-0ohDcNK_4x4z3ZAOsvhVuA9DWYDznKM',
          rating: 4,
          text: 'Great plumbing repair. They were able to fix the leak in my kitchen within 30 minutes. Only giving 4 stars because they were 15 minutes late, but the work itself is 5-star quality.',
          serviceName: 'Kitchen Plumbing Repair',
          orderId: '#RD-9755',
          serviceIcon: 'plumbing',
          createdAt: new Date(Date.now() - 26 * 3600000),
          verified: true,
        },
        {
          id: 'rev-003',
          author: 'Nadia Hassan',
          avatarUrl: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDVZbG5Wd9VGcV-Tg96Yo_2bIOfWf9Uc3E6g3Y6RgCZ1hnkKtBYApsobyY3592yhSLgNfg9xY3-ig7vU4TEV8LZMI_WCJe92j3VKjS22CU2ZP7crmDb3mim4Tk58On-mEPc83MHJQtsRBxX60IfSm6FJVG788kmTKE9tbXhVZxq71aGensHq50_8CWhZ2Q3UGFWBN4ToeBh9jJnRG_-JHI3RjYlA0e8Hyef_kbbnPJ8OVuMX7tcgTWdm4eHNmyqpt3QFPhJVwVCZr8',
          rating: 5,
          text: 'Bless their hearts! They helped me move some heavy furniture during the maintenance visit without even charging extra. So helpful and kind.',
          serviceName: 'General Home Maintenance',
          orderId: '#RD-9610',
          serviceIcon: 'home_repair_service',
          createdAt: new Date(Date.now() - 3 * 24 * 3600000),
          verified: true,
        },
      ],
      total: 1248,
    };
  },

  getMockStats: () => ({
    averageRating:   4.9,
    totalReviews:    1248,
    satisfactionPct: 98,
    fiveStarCount:   842,
    weeklyCount:     45,
  }),
};

export default ReviewService;
