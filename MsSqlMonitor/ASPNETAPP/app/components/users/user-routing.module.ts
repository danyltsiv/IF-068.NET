import { NgModule }     from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserListComponent }    from './user-list.component';
import { UserDetailComponent } from './user-detail.component';
import { AddUserComponent } from './add-user.component';
import { AuthGuardService } from '../auth-guard.service';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'users',
                component: UserListComponent,
                canActivate: [AuthGuardService]
            },

            {
                path: 'add-user',
                component: AddUserComponent,
                canActivate: [AuthGuardService]
            },


            {
                path: 'users/:id',
                component: UserDetailComponent,
                canActivate: [AuthGuardService]
            },
            {
                path: 'user-add',
                component: AddUserComponent,
                canActivate: [AuthGuardService]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class UserRoutingModule { }
