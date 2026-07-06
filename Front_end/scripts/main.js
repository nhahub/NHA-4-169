/**
 * EKHDEMNI ADMIN — Application Entry Point
 *
 * 1. Bootstraps shared layout (sidebar / header / footer)
 * 2. Lazy-loads the correct page controller via data-page on <body>
 */

import AdminSidebarComponent from './components/admin-sidebar.js';

const CONTROLLERS = {
  analytics:          () => import('./controllers/analyticsController.js'),
  providers:          () => import('./controllers/providersController.js'),
  users:              () => import('./controllers/usersController.js'),
  'user-management':  () => import('./controllers/userManagementController.js'),
  categories:         () => import('./controllers/categoriesController.js'),
  orders:             () => import('./controllers/ordersController.js'),
  verification:       () => import('./controllers/verificationController.js'),
  'verification-review': () => import('./controllers/verificationReviewController.js'),
  roles:              () => import('./controllers/rolesController.js'),
  settings:           () => import('./controllers/settingsController.js'),
  login:              () => import('./controllers/loginController.js'),
};

async function bootstrap() {
  const page = document.body.dataset.page ?? 'analytics';

  if (page !== 'login' && page !== 'not-found') {
    await AdminSidebarComponent.init();
  }

  const loader = CONTROLLERS[page];
  if (loader) {
    const module     = await loader();
    const Controller = module.default;
    await Controller.init?.();
  } else {
    console.warn(`[main.js] No controller for page: "${page}"`);
  }
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', bootstrap);
} else {
  bootstrap();
}
