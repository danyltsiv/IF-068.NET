import { Component, Input } from '@angular/core';

import { InstanceService } from './instance.service';
import { DatabaseUser, Permission } from './view-models'

@Component({
    moduleId: module.id,
    selector: 'database-users-list',
    templateUrl: 'database-users-list.component.html'
})
export class DatabaseUsersList {
    selectedUser: DatabaseUser;

    @Input()
    users: DatabaseUser[];

    constructor(
        private instanceService: InstanceService) { }

    onSelectUser(user: DatabaseUser): void {
        this.selectedUser = user;
    }
}