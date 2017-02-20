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
var user_service_1 = require('./user.service');
var AddUserComponent = (function () {
    function AddUserComponent(router, route, location, userService) {
        this.router = router;
        this.route = route;
        this.location = location;
        this.userService = userService;
        this.error = false;
        this.submit = false;
    }
    AddUserComponent.prototype.ngOnInit = function () {
        this.role = "Admin";
    };
    AddUserComponent.prototype.checkPasswordEquality = function () {
        if (this.confirmPassword !== this.password) {
            this.error = true;
            this.errorMsg = "Password mismatch!";
        }
    };
    AddUserComponent.prototype.refreshPage = function () {
        this.submit = false;
    };
    AddUserComponent.prototype.goToUserList = function () {
        this.submit = false;
        var link = ['/users'];
        this.router.navigate(link);
    };
    AddUserComponent.prototype.addUser = function () {
        var _this = this;
        this.error = false;
        this.checkPasswordEquality();
        if (!this.error) {
            var user = {
                username: this.login
            };
            this.userService.addUser(user, this.password, this.role).then(function (users) {
                if (users && users.length) {
                    _this.submit = true;
                    console.log(999);
                } //ppop
            }).catch(function (g) {
                _this.error = true;
                _this.errorMsg = g;
            });
        }
    };
    AddUserComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'add-user',
            templateUrl: 'add-user.component.html',
            styleUrls: ['add-user.component.css']
        }), 
        __metadata('design:paramtypes', [router_1.Router, router_1.ActivatedRoute, common_1.Location, user_service_1.UserService])
    ], AddUserComponent);
    return AddUserComponent;
}());
exports.AddUserComponent = AddUserComponent;
//# sourceMappingURL=add-user.component.js.map