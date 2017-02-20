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
var view_models_1 = require('../instances/view-models');
var user_service_1 = require('./user.service');
var role_service_1 = require('../roles/role.service');
var router_2 = require("@angular/router");
var UserDetailComponent = (function () {
    function UserDetailComponent(route, router, location, userService, roleService) {
        this.route = route;
        this.router = router;
        this.location = location;
        this.userService = userService;
        this.roleService = roleService;
        this.error = false;
        this.errorMsg = "error";
        this.appState = "default";
        this.visibility = "hidden";
    }
    UserDetailComponent.prototype.ngOnInit = function () {
        this.viewMode = viewState.assignedInstances;
        this.initSQLVersionsArray();
        this.page = 0;
        this.pages = [];
        this.instanceNameFilter = "";
        this.instanceServerFilter = "";
        this.versionFiler = "";
        var userId;
        this.route.params.forEach(function (params) { userId = +params['id']; });
        this.getUserById(userId);
        this.getAssignedInstances(userId);
    };
    UserDetailComponent.prototype.initSQLVersionsArray = function () {
        this.sqlVersions = [];
        this.sqlVersions.push(new view_models_1.VersionCheckbox("others", true, ""));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2016", true, "13"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2014", true, "12"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2012", true, "11"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2008", true, "10"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2005", true, "9"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2000", true, "8"));
    };
    UserDetailComponent.prototype.setVisibleVersions = function (arr) {
        if (arr == null)
            return;
        for (var i = 0; i < this.sqlVersions.length; i++)
            this.sqlVersions[i].setVisibleFalse();
        console.log("user detail set visible versions arr");
        console.dir(arr);
        for (var i = 0; i < arr.length; i++) {
            console.log(arr[i]);
            switch (arr[i]) {
                case "13":
                    this.sqlVersions[1].setVisibleTrue();
                    break;
                case "12":
                    this.sqlVersions[2].setVisibleTrue();
                    break;
                case "11":
                    this.sqlVersions[3].setVisibleTrue();
                    break;
                case "10":
                    this.sqlVersions[4].setVisibleTrue();
                    break;
                case "9":
                    this.sqlVersions[5].setVisibleTrue();
                    break;
                case "8":
                    this.sqlVersions[6].setVisibleTrue();
                    break;
                default:
                    this.sqlVersions[0].setVisibleTrue();
                    break;
            }
        }
    };
    UserDetailComponent.prototype.getVersionsString = function () {
        var res = "";
        for (var i = 0; i < this.sqlVersions.length; i++) {
            if (this.sqlVersions[i].ischecked)
                res = res + "1";
            else
                res = res + "0";
        }
        return res;
    };
    UserDetailComponent.prototype.onVersionChange = function (event, index) {
        console.log(event.target.checked);
        console.log(index);
        this.sqlVersions[index].ischecked = event.target.checked;
        this.getAssignedInstances(this.user.id);
    };
    UserDetailComponent.prototype.filterInstancesNames = function (value, event) {
        this.page = 0;
        this.instanceNameFilter = value;
        this.getAssignedInstances(this.user.id);
    };
    UserDetailComponent.prototype.getInstancesPage = function (value) {
        this.page = value;
        this.getAssignedInstances(this.user.id);
    };
    UserDetailComponent.prototype.getNotAssignedInstancesPage = function (value) {
        this.page = value;
        this.getNotAssignedInstances(this.user.id);
    };
    UserDetailComponent.prototype.filterInstancesServers = function (value, event) {
        this.page = 0;
        this.instanceServerFilter = value;
        this.getAssignedInstances(this.user.id);
    };
    UserDetailComponent.prototype.filterInstancesVersions = function (value, event) {
        this.page = 0;
        this.versionFiler = value;
        this.getAssignedInstances(this.user.id);
    };
    UserDetailComponent.prototype.checkPasswordEquality = function () {
        if (this.confirmPassword !== this.password && this.password != "") {
            this.visibility = "visible";
            this.errorMsg = "Password mismatch!";
        }
    };
    UserDetailComponent.prototype.getUserById = function (userId) {
        var _this = this;
        this.userService.getUserById(userId)
            .then(function (us) { return _this.user = us; })
            .then(function () { return _this.roleService.getRoleById(_this.user.roles[0].roleId)
            .then(function (resp) { return _this.role = resp.name; }); });
    };
    UserDetailComponent.prototype.updateUser = function () {
        var _this = this;
        this.visibility = "hidden";
        this.checkPasswordEquality();
        if (this.visibility == "hidden") {
            this.userService.updateUser(this.user, this.password, this.role).then(function () { return _this.back(); })
                .catch(function (g) { _this.visibility = "visible"; _this.errorMsg += g; });
        }
    };
    UserDetailComponent.prototype.back = function () {
        this.location.back();
    };
    UserDetailComponent.prototype.deassignInstance = function (instance) {
        var _this = this;
        this.userService.deassignInstance(this.user.id, instance.id)
            .then(function (result) {
            _this.userInstances.splice(_this.userInstances.indexOf(instance), 1);
        });
    };
    UserDetailComponent.prototype.assignInstance = function (instance) {
        var _this = this;
        this.userService.assignInstance(this.user.id, instance.id)
            .then(function (result) {
            _this.notAssignedInstances.splice(_this.notAssignedInstances.indexOf(instance), 1);
        });
    };
    UserDetailComponent.prototype.getAssignedInstances = function (userId) {
        var _this = this;
        this.userService.getUserInstances(userId, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then(function (instances) {
            _this.userInstances = instances;
            _this.page = _this.userService.getPage();
            _this.pageCount = _this.userService.getPageCount();
            _this.pageSize = _this.userService.getPageSize();
            _this.setVisibleVersions(_this.userService.getVersions());
            _this.pages = [];
            for (var i = 0; i < _this.pageCount; i++) {
                _this.pages.push(i + 1);
            }
        })
            .catch(function (error) {
            console.error("user detail component: getAssignedInstances");
            console.error(error);
        });
    };
    UserDetailComponent.prototype.getNotAssignedInstances = function (userId) {
        var _this = this;
        console.log('getting not assigned instances');
        this.userService.getNotAssignedInstances(userId, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then(function (instances) {
            _this.notAssignedInstances = instances;
            _this.page = _this.userService.getPage();
            _this.pageCount = _this.userService.getPageCount();
            _this.pageSize = _this.userService.getPageSize();
            _this.setVisibleVersions(_this.userService.getVersions());
            _this.pages = [];
            for (var i = 0; i < _this.pageCount; i++) {
                _this.pages.push(i + 1);
            }
        })
            .catch(function (error) {
            console.error("user detail component: getAssignedInstances");
            console.error(error);
        });
    };
    UserDetailComponent.prototype.goToInstanceDetail = function (instance) {
        var link = ['/instance', instance.id];
        this.router.navigate(link);
    };
    UserDetailComponent.prototype.changeViewState = function () {
        if (this.viewMode == viewState.notAssignedInstances) {
            this.viewMode = viewState.assignedInstances;
            this.getAssignedInstances(this.user.id);
        }
        else {
            this.viewMode = viewState.notAssignedInstances;
            this.getNotAssignedInstances(this.user.id);
        }
    };
    UserDetailComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'user-detail',
            styleUrls: ['user-detail.component.css'],
            templateUrl: 'user-detail.component.html'
        }), 
        __metadata('design:paramtypes', [router_1.ActivatedRoute, router_2.Router, common_1.Location, user_service_1.UserService, role_service_1.RoleService])
    ], UserDetailComponent);
    return UserDetailComponent;
}());
exports.UserDetailComponent = UserDetailComponent;
var viewState;
(function (viewState) {
    viewState[viewState["assignedInstances"] = 0] = "assignedInstances";
    viewState[viewState["notAssignedInstances"] = 1] = "notAssignedInstances";
})(viewState || (viewState = {}));
//# sourceMappingURL=user-detail.component.js.map