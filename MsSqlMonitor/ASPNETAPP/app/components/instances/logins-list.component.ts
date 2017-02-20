import { Component, Input } from '@angular/core';

import { InstanceService } from './instance.service';
import { Login, Permission } from './view-models'

@Component({
    moduleId: module.id,
    selector: 'logins-list',
    templateUrl: 'logins-list.component.html'
})
export class LoginsList {
    selectedLogin: Login;

    @Input()
    logins: Login[];

    constructor(
        private instanceService: InstanceService) { }
    
    onSelectLogin(login: Login): void {
        this.selectedLogin = login;
    }
}