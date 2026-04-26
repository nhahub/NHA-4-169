#!/bin/bash

cd frontend

mkdir -p assets/images assets/icons assets/fonts

mkdir -p components-html
touch components-html/header.html
touch components-html/sidebar.html
touch components-html/footer.html
touch components-html/modal.html

mkdir -p scripts/core scripts/services scripts/controllers scripts/components
touch scripts/main.js
touch scripts/core/api.js
touch scripts/core/config.js
touch scripts/core/helpers.js
touch scripts/core/storage.js

touch scripts/services/authService.js
touch scripts/services/rolesService.js
touch scripts/services/usersService.js
touch scripts/services/categoriesService.js

touch scripts/controllers/loginController.js
touch scripts/controllers/dashboardController.js
touch scripts/controllers/rolesController.js
touch scripts/controllers/usersController.js
touch scripts/controllers/categoriesController.js

touch scripts/components/modal.js
touch scripts/components/dropdown.js
touch scripts/components/sidebar.js

mkdir -p styles/base styles/layout styles/components styles/pages
touch styles/main.css
touch styles/base/reset.css
touch styles/base/variables.css
touch styles/base/typography.css

touch styles/layout/header.css
touch styles/layout/sidebar.css
touch styles/layout/footer.css

touch styles/components/buttons.css
touch styles/components/inputs.css
touch styles/components/cards.css

touch styles/pages/auth.css
touch styles/pages/dashboard.css
touch styles/pages/roles.css
touch styles/pages/users.css
touch styles/pages/categories.cssbash structure.sh