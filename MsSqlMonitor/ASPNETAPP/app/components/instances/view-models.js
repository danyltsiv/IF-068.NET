"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var dto_models_1 = require('./dto-models');
var BrowsableInstance = (function () {
    function BrowsableInstance(instanceService) {
        this.instanceService = instanceService;
    }
    BrowsableInstance.prototype.loadFromDto = function (instanceDto) {
        this.serverName = instanceDto.serverName;
        this.instanceName = instanceDto.instanceName;
        this.version = instanceDto.version;
    };
    return BrowsableInstance;
}());
exports.BrowsableInstance = BrowsableInstance;
var VersionCheckbox = (function () {
    function VersionCheckbox(version, ischecked, versionnumber) {
        this.version = version;
        this.ischecked = ischecked;
        this.isvisible = true;
        this.versionnumber = versionnumber;
    }
    VersionCheckbox.prototype.setVisibleFalse = function () {
        this.isvisible = false;
        // this.ischecked = true;
    };
    VersionCheckbox.prototype.setVisibleTrue = function () {
        this.isvisible = true;
    };
    return VersionCheckbox;
}());
exports.VersionCheckbox = VersionCheckbox;
// Instance
var Instance = (function () {
    function Instance(instanceService) {
        this.instanceService = instanceService;
    }
    Instance.prototype.loadFromDto = function (instanceDto) {
        this.id = instanceDto.id;
        this.serverName = instanceDto.serverName;
        this.instanceName = instanceDto.instanceName;
        this.status = instanceDto.status;
        this.osVersion = instanceDto.osVersion;
        this.cpuCount = instanceDto.cpuCount;
        this.memory = instanceDto.memory;
        this.isOK = instanceDto.isOK;
        this.isDeleted = instanceDto.isDeleted;
        this.alias = instanceDto.alias;
        this.isHidden = instanceDto.isHidden;
        this.databasesCount = instanceDto.databasesCount;
        this.version = instanceDto.version;
        this.isHidden = instanceDto.isHidden;
    };
    Instance.prototype.toAddUpdateDto = function () {
        var result = new dto_models_1.InstanceAddUpdateDto();
        result.id = this.id;
        result.serverName = this.serverName;
        result.instanceName = this.instanceName;
        result.isWindowsAuthentication = false;
        return result;
    };
    Object.defineProperty(Instance.prototype, "logins", {
        get: function () {
            var _this = this;
            if (this._logins == null) {
                this.instanceService.getLogins(this).then(function (response) { return _this._logins = response; });
                if (this._logins == null)
                    this._logins = [];
            }
            return this._logins;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Instance.prototype, "roles", {
        get: function () {
            var _this = this;
            if (this._roles == null) {
                this.instanceService.getInstanceRoles(this).then(function (response) { _this._roles = response; });
                if (this._roles == null)
                    this._roles = [];
            }
            return this._roles;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Instance.prototype, "assignedUsers", {
        get: function () {
            var _this = this;
            if (this._assignedUsers == null) {
                this._assignedUsers = [];
                this.instanceService.getAssignedUsers(this.id).then(function (response) { return _this._assignedUsers = response; });
            }
            return this._assignedUsers;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Instance.prototype, "notAssignedUsers", {
        get: function () {
            var _this = this;
            if (this._notAssignedUsers == null) {
                this._notAssignedUsers = [];
                this.instanceService.getNotAssignedUsers(this.id).then(function (response) { return _this._notAssignedUsers = response; });
            }
            return this._notAssignedUsers;
        },
        enumerable: true,
        configurable: true
    });
    Instance.prototype.grantAccess = function (user) {
        var _this = this;
        this.instanceService.grantInstanceAccess(this.id, user.id).then(function (response) {
            if (response) {
                _this._assignedUsers.push(user);
                _this._notAssignedUsers.splice(_this._notAssignedUsers.indexOf(user), 1);
            }
        });
    };
    Instance.prototype.revokeAccess = function (user) {
        var _this = this;
        this.instanceService.revokeInstanceAccess(this.id, user.id).then(function (response) {
            if (response) {
                _this._notAssignedUsers.push(user);
                _this._assignedUsers.splice(_this._assignedUsers.indexOf(user), 1);
            }
        });
    };
    return Instance;
}());
exports.Instance = Instance;
// Base Principal
var Principal = (function () {
    function Principal(owner, getPermissions) {
        this.owner = owner;
        this.getPermissions = getPermissions;
    }
    Principal.prototype.loadFromDto = function (dto) {
        this.id = dto.id;
        this.name = dto.name;
        this.type = dto.type;
    };
    Object.defineProperty(Principal.prototype, "permissions", {
        get: function () {
            var _this = this;
            if (this._permissions == null) {
                this.getPermissions(this.id).then(function (response) { return _this._permissions = response; });
                if (this._permissions == null)
                    this._permissions = [];
            }
            return this._permissions;
        },
        enumerable: true,
        configurable: true
    });
    return Principal;
}());
exports.Principal = Principal;
// Instance Login
var Login = (function (_super) {
    __extends(Login, _super);
    function Login() {
        _super.apply(this, arguments);
    }
    Login.prototype.loadFromDto = function (loginDto) {
        _super.prototype.loadFromDto.call(this, loginDto);
        this.isDisabled = loginDto.isDisabled;
        if (loginDto.assignedRolesIds == null)
            this.assignedRolesIds = [];
        else
            this.assignedRolesIds = loginDto.assignedRolesIds;
    };
    Object.defineProperty(Login.prototype, "roles", {
        get: function () {
            if (this._roles == null) {
                this._roles = [];
                for (var _i = 0, _a = this.owner.roles; _i < _a.length; _i++) {
                    var role = _a[_i];
                    if (this.assignedRolesIds.includes(role.id))
                        this._roles.push(role);
                }
            }
            return this._roles;
        },
        enumerable: true,
        configurable: true
    });
    return Login;
}(Principal));
exports.Login = Login;
// Instance Role
var InstanceRole = (function (_super) {
    __extends(InstanceRole, _super);
    function InstanceRole() {
        _super.apply(this, arguments);
    }
    InstanceRole.prototype.loadFromDto = function (instanceRoleDto) {
        _super.prototype.loadFromDto.call(this, instanceRoleDto);
        if (instanceRoleDto.assignedLoginsIds == null)
            this.assignedLoginsIds = [];
        else
            this.assignedLoginsIds = instanceRoleDto.assignedLoginsIds;
    };
    Object.defineProperty(InstanceRole.prototype, "logins", {
        get: function () {
            if (this._logins == null) {
                this._logins = [];
                for (var _i = 0, _a = this.owner.logins; _i < _a.length; _i++) {
                    var login = _a[_i];
                    if (this.assignedLoginsIds.includes(login.id))
                        this._logins.push(login);
                }
            }
            return this._logins;
        },
        enumerable: true,
        configurable: true
    });
    return InstanceRole;
}(Principal));
exports.InstanceRole = InstanceRole;
//Permission
var Permission = (function () {
    function Permission() {
    }
    return Permission;
}());
exports.Permission = Permission;
// Database
var Database = (function () {
    function Database(instanceService) {
        this.instanceService = instanceService;
    }
    Database.prototype.loadFromDto = function (dto) {
        this.id = dto.id;
        this.name = dto.name;
        this.size = dto.size;
        this.createDate = dto.createDate;
    };
    Object.defineProperty(Database.prototype, "users", {
        get: function () {
            var _this = this;
            if (this._users == null) {
                this.instanceService.getDatabaseUsers(this).then(function (response) { _this._users = response; });
                if (this._users == null)
                    this._users = [];
            }
            return this._users;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Database.prototype, "roles", {
        get: function () {
            var _this = this;
            if (this._roles == null) {
                this.instanceService.getDatabaseRoles(this).then(function (response) { _this._roles = response; });
                if (this._roles == null)
                    this._roles = [];
            }
            return this._roles;
        },
        enumerable: true,
        configurable: true
    });
    return Database;
}());
exports.Database = Database;
// DatabaseUser
var DatabaseUser = (function (_super) {
    __extends(DatabaseUser, _super);
    function DatabaseUser() {
        _super.apply(this, arguments);
    }
    DatabaseUser.prototype.loadFromDto = function (dto) {
        _super.prototype.loadFromDto.call(this, dto);
        this._assignedRoles = dto.assignedRoles;
    };
    Object.defineProperty(DatabaseUser.prototype, "roles", {
        get: function () {
            if (this._roles == null) {
                this._roles = [];
                for (var _i = 0, _a = this.owner.roles; _i < _a.length; _i++) {
                    var role = _a[_i];
                    if (this._assignedRoles.includes(role.id))
                        this._roles.push(role);
                }
            }
            return this._roles;
        },
        enumerable: true,
        configurable: true
    });
    return DatabaseUser;
}(Principal));
exports.DatabaseUser = DatabaseUser;
// DatabaseRole
var DatabaseRole = (function (_super) {
    __extends(DatabaseRole, _super);
    function DatabaseRole() {
        _super.apply(this, arguments);
    }
    DatabaseRole.prototype.loadFromDto = function (dto) {
        _super.prototype.loadFromDto.call(this, dto);
        this._assignedUsers = dto.assignedUsers;
    };
    Object.defineProperty(DatabaseRole.prototype, "users", {
        get: function () {
            if (this._users == null) {
                this._users = [];
                for (var _i = 0, _a = this.owner.users; _i < _a.length; _i++) {
                    var user = _a[_i];
                    if (this._assignedUsers.includes(user.id))
                        this._users.push(user);
                }
            }
            return this._users;
        },
        enumerable: true,
        configurable: true
    });
    return DatabaseRole;
}(Principal));
exports.DatabaseRole = DatabaseRole;
//# sourceMappingURL=view-models.js.map