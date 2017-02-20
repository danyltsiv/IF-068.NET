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
var user_service_1 = require('./user.service');
var role_service_1 = require('../roles/role.service');
var UserListComponent = (function () {
    function UserListComponent(userService, router, roleService) {
        this.userService = userService;
        this.router = router;
        this.roleService = roleService;
    }
    UserListComponent.prototype.ngOnInit = function () {
        this.getUsers();
    };
    UserListComponent.prototype.getUsers = function () {
        var _this = this;
        this.userService.getUsersList()
            .then(function (users) { _this.usersList = users; })
            .then(function () { return _this.usersList.forEach(function (g) {
            _this.getUserRole(g.roles[0].roleId).then(function (resp) { return g.role = resp.name; });
        }); })
            .catch(function (err) { return console.log(err); });
    };
    UserListComponent.prototype.gotoDetail = function (user) {
        var link = ['/users', user.id];
        this.router.navigate(link);
    };
    UserListComponent.prototype.gotoAddView = function () {
        var link = ['/user-add'];
        this.router.navigate(link);
    };
    UserListComponent.prototype.deleteUser = function (user) {
        var _this = this;
        if (confirm("Are you sure you want to remove " + user.username + "???")) {
            this.userService.deleteUser(user)
                .then(function (users) { _this.usersList = users; })
                .catch(function (err) { return console.log(err); });
        }
    };
    UserListComponent.prototype.getUserRole = function (id) {
        return this.roleService.getRoleById(id).then(function (response) { return response; });
    };
    UserListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'user-list',
            templateUrl: 'user-list.component.html',
            styleUrls: ['user-list.component.css'],
        }), 
        __metadata('design:paramtypes', [user_service_1.UserService, router_1.Router, role_service_1.RoleService])
    ], UserListComponent);
    return UserListComponent;
}());
exports.UserListComponent = UserListComponent;
//# sourceMappingURL=user-list.component.js.map