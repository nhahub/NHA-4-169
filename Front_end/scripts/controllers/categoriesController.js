/**
 * EKHDEMNI ADMIN — Categories Controller
 * Full CRUD for service categories, backed by BayTack.API
 * (GET/POST/PUT/DELETE /api/admin/categories, PATCH .../{id}/toggle).
 */

import AuthService      from '../services/authService.js';
import CategoriesService from '../services/categoriesService.js';
import { showToast }    from '../core/helpers.js';

const ICON_COLORS = [
  { bg:'rgba(0,80,212,0.08)',   fg:'var(--color-primary)' },
  { bg:'rgba(26,122,74,0.1)',   fg:'#1a7a4a'              },
  { bg:'rgba(245,158,11,0.1)',  fg:'#d97706'              },
  { bg:'rgba(0,80,212,0.06)',   fg:'var(--color-primary)' },
  { bg:'rgba(111,89,0,0.1)',    fg:'#6f5900'              },
  { bg:'rgba(251,81,81,0.1)',   fg:'var(--color-error)'   },
];

let _categories = [];
let _editId     = null; // string id (backend), or null when creating
let _deleteId   = null;

const $ = id => document.getElementById(id);

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
      <td class="cat-id">#CAT-${String(cat.id).slice(0, 8).toUpperCase()}</td>
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

async function _load() {
  try {
    _categories = await CategoriesService.getAll() ?? [];
    _renderTable();
    _updateStats();
  } catch (err) {
    console.error('[categoriesController] load', err);
    showToast(err.message || 'Could not load categories', 'error');
    _categories = [];
    _renderTable();
    _updateStats();
  }
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

async function _saveCategory() {
  const name = $('admin-cat-name-input')?.value.trim();
  const description = $('admin-cat-desc-input')?.value.trim();
  const icon = $('admin-cat-icon-input')?.value.trim() || 'category';
  if (!name) { $('admin-cat-name-input')?.focus(); return; }

  const wasEdit  = _editId !== null;
  const editId   = _editId;
  const saveBtn  = $('admin-cat-modal-save');
  try {
    if (saveBtn) saveBtn.disabled = true;
    if (wasEdit) {
      await CategoriesService.update(editId, { name, description, icon });
    } else {
      await CategoriesService.create({ name, description, icon });
    }
    _closeModal();
    await _load();
    showToast(wasEdit ? 'Category updated' : 'Category created', 'success');
  } catch (err) {
    console.error('[categoriesController] save', err);
    showToast(err.message || 'Could not save category', 'error');
  } finally {
    if (saveBtn) saveBtn.disabled = false;
  }
}

async function _deleteCategory() {
  if (_deleteId === null) return;
  const id = _deleteId;
  const confirmBtn = $('admin-cat-delete-confirm');
  try {
    if (confirmBtn) confirmBtn.disabled = true;
    await CategoriesService.delete(id);
    _closeDeleteModal();
    await _load();
    showToast('Category deleted', 'success');
  } catch (err) {
    console.error('[categoriesController] delete', err);
    showToast(err.message || 'Could not delete category', 'error');
  } finally {
    if (confirmBtn) confirmBtn.disabled = false;
  }
}

async function _toggleCategory(id) {
  try {
    await CategoriesService.toggleActive(id);
    await _load();
  } catch (err) {
    console.error('[categoriesController] toggle', err);
    showToast(err.message || 'Could not update category', 'error');
  }
}

const CategoriesController = {
  async init() {
    AuthService.requireAuth();
    await _load();

    $('admin-categories-tbody')?.addEventListener('click', e => {
      const btn = e.target.closest('[data-action]');
      if (!btn) return;
      const id  = btn.dataset.id;
      const cat = _categories.find(c => String(c.id) === String(id));
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
