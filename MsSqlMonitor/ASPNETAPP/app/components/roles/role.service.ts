import { Injectable } from '@angular/core';
import { Role } from './role';
import { AuthHttp } from '../auth.http';
import { URLSearchParams,
         Jsonp,
         Response,
         Headers,
         RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class RoleService {

    private apiUrl = 'https://localhost:44301/api/roles';
    private headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(
        private http: AuthHttp) { }

    getRoleById(id: Number):Promise<Role> {
        return this.http.get(this.apiUrl + "/" + id)
            .toPromise()
            .then((response: any) => <Role>response.json() as Role)
            .catch(this.handleError);
    }

    private handleError(error: Response | any): Promise<any> {
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }

        console.log("UserService get error");
        console.error(errMsg);
        return Promise.reject(errMsg);
    }
}