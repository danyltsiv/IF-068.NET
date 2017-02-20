import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpModule, XHRBackend, JsonpModule } from '@angular/http';
import { UsersModule } from '../components/users/user.module';
import { AuthService } from '../components/auth.service';
import { AuthGuardService } from '../components/auth-guard.service';
import { AppComponent } from '../app_component/app.component';
import { LoginComponent } from '../components/login/login.component';
import { AppRoutingModule } from '../app_routing/app.routing';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { HomeComponent } from '../components/home/home.component';
import { DropdownDirective } from '../dropdown_directive/dropdown.directive';
import { InstancesModule } from '../components/instances/instances.module';
import { AuthHttp } from '../components/auth.http'; 
import { MonitorDataService } from '../components/instances/monitor-data.service';
import { AddBrowsableInstanceComponent } from "../components/instances/add-browsableinstance.component";
import { AuthenticationConnectionBackend } from "../authenticationConnectionBackend";

@NgModule({
    imports: [BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        JsonpModule,
        NgbModule.forRoot(),
        InstancesModule,
        UsersModule,
        HttpModule,
        AppRoutingModule],

    declarations: [AppComponent,
        NavbarComponent,
        HomeComponent,
        DropdownDirective,
        LoginComponent
    ],
    bootstrap: [AppComponent],
    providers: [MonitorDataService,
                AuthHttp,
                AuthService,
                AuthGuardService,
                {
                    provide: XHRBackend,
                    useClass: AuthenticationConnectionBackend
                }]
})
export class AppModule { }
