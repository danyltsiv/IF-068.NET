import { NgModule, Component, Input, Output, ChangeDetectionStrategy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Database } from './database';
import { Observable } from 'rxjs/Rx';
import { DatabaseService } from './database.service';


@Component({
    moduleId: module.id,
    selector: 'database-list',
    templateUrl: 'database-list.component.html',
    //styleUrls: ['database-list.component.css'],

    //changeDetection: ChangeDetectionStrategy.OnPush
})


export class DatabaseListComponent implements OnInit {

    public databaseList: Database[];


    constructor(private databaseService: DatabaseService, private router: Router) { }

    ngOnInit() : void {


        this.getDatabases();
    }


    getDatabases(): void {


        this.databaseService.getDatabaseList(0)
            .then(bases => { this.databaseList = bases; console.log(this.databaseList); })
            .catch(err => console.log(err));
    }

    gotoDetail(database: Database): void {

        let link = ['/database', database.id];
        this.router.navigate(link);
    }

 
}