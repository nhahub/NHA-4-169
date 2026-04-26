#!/bin/bash

mkdir -p frontend
cd frontend

# HTML pages
touch index.html
touch login.html
touch not-found.html

# Assets
mkdir -p assets/images
mkdir -p assets/icons
mkdir -p assets/fonts

# Styles
mkdir -p styles/base
touch styles/base/reset.css
touch styles/base/variables.css
touch styles/base/typography.css

mkdir -p styles/layout
touch styles/layout/header.css
touch styles/layout/sidebar.css
touch styles/layout/footer.css

mkdir -p styles/components
touch styles/components/buttons.css
touch styles/components/inputs.css
touch styles/components/cards.css

mkdir -p styles/pages
touch styles/pages/auth.css
touch styles/pages/dashboard.css
touch styles/pages/roles.css
touch styles/pages/users.css
touch styles/pages/categories.css

touch styles/main.css

# Scripts
mkdir -p scripts/core
touch scripts/core/api.js
touch scripts/core/config.js
touch scripts/core/helpers.js
touch scripts/core/storage.js

mkdir -p scripts/services
touch scripts/services/authService.js
touch scripts/services/rolesService.js
touch scripts/services/usersService.js
touch scripts/services/categoriesService.js

mkdir -p scripts/controllers
touch scripts/controllers/loginController.js
touch scripts/controllers/dashboardController.js
touch scripts/controllers/rolesController.js
touch scripts/controllers/usersController.js
touch scripts/controllers/categoriesController.js

mkdir -p scripts/components
touch scripts/components/modal.js
touch scripts/components/dropdown.js
touch scripts/components/sidebar.js

touch scripts/main.js

# Components HTML
mkdir -p components-html
touch components-html/header.html
touch components-html/sidebar.html
touch components-html/footer.html
touch components-html/modal.html

echo "Project structure created successfully!"