/**
 * EKHDEMNI ADMIN — Categories Controller
 * Full CRUD for service categories — uses localStorage mock (no backend needed).
 * Pre-seeded with the 6 Ekhdemni services.
 */

import AuthService from '../services/authService.js';
import { showToast } from '../core/helpers.js';

/* ── Default 6 categories ── */
const DEFAULT_CATEGORIES = [
  { id:1, name:'Carpentry',  icon:'carpenter',         description:'Furniture making, custom woodwork, doors & window frames',       isActive:true, createdAt:'2024-01-10' },
  { id:2, name:'Painting',   icon:'format_paint',      description:'Interior & exterior painting, decorative finishes, wallpaper',   isActive:true, createdAt:'2024-01-10' },
  { id:3, name:'AC Repair',  icon:'ac_unit',           description:'AC installation, maintenance, duct cleaning, refrigerant refill',isActive:true, createdAt:'2024-01-10' },
  { id:4, name:'Electrical', icon:'bolt',              description:'Wiring, electrical panels, lighting, smart home installation',   isActive:true, createdAt:'2024-01-10' },
  { id:5, name:'Plumbing',   icon:'plumbing',          description:'Pipe repair, bathroom fitting, water heater, leak detection',    isActive:true, createdAt:'2024-01-10' },
  { id:6, name:'Cleaning',   icon:'cleaning_services', description:'Deep cleaning, move-in/out, post-construction, carpet & sofa',  isActive:true, createdAt:'2024-01-10' },
];

const ICON_COLORS = [
  { bg:'rgba(0,80,212,0.08)',   fg:'var(--color-primary)' },
  { bg:'rgba(26,122,74,0.1)',   fg:'#1a7a4a'              },
  { bg:'rgba(245,158,11,0.1)',  fg:'#d97706'              },
  { bg:'rgba(0,80,212,0.06)',   fg:'var(--color-primary)' },
  { bg:'rgba(111,89,0,0.1)',    fg:'#6f5900'              },
  { bg:'rgba(251,81,81,0.1)',   fg:'var(--color-error)'   },
];

const STORAGE_KEY = 'ek_admin_categories';

let _categories = [];
let _editId     = null;
let _deleteId   = null;

const $ = id => document.getElementById(id);

function _loadFromStorage() {
  try {
    const stored = JSON.parse(localStorage.getItem(STORAGE_KEY) || 'null');
    _categories  = stored ?? DEFAULT_CATEGORIES;
  } catch { _categories = [...DEFAULT_CATEGORIES]; }
}

function _persist() {
  try { localStorage.setItem(STORAGE_KEY, JSON.stringify(_categories)); } catch {}
}

function _nextId() {
  return _categories.length ? Math.max(..._categories.map(c => c.id ?? 0)) + 1 : 1;
}

function _fmt(d) {
  if (!d) return '—';
  return new Date(d).toLocaleDateString('en-EG', { day:'numeric', month:'short', year:'numeric' });
}

function _updateStats() {
  const active   = _categories.filter(c => c.isActive !== false).length;
  const topCat   = _categories.find(c => c.isActive !== false);
  const set = (id, v) => { const el = $(id); if (el) el.textContent = v; };
  set('admin-cat-stat-total',    _categories.length);
  set('admin-cat-stat-active',   active);
  set('admin-cat-stat-inactive', _categories.length - active);
  set('admin-cat-stat-top',      topCat?.name ?? '—');
  const detail = $('admin-cat-top-detail');
  if (detail) detail.textContent = topCat ? `${topCat.name} is the most requested category this week.` : 'No data yet.';
}

function _renderTable() {
  const tbody = $('admin-categories-tbody');
  if (!tbody) return;
  if (!_categories.length) {
    tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;padding:var(--sp-10);color:var(--color-on-surface-variant)">No categories yet. Add your first one!</td></tr>`;
    return;
  }
  tbody.innerHTML = _categories.map((cat, idx) => {
    const clr    = ICON_COLORS[idx % ICON_COLORS.length];
    const active = cat.isActive !== false;
    return `<tr>
      <td class="cat-id">#CAT-${String(cat.id).padStart(3,'0')}</td>
      <td><div class="cat-name-cell" style="display:flex;align-items:center;gap:0.75rem">
        <div style="width:2.25rem;height:2.25rem;border-radius:0.5rem;display:flex;align-items:center;justify-content:center;flex-shrink:0;background:${clr.bg}">
          <span class="material-symbols-outlined" style="color:${clr.fg};font-size:1.2rem;font-variation-settings:'FILL' 1">${cat.icon||'category'}</span>
        </div>
        <span style="font-weight:700">${cat.name}</span>
      </div></td>
      <td style="color:var(--color-on-surface-variant);font-size:0.85rem">${cat.description ?? '—'}</td>
      <td><span style="display:inline-flex;align-items:center;gap:0.35rem;padding:0.28rem 0.7rem;border-radius:9999px;font-size:0.78rem;font-weight:700;background:${active?'rgba(26,122,74,0.1)':'rgba(186,26,26,0.1)'};color:${active?'#1a7a4a':'var(--color-error)'}">
        <span style="width:0.42rem;height:0.42rem;border-radius:50%;background:currentColor"></span>${active?'Active':'Inactive'}
      </span></td>
      <td style="font-size:0.85rem;color:var(--color-on-surface-variant)">${_fmt(cat.createdAt)}</td>
      <td><div style="display:flex;gap:0.4rem;justify-content:flex-end">
        <button data-action="edit"   data-id="${cat.id}" aria-label="Edit"   style="padding:0.4rem;border:none;background:rgba(0,80,212,0.08);border-radius:0.4rem;cursor:pointer;color:var(--color-primary)"><span class="material-symbols-outlined" style="font-size:1.05rem">edit</span></button>
        <button data-action="toggle" data-id="${cat.id}" aria-label="Toggle" style="padding:0.4rem;border:none;background:rgba(0,0,0,0.05);border-radius:0.4rem;cursor:pointer;color:var(--color-on-surface-variant)"><span class="material-symbols-outlined" style="font-size:1.05rem">${active?'visibility_off':'visibility'}</span></button>
        <button data-action="delete" data-id="${cat.id}" aria-label="Delete" style="padding:0.4rem;border:none;background:rgba(186,26,26,0.08);border-radius:0.4rem;cursor:pointer;color:var(--color-error)"><span class="material-symbols-outlined" style="font-size:1.05rem">delete</span></button>
      </div></td>
    </tr>`;
  }).join('');
}

function _openModal(cat=null) {
  _editId = cat?.id ?? null;
  const t = $('admin-cat-modal-title'); if (t) t.textContent = cat ? 'Edit Category' : 'New Category';
  const n = $('admin-cat-name-input'); if (n) { n.value = cat?.name ?? ''; }
  const d = $('admin-cat-desc-input'); if (d) { d.value = cat?.description ?? ''; }
  const i = $('admin-cat-icon-input'); if (i) { i.value = cat?.icon ?? ''; }
  const b = $('admin-cat-modal-backdrop'); if (b) b.hidden = false;
  n?.focus();
}
function _closeModal()       { const b=$('admin-cat-modal-backdrop');  if(b) b.hidden=true;  _editId=null; }
function _openDeleteModal(c) { _deleteId=c.id; const n=$('admin-cat-delete-name'); if(n) n.textContent=c.name; const b=$('admin-cat-delete-backdrop'); if(b) b.hidden=false; }
function _closeDeleteModal() { const b=$('admin-cat-delete-backdrop'); if(b) b.hidden=true; _deleteId=null; }

function _saveCategory() {
  const name = $('admin-cat-name-input')?.value.trim();
  const desc = $('admin-cat-desc-input')?.value.trim();
  const icon = $('admin-cat-icon-input')?.value.trim() || 'category';
  if (!name) { $('admin-cat-name-input')?.focus(); return; }
  if (_editId !== null) {
    const idx = _categories.findIndex(c => c.id === _editId);
    if (idx !== -1) _categories[idx] = { ..._categories[idx], name, description: desc, icon };
  } else {
    _categories.unshift({ id: _nextId(), name, description: desc, icon, isActive: true, createdAt: new Date().toISOString().split('T')[0] });
  }
  _persist(); _renderTable(); _updateStats(); _closeModal();
}

function _deleteCategory() {
  if (_deleteId === null) return;
  _categories = _categories.filter(c => c.id !== _deleteId);
  _persist(); _renderTable(); _updateStats(); _closeDeleteModal();
}

function _toggleCategory(id) {
  const idx = _categories.findIndex(c => c.id === id);
  if (idx === -1) return;
  _categories[idx].isActive = !_categories[idx].isActive;
  _persist(); _renderTable(); _updateStats();
}

const CategoriesController = {
  async init() {
    AuthService.requireAuth();
    _loadFromStorage();
    _renderTable();
    _updateStats();

    $('admin-categories-tbody')?.addEventListener('click', e => {
      const btn = e.target.closest('[data-action]');
      if (!btn) return;
      const id  = Number(btn.dataset.id);
      const cat = _categories.find(c => c.id === id);
      if (btn.dataset.action === 'edit'   && cat) _openModal(cat);
      if (btn.dataset.action === 'delete' && cat) _openDeleteModal(cat);
      if (btn.dataset.action === 'toggle')         _toggleCategory(id);
    });

    $('admin-add-category-btn')?.addEventListener('click', () => _openModal());
    $('admin-cat-modal-close')?.addEventListener('click',   _closeModal);
    $('admin-cat-modal-cancel')?.addEventListener('click',  _closeModal);
    $('admin-cat-modal-backdrop')?.addEventListener('click', e => { if(e.target===$('admin-cat-modal-backdrop')) _closeModal(); });
    $('admin-cat-modal-save')?.addEventListener('click', _saveCategory);
    $('admin-cat-delete-close')?.addEventListener('click',   _closeDeleteModal);
    $('admin-cat-delete-cancel')?.addEventListener('click',  _closeDeleteModal);
    $('admin-cat-delete-confirm')?.addEventListener('click', _deleteCategory);
    $('admin-cat-delete-backdrop')?.addEventListener('click', e => { if(e.target===$('admin-cat-delete-backdrop')) _closeDeleteModal(); });
    $('admin-cat-search')?.addEventListener('input', e => {
      const q = e.target.value.trim().toLowerCase();
      document.querySelectorAll('#admin-categories-tbody tr').forEach(row => {
        row.style.display = !q || row.textContent.toLowerCase().includes(q) ? '' : 'none';
      });
    });
    document.addEventListener('keydown', e => { if(e.key==='Escape') { _closeModal(); _closeDeleteModal(); } });

    console.log('[CategoriesController] ready —', _categories.length, 'categories loaded');
  },
};

export default CategoriesController;
