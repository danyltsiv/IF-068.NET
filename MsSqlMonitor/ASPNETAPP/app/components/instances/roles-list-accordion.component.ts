import { Component, Input } from '@angular/core';
import { Principal, InstanceRole } from './view-models';

@Component({
    moduleId: module.id,
    selector: 'roles-list-accordion',
    templateUrl: 'roles-list-accordion.component.html'
})
export class RolesListAccordion {
    @Input()
    roles: Principal<any>;
}