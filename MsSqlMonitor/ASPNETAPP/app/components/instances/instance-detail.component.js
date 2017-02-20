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
var view_models_1 = require('./view-models');
var buffer_1 = require('./buffer');
var InstanceDetailComponent = (function () {
    function InstanceDetailComponent(router, dataService, buffer, modalService, route, location) {
        this.router = router;
        this.dataService = dataService;
        this.buffer = buffer;
        this.modalService = modalService;
        this.route = route;
        this.location = location;
        this.isChangeCredentialsCollapsed = true;
    }
    InstanceDetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.buffer.instanceForDetails != null)
            this.instance = this.buffer.instanceForDetails;
        else {
            var instId_1;
            this.route.params.forEach(function (params) { return instId_1 = +params['id']; });
            this.dataService.getInstanceById(instId_1).then(function (instance) { return _this.instance = instance; });
        }
    };
    InstanceDetailComponent.prototype.ngOnDestroy = function () {
        this.buffer.instanceForDetails = null;
    };
    InstanceDetailComponent.prototype.onSaveChanges = function () {
    };
    InstanceDetailComponent.prototype.onSelectLogin = function (login) {
        console.info("In selected login: login name '" + login.name + "'");
        console.info("Roles: " + login.roles.length);
        this.selectedLogin = login;
        console.info("In selected login: selected login name '" + this.selectedLogin.name + "'");
    };
    InstanceDetailComponent.prototype.openModal = function (content) {
        var _this = this;
        console.log('at openModal');
        this.modalService.open(content, { size: 'lg' }).result.then(function (result) { _this.onSaveChanges(); console.info("onSaveChanges"); }, function (reason) { });
    };
    InstanceDetailComponent.prototype.editInstance = function (editInstanceModal, instance) {
        var _this = this;
        this.instToUpdate = instance.toAddUpdateDto();
        this.modalService.open(editInstanceModal, { size: 'sm' })
            .result.then(function () {
            _this.dataService.updateInstance(_this.instToUpdate).then(function (result) {
                instance.serverName = result.serverName;
                instance.instanceName = result.instanceName;
            })
                .catch(function (reason) { return ("Instance edit falled: " + reason); });
            _this.instance = null;
        })
            .catch(function (reason) { return console.info('Instance edit cancelled'); });
    };
    InstanceDetailComponent.prototype.deleteInstance = function (deleteInstanceModal, instance) {
        this.dataService.deleteInstance(instance.id);
        this.modalService.open(deleteInstanceModal, { size: 'lg' });
    };
    InstanceDetailComponent.prototype.showInstanceList = function () {
        var link = ['/instances'];
        this.router.navigate(link);
    };
    InstanceDetailComponent.prototype.showDatabases = function () {
        var link = ['/instance', this.instance.id, 'databases'];
        this.router.navigate(link);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', view_models_1.Instance)
    ], InstanceDetailComponent.prototype, "instance", void 0);
    InstanceDetailComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'instance-details',
            templateUrl: 'instance-detail.component.html',
            styleUrls: ['instance-detail.component.css']
        }), 
        __metadata('design:paramtypes', [router_1.Router, instance_service_1.InstanceService, buffer_1.Buffer, ng_bootstrap_1.NgbModal, router_1.ActivatedRoute, common_1.Location])
    ], InstanceDetailComponent);
    return InstanceDetailComponent;
}());
exports.InstanceDetailComponent = InstanceDetailComponent;
//# sourceMappingURL=instance-detail.component.js.map