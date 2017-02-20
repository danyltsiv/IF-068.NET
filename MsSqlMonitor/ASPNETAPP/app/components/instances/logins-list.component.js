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
var instance_service_1 = require('./instance.service');
var LoginsList = (function () {
    function LoginsList(instanceService) {
        this.instanceService = instanceService;
    }
    LoginsList.prototype.onSelectLogin = function (login) {
        this.selectedLogin = login;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], LoginsList.prototype, "logins", void 0);
    LoginsList = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'logins-list',
            templateUrl: 'logins-list.component.html'
        }), 
        __metadata('design:paramtypes', [instance_service_1.InstanceService])
    ], LoginsList);
    return LoginsList;
}());
exports.LoginsList = LoginsList;
//# sourceMappingURL=logins-list.component.js.map