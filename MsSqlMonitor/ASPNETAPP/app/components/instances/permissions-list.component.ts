import { Component, Input } from '@angular/core';
import { Permission } from './view-models';

@Component({
    moduleId: module.id,
    selector: 'permissions-list',
    templateUrl: 'permissions-list.component.html'
})
export class PermissionsList {
    @Input()
    permissions: Permission[];

    @Input()
    emptyListMessage = '';
}