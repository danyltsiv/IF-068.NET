"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var InstanceDto = (function () {
    function InstanceDto() {
    }
    return InstanceDto;
}());
exports.InstanceDto = InstanceDto;
var InstanceAddUpdateDto = (function () {
    function InstanceAddUpdateDto() {
    }
    return InstanceAddUpdateDto;
}());
exports.InstanceAddUpdateDto = InstanceAddUpdateDto;
var PrincipalDto = (function () {
    function PrincipalDto() {
    }
    return PrincipalDto;
}());
exports.PrincipalDto = PrincipalDto;
var LoginDto = (function (_super) {
    __extends(LoginDto, _super);
    function LoginDto() {
        _super.apply(this, arguments);
    }
    return LoginDto;
}(PrincipalDto));
exports.LoginDto = LoginDto;
var InstanceRoleDto = (function (_super) {
    __extends(InstanceRoleDto, _super);
    function InstanceRoleDto() {
        _super.apply(this, arguments);
    }
    return InstanceRoleDto;
}(PrincipalDto));
exports.InstanceRoleDto = InstanceRoleDto;
var DatabaseDto = (function () {
    function DatabaseDto() {
    }
    return DatabaseDto;
}());
exports.DatabaseDto = DatabaseDto;
var DatabaseRoleDto = (function (_super) {
    __extends(DatabaseRoleDto, _super);
    function DatabaseRoleDto() {
        _super.apply(this, arguments);
    }
    return DatabaseRoleDto;
}(PrincipalDto));
exports.DatabaseRoleDto = DatabaseRoleDto;
var DatabaseUserDto = (function (_super) {
    __extends(DatabaseUserDto, _super);
    function DatabaseUserDto() {
        _super.apply(this, arguments);
    }
    return DatabaseUserDto;
}(PrincipalDto));
exports.DatabaseUserDto = DatabaseUserDto;
//# sourceMappingURL=dto-models.js.map