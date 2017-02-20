import { Component, Input } from '@angular/core';
import { Instance } from './view-models';
import { User } from '../users/user';

@Component({
    moduleId: module.id,
    selector: 'instance-assigns',
    templateUrl: 'instance-assigns.component.html',
    styleUrls: [ 'instance-assigns.component.css' ]
})
export class InstanceAssignsComponent {
    @Input() instance: Instance;
    selectedAssignedUser: User;
    selectedNotAssignedUser: User;

    selectAssignedUser(user: User): void {
        this.selectedAssignedUser = user;
        this.selectedNotAssignedUser = null;
    }

    selectNotAssignedUser(user: User): void {
        this.selectedNotAssignedUser = user;
        this.selectedAssignedUser = null;
    }

    grantAccess(): void {
        this.instance.grantAccess(this.selectedNotAssignedUser);
        this.selectedNotAssignedUser = null;
    }

    revokeAccess(): void {
        this.instance.revokeAccess(this.selectedAssignedUser);
        this.selectedAssignedUser = null;
    }
}