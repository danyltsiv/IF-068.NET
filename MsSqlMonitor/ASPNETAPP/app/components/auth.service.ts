import { Injectable, OnInit, EventEmitter } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { User } from './users/user';
import { Role } from '../components/roles/role';
import { AuthHttp } from './auth.http';
import { RoleService } from '../components/roles/role.service';

@Injectable()
export class AuthService {
    private authKey = 'auth';
    private roleKey = '#role';
    private userUrl = 'https://localhost:44301';
    public token: string;
    private user: User;
    private role: string;

    constructor(
        private http: AuthHttp,
        private roleService: RoleService) {
      
    }

    getUser(): User
    {
        if (this.user == undefined)
        {
            console.log("Restore user");
            this.user = new User();
            this.user.id = parseInt( localStorage.getItem("#id"));
            this.user.username = localStorage.getItem("#username");
            this.user.role = localStorage.getItem("#role");
        }
        return this.user;
    }

    login(username: string, password: string): any {

        var data = 'grant_type=password&username=' + username + '&password=' + password + '&client_id=MsSqlMonitor';
        var url = '/token';

        return this.http.post(
            url,
            data,
            new RequestOptions({
                headers: new Headers({
                    'Content-Type': 'application/x-www-form-urlencoded'
                })
            }))
            .map((response: any) => {
                var auth = response.json();
                this.setAuth(auth);
                this.getUserByName(username,password);               
                return auth;
            });       
    }

    logout(): boolean {
        this.setAuth(null);
        this.setRole(null);

        localStorage.setItem("#id",null);
        localStorage.setItem("#username", null);
        localStorage.setItem("#role", null);

        return false;
    }

    isLoggedIn(): boolean {
        return localStorage.getItem(this.authKey) != null;
    }

    getAuth(): any {
        var i = localStorage.getItem(this.authKey);

        if (i) {
            return JSON.parse(i);
        }
        else {
            return null;
        }
    }

    setAuth(auth: any): boolean {

        if (auth) {
            localStorage.setItem(this.authKey, JSON.stringify(auth));
        }
        else {
            localStorage.removeItem(this.authKey);
        }
        return true;
    }

    setRole(role: any): boolean {

        if (role) {
            localStorage.setItem(this.roleKey, JSON.stringify(role));
        }
        else {
            localStorage.removeItem(this.roleKey);
        }
        return true;
    }

    isAdmin(username: string): boolean {
        let i =localStorage.getItem(this.roleKey);
        if (i != null) {
            return i.includes("Admin");
        }
        return false;
    }

    isUser(username: string): boolean {
        let i = localStorage.getItem(this.roleKey);
        if (i != null) {
            return i.includes("User");
        }
        return false;
    }

    isGuest(username: string): boolean {
        let i = localStorage.getItem(this.roleKey);
        if (i != null) {
            return i.includes("Guest");
        }
        return false;
    }

    getUserByName(username: string, password: string): any {

        this.http.get(this.userUrl + '/api/users/username?username=' + username)
            .toPromise()
            .then((response: any) => {
                this.user = <User>response.json() as User;
                this.getUserRole(this.user.roles[0].roleId)
                    .then(responce => this.role = responce.name)
                    .then(respone => localStorage.setItem("#role", JSON.stringify(this.role)));

               localStorage.setItem("#id", JSON.stringify(this.user.id));
               localStorage.setItem("#username", JSON.stringify(this.user.username));
            });
    }
    getUserRole(id: Number): Promise<Role> {
        return this.roleService.getRoleById(id).then(response => response);
    }
}