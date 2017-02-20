import { Injectable } from '@angular/core';
import { User } from './user';
import { Instance } from '../instances/view-models';
import { AuthHttp } from '../auth.http';
import { URLSearchParams, Jsonp, Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import {InstanceService} from '../instances/instance.service';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class UserService {

    private apiUrl = 'https://localhost:44301/api/users';  
    private headers = new Headers({ 'Content-Type': 'application/json' });
    private pageParameter = 'page=';
    private nameFilterParameter = 'namefilter=';
    private serverFilterParameter = 'serverNamefilter=';
    private versionFilterParameter = 'versionFilter=';
    private sqlVersionsParameter = 'sqlversions=';

    page: number;
    pageCount: number;
    pageSize: number;
    versions: string[] = [];

    constructor(
        private http: AuthHttp,
        private instanceService: InstanceService
    ) { }


    getPage(): number {
        return this.page;
    }

    getPageCount(): number {
        return this.pageCount;
    }

    getPageSize(): number {
        return this.pageSize;
    }

    getVersions(): string[] {

        return this.versions;
    }

    getUsersList(): Promise<User[]> {

        console.log("service get users");

        return this.http.get(this.apiUrl)
            .toPromise()
            .then((response: any) => <User[]>response.json() as User[])
            .catch(this.handleError);
    }

    getUserById(id: Number): Promise<User> {
        return this.getUsersList()
            .then(users => users.find(user => user.id === id));
    }

    private joinStrings(values: any[]) {
        return values.join('');
    }

    getUserInstances(userId: Number, page: number, nameFilter: string, serverName: string, version: string,sqlversions: string): Promise<Instance[]> {
        if (page == undefined) page = 0;
        let url = this.joinStrings([this.apiUrl, "/assigned?id=", userId,
            "&", this.pageParameter, page,
        ]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, sqlversions]);
        console.log(url);
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as PaginatedInstances;
                console.dir(parsed);

                this.page = parsed.pageNumber;
                this.pageCount = parsed.pagesCount;
                this.pageSize = parsed.pageSize;
                this.versions = parsed.versions;

                console.log("getUserInstances this.pageCount=" + this.pageCount);

                return parsed.list;
            })
            .catch(this.handleError);
    }

    getNotAssignedInstances(userId: Number, page: number, nameFilter: string, serverName: string, version: string, sqlversions: string): Promise<Instance[]> {
        if (page == undefined) page = 0;
        let url = this.joinStrings([this.apiUrl, "/not-assigned?id=", userId,
            "&", this.pageParameter, page,
        ]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);
        url = this.joinStrings([url, "&", this.sqlVersionsParameter, sqlversions]);
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as PaginatedInstances;
                console.dir(parsed);
                this.page = parsed.pageNumber;
                this.pageCount = parsed.pagesCount;
                this.pageSize = parsed.pageSize;
                this.versions = parsed.versions;

                console.log("getUserNotAssignedInstances this.pageCount=" + this.pageCount);

                return parsed.list;
            })
            .catch(this.handleError);
    }
    
    assignInstance(userId: Number, instanceId: Number): Promise<Instance[]> {
        let body = {
            userId: userId,
            instanceId: instanceId
        }
        return this.http.post(this.apiUrl + "/assign", JSON.stringify(body), new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => <Instance[]>response.json() as Instance[])
            .catch(this.handleError);
    }

    deassignInstance(userId: Number, instanceId: Number): Promise<Instance[]> {

        return this.http.delete(this.apiUrl + "/deassign?userid=" + userId + "&instanceid=" + instanceId,
            new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => <Instance[]>response.json() as Instance[])
            .catch(this.handleError);
    }

    deleteUser(user: User): Promise<User[]> {
        console.log("service delete user");
        return this.http.delete(this.apiUrl + '/' + user.id, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => <User[]>response.json() as User[])
            .catch(this.handleError);
    }

    addUser(user: any, password: string, role: string): Promise<User[]> {
        let body = {
            user: user,
            password: password,
            role: role
        }
        return this.http.post(this.apiUrl + "/create", JSON.stringify(body), new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => <User[]>response.json() as User[])
            .catch(this.handleError);
    }

    updateUser(user: User, password: string, role: string): Promise<User> {
        console.log("service delete user");
        let body = {
            user: user,
            password: password,
            role: role
        }
        return this.http.put(this.apiUrl, JSON.stringify(body), new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => <User>response.json() as User)
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
        return Promise.reject(errMsg);//return Observable.throw(errMsg);
    }
}


export class PaginatedInstances {
    list: Instance[];
    pageNumber: number;
    pageSize: number;
    pagesCount: number;
    error: string;
    versions: string[];
}