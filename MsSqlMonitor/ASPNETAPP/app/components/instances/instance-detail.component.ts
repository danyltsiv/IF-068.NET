import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { Location } from '@angular/common';
import { NgbModal, ModalDismissReasons, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { InstanceAddUpdateDto } from "./dto-models";
import { InstanceService } from './instance.service';
import { Instance, Login, InstanceRole } from './view-models';
import { Buffer } from './buffer';

@Component({
    moduleId: module.id,
    selector: 'instance-details',
    templateUrl: 'instance-detail.component.html',
    styleUrls: ['instance-detail.component.css']
})
export class InstanceDetailComponent implements OnInit, OnDestroy {
    @Input()
    instance: Instance;
    logins: Login[];
    roles: InstanceRole[];
    selectedLogin: Login;
    isChangeCredentialsCollapsed = true;
    instToUpdate: InstanceAddUpdateDto;


    constructor(
        private router: Router,
        private dataService: InstanceService,
        private buffer: Buffer,
        private modalService: NgbModal,
        private route: ActivatedRoute,
        private location: Location) { }

    ngOnInit(): void {
        if (this.buffer.instanceForDetails != null)
            this.instance = this.buffer.instanceForDetails;
        else {
            let instId: number;
            this.route.params.forEach((params: Params) => instId = +params['id']);
            this.dataService.getInstanceById(instId).then(instance => this.instance = instance);
        }
    }

    ngOnDestroy(): void {
        this.buffer.instanceForDetails = null;
    }

    onSaveChanges(): void {
        
    }

    onSelectLogin(login: Login): void {
        console.info(`In selected login: login name '${login.name}'`);
        console.info(`Roles: ${login.roles.length}`);
        this.selectedLogin = login;
        console.info(`In selected login: selected login name '${this.selectedLogin.name}'`);
    }

    openModal(content: any): void {
        console.log('at openModal');
        this.modalService.open(content, { size: 'lg' }).result.then(result =>
        { this.onSaveChanges(); console.info("onSaveChanges"); },
            reason =>{}
        );
    }

    editInstance(editInstanceModal: any, instance: Instance): void {
        this.instToUpdate = instance.toAddUpdateDto();
        this.modalService.open(editInstanceModal, { size: 'sm' })
            .result.then(() => {
                this.dataService.updateInstance(this.instToUpdate).then(result => {
                    instance.serverName = result.serverName;
                    instance.instanceName = result.instanceName;
                })
                    .catch(reason => `Instance edit falled: ${reason}`);
                this.instance = null;
            })
            .catch(reason => console.info('Instance edit cancelled'));
    }

    deleteInstance(deleteInstanceModal: any, instance: Instance): void {
        this.dataService.deleteInstance(instance.id);
        this.modalService.open(deleteInstanceModal, { size: 'lg' });
    }

    showInstanceList() {
        let link = ['/instances'];
        this.router.navigate(link);
    }

    showDatabases() {
        let link = ['/instance', this.instance.id, 'databases'];
        this.router.navigate(link);
    }
}