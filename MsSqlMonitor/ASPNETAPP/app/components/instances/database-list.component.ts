import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import { NgbModal, ModalDismissReasons, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { Database } from './view-models';
import { InstanceService } from './instance.service';

@Component({
    moduleId: module.id,
    templateUrl: './database-list.component.html'
})
export class DatabaseList implements OnInit {

    databases: Database[];
    selectedDatabase: Database;

    constructor(private instanceService: InstanceService,
        private modalService: NgbModal,
        private route: ActivatedRoute,
        private location: Location) { }

    ngOnInit(): void {
        let instId: number;
        this.route.params.forEach((params: Params) => instId = +params['id']);
        this.instanceService.getDatabases(instId)
            .then(result => { this.databases = result; console.info(this.databases.length); })
            .catch(reason => console.error(reason));
    }

    openModal(content: any, selectedDatabase: Database) {
        this.selectedDatabase = selectedDatabase;
        this.modalService.open(content, { size: 'lg' })
            .result.then(() => {
                this.selectedDatabase = null;
            });
    }
}