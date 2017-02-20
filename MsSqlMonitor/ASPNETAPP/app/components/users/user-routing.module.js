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
var router_1 = require('@angular/router');
var user_list_component_1 = require('./user-list.component');
var user_detail_component_1 = require('./user-detail.component');
var add_user_component_1 = require('./add-user.component');
var auth_guard_service_1 = require('../auth-guard.service');
var UserRoutingModule = (function () {
    function UserRoutingModule() {
    }
    UserRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forChild([
                    {
                        path: 'users',
                        component: user_list_component_1.UserListComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    },
                    {
                        path: 'add-user',
                        component: add_user_component_1.AddUserComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    },
                    {
                        path: 'users/:id',
                        component: user_detail_component_1.UserDetailComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    },
                    {
                        path: 'user-add',
                        component: add_user_component_1.AddUserComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    }
                ])
            ],
            exports: [
                router_1.RouterModule
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], UserRoutingModule);
    return UserRoutingModule;
}());
exports.UserRoutingModule = UserRoutingModule;
//# sourceMappingURL=user-routing.module.js.map