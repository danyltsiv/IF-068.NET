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
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var instance_list_component_1 = require("./instance-list.component");
var instance_detail_component_1 = require("./instance-detail.component");
var database_list_component_1 = require("./database-list.component");
var auth_guard_service_1 = require('../auth-guard.service');
var add_browsableinstance_component_1 = require("./add-browsableinstance.component");
var InstanceRoutingModule = (function () {
    function InstanceRoutingModule() {
    }
    InstanceRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forChild([
                    {
                        path: 'instances',
                        component: instance_list_component_1.InstanceListComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    },
                    {
                        path: 'add-instance',
                        component: add_browsableinstance_component_1.AddBrowsableInstanceComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    },
                    {
                        path: 'instance/:id',
                        component: instance_detail_component_1.InstanceDetailComponent,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    },
                    {
                        path: 'instance/:id/databases',
                        component: database_list_component_1.DatabaseList,
                        canActivate: [auth_guard_service_1.AuthGuardService]
                    }
                ])
            ],
            exports: [
                router_1.RouterModule
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], InstanceRoutingModule);
    return InstanceRoutingModule;
}());
exports.InstanceRoutingModule = InstanceRoutingModule;
//# sourceMappingURL=instances-routing.module.js.map