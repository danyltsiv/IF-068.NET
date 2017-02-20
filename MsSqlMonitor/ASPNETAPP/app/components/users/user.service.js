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
var auth_http_1 = require('../auth.http');
var http_1 = require('@angular/http');
var instance_service_1 = require('../instances/instance.service');
require('rxjs/add/operator/map');
require('rxjs/add/operator/catch');
require('rxjs/add/operator/toPromise');
var UserService = (function () {
    function UserService(http, instanceService) {
        this.http = http;
        this.instanceService = instanceService;
        this.apiUrl = 'https://localhost:44301/api/users';
        this.headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        this.pageParameter = 'page=';
        this.nameFilterParameter = 'namefilter=';
        this.serverFilterParameter = 'serverNamefilter=';
        this.versionFilterParameter = 'versionFilter=';
        this.sqlVersionsParameter = 'sqlversions=';
        this.versions = [];
    }
    UserService.prototype.getPage = function () {
        return this.page;
    };
    UserService.prototype.getPageCount = function () {
        return this.pageCount;
    };
    UserService.prototype.getPageSize = function () {
        return this.pageSize;
    };
    UserService.prototype.getVersions = function () {
        return this.versions;
    };
    UserService.prototype.getUsersList = function () {
        console.log("service get users");
        return this.http.get(this.apiUrl)
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    UserService.prototype.getUserById = function (id) {
        return this.getUsersList()
            .then(function (users) { return users.find(function (user) { return user.id === id; }); });
    };
    UserService.prototype.joinStrings = function (values) {
        return values.join('');
    };
    UserService.prototype.getUserInstances = function (userId, page, nameFilter, serverName, version, sqlversions) {
        var _this = this;
        if (page == undefined)
            page = 0;
        var url = this.joinStrings([this.apiUrl, "/assigned?id=", userId,
            "&", this.pageParameter, page,
        ]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, sqlversions]);
        console.log(url);
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            console.dir(parsed);
            _this.page = parsed.pageNumber;
            _this.pageCount = parsed.pagesCount;
            _this.pageSize = parsed.pageSize;
            _this.versions = parsed.versions;
            console.log("getUserInstances this.pageCount=" + _this.pageCount);
            return parsed.list;
        })
            .catch(this.handleError);
    };
    UserService.prototype.getNotAssignedInstances = function (userId, page, nameFilter, serverName, version, sqlversions) {
        var _this = this;
        if (page == undefined)
            page = 0;
        var url = this.joinStrings([this.apiUrl, "/not-assigned?id=", userId,
            "&", this.pageParameter, page,
        ]);
        if (nameFilter.length > 0)
            url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0)
            url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0)
            url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, sqlversions]);
        return this.http.get(url)
            .toPromise()
            .then(function (response) {
            var parsed = response.json();
            console.dir(parsed);
            _this.page = parsed.pageNumber;
            _this.pageCount = parsed.pagesCount;
            _this.pageSize = parsed.pageSize;
            _this.versions = parsed.versions;
            console.log("getUserNotAssignedInstances this.pageCount=" + _this.pageCount);
            return parsed.list;
        })
            .catch(this.handleError);
    };
    UserService.prototype.assignInstance = function (userId, instanceId) {
        var body = {
            userId: userId,
            instanceId: instanceId
        };
        return this.http.post(this.apiUrl + "/assign", JSON.stringify(body), new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    UserService.prototype.deassignInstance = function (userId, instanceId) {
        return this.http.delete(this.apiUrl + "/deassign?userid=" + userId + "&instanceid=" + instanceId, new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    UserService.prototype.deleteUser = function (user) {
        console.log("service delete user");
        return this.http.delete(this.apiUrl + '/' + user.id, new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    UserService.prototype.addUser = function (user, password, role) {
        var body = {
            user: user,
            password: password,
            role: role
        };
        return this.http.post(this.apiUrl + "/create", JSON.stringify(body), new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    UserService.prototype.updateUser = function (user, password, role) {
        console.log("service delete user");
        var body = {
            user: user,
            password: password,
            role: role
        };
        return this.http.put(this.apiUrl, JSON.stringify(body), new http_1.RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    UserService.prototype.handleError = function (error) {
        var errMsg;
        if (error instanceof http_1.Response) {
            var body = error.json() || '';
            var err = body.error || JSON.stringify(body);
            errMsg = error.status + " - " + (error.statusText || '') + " " + err;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.log("UserService get error");
        console.error(errMsg);
        return Promise.reject(errMsg); //return Observable.throw(errMsg);
    };
    UserService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [auth_http_1.AuthHttp, instance_service_1.InstanceService])
    ], UserService);
    return UserService;
}());
exports.UserService = UserService;
var PaginatedInstances = (function () {
    function PaginatedInstances() {
    }
    return PaginatedInstances;
}());
exports.PaginatedInstances = PaginatedInstances;
//# sourceMappingURL=user.service.js.map