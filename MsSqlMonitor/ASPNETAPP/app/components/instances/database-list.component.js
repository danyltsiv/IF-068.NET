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
var ng_bootstrap_1 = require('@ng-bootstrap/ng-bootstrap');
var instance_service_1 = require('./instance.service');
var DatabaseList = (function () {
    function DatabaseList(instanceService, modalService, route, location) {
        this.instanceService = instanceService;
        this.modalService = modalService;
        this.route = route;
        this.location = location;
    }
    DatabaseList.prototype.ngOnInit = function () {
        var _this = this;
        var instId;
        this.route.params.forEach(function (params) { return instId = +params['id']; });
        this.instanceService.getDatabases(instId)
            .then(function (result) { _this.databases = result; console.info(_this.databases.length); })
            .catch(function (reason) { return console.error(reason); });
    };
    DatabaseList.prototype.openModal = function (content, selectedDatabase) {
        var _this = this;
        this.selectedDatabase = selectedDatabase;
        this.modalService.open(content, { size: 'lg' })
            .result.then(function () {
            _this.selectedDatabase = null;
        });
    };
    DatabaseList = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './database-list.component.html'
        }), 
        __metadata('design:paramtypes', [instance_service_1.InstanceService, ng_bootstrap_1.NgbModal, router_1.ActivatedRoute, common_1.Location])
    ], DatabaseList);
    return DatabaseList;
}());
exports.DatabaseList = DatabaseList;
//# sourceMappingURL=database-list.component.js.map