"use strict";
var router_1 = require('@angular/router');
var home_component_1 = require('../components/home/home.component');
var login_component_1 = require('../components/login/login.component');
var appRoutes = [
    {
        path: 'home',
        component: home_component_1.HomeComponent
    },
    {
        path: 'login',
        component: login_component_1.LoginComponent
    },
    {
        path: '',
        component: home_component_1.HomeComponent
    }
];
exports.AppRoutingModule = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map