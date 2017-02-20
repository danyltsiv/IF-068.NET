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
var common_1 = require('@angular/common');
var ng_bootstrap_1 = require('@ng-bootstrap/ng-bootstrap');
var dto_models_1 = require("./dto-models");
var instance_service_1 = require("./instance.service");
var buffer_1 = require("./buffer");
var AddBrowsableInstanceComponent = (function () {
    function AddBrowsableInstanceComponent(router, instanceService, buffer, modalService, location) {
        this.router = router;
        this.instanceService = instanceService;
        this.buffer = buffer;
        this.modalService = modalService;
        this.location = location;
    }
    AddBrowsableInstanceComponent.prototype.ngOnInit = function () {
        this.message = "";
        this.getBrowsableInstances();
    };
    AddBrowsableInstanceComponent.prototype.setWindowsAuth = function () {
        this.newInstance.isWindowsAuthentication = true;
    };
    AddBrowsableInstanceComponent.prototype.setSQLAuth = function () {
        this.newInstance.isWindowsAuthentication = false;
    };
    AddBrowsableInstanceComponent.prototype.getBrowsableInstances = function () {
        var _this = this;
        this.instanceService.getBrowsableInstances()
            .then(function (instances) {
            _this.instances = instances;
        })
            .catch(function (error) {
            console.error("instancel list component: getInstances");
            console.error(error);
        });
    };
    AddBrowsableInstanceComponent.prototype.openModal = function (content, selectedInstance) {
        var _this = this;
        this.selectedInstance = selectedInstance;
        this.modalService.open(content, { size: 'lg' })
            .result.then(function () {
            _this.selectedInstance = null;
        });
    };
    AddBrowsableInstanceComponent.prototype.openAddInstanceModal = function (content, serverName, instanceName) {
        var _this = this;
        this.newInstance = new dto_models_1.InstanceAddUpdateDto();
        this.newInstance.instanceName = instanceName;
        this.newInstance.serverName = serverName;
        this.modalService.open(content, { size: 'sm' })
            .result.then(function (result) {
            _this.instanceService.addInstance(_this.newInstance)
                .then(function (addedInstance) {
                // if (addedInstance != null)
                //     this.instances.push(addedInstance);
            });
        })
            .catch(function (reason) { return console.info('Instance add cancelled'); });
    };
    AddBrowsableInstanceComponent.prototype.showModalMessage = function (msg) {
        this.message = msg;
        this.modalService.open(this.okModal, { windowClass: 'OkModal' })
            .result.then(function (result) {
        });
    };
    AddBrowsableInstanceComponent.prototype.checkConnection = function (instance) {
        //alert("You are checking instance connection: " + instance);
        var _this = this;
        this.instanceService.checkConnection(instance)
            .then(function (result) {
            //  alert(result);
            _this.showModalMessage(result);
        })
            .catch(function (error) {
            console.error("Can't connect!");
            console.error(error);
            // alert("Can't connect!\n"+error);
        });
    };
    __decorate([
        core_1.ViewChild('okModal'), 
        __metadata('design:type', String)
    ], AddBrowsableInstanceComponent.prototype, "okModal", void 0);
    AddBrowsableInstanceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'browsableinstance-list',
            templateUrl: 'add-browsableinstance.component.html',
            styleUrls: ['add-browsableinstance.component.css'],
        }), 
        __metadata('design:paramtypes', [router_1.Router, instance_service_1.InstanceService, buffer_1.Buffer, ng_bootstrap_1.NgbModal, common_1.Location])
    ], AddBrowsableInstanceComponent);
    return AddBrowsableInstanceComponent;
}());
exports.AddBrowsableInstanceComponent = AddBrowsableInstanceComponent;
//# sourceMappingURL=add-browsableinstance.component.js.map