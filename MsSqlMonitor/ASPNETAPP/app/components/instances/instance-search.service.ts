import { Injectable }     from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { Instance }           from './view-models';
import {InstanceService} from './instance.service';
import 'rxjs/add/observable/of';

@Injectable()
export class InstanceSearchService {
    constructor(private http: Http, private instanceService: InstanceService) { }


    instInNetworkSearch(term: string): Observable<Instance[]> {
        return null;// this.instanceService.getInstancesInNetwork();
            //.then(instances => instances);//.filter(ins => (ins.serverName + ins.instanceName).includes(term))
    }
}