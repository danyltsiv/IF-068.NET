import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Observable }        from 'rxjs/Observable';
import { Subject }           from 'rxjs/Subject';
import { InstanceSearchService } from './instance-search.service';
import { Instance } from './view-models';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/switchMap';

@Component({
    moduleId: module.id,
    selector: 'instance-search',
    templateUrl: 'instance-search.component.html',
    styleUrls: ['instance-search.component.css'],
    providers: [InstanceSearchService]
})

export class InstanceSearchComponent implements OnInit {
    instances: Observable<Instance[]>;
    public selected: string = "";
    public instancesCount: Number = 0;
    
    private searchTerms = new Subject<string>();

    @Output() select = new EventEmitter();

    constructor(
        private instanceSearchService: InstanceSearchService) { }
    // Push a search term into the observable stream.
    search(term: string): void {
        this.searchTerms.next(term);
    }

    selectInst(instance: Instance) {
        this.selected = instance.serverName;
        console.log(this.selected);
        this.select.emit(this.selected);
    }

    ngOnInit(): void {
        this.instances = this.searchTerms
            .debounceTime(300)        // wait for 300ms pause in events
            .distinctUntilChanged()   // ignore if next search term is same as previous
            .switchMap(term => term   // switch to new observable each time
                // return the http search observable
                ? this.instanceSearchService.instInNetworkSearch(term)
                // or the observable of empty heroes if no search term
                : Observable.of<Instance[]>([]))
            .catch(error => {
                // TODO: real error handling
                console.log(error);
                return Observable.of<Instance[]>([]);
            });
        this.instances.subscribe(g => this.instancesCount = g.length);
    }
}