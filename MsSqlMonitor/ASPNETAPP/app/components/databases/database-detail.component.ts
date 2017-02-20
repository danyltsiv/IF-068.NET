import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';

import {Database} from './database';

import { DatabaseService } from './database.service';



@Component({
    moduleId: module.id,
    selector: 'database-detail',
    templateUrl: 'database-detail.component.html'
})
export class DatabaseDetailComponent implements OnInit {

    database: Database;

    constructor(private route: ActivatedRoute, 
        private location: Location, private databaseService: DatabaseService) { }

    ngOnInit(): void {
       
    }

   
}