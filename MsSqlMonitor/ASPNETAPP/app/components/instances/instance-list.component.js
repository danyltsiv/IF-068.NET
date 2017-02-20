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
var view_models_1 = require("./view-models");
var dto_models_1 = require("./dto-models");
var instance_service_1 = require("./instance.service");
var buffer_1 = require("./buffer");
var auth_service_1 = require('../auth.service');
var InstanceListComponent = (function () {
    function InstanceListComponent(router, instanceService, buffer, authService, modalService, location) {
        this.router = router;
        this.instanceService = instanceService;
        this.buffer = buffer;
        this.authService = authService;
        this.modalService = modalService;
        this.location = location;
    }
    InstanceListComponent.prototype.ngOnInit = function () {
        this.initSQLVersionsArray();
        this.message = "";
        this.page = 0;
        this.pages = [];
        this.user = this.authService.getUser();
        console.log("instance list, cur user is: " + this.user.username);
        this.instanceNameFilter = "";
        this.instanceServerFilter = "";
        this.versionFiler = "";
        this.viewMode = Mode.instances;
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.initSQLVersionsArray = function () {
        this.sqlVersions = [];
        this.sqlVersions.push(new view_models_1.VersionCheckbox("others", true, ""));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2016", true, "13"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2014", true, "12"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2012", true, "11"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2008", true, "10"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2005", true, "9"));
        this.sqlVersions.push(new view_models_1.VersionCheckbox("2000", true, "8"));
    };
    InstanceListComponent.prototype.setVisibleVersions = function (arr) {
        if (arr == null)
            return;
        for (var i = 0; i < this.sqlVersions.length; i++)
            this.sqlVersions[i].setVisibleFalse();
        for (var i = 0; i < arr.length; i++) {
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
    InstanceListComponent.prototype.getVersionsString = function () {
        var res = "";
        for (var i = 0; i < this.sqlVersions.length; i++) {
            if (this.sqlVersions[i].ischecked)
                res = res + "1";
            else
                res = res + "0";
        }
        return res;
    };
    InstanceListComponent.prototype.checkConnection = function (instance) {
        var _this = this;
        this.instanceService.checkConnection(instance)
            .then(function (result) {
            //alert(result);
            _this.showModalMessage(result);
        })
            .catch(function (error) {
            console.error("Can't connect!");
            console.error(error);
        });
    };
    InstanceListComponent.prototype.filterInstancesNames = function (value, event) {
        this.instanceNameFilter = value;
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.filterInstancesServers = function (value, event) {
        this.instanceServerFilter = value;
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.filterInstancesVersions = function (value, event) {
        this.versionFiler = value;
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.onVersionChange = function (event, index) {
        console.log(event.target.checked);
        console.log(index);
        this.sqlVersions[index].ischecked = event.target.checked;
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.getInstancesPage = function (i) {
        var _this = this;
        if (this.user == undefined) {
            this.authService.logout();
            return;
        }
        this.page = i;
        if (this.authService.isAdmin(this.user.username)) {
            this.instanceService.getInstances(this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
                .then(function (instances) {
                _this.instances = instances;
                _this.page = _this.instanceService.getPage();
                _this.pageCount = _this.instanceService.getPageCount();
                _this.pageSize = _this.instanceService.getPageSize();
                //versions
                _this.setVisibleVersions(_this.instanceService.getVersions());
                console.log("get all instanes page=" + i);
                console.log("pageCount " + _this.pageCount);
                console.log("page " + _this.page);
                console.dir(instances);
                _this.pages = [];
                for (var i = 0; i < _this.pageCount; i++) {
                    _this.pages.push(i + 1);
                }
            })
                .catch(function (error) {
                console.error("instancel list component for admin: getInstances");
                console.error(error);
            });
        }
        else {
            this.instanceService.getUserInstances(this.user.id, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
                .then(function (instances) {
                _this.instances = instances;
                _this.page = _this.instanceService.getPage();
                _this.pageCount = _this.instanceService.getPageCount();
                _this.pageSize = _this.instanceService.getPageSize();
                _this.setVisibleVersions(_this.instanceService.getVersions());
                console.log("get user instanes page=" + i);
                console.log("pageCount " + _this.pageCount);
                console.log("page " + _this.page);
                console.dir(instances);
                _this.pages = [];
                for (var i = 0; i < _this.pageCount; i++) {
                    _this.pages.push(i + 1);
                }
            })
                .catch(function (error) {
                console.error("instancel list component for user or guest: getUserInstances");
                console.error(error);
            });
        }
    };
    InstanceListComponent.prototype.getUserHidenInstancesPage = function (i) {
        var _this = this;
        this.page = i;
        var user = this.authService.getUser();
        this.instanceService.getUserHidenInstances(user.id, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then(function (instances) {
            _this.instances = instances;
            _this.page = _this.instanceService.getPage();
            _this.pageCount = _this.instanceService.getPageCount();
            _this.pageSize = _this.instanceService.getPageSize();
            _this.setVisibleVersions(_this.instanceService.getVersions());
            _this.pages = [];
            for (var i = 0; i < _this.pageCount; i++) {
                _this.pages.push(i + 1);
            }
        })
            .catch(function (error) {
            console.error("instance list component: getInstances");
            console.error(error);
        });
    };
    InstanceListComponent.prototype.getDeletedInstancesPage = function (i) {
        var _this = this;
        this.page = i;
        this.instanceService.getDeletedInstancesPage(this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then(function (instances) {
            _this.instances = instances;
            _this.page = _this.instanceService.getPage();
            _this.pageCount = _this.instanceService.getPageCount();
            _this.pageSize = _this.instanceService.getPageSize();
            _this.setVisibleVersions(_this.instanceService.getVersions());
            _this.pages = [];
            for (var i = 0; i < _this.pageCount; i++) {
                _this.pages.push(i + 1);
            }
        })
            .catch(function (error) {
            console.error("instance list component: getDeletedInstances");
            console.error(error);
        });
    };
    InstanceListComponent.prototype.getAssignedDeletedInstancesPage = function (i) {
        var _this = this;
        this.page = i;
        var user = this.authService.getUser();
        this.instanceService.getAssignedDeletedInstancesPage(user.id, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then(function (instances) {
            _this.instances = instances;
            _this.page = _this.instanceService.getPage();
            _this.pageCount = _this.instanceService.getPageCount();
            _this.pageSize = _this.instanceService.getPageSize();
            _this.setVisibleVersions(_this.instanceService.getVersions());
            _this.pages = [];
            for (var i = 0; i < _this.pageCount; i++) {
                _this.pages.push(i + 1);
            }
        })
            .catch(function (error) {
            console.error("instance list component: getAssignedDeletedInstances");
            console.error(error);
        });
    };
    InstanceListComponent.prototype.openModal = function (content, selectedInstance) {
        var _this = this;
        this.selectedInstance = selectedInstance;
        this.modalService.open(content, { size: 'lg' })
            .result.then(function () {
            _this.selectedInstance = null;
        });
    };
    InstanceListComponent.prototype.openAddInstanceModal = function (content) {
        var _this = this;
        this.newInstance = new dto_models_1.InstanceAddUpdateDto();
        this.newInstance.isWindowsAuthentication = false;
        this.modalService.open(content, { size: 'sm' })
            .result.then(function (result) {
            _this.instanceService.addInstance(_this.newInstance)
                .then(function (addedInstance) {
                if (addedInstance != null)
                    _this.instances.push(addedInstance);
            });
        })
            .catch(function (reason) { return console.info('Instance add cancelled'); });
    };
    InstanceListComponent.prototype.gotoDeletedInstances = function () {
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.showModalMessage = function (msg) {
        this.message = msg;
        this.modalService.open(this.okModal, { windowClass: 'OkModal' })
            .result.then(function (result) {
        });
    };
    InstanceListComponent.prototype.gotoUserHidenInstances = function () {
        this.viewMode = Mode.hiden;
        this.getUserHidenInstancesPage(0);
    };
    InstanceListComponent.prototype.showAssignedDeletedInstances = function () {
        this.viewMode = Mode.recycleBin;
        if (this.authService.isAdmin(this.user.username)) {
            this.getDeletedInstancesPage(0);
        }
        else {
            this.getAssignedDeletedInstancesPage(0);
        }
    };
    InstanceListComponent.prototype.gotoInstances = function () {
        this.viewMode = Mode.instances;
        this.getInstancesPage(0);
    };
    InstanceListComponent.prototype.getInstancesPageByMode = function (page) {
        if (this.viewMode == Mode.recycleBin) {
            if (this.authService.isAdmin(this.user.username)) {
                console.log("getDeletedInstancesPage " + page);
                this.getDeletedInstancesPage(page);
            }
            else {
                console.log("getAssignedDeletedInstancesPage " + page);
                this.getAssignedDeletedInstancesPage(page);
            }
            return;
        }
        if (this.viewMode == Mode.hiden) {
            console.log("getUserHidenInstancesPage " + page);
            this.getUserHidenInstancesPage(page);
            return;
        }
        console.log("getInstancesPage " + page);
        this.getInstancesPage(page);
    };
    InstanceListComponent.prototype.gotoDetail = function (instance) {
        this.buffer.instanceForDetails = instance;
        var link = ['/instance', instance.id];
        this.router.navigate(link);
    };
    InstanceListComponent.prototype.hideInstance = function (instance) {
        var _this = this;
        this.instanceService.hideInstance(instance.id, this.user.id)
            .then(function (result) {
            _this.instances.splice(_this.instances.indexOf(instance), 1);
        });
    };
    InstanceListComponent.prototype.deleteInstance = function (instance) {
        var _this = this;
        this.instanceService.deleteInstance(instance.id)
            .then(function (result) {
            _this.instances.splice(_this.instances.indexOf(instance), 1);
        });
    };
    InstanceListComponent.prototype.recoverInstance = function (instance) {
        var _this = this;
        this.instanceService.recoverInstance(instance.id)
            .then(function (result) {
            _this.instances.splice(_this.instances.indexOf(instance), 1);
        });
    };
    InstanceListComponent.prototype.showInstance = function (instance) {
        var _this = this;
        this.instanceService.showInstance(instance.id, this.user.id)
            .then(function (result) {
            _this.instances.splice(_this.instances.indexOf(instance), 1);
        });
    };
    InstanceListComponent.prototype.editInstance = function (editInstanceModal, instance) {
        var _this = this;
        this.instToUpdate = instance.toAddUpdateDto();
        this.modalService.open(editInstanceModal, { size: 'sm' })
            .result.then(function () {
            _this.instanceService.updateInstance(_this.instToUpdate).then(function (result) {
                instance.serverName = result.serverName;
                instance.instanceName = result.instanceName;
            })
                .catch(function (reason) { return ("Instance edit falled: " + reason); });
            _this.selectedInstance = null;
        })
            .catch(function (reason) { return console.info('Instance edit cancelled'); });
    };
    InstanceListComponent.prototype.navigateToDatabases = function (instanceId) {
        var link = ['/instance', instanceId, 'databases'];
        this.router.navigate(link);
    };
    InstanceListComponent.prototype.refreshInstance = function (instanceId) {
        this.instanceService.refreshInstance(instanceId)
            .then(function (result) {
            console.log(result);
        });
    };
    __decorate([
        core_1.ViewChild('okModal'), 
        __metadata('design:type', String)
    ], InstanceListComponent.prototype, "okModal", void 0);
    InstanceListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'instance-list',
            templateUrl: 'instance-list.component.html',
            styleUrls: ['instance-list.component.css']
        }), 
        __metadata('design:paramtypes', [router_1.Router, instance_service_1.InstanceService, buffer_1.Buffer, auth_service_1.AuthService, ng_bootstrap_1.NgbModal, common_1.Location])
    ], InstanceListComponent);
    return InstanceListComponent;
}());
exports.InstanceListComponent = InstanceListComponent;
var Mode;
(function (Mode) {
    Mode[Mode["instances"] = 0] = "instances";
    Mode[Mode["hiden"] = 1] = "hiden";
    Mode[Mode["recycleBin"] = 2] = "recycleBin";
})(Mode || (Mode = {}));
//# sourceMappingURL=instance-list.component.js.map