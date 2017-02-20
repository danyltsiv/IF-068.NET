import { NgModule }     from "@angular/core";
import { RouterModule } from "@angular/router";

import { DatabaseListComponent }    from "./database-list.component";
import { DatabaseDetailComponent } from './database-detail.component';
import { AuthGuardService } from '../auth-guard.service';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'databases',
                component: DatabaseListComponent,
                canActivate: [AuthGuardService]
            },
            {
                path: 'databases/:id',
                component: DatabaseDetailComponent,
                canActivate: [AuthGuardService]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class DatabaseRoutingModule { }
