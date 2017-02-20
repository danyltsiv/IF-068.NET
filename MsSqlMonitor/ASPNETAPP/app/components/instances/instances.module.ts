import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { JsonpModule } from '@angular/http';
import { CommonModule } from "@angular/common";
import { InstanceListComponent } from "./instance-list.component";
import { DatabaseList } from "./database-list.component";
import { InstanceDetailComponent } from "./instance-detail.component";
import { PermissionsList } from "./permissions-list.component";
import { LoginsList } from "./logins-list.component";
import { InstanceRoutingModule } from "./instances-routing.module";
import { RolesListAccordion } from "./roles-list-accordion.component";
import { RolesList } from "./roles-list.component";
import { DatabaseUsersList } from "./database-users-list.component";
import { InstanceAssignsComponent } from "./instance-assigns.component";
import { InstanceService } from "./instance.service";
import { ShowError, ErrorView } from "./show-error.component";
import { Buffer } from "./buffer";
import { AddBrowsableInstanceComponent } from "./add-browsableinstance.component";

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        JsonpModule,
        NgbModule.forRoot(),
        InstanceRoutingModule
    ],
    declarations: [
        InstanceListComponent,
        InstanceDetailComponent,
        InstanceAssignsComponent,
        PermissionsList,
        RolesListAccordion,
        RolesList,
        LoginsList,
        DatabaseUsersList,
        DatabaseList,
        ErrorView,
        AddBrowsableInstanceComponent,
    ],
    providers: [
        InstanceService,
        ShowError,
        Buffer
    ]
})
export class InstancesModule { }
