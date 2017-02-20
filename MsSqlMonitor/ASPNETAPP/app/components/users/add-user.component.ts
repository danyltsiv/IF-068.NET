import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';
import {UserService} from './user.service';
import {User} from './user';

@Component({
    moduleId: module.id,
    selector: 'add-user',
    templateUrl: 'add-user.component.html',
    styleUrls: ['add-user.component.css']
})

export class AddUserComponent implements OnInit {
    submit: boolean;
    login: string;
    role: string;
    password: string;
    confirmPassword: string;
    error: boolean = false;
    errorMsg: string;

    constructor(
        
        private router: Router,
        private route: ActivatedRoute,
        private location: Location,
        private userService: UserService) {
        this.submit = false;
    }

    ngOnInit(): void {
        this.role = "Admin";
    }

    checkPasswordEquality(): void {
        if (this.confirmPassword !== this.password) {
            this.error = true;
            this.errorMsg = "Password mismatch!";
        }
    }

    refreshPage():void {
        this.submit = false;
    }

    goToUserList(): void {
        this.submit = false
        let link = ['/users'];
        this.router.navigate(link);
    }

    addUser(): void {
        this.error = false;
        this.checkPasswordEquality();
        if (!this.error) {
            let user = {
                username: this.login
            };

            this.userService.addUser(user, this.password, this.role).then(users => {
                if (users && users.length) {
                    this.submit = true;
                    console.log(999);
                }//ppop
            }).catch(g => {
                this.error = true;
                this.errorMsg = g
            });
        }
    }
}