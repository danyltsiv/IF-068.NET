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
var database_service_1 = require('./database.service');
var DatabaseListComponent = (function () {
    function DatabaseListComponent(databaseService, router) {
        this.databaseService = databaseService;
        this.router = router;
    }
    DatabaseListComponent.prototype.ngOnInit = function () {
        this.getDatabases();
    };
    DatabaseListComponent.prototype.getDatabases = function () {
        var _this = this;
        this.databaseService.getDatabaseList(0)
            .then(function (bases) { _this.databaseList = bases; console.log(_this.databaseList); })
            .catch(function (err) { return console.log(err); });
    };
    DatabaseListComponent.prototype.gotoDetail = function (database) {
        var link = ['/database', database.id];
        this.router.navigate(link);
    };
    DatabaseListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'database-list',
            templateUrl: 'database-list.component.html',
        }), 
        __metadata('design:paramtypes', [database_service_1.DatabaseService, router_1.Router])
    ], DatabaseListComponent);
    return DatabaseListComponent;
}());
exports.DatabaseListComponent = DatabaseListComponent;
//# sourceMappingURL=database-list.component.js.map