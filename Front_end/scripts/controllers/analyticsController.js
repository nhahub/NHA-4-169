/**
 * BAYTACK ADMIN — Analytics Controller
 * (Renamed from dashboardController — page: analytics.html)
 *
 * Orchestrates all widgets on the analytics page.
 */

import AuthService      from '../services/authService.js';
import AnalyticsService from '../services/analyticsService.js';
import { formatCurrency, showToast, exportToCsv } from '../core/helpers.js';

/* ── Mock data (swap with real API calls when backend is ready) ── */
const MOCK_KPI = {
  totalRevenue:    1_248_500,
  activeUsers:     42_890,
  completedOrders: 12_604,
  newProviders:    856,
};

const MOCK_TREND = [
  { month: 'Jan', revenue: 72000  },
  { month: 'Feb', revenue: 58000  },
  { month: 'Mar', revenue: 65000  },
  { month: 'Apr', revenue: 95000  },
  { month: 'May', revenue: 84000  },
  { month: 'Jun', revenue: 118000 },
  { month: 'Jul', revenue: 107000 },
  { month: 'Aug', revenue: 141000 },
  { month: 'Sep', revenue: 132000 },
  { month: 'Oct', revenue: 158000 },
  { month: 'Nov', revenue: 147000 },
  { month: 'Dec', revenue: 152400 },
];

const MOCK_PROVIDERS = [
  {
    name: 'Cairo Electric Pros',
    statusLabel: 'Verified 1h ago',
    icon: 'verified',
    iconColor: 'var(--color-secondary)',
    img: 'https://ui-avatars.com/api/?name=Cairo+Electric&background=006947&color=fff',
  },
  {
    name: 'Nile Cleaning Co.',
    statusLabel: 'Pending Approval',
    icon: 'pending',
    iconColor: 'var(--color-outline)',
    img: 'https://ui-avatars.com/api/?name=Nile+Cleaning&background=0050d4&color=fff',
  },
  {
    name: 'Master Plumbers Giza',
    statusLabel: 'Background Check',
    icon: 'history',
    iconColor: 'var(--color-outline)',
    img: 'https://ui-avatars.com/api/?name=Master+Plumbers&background=6f5900&color=fff',
  },
];

const MOCK_TRANSACTIONS = [
  { id: '#EK-9482', customer: 'Ahmed Mansour', service: 'Full Villa AC Revamp',   amount: 12400, status: 'completed',   sparkline: [1,2,3,4] },
  { id: '#EK-9477', customer: 'Laila Hassan',  service: 'Smart Home Wiring',      amount: 8950,  status: 'in-progress', sparkline: [2,4,1,3] },
  { id: '#EK-9412', customer: 'Youssef Zaid',  service: 'Emergency Plumbing',     amount: 4200,  status: 'completed',   sparkline: [3,4,2,4] },
];

/* ── Controller ── */
const AnalyticsController = {

  async init() {
    AuthService.requireAuth();
    this._bindExportButtons();
    this._bindPeriodSelect();

    await Promise.all([
      this._loadKPIs(),
      this._drawTrendChart(MOCK_TREND),
      this._renderTransactions(),
      this._renderProviders(),
    ]);
  },

  /* ── KPI Cards ── */
  async _loadKPIs() {
    // TODO: swap mock with → const kpi = await AnalyticsService.getKPIs();
    const kpi = MOCK_KPI;
    this._setKPI('admin-kpi-revenue',   formatCurrency(kpi.totalRevenue));
    this._setKPI('admin-kpi-users',     kpi.activeUsers.toLocaleString('ar-EG'));
    this._setKPI('admin-kpi-orders',    kpi.completedOrders.toLocaleString('ar-EG'));
    this._setKPI('admin-kpi-providers', kpi.newProviders.toLocaleString('ar-EG'));
  },

  _setKPI(id, value) {
    const el = document.getElementById(id);
    if (el) el.textContent = value;
  },

  /* ── Revenue Trend SVG Chart ── */
  _drawTrendChart(data) {
    const svg = document.getElementById('admin-trend-svg');
    if (!svg) return;

    const W = 1000, H = 400, PAD = 40;
    const max   = Math.max(...data.map(d => d.revenue));
    const xStep = (W - PAD * 2) / (data.length - 1);

    const toX = (i)   => PAD + i * xStep;
    const toY = (val) => PAD + (1 - val / max) * (H - PAD * 2);

    const pts  = data.map((d, i) => [toX(i), toY(d.revenue)]);
    const line = pts.map((p, i) => `${i === 0 ? 'M' : 'L'}${p[0].toFixed(1)},${p[1].toFixed(1)}`).join(' ');
    const area = `${line} L${pts[pts.length-1][0]},${H} L${pts[0][0]},${H} Z`;

    svg.innerHTML = `
      <defs>
        <linearGradient id="adminAreaGrad" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%"   stop-color="#0050d4" stop-opacity="0.2"/>
          <stop offset="100%" stop-color="#0050d4" stop-opacity="0"/>
        </linearGradient>
      </defs>
      ${[0.25, 0.5, 0.75].map(f =>
        `<line x1="${PAD}" x2="${W-PAD}"
               y1="${toY(max*f).toFixed(1)}" y2="${toY(max*f).toFixed(1)}"
               stroke="var(--color-surface-container-high)" stroke-width="1"/>`
      ).join('')}
      <path d="${area}" fill="url(#adminAreaGrad)"/>
      <path d="${line}" fill="none" stroke="var(--color-primary)" stroke-width="3" stroke-linejoin="round"/>
      ${pts.map((p, i) => `
        <circle class="admin-trend-dot"
                cx="${p[0].toFixed(1)}" cy="${p[1].toFixed(1)}" r="5"
                fill="var(--color-primary)" stroke="white" stroke-width="2"
                data-month="${data[i].month}" data-value="${data[i].revenue}"
                style="cursor:pointer"/>
      `).join('')}
    `;

    const tooltip = document.getElementById('admin-trend-tooltip');
    svg.querySelectorAll('.admin-trend-dot').forEach(dot => {
      dot.addEventListener('mouseenter', () => {
        if (!tooltip) return;
        tooltip.querySelector('.tt-month').textContent = dot.dataset.month;
        tooltip.querySelector('.tt-value').textContent = formatCurrency(dot.dataset.value);
        tooltip.classList.add('is-visible');
        const rect  = svg.getBoundingClientRect();
        const dotCx = parseFloat(dot.getAttribute('cx')) / W * rect.width;
        tooltip.style.left = `${dotCx}px`;
      });
      dot.addEventListener('mouseleave', () => tooltip?.classList.remove('is-visible'));
    });
  },

  /* ── Transactions Table ── */
  _renderTransactions() {
    const tbody = document.getElementById('admin-transactions-tbody');
    if (!tbody) return;

    const statusMap = {
      'completed':   { label: 'Completed',   cls: 'status-badge--success' },
      'in-progress': { label: 'In Progress', cls: 'status-badge--warning' },
      'cancelled':   { label: 'Cancelled',   cls: 'status-badge--error'   },
    };

    tbody.innerHTML = MOCK_TRANSACTIONS.map(tx => {
      const st   = statusMap[tx.status] ?? statusMap['completed'];
      const bars = tx.sparkline.map(h =>
        `<div class="admin-sparkline-bar" style="height:${h * 4}px;opacity:${0.3 + h * 0.18}"></div>`
      ).join('');

      return `
        <tr class="admin-data-table__row">
          <td class="font-mono text-label-md">${tx.id}</td>
          <td style="font-weight:700">${tx.customer}</td>
          <td>
            <div style="display:flex;flex-direction:column;gap:4px">
              <span style="font-size:var(--text-xs);color:var(--color-on-surface-variant)">${tx.service}</span>
              <div class="admin-sparkline">${bars}</div>
            </div>
          </td>
          <td style="font-weight:700;color:var(--color-primary);text-align:right">${formatCurrency(tx.amount)}</td>
          <td><span class="status-badge ${st.cls}">${st.label}</span></td>
          <td style="text-align:right">
            <button class="btn-icon" data-tx-id="${tx.id}" aria-label="View insights">
              <span class="material-symbols-outlined" style="color:var(--color-outline)">insights</span>
            </button>
          </td>
        </tr>
      `;
    }).join('');

    tbody.querySelectorAll('[data-tx-id]').forEach(btn => {
      btn.addEventListener('click', () => {
        showToast(`Viewing insights for ${btn.dataset.txId}`, 'info');
      });
    });
  },

  /* ── Providers List ── */
  _renderProviders() {
    const list = document.getElementById('admin-providers-list');
    if (!list) return;

    list.innerHTML = MOCK_PROVIDERS.map(p => `
      <div class="provider-card">
        <img class="provider-card__avatar" src="${p.img}" alt="${p.name}" width="48" height="48"/>
        <div class="provider-card__info">
          <p class="provider-card__name truncate">${p.name}</p>
          <p class="provider-card__meta">${p.statusLabel}</p>
        </div>
        <span class="material-symbols-outlined" style="color:${p.iconColor}">${p.icon}</span>
      </div>
    `).join('');
  },

  /* ── Period Select ── */
  _bindPeriodSelect() {
    document.getElementById('admin-period-select')?.addEventListener('change', (e) => {
      const map = {
        '12m':    MOCK_TREND,
        '6m':     MOCK_TREND.slice(6),
        'quarter':MOCK_TREND.slice(9),
      };
      this._drawTrendChart(map[e.target.value] ?? MOCK_TREND);
    });
  },

  /* ── Export Buttons ── */
  _bindExportButtons() {
    document.getElementById('admin-export-csv-btn')?.addEventListener('click', () => {
      this._exportCsv();
    });
    document.getElementById('admin-export-pdf-btn')?.addEventListener('click', () => {
      this._exportPdf();
    });
  },

  /** Build a stamped filename, e.g. "baytack-analytics-2026-07-13.csv" */
  _fileStamp(ext) {
    const today = new Date().toISOString().slice(0, 10);
    return `baytack-analytics-${today}.${ext}`;
  },

  _exportCsv() {
    try {
      const section = (title, headers, rows) => [
        [title], headers, ...rows, [],
      ];

      const rows = [
        ...section('Key Metrics', ['Metric', 'Value'], [
          ['Total Revenue', MOCK_KPI.totalRevenue],
          ['Active Users', MOCK_KPI.activeUsers],
          ['Completed Orders', MOCK_KPI.completedOrders],
          ['New Providers', MOCK_KPI.newProviders],
        ]),
        ...section('Revenue Trend', ['Month', 'Revenue'],
          MOCK_TREND.map(t => [t.month, t.revenue]),
        ),
        ...section('Recent Transactions', ['Transaction ID', 'Customer', 'Service', 'Amount', 'Status'],
          MOCK_TRANSACTIONS.map(tx => [tx.id, tx.customer, tx.service, tx.amount, tx.status]),
        ),
      ];

      exportToCsv(this._fileStamp('csv'), [], rows);
      showToast('CSV downloaded successfully', 'success');
    } catch (err) {
      console.error('[AnalyticsController] CSV export failed', err);
      showToast('Could not export the CSV file', 'error');
    }
  },

  /** jsPDF's built-in fonts can't render Arabic glyphs, so PDF text uses
   *  plain Latin formatting instead of the Arabic-locale formatCurrency(). */
  _pdfCurrency(amount) {
    return `EGP ${Number(amount).toLocaleString('en-US')}`;
  },

  async _exportPdf() {
    const jsPDFCtor = window.jspdf?.jsPDF;
    if (!jsPDFCtor) {
      showToast('PDF library failed to load — check your connection', 'error');
      return;
    }

    try {
      const doc = new jsPDFCtor({ unit: 'pt', format: 'a4' });
      const marginX = 40;
      let y = 50;

      doc.setFont('helvetica', 'bold');
      doc.setFontSize(18);
      doc.setTextColor(15, 76, 129);
      doc.text('BayTack — Analytics Report', marginX, y);

      doc.setFont('helvetica', 'normal');
      doc.setFontSize(10);
      doc.setTextColor(120, 120, 120);
      y += 18;
      doc.text(`Generated ${new Date().toLocaleString('en-GB')}`, marginX, y);

      y += 30;
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(13);
      doc.setTextColor(30, 30, 30);
      doc.text('Key Metrics', marginX, y);
      y += 10;

      const kpiRows = [
        ['Total Revenue', this._pdfCurrency(MOCK_KPI.totalRevenue)],
        ['Active Users', MOCK_KPI.activeUsers.toLocaleString('en-US')],
        ['Completed Orders', MOCK_KPI.completedOrders.toLocaleString('en-US')],
        ['New Providers', MOCK_KPI.newProviders.toLocaleString('en-US')],
      ];

      if (doc.autoTable) {
        doc.autoTable({
          startY: y + 8,
          margin: { left: marginX, right: marginX },
          head: [['Metric', 'Value']],
          body: kpiRows,
          theme: 'grid',
          headStyles: { fillColor: [15, 76, 129] },
          styles: { fontSize: 10 },
        });
        y = doc.lastAutoTable.finalY + 30;

        doc.setFont('helvetica', 'bold');
        doc.setFontSize(13);
        doc.text('Recent Transactions', marginX, y);

        doc.autoTable({
          startY: y + 8,
          margin: { left: marginX, right: marginX },
          head: [['ID', 'Customer', 'Service', 'Amount', 'Status']],
          body: MOCK_TRANSACTIONS.map(tx => [
            tx.id, tx.customer, tx.service, this._pdfCurrency(tx.amount), tx.status,
          ]),
          theme: 'grid',
          headStyles: { fillColor: [15, 76, 129] },
          styles: { fontSize: 9 },
        });
      } else {
        // Fallback if the autoTable plugin didn't load — plain text lines.
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(11);
        y += 20;
        kpiRows.forEach(([label, val]) => {
          doc.text(`${label}: ${val}`, marginX, y);
          y += 18;
        });
        y += 15;
        doc.setFont('helvetica', 'bold');
        doc.text('Recent Transactions', marginX, y);
        doc.setFont('helvetica', 'normal');
        y += 18;
        MOCK_TRANSACTIONS.forEach(tx => {
          doc.text(`${tx.id} — ${tx.customer} — ${tx.service} — ${this._pdfCurrency(tx.amount)} — ${tx.status}`, marginX, y);
          y += 16;
        });
      }

      doc.save(this._fileStamp('pdf'));
      showToast('PDF downloaded successfully', 'success');
    } catch (err) {
      console.error('[AnalyticsController] PDF export failed', err);
      showToast('Could not generate the PDF file', 'error');
    }
  },
};

export default AnalyticsController;
