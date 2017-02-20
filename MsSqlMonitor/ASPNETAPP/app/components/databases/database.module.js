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
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var database_list_component_1 = require("./database-list.component");
var database_detail_component_1 = require('./database-detail.component');
var database_service_1 = require("./database.service");
var database_routing_module_1 = require("./database-routing.module");
var DatabasesModule = (function () {
    function DatabasesModule() {
    }
    DatabasesModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule,
                forms_1.FormsModule,
                database_routing_module_1.DatabaseRoutingModule
            ],
            declarations: [
                database_list_component_1.DatabaseListComponent,
                database_detail_component_1.DatabaseDetailComponent
            ],
            providers: [
                database_service_1.DatabaseService
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], DatabasesModule);
    return DatabasesModule;
}());
exports.DatabasesModule = DatabasesModule;
//# sourceMappingURL=database.module.js.map