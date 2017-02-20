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
var common_1 = require('@angular/common');
var database_service_1 = require('./database.service');
var DatabaseDetailComponent = (function () {
    function DatabaseDetailComponent(route, location, databaseService) {
        this.route = route;
        this.location = location;
        this.databaseService = databaseService;
    }
    DatabaseDetailComponent.prototype.ngOnInit = function () {
    };
    DatabaseDetailComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'database-detail',
            templateUrl: 'database-detail.component.html'
        }), 
        __metadata('design:paramtypes', [router_1.ActivatedRoute, common_1.Location, database_service_1.DatabaseService])
    ], DatabaseDetailComponent);
    return DatabaseDetailComponent;
}());
exports.DatabaseDetailComponent = DatabaseDetailComponent;
//# sourceMappingURL=database-detail.component.js.map