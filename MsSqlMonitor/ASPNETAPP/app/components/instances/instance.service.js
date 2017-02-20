"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var http_1 = require('@angular/http');
var auth_http_1 = require('../auth.http');
var show_error_component_1 = require('./show-error.component');
var view_models_1 = require('./view-models');
var auth_service_1 = require('../auth.service');
var InstanceService = (function () {
    function InstanceService(authService, http, showError) {
        this.authService = authService;
        this.http = http;
        this.showError = showError;
        this.baseUrl = 'https://localhost:44301/api/instances/';
        this.baseUrl2 = 'https://localhost:44301/api/instances';
        this.loginsSuffix = '/logins';
        this.instanceRolesSuffix = '/roles';
        this.instancePermissionsSuffix = '/permissions';
        this.databasesSuffix = '/databases';
        this.databaseUsersSuffix = '/users';
        this.databaseRolesSuffix = '/database-roles';
        this.databasePermissionsSuffix = '/database-permissions';
        this.assignedUsersSuffix = '/assigned-users';
        this.notAssignedUsersSuffix = '/not-assigned-users';
        this.grantAssignSuffix = '/grant-instance-access';
        this.revokeAssignSuffix = '/revoke-instance-access';
        this.hideInsSuffix = '/hide-instance';
        this.showInsSuffix = '/show-instance';
        this.browsableSuffix = '/browsable';
        this.checkConnectionSuffix = '/checkconnection';
        this.refreshSuffix = '/refresh';
        this.pageParameter = 'page=';
        this.nameFilterParameter = 'namefilter=';
        this.serverFilterParameter = 'serverNamefilter=';
        this.versionFilterParameter = 'versionFilter=';
        this.sqlVersionsParameter = 'sqlversions=';
        this.headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        this.versions = [];
    }
    InstanceService.prototype.getPage = function () {
        return this.page;
    };
    InstanceService.prototype.getPageCount = function () {
        return this.pageCount;
    };
    InstanceService.prototype.getPageSize = function () {
        return this.pageSize;
    };
    InstanceService.prototype.getVersions = function () {
        // this.versions.
        return this.versions;
    };
    InstanceService.prototype.checkConnection = function (instance) {
        var url = this.joinStrings([this.baseUrl2, this.checkConnectionSuffix,
            "?", "servername=", instance.serverName,
            "&", "instanceName=", instance.instanceName,
            "&", "login=" + instance.login,
            "&", "pswd=" + instance.password,
            "&", "windowsoauth=" + instance.isWindowsAuthentication]);
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            return parsed;
        })
            .catch(function (error) {
            console.error("iinstanceservice:checkConnection error");
            console.error(error);
        });
    };
    InstanceService.prototype.getBrowsableInstances = function () {
        var _this = this;
        var url = this.joinStrings([this.baseUrl2, this.browsableSuffix]);
        var instances = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var instance = _a[_i];
                    var newInstance = new view_models_1.BrowsableInstance(_this);
                    newInstance.loadFromDto(instance);
                    instances.push(newInstance);
                }
            }
            return instances;
        })
            .catch(function (error) {
            console.error("iinstanceservice:get getBrowsableInstances error");
            console.error(error);
            //this.showError.showError(error);
        });
    };
    InstanceService.prototype.getUserInstances = function (userid, page, nameFilter, serverName, version, vesrsions) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl2, "/assigned?id=", userid, "&", this.pageParameter, page,]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);
        var instances = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var instance = _a[_i];
                    var newInstance = new view_models_1.Instance(_this);
                    newInstance.loadFromDto(instance);
                    instances.push(newInstance);
                }
                _this.page = parsed.pageNumber;
                _this.pageCount = parsed.pagesCount;
                _this.pageSize = parsed.pageSize;
                _this.versions = parsed.versions;
            }
            return instances;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getUserHidenInstances = function (userid, page, nameFilter, serverName, version, vesrsions) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl2, "/hiden-assigned?id=", userid,
            "&", this.pageParameter, page,
        ]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);
        var instances = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var instance = _a[_i];
                    var newInstance = new view_models_1.Instance(_this);
                    newInstance.loadFromDto(instance);
                    instances.push(newInstance);
                }
                _this.page = parsed.pageNumber;
                _this.pageCount = parsed.pagesCount;
                _this.pageSize = parsed.pageSize;
                _this.versions = parsed.versions;
            }
            return instances;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getDeletedInstancesPage = function (page, nameFilter, serverName, version, vesrsions) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl2, "/deleted", "?", this.pageParameter, page,]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);
        var instances = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var instance = _a[_i];
                    var newInstance = new view_models_1.Instance(_this);
                    newInstance.loadFromDto(instance);
                    instances.push(newInstance);
                }
                _this.page = parsed.pageNumber;
                _this.pageCount = parsed.pagesCount;
                _this.pageSize = parsed.pageSize;
                _this.versions = parsed.versions;
            }
            return instances;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getAssignedDeletedInstancesPage = function (userid, page, nameFilter, serverName, version, vesrsions) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl2, "/deleted-assigned?id=", userid, "&", this.pageParameter, page,]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);
        var instances = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var instance = _a[_i];
                    var newInstance = new view_models_1.Instance(_this);
                    newInstance.loadFromDto(instance);
                    instances.push(newInstance);
                }
                _this.page = parsed.pageNumber;
                _this.pageCount = parsed.pagesCount;
                _this.pageSize = parsed.pageSize;
                _this.versions = parsed.versions;
            }
            return instances;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getInstances = function (page, nameFilter, serverName, version, vesrsions) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl2, "?", this.pageParameter, page]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);
        var instances = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var instance = _a[_i];
                    var newInstance = new view_models_1.Instance(_this);
                    newInstance.loadFromDto(instance);
                    instances.push(newInstance);
                }
                _this.page = parsed.pageNumber;
                _this.pageCount = parsed.pagesCount;
                _this.pageSize = parsed.pageSize;
                _this.versions = parsed.versions;
                console.log("instance.service versions= ");
                console.dir(_this.versions);
            }
            return instances;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getInstanceById = function (id) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, ("" + id)]);
        var instance;
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                instance = new view_models_1.Instance(_this);
                instance.loadFromDto(parsed.model);
            }
            return instance;
        })
            .catch(function (error) {
            return _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getLogins = function (instance) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instance.id, this.loginsSuffix]);
        var logins = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var login = _a[_i];
                    var newLogin = new view_models_1.Login(instance, function (principalId) { return _this.getInstancePrincipalPermissions(principalId); });
                    newLogin.loadFromDto(login);
                    logins.push(newLogin);
                }
            }
            return logins;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getInstanceRoles = function (instance) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instance.id, this.instanceRolesSuffix]);
        var roles = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var role = _a[_i];
                    var newRole = new view_models_1.InstanceRole(instance, function (principalId) { return _this.getInstancePrincipalPermissions(principalId); });
                    newRole.loadFromDto(role);
                    roles.push(newRole);
                }
            }
            return roles;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getInstancePrincipalPermissions = function (principalId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, principalId, this.instancePermissionsSuffix]);
        var permissions = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                permissions = parsed.model;
            }
            return permissions;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.updateInstance = function (instance) {
        var _this = this;
        return this.http.put(this.baseUrl, "'" + JSON.stringify(instance) + "'", new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json().model; })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.addInstance = function (newInstance) {
        var _this = this;
        var addedInstance;
        return this.http.post(this.baseUrl, "'" + JSON.stringify(newInstance) + "'", new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                console.info(parsed);
                addedInstance = new view_models_1.Instance(_this);
                addedInstance.loadFromDto(parsed.model);
                _this.grantInstanceAccess(addedInstance.id, _this.authService.getUser().id);
            }
            return addedInstance;
        })
            .catch(function (error) {
            console.error(error);
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.hideInstance = function (instanceId, userId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.hideInsSuffix]);
        var result = false;
        return this.http.put(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                result = true;
            }
            else {
                _this.showError.showError(parsed.errorMessage);
            }
            return result;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.showInstance = function (instanceId, userId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.showInsSuffix]);
        return this.http.put(url, new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json().model; })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.recoverInstance = function (instanceId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, ("" + instanceId)]);
        return this.http.post(url, new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json().model; })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.deleteInstance = function (instanceId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, ("" + instanceId)]);
        return this.http.delete(url, new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json().model; })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    ////// Work with Databases, their Principals and Permissions
    InstanceService.prototype.getDatabases = function (instanceId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, ("" + instanceId), this.databasesSuffix]);
        var databases = [];
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var database = _a[_i];
                    var newDatabase = new view_models_1.Database(_this);
                    newDatabase.loadFromDto(database);
                    databases.push(newDatabase);
                }
            }
            return databases;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getDatabaseRoles = function (database) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, database.id, this.databaseRolesSuffix]);
        var roles = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var role = _a[_i];
                    var newRole = new view_models_1.DatabaseRole(database, function (principalId) { return _this.getDatabasePrincipalPermissions(principalId); });
                    newRole.loadFromDto(role);
                    roles.push(newRole);
                }
            }
            return roles;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.refreshInstance = function (instanceId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, this.refreshSuffix]);
        console.error("refreshInstance url = " + url);
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            return parsed.errorMessage;
        })
            .catch(function (error) {
            console.error("iinstanceservice:refreshInstance error");
            console.error(error);
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getDatabaseUsers = function (database) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, database.id, this.databaseUsersSuffix]);
        var users = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                for (var _i = 0, _a = parsed.model; _i < _a.length; _i++) {
                    var user = _a[_i];
                    var newUser = new view_models_1.DatabaseUser(database, function (principalId) { return _this.getDatabasePrincipalPermissions(principalId); });
                    newUser.loadFromDto(user);
                    users.push(newUser);
                }
            }
            return users;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getDatabasePrincipalPermissions = function (principalId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, principalId, this.databasePermissionsSuffix]);
        var permissions = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                permissions = parsed.model;
            }
            else {
                _this.showError.showError(parsed.errorMessage);
            }
            return permissions;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    ////// Work with Assignments
    InstanceService.prototype.getAssignedUsers = function (instanceId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, this.assignedUsersSuffix]);
        var users = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                users = parsed.model;
            }
            else {
                _this.showError.showError(parsed.errorMessage);
            }
            return users;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.getNotAssignedUsers = function (instanceId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, this.notAssignedUsersSuffix]);
        var users = [];
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                users = parsed.model;
            }
            else {
                _this.showError.showError(parsed.errorMessage);
            }
            return users;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.grantInstanceAccess = function (instanceId, userId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.grantAssignSuffix]);
        var result = false;
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                result = true;
            }
            else {
                _this.showError.showError(parsed.errorMessage);
            }
            return result;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.revokeInstanceAccess = function (instanceId, userId) {
        var _this = this;
        var url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.revokeAssignSuffix]);
        var result = false;
        return this.http.get(url, new http_1.Headers(this.headers))
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            if (!parsed.didError) {
                result = true;
            }
            else {
                _this.showError.showError(parsed.errorMessage);
            }
            return result;
        })
            .catch(function (error) {
            _this.showError.showError(error);
        });
    };
    InstanceService.prototype.joinStrings = function (values) {
        return values.join('');
    };
    InstanceService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [auth_service_1.AuthService, auth_http_1.AuthHttp, show_error_component_1.ShowError])
    ], InstanceService);
    return InstanceService;
}());
exports.InstanceService = InstanceService;
// Server Response
var ServerResponse = (function () {
    function ServerResponse() {
    }
    return ServerResponse;
}());
// Single Server Response
var SingleServerResponse = (function (_super) {
    __extends(SingleServerResponse, _super);
    function SingleServerResponse() {
        _super.apply(this, arguments);
    }
    return SingleServerResponse;
}(ServerResponse));
exports.SingleServerResponse = SingleServerResponse;
// List Server Response
var ListServerResponse = (function (_super) {
    __extends(ListServerResponse, _super);
    function ListServerResponse() {
        _super.apply(this, arguments);
    }
    return ListServerResponse;
}(ServerResponse));
exports.ListServerResponse = ListServerResponse;
//# sourceMappingURL=instance.service.js.map