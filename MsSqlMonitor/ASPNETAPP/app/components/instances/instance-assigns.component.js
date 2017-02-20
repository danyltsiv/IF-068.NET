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
var view_models_1 = require('./view-models');
var InstanceAssignsComponent = (function () {
    function InstanceAssignsComponent() {
    }
    InstanceAssignsComponent.prototype.selectAssignedUser = function (user) {
        this.selectedAssignedUser = user;
        this.selectedNotAssignedUser = null;
    };
    InstanceAssignsComponent.prototype.selectNotAssignedUser = function (user) {
        this.selectedNotAssignedUser = user;
        this.selectedAssignedUser = null;
    };
    InstanceAssignsComponent.prototype.grantAccess = function () {
        this.instance.grantAccess(this.selectedNotAssignedUser);
        this.selectedNotAssignedUser = null;
    };
    InstanceAssignsComponent.prototype.revokeAccess = function () {
        this.instance.revokeAccess(this.selectedAssignedUser);
        this.selectedAssignedUser = null;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', view_models_1.Instance)
    ], InstanceAssignsComponent.prototype, "instance", void 0);
    InstanceAssignsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'instance-assigns',
            templateUrl: 'instance-assigns.component.html',
            styleUrls: ['instance-assigns.component.css']
        }), 
        __metadata('design:paramtypes', [])
    ], InstanceAssignsComponent);
    return InstanceAssignsComponent;
}());
exports.InstanceAssignsComponent = InstanceAssignsComponent;
//# sourceMappingURL=instance-assigns.component.js.map