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
var ShowError = (function () {
    function ShowError() {
        this.errorViews = [];
    }
    ShowError.prototype.showError = function (error) {
        console.error(error);
        for (var _i = 0, _a = this.errorViews; _i < _a.length; _i++) {
            var errorView = _a[_i];
            errorView.setError(error);
        }
    };
    ShowError.prototype.addErrorView = function (errorView) {
        if (!this.errorViews.includes(errorView))
            this.errorViews.push(errorView);
    };
    ShowError.prototype.deleteErrorView = function (errorView) {
        if (this.errorViews.includes(errorView))
            this.errorViews.slice(this.errorViews.indexOf(errorView), 1);
    };
    ShowError = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], ShowError);
    return ShowError;
}());
exports.ShowError = ShowError;
var ErrorView = (function () {
    function ErrorView(showError) {
        this.showError = showError;
        this.errorText = null;
        this.timeout = 7000;
    }
    ErrorView.prototype.setError = function (errorText) {
        var _this = this;
        this.errorText = errorText;
        if (this.timer != null)
            clearTimeout(this.timer);
        this.timer = setTimeout(function () {
            _this.errorText = null, _this.timeout;
            _this.timer = null;
        }, this.timeout);
    };
    ErrorView.prototype.ngOnInit = function () {
        this.showError.addErrorView(this);
    };
    ErrorView.prototype.ngOnDestroy = function () {
        this.showError.deleteErrorView(this);
    };
    ErrorView.prototype.closeErrorView = function () {
        this.errorText = null;
        if (this.timer != null)
            clearTimeout(this.timer);
    };
    ErrorView = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'error-view',
            templateUrl: 'show-error.component.html'
        }), 
        __metadata('design:paramtypes', [ShowError])
    ], ErrorView);
    return ErrorView;
}());
exports.ErrorView = ErrorView;
//# sourceMappingURL=show-error.component.js.map