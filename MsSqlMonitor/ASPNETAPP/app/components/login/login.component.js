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
var forms_1 = require('@angular/forms');
var auth_service_1 = require('../auth.service');
var LoginComponent = (function () {
    function LoginComponent(fb, authService, router) {
        this.fb = fb;
        this.authService = authService;
        this.router = router;
        this.loginForm = null;
        this.loginError = false;
        if (this.authService.isLoggedIn()) {
            this.router.navigate([""]);
        }
        this.loginForm = fb.group({
            username: ['', forms_1.Validators.required],
            password: ['', forms_1.Validators.required]
        });
    }
    LoginComponent.prototype.performLogin = function (e) {
        var _this = this;
        e.preventDefault();
        var username = this.loginForm.value.username;
        var password = this.loginForm.value.password;
        this.authService.login(username, password)
            .subscribe(function (data) {
            _this.loginError = false;
            _this.router.navigate(['']);
        }, function (err) {
            _this.loginError = true;
        });
    };
    LoginComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'login',
            templateUrl: 'login.component.html',
            styleUrls: ['login.component.css']
        }), 
        __metadata('design:paramtypes', [forms_1.FormBuilder, auth_service_1.AuthService, router_1.Router])
    ], LoginComponent);
    return LoginComponent;
}());
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map