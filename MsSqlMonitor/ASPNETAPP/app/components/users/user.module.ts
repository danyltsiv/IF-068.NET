import { NgModule }       from "@angular/core";
import { CommonModule }   from "@angular/common";
import { FormsModule }    from "@angular/forms";
import { UserListComponent }    from "./user-list.component";
import { UserDetailComponent } from './user-detail.component';
import { AddUserComponent } from "./add-user.component";
import { UserService } from "./user.service";
import { UserRoutingModule } from "./user-routing.module";
import { RoleService } from '../roles/role.service';
import { InstanceService } from '../instances/instance.service';
import { InstanceSearchComponent } from '../instances/instance-search.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        UserRoutingModule
    ],
    declarations: [
        UserListComponent,
        UserDetailComponent,
        InstanceSearchComponent,
        AddUserComponent,
    ],
    providers: [
        UserService,
        InstanceService,
        RoleService
    ]
})
export class UsersModule { }
