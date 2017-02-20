import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../auth.service";

@Component({
    moduleId: module.id,
    selector: 'navbar',
    templateUrl: 'navbar.component.html',
})
export class NavbarComponent {
    branding = 'SQL Monitor System';

    constructor(
        public router: Router,
        public authService: AuthService) { }

    logout(): boolean {
        if (this.authService.logout()) {
            this.router.navigate([""]);
        }
        return false;
    }
}