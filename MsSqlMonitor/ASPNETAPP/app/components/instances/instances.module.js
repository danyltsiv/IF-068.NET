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
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var ng_bootstrap_1 = require('@ng-bootstrap/ng-bootstrap');
var http_1 = require('@angular/http');
var common_1 = require("@angular/common");
var instance_list_component_1 = require("./instance-list.component");
var database_list_component_1 = require("./database-list.component");
var instance_detail_component_1 = require("./instance-detail.component");
var permissions_list_component_1 = require("./permissions-list.component");
var logins_list_component_1 = require("./logins-list.component");
var instances_routing_module_1 = require("./instances-routing.module");
var roles_list_accordion_component_1 = require("./roles-list-accordion.component");
var roles_list_component_1 = require("./roles-list.component");
var database_users_list_component_1 = require("./database-users-list.component");
var instance_assigns_component_1 = require("./instance-assigns.component");
var instance_service_1 = require("./instance.service");
var show_error_component_1 = require("./show-error.component");
var buffer_1 = require("./buffer");
var add_browsableinstance_component_1 = require("./add-browsableinstance.component");
var InstancesModule = (function () {
    function InstancesModule() {
    }
    InstancesModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule,
                platform_browser_1.BrowserModule,
                forms_1.FormsModule,
                forms_1.ReactiveFormsModule,
                http_1.JsonpModule,
                ng_bootstrap_1.NgbModule.forRoot(),
                instances_routing_module_1.InstanceRoutingModule
            ],
            declarations: [
                instance_list_component_1.InstanceListComponent,
                instance_detail_component_1.InstanceDetailComponent,
                instance_assigns_component_1.InstanceAssignsComponent,
                permissions_list_component_1.PermissionsList,
                roles_list_accordion_component_1.RolesListAccordion,
                roles_list_component_1.RolesList,
                logins_list_component_1.LoginsList,
                database_users_list_component_1.DatabaseUsersList,
                database_list_component_1.DatabaseList,
                show_error_component_1.ErrorView,
                add_browsableinstance_component_1.AddBrowsableInstanceComponent,
            ],
            providers: [
                instance_service_1.InstanceService,
                show_error_component_1.ShowError,
                buffer_1.Buffer
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], InstancesModule);
    return InstancesModule;
}());
exports.InstancesModule = InstancesModule;
//# sourceMappingURL=instances.module.js.map