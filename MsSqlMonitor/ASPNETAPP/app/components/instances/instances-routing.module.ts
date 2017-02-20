import { NgModule }     from "@angular/core";
import { RouterModule } from "@angular/router";

import { InstanceListComponent }    from "./instance-list.component";
import { InstanceDetailComponent } from "./instance-detail.component";
import { DatabaseList } from "./database-list.component";
import { AuthGuardService } from '../auth-guard.service';


import { AddBrowsableInstanceComponent } from "./add-browsableinstance.component";

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'instances',
                component: InstanceListComponent,
                canActivate: [AuthGuardService]
            },

            {
                path: 'add-instance',
                component: AddBrowsableInstanceComponent,
                canActivate: [AuthGuardService]
            },

            {
                path: 'instance/:id',
                component: InstanceDetailComponent,
                canActivate: [AuthGuardService]
            },

            {
                path: 'instance/:id/databases',
                component: DatabaseList,
                canActivate: [AuthGuardService]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class InstanceRoutingModule { }
