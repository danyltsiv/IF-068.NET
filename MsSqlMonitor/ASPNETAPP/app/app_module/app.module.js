"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var platform_browser_1 = require('@angular/platform-browser');
var forms_1 = require('@angular/forms');
var ng_bootstrap_1 = require('@ng-bootstrap/ng-bootstrap');
var http_1 = require('@angular/http');
var user_module_1 = require('../components/users/user.module');
var auth_service_1 = require('../components/auth.service');
var auth_guard_service_1 = require('../components/auth-guard.service');
var app_component_1 = require('../app_component/app.component');
var login_component_1 = require('../components/login/login.component');
var app_routing_1 = require('../app_routing/app.routing');
var navbar_component_1 = require('../components/navbar/navbar.component');
var home_component_1 = require('../components/home/home.component');
var dropdown_directive_1 = require('../dropdown_directive/dropdown.directive');
var instances_module_1 = require('../components/instances/instances.module');
var auth_http_1 = require('../components/auth.http');
var monitor_data_service_1 = require('../components/instances/monitor-data.service');
var authenticationConnectionBackend_1 = require("../authenticationConnectionBackend");
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule,
                forms_1.FormsModule,
                forms_1.ReactiveFormsModule,
                http_1.JsonpModule,
                ng_bootstrap_1.NgbModule.forRoot(),
                instances_module_1.InstancesModule,
                user_module_1.UsersModule,
                http_1.HttpModule,
                app_routing_1.AppRoutingModule],
            declarations: [app_component_1.AppComponent,
                navbar_component_1.NavbarComponent,
                home_component_1.HomeComponent,
                dropdown_directive_1.DropdownDirective,
                login_component_1.LoginComponent
            ],
            bootstrap: [app_component_1.AppComponent],
            providers: [monitor_data_service_1.MonitorDataService,
                auth_http_1.AuthHttp,
                auth_service_1.AuthService,
                auth_guard_service_1.AuthGuardService,
                {
                    provide: http_1.XHRBackend,
                    useClass: authenticationConnectionBackend_1.AuthenticationConnectionBackend
                }]
        }), 
        __metadata('design:paramtypes', [])
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map