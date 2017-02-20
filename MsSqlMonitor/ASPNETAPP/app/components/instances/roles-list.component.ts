import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { InstanceRole } from './view-models';

@Component({
    moduleId: module.id,
    selector: 'roles-list',
    templateUrl: 'roles-list.component.html'
})
export class RolesList implements OnChanges {
    @Input()
    roles: InstanceRole[];

    selectedRole: InstanceRole;

    onSelectRole(role: InstanceRole){
        this.selectedRole = role;
    }

    ngOnChanges(changes: SimpleChanges) {
        if (this.roles == null || this.roles.length == 0)
            this.selectedRole = null;
    }
}