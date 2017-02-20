import { NgModule, Component, Input, Output, ChangeDetectionStrategy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { User } from './user';
import { Role } from '../roles/role';
import { Observable } from 'rxjs/Rx';
import { UserService } from './user.service';
import { RoleService } from '../roles/role.service';


@Component({
    moduleId: module.id,
    selector: 'user-list',
    templateUrl: 'user-list.component.html',
    styleUrls: ['user-list.component.css'],
})

export class UserListComponent implements OnInit {

    public usersList: User[];

    constructor(private userService: UserService, private router: Router, private roleService: RoleService) { }

    ngOnInit() : void {
        this.getUsers();
    }

    getUsers(): void {
        this.userService.getUsersList()
            .then(users => { this.usersList = users })
            .then(() => this.usersList.forEach(g => {
                this.getUserRole(g.roles[0].roleId).then(resp => g.role = resp.name);
            }))
            .catch(err => console.log(err));
    }

    gotoDetail(user: User): void {
        let link = ['/users', user.id];
        this.router.navigate(link);
    }

    gotoAddView(): void {
        let link = ['/user-add'];
        this.router.navigate(link);
    }

    deleteUser(user: User): void {
        if (confirm("Are you sure you want to remove " + user.username + "???")) {
            this.userService.deleteUser(user)
                .then(users => { this.usersList = users; })
                .catch(err => console.log(err));
        }
    }

    getUserRole(id: Number): Promise<Role> {
        return this.roleService.getRoleById(id).then(response => response);
    }
}