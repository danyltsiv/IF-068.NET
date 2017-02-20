import { Component } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
    moduleId: module.id,
    selector: 'home',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.css']
})
export class HomeComponent {
    private mainText: string;
    private text: string;

    constructor(public authService: AuthService) {
        this.mainText = 'Save 2 hours each day monitoring your SQL Servers. ';
        this.text = `SQL Monitor is a SQL server monitoring tool that transforms
                           the way you look at your database. It cuts your daily check
                            to minutes, with a web-based overview of all your SQL Servers. `;
    }
}

