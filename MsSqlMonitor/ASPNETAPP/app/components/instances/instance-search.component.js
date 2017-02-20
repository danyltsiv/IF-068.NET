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
var Observable_1 = require('rxjs/Observable');
var Subject_1 = require('rxjs/Subject');
var instance_search_service_1 = require('./instance-search.service');
require('rxjs/add/operator/debounceTime');
require('rxjs/add/operator/distinctUntilChanged');
require('rxjs/add/observable/of');
require('rxjs/add/operator/catch');
require('rxjs/add/operator/switchMap');
var InstanceSearchComponent = (function () {
    function InstanceSearchComponent(instanceSearchService) {
        this.instanceSearchService = instanceSearchService;
        this.selected = "";
        this.instancesCount = 0;
        this.searchTerms = new Subject_1.Subject();
        this.select = new core_1.EventEmitter();
    }
    // Push a search term into the observable stream.
    InstanceSearchComponent.prototype.search = function (term) {
        this.searchTerms.next(term);
    };
    InstanceSearchComponent.prototype.selectInst = function (instance) {
        this.selected = instance.serverName;
        console.log(this.selected);
        this.select.emit(this.selected);
    };
    InstanceSearchComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.instances = this.searchTerms
            .debounceTime(300) // wait for 300ms pause in events
            .distinctUntilChanged() // ignore if next search term is same as previous
            .switchMap(function (term) { return term // switch to new observable each time
            ? _this.instanceSearchService.instInNetworkSearch(term)
            : Observable_1.Observable.of([]); })
            .catch(function (error) {
            // TODO: real error handling
            console.log(error);
            return Observable_1.Observable.of([]);
        });
        this.instances.subscribe(function (g) { return _this.instancesCount = g.length; });
    };
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], InstanceSearchComponent.prototype, "select", void 0);
    InstanceSearchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'instance-search',
            templateUrl: 'instance-search.component.html',
            styleUrls: ['instance-search.component.css'],
            providers: [instance_search_service_1.InstanceSearchService]
        }), 
        __metadata('design:paramtypes', [instance_search_service_1.InstanceSearchService])
    ], InstanceSearchComponent);
    return InstanceSearchComponent;
}());
exports.InstanceSearchComponent = InstanceSearchComponent;
//# sourceMappingURL=instance-search.component.js.map