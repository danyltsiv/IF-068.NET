import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
    moduleId: module.id,
    selector: 'login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css']
})
export class LoginComponent {
    loginForm: any = null;
    loginError: boolean = false;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router) {

        if (this.authService.isLoggedIn()) {
            this.router.navigate([""]);
        }

        this.loginForm = fb.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    performLogin(e: any) {
        e.preventDefault();
        var username = this.loginForm.value.username;
        var password = this.loginForm.value.password;

        this.authService.login(username, password)
            .subscribe((data: any) => {
                this.loginError = false;
                this.router.navigate(['']);
            },

            (err: any) => {
                this.loginError = true;
            });
    }
}