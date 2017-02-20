import { Injectable } from '@angular/core';
import { Database } from './database';
import { URLSearchParams,
         Jsonp,
         Http,
         Response,
         Headers,
         RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { InstanceService } from '../instances/instance.service';
import { AuthHttp } from '../auth.http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class DatabaseService {

    private apiUrl = 'https://localhost:6297/api/database'; 

    constructor(private http: AuthHttp,
        private instanceService: InstanceService) { }

    getDatabaseList(instanceID: Number): Promise<Database[]> {

        return this.http.get(this.apiUrl)
            .toPromise()
            .then((response: any) => <Database[]>response.json() as Database[])
            .catch(this.handleError);       
    }
    
    private handleError(error: Response | any) {

        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        return Observable.throw(errMsg);
    }
}