import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Instance, Login, InstanceRole, Permission } from './view-models';

@Injectable()
export class MonitorDataService
{
    private instancesUrl = 'https://localhost:5000/api/instances/';
    private loginsUrl = 'https://localhost:5000/api/instances/logins/';
    private rolesUrl = 'https://localhost:5000/api/instances/roles/';

    private headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) { }

    getInstances(): Promise<Instance[]>{
        return this.http.get(this.instancesUrl)
            .toPromise()
            .then(response => response.json().model as Instance[])
            .catch(error => console.error(error));
    }

    getInstanceById(id: number): Promise<Instance> {
        let url = this.instancesUrl.concat(`${id}`);
        return this.http.get(url)
            .toPromise()
            .then(response => response.json().model as Instance)
            .catch(error => console.error(error));
    }

    updateInstance(instance: Instance): Promise<Instance> {
        return this.http.put(this.instancesUrl, `'${JSON.stringify(instance)}'`, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then(response => response.json().model as Instance)
            .catch(error => console.error(error));
    }

    getInstanceLogins(id: number): Promise<Login[]> {
        let url = this.loginsUrl.concat(`${id}`);
        return this.http.get(url)
            .toPromise()
            .then(response => response.json().model as Login[])
            .catch(error => console.error(error));
    }
}