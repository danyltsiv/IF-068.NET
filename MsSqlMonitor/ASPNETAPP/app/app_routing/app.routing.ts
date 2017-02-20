import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import { LoginComponent } from '../components/login/login.component';

const appRoutes: Routes = [
    {
        path: 'home',
        component: HomeComponent
    },

    {
        path: 'login',
        component: LoginComponent
    },

    {
        path: '',
        component: HomeComponent
    }
];

export const AppRoutingModule:
    ModuleWithProviders = RouterModule.forRoot(appRoutes);