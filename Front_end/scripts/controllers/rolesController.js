/**
 * EKHDEMNI ADMIN — rolesController
 * Full CRUD for admin roles + permissions assignment.
 */

import AuthService   from '../services/authService.js';
import RolesService  from '../services/rolesService.js';

const TIER_LABELS = {
  tier1: 'Tier 1 — Global',
  tier2: 'Tier 2 — Regional',
  tier3: 'Tier 3 — Limited',
};

const TIER_CLASS = {
  tier1: 'role-level-badge--tier1',
  tier2: 'role-level-badge--tier2',
  tier3: 'role-level-badge--tier3',
};

const ROLE_ICONS = ['military_tech','shield_person','manage_accounts','supervised_user_circle','person_check'];

let _roles       = [];
let _permissions = [];
let _editId      = null;
let _deleteId    = null;

const $ = id => document.getElementById(id);

function _updateStats(roles) {
  const adminCount = roles.reduce((sum, r) => sum + (r.userCount ?? 0), 0);
  $('admin-roles-stat-admins').textContent      = adminCount;
  $('admin-roles-stat-roles').textContent       = roles.length;
  $('admin-roles-stat-permissions').textContent = _permissions.length || '—';
}

function _renderTable(roles) {
  const tbody = $('admin-roles-tbody');
  if (!tbody) return;
  if (!roles.length) {
    tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;padding:var(--sp-10);color:var(--color-on-surface-variant)">No roles defined yet.</td></tr>`;
    return;
  }

  tbody.innerHTML = roles.map((role, idx) => {
    const icon  = ROLE_ICONS[idx % ROLE_ICONS.length];
    const tier  = role.accessLevel ?? 'tier3';
    const perms = Array.isArray(role.permissions) ? role.permissions : [];
    const shown = perms.slice(0, 3);
    const extra = perms.length - shown.length;

    const permTags = shown.map(p =>
      `<span class="role-perm-tag">${p.name ?? p}</span>`
    ).join('');
    const moreBadge = extra > 0
      ? `<span class="role-perm-tag role-perm-tag--more">+${extra} more</span>` : '';

    return `
    <tr>
      <td><div class="role-name-cell">
        <div class="role-icon-wrap">
          <span class="material-symbols-outlined" style="font-variation-settings:'FILL' 1">${icon}</span>
        </div>
        <span class="role-name">${role.name}</span>
      </div></td>
      <td><span class="role-level-badge ${TIER_CLASS[tier] ?? TIER_CLASS.tier3}">${TIER_LABELS[tier] ?? tier}</span></td>
      <td style="text-align:center;font-family:var(--font-headline);font-weight:700">${role.userCount ?? 0}</td>
      <td><div class="role-permissions">${permTags}${moreBadge}</div></td>
      <td><div class="role-actions">
        <button class="btn-icon btn-icon--edit"   data-action="edit"   data-id="${role.id}" aria-label="Edit ${role.name}"><span class="material-symbols-outlined">edit</span></button>
        <button class="btn-icon btn-icon--delete" data-action="delete" data-id="${role.id}" aria-label="Delete ${role.name}"><span class="material-symbols-outlined">delete</span></button>
      </div></td>
    </tr>`;
  }).join('');
}

function _renderPermissionsGrid(selectedIds = []) {
  const grid = $('admin-role-permissions-grid');
  if (!grid) return;
  if (!_permissions.length) {
    grid.innerHTML = '<p style="color:var(--color-on-surface-variant);font-size:var(--text-sm)">No permissions available.</p>';
    return;
  }
  grid.innerHTML = _permissions.map(p => `
    <label>
      <input type="checkbox" value="${p.id}" ${selectedIds.includes(p.id) ? 'checked' : ''}/>
      ${p.name}
    </label>`).join('');
}

function _openModal(role = null) {
  _editId = role?.id ?? null;
  $('admin-role-modal-title').textContent = role ? 'Edit Role' : 'Create New Role';
  $('admin-role-name-input').value        = role?.name        ?? '';
  $('admin-role-level-select').value      = role?.accessLevel ?? 'tier3';
  const selectedIds = (role?.permissions ?? []).map(p => p.id ?? p);
  _renderPermissionsGrid(selectedIds);
  $('admin-role-modal-backdrop').hidden = false;
  $('admin-role-name-input').focus();
}

function _closeModal() { $('admin-role-modal-backdrop').hidden = true; _editId = null; }

function _openDeleteModal(role) {
  _deleteId = role.id;
  $('admin-role-delete-name').textContent = role.name;
  $('admin-role-delete-backdrop').hidden = false;
}

function _closeDeleteModal() { $('admin-role-delete-backdrop').hidden = true; _deleteId = null; }

function _getSelectedPermissions() {
  const checks = document.querySelectorAll('#admin-role-permissions-grid input[type="checkbox"]:checked');
  return Array.from(checks).map(c => c.value);
}

async function _saveRole() {
  const name  = $('admin-role-name-input').value.trim();
  const level = $('admin-role-level-select').value;
  if (!name) { $('admin-role-name-input').focus(); return; }
  const payload = { name, accessLevel: level };
  const permIds = _getSelectedPermissions();

  try {
    $('admin-role-modal-save').disabled = true;
    let roleId = _editId;
    if (_editId) {
      await RolesService.update(_editId, payload);
    } else {
      const res = await RolesService.create(payload);
      roleId = res?.id ?? res?.data?.id;
    }
    if (roleId && permIds.length) {
      await RolesService.assignPermissions(roleId, permIds);
    }
    _closeModal();
    await _load();
  } catch(err) { console.error('[rolesController] save', err); }
  finally { $('admin-role-modal-save').disabled = false; }
}

async function _deleteRole() {
  if (!_deleteId) return;
  try {
    $('admin-role-delete-confirm').disabled = true;
    await RolesService.delete(_deleteId);
    _closeDeleteModal();
    await _load();
  } catch(err) { console.error('[rolesController] delete', err); }
  finally { $('admin-role-delete-confirm').disabled = false; }
}

async function _load() {
  try {
    const [rolesRes, permsRes] = await Promise.all([
      RolesService.getAll(),
      RolesService.getPermissions(),
    ]);
    _roles       = Array.isArray(rolesRes) ? rolesRes : (rolesRes?.data ?? []);
    _permissions = Array.isArray(permsRes) ? permsRes : (permsRes?.data ?? []);
    _renderTable(_roles);
    _updateStats(_roles);
  } catch(err) {
    console.error('[rolesController] load', err);
    _renderTable([]);
    _updateStats([]);
  }
}

const Controller = {
  async init() {
    AuthService.requireAuth();
    await _load();

    $('admin-roles-tbody')?.addEventListener('click', e => {
      const btn = e.target.closest('[data-action]');
      if (!btn) return;
      const id   = btn.dataset.id;
      const role = _roles.find(r => String(r.id) === String(id));
      if (btn.dataset.action === 'edit'   && role) _openModal(role);
      if (btn.dataset.action === 'delete' && role) _openDeleteModal(role);
    });

    $('admin-add-role-btn')?.addEventListener('click', () => _openModal());

    $('admin-role-modal-close')?.addEventListener('click',  _closeModal);
    $('admin-role-modal-cancel')?.addEventListener('click', _closeModal);
    $('admin-role-modal-backdrop')?.addEventListener('click', e => { if (e.target === $('admin-role-modal-backdrop')) _closeModal(); });
    $('admin-role-modal-save')?.addEventListener('click', _saveRole);

    $('admin-role-delete-close')?.addEventListener('click',   _closeDeleteModal);
    $('admin-role-delete-cancel')?.addEventListener('click',  _closeDeleteModal);
    $('admin-role-delete-confirm')?.addEventListener('click', _deleteRole);
    $('admin-role-delete-backdrop')?.addEventListener('click', e => { if (e.target === $('admin-role-delete-backdrop')) _closeDeleteModal(); });

    document.addEventListener('keydown', e => { if (e.key === 'Escape') { _closeModal(); _closeDeleteModal(); } });

    console.log('[rolesController] ready');
  },
};

export default Controller;
