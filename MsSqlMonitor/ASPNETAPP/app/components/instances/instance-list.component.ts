import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { Location } from '@angular/common';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { User } from '../users/user';
import { VersionCheckbox,Instance, Login, InstanceRole } from "./view-models";
import { InstanceAddUpdateDto } from "./dto-models";
import { InstanceService } from "./instance.service";
import { Buffer } from "./buffer";
import { AuthService } from '../auth.service';

@Component({
    moduleId: module.id,
    selector: 'instance-list',
    templateUrl: 'instance-list.component.html',
    styleUrls: ['instance-list.component.css']
})
export class InstanceListComponent implements OnInit {
    instances: Instance[];
    selectedInstance: Instance;
    newInstance: InstanceAddUpdateDto;
    instToUpdate: InstanceAddUpdateDto;
    pages: number[];
    page: number;
    pageCount: number;
    pageSize: number;
    instanceNameFilter: string;
    instanceServerFilter: string;
    versionFiler: string;
    user: User;
    viewMode: Mode;
    sqlVersions: VersionCheckbox[];
    message: string;

    @ViewChild('okModal') okModal: string;

    constructor(
        private router: Router,
        private instanceService: InstanceService,
        private buffer: Buffer,
        private authService: AuthService,
        private modalService: NgbModal,
        private location: Location) {
    }

    ngOnInit(): void {

        this.initSQLVersionsArray();
        this.message = "";

        this.page = 0;       
        this.pages = [];
        this.user = this.authService.getUser();
        console.log("instance list, cur user is: " + this.user.username);
        this.instanceNameFilter = "";
        this.instanceServerFilter = "";
        this.versionFiler = "";
        this.viewMode = Mode.instances;
        this.getInstancesPage(0);
    }


    initSQLVersionsArray():void
    {
        this.sqlVersions = [];
        this.sqlVersions.push(new VersionCheckbox("others", true,""));
        this.sqlVersions.push(new VersionCheckbox("2016", true, "13"));
        this.sqlVersions.push(new VersionCheckbox("2014", true, "12"));
        this.sqlVersions.push(new VersionCheckbox("2012", true, "11"));
        this.sqlVersions.push(new VersionCheckbox("2008", true, "10"));
        this.sqlVersions.push(new VersionCheckbox("2005", true, "9"));
        this.sqlVersions.push(new VersionCheckbox("2000", true, "8"));
    }

    setVisibleVersions(arr: string[]): void {

        if (arr == null) return;

        for (var i = 0; i < this.sqlVersions.length; i++) this.sqlVersions[i].setVisibleFalse();

        for (var i = 0; i < arr.length; i++) {
            switch (arr[i]) {

                case "13": this.sqlVersions[1].setVisibleTrue(); break;
                case "12": this.sqlVersions[2].setVisibleTrue(); break;
                case "11": this.sqlVersions[3].setVisibleTrue(); break;
                case "10": this.sqlVersions[4].setVisibleTrue(); break;
                case "9": this.sqlVersions[5].setVisibleTrue(); break;
                case "8": this.sqlVersions[6].setVisibleTrue(); break;
                default: this.sqlVersions[0].setVisibleTrue(); break;
     
            }

        }
    }

    getVersionsString(): string
    {
        let res = "";
        for (var i = 0; i < this.sqlVersions.length; i++) {
            if (this.sqlVersions[i].ischecked) res = res + "1";
            else res = res + "0";
        }
        return res;
    }

    checkConnection(instance: InstanceAddUpdateDto): void {

        this.instanceService.checkConnection(instance)
            .then(result => {

                //alert(result);
                this.showModalMessage(result);

            })
            .catch(error => {
                console.error("Can't connect!");
                console.error(error);
            }
            );
    }

    filterInstancesNames(value: string, event: any): void {
        this.instanceNameFilter = value;
        this.getInstancesPage(0);
    }

    filterInstancesServers(value: string, event: any): void {
        this.instanceServerFilter = value;
        this.getInstancesPage(0);
    }

    filterInstancesVersions(value: string, event: any): void {
        this.versionFiler = value;
        this.getInstancesPage(0);
    }

    onVersionChange(event: any, index: number): void
    {
        console.log(event.target.checked);
        console.log(index);
        this.sqlVersions[index].ischecked = event.target.checked;
        this.getInstancesPage(0);
    }

    getInstancesPage(i: number): void {
        if (this.user == undefined)
        {
            this.authService.logout(); return;
        }     
        this.page = i;
        if (this.authService.isAdmin(this.user.username)) {
            this.instanceService.getInstances(this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
                .then((instances: any) => {
                    this.instances = instances;
                    this.page = this.instanceService.getPage();
                    this.pageCount = this.instanceService.getPageCount();
                    this.pageSize = this.instanceService.getPageSize();

                    //versions

                    this.setVisibleVersions(this.instanceService.getVersions());

                    console.log("get all instanes page=" + i);
                    console.log("pageCount " + this.pageCount);
                    console.log("page " + this.page);
                    console.dir(instances);

                    this.pages = [];
                    for (var i = 0; i < this.pageCount; i++) {
                        this.pages.push(i + 1);
                    }
                })
                .catch((error: any) => {
                    console.error("instancel list component for admin: getInstances");
                    console.error(error);
                });
        }
        else {
            this.instanceService.getUserInstances(this.user.id, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
                .then((instances: any) => {
                    this.instances = instances;
                    this.page = this.instanceService.getPage();
                    this.pageCount = this.instanceService.getPageCount();
                    this.pageSize = this.instanceService.getPageSize();

                  

                    this.setVisibleVersions(this.instanceService.getVersions());

                    console.log("get user instanes page=" + i);
                    console.log("pageCount " + this.pageCount);
                    console.log("page " + this.page);
                    console.dir(instances);

                    this.pages = [];
                    for (var i = 0; i < this.pageCount; i++) {
                        this.pages.push(i + 1);
                    }
                })
                .catch((error: any) => {
                    console.error("instancel list component for user or guest: getUserInstances");
                    console.error(error);
                });
        }
    }

    getUserHidenInstancesPage(i: number): void {
        this.page = i;
        let user = this.authService.getUser();

        this.instanceService.getUserHidenInstances(user.id, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then((instances: any) => {
                this.instances = instances;
                this.page = this.instanceService.getPage();
                this.pageCount = this.instanceService.getPageCount();
                this.pageSize = this.instanceService.getPageSize();

                this.setVisibleVersions(this.instanceService.getVersions());

                this.pages = [];
                for (var i = 0; i < this.pageCount; i++) {
                    this.pages.push(i + 1);
                }
            })
            .catch((error: any) => {
                console.error("instance list component: getInstances");
                console.error(error);
            });
    }
    
    getDeletedInstancesPage(i: number): void {
        this.page = i;

        this.instanceService.getDeletedInstancesPage(this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then((instances: any) => {
                this.instances = instances;
                this.page = this.instanceService.getPage();
                this.pageCount = this.instanceService.getPageCount();
                this.pageSize = this.instanceService.getPageSize();

                this.setVisibleVersions(this.instanceService.getVersions());

                this.pages = [];
                for (var i = 0; i < this.pageCount; i++) {
                    this.pages.push(i + 1);
                }
            })
            .catch((error: any) => {
                console.error("instance list component: getDeletedInstances");
                console.error(error);
            });
    }

    getAssignedDeletedInstancesPage(i: number): void {
        this.page = i;
        let user = this.authService.getUser();

        this.instanceService.getAssignedDeletedInstancesPage(user.id, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then((instances: any) => {
                this.instances = instances;
                this.page = this.instanceService.getPage();
                this.pageCount = this.instanceService.getPageCount();
                this.pageSize = this.instanceService.getPageSize();

                this.setVisibleVersions(this.instanceService.getVersions());

                this.pages = [];
                for (var i = 0; i < this.pageCount; i++) {
                    this.pages.push(i + 1);
                }
            })
            .catch((error: any) => {
                console.error("instance list component: getAssignedDeletedInstances");
                console.error(error);
            });
    }

    openModal(content: any, selectedInstance: Instance): void {
        this.selectedInstance = selectedInstance;
        this.modalService.open(content, { size: 'lg' })
            .result.then(() => {
                this.selectedInstance = null;
            });
    }
    
    openAddInstanceModal(content: HTMLElement) {

        this.newInstance = new InstanceAddUpdateDto();
        this.newInstance.isWindowsAuthentication = false;

        this.modalService.open(content, { size: 'sm' })
            .result.then(result => {
                this.instanceService.addInstance(this.newInstance)
                    .then(addedInstance => {
                        if (addedInstance != null)
                            this.instances.push(addedInstance);
                    });
            })
            .catch(reason => console.info('Instance add cancelled'));
    }

    gotoDeletedInstances() {
        this.getInstancesPage(0);
    }


    showModalMessage(msg: string) {

        this.message = msg;

        this.modalService.open(this.okModal, { windowClass: 'OkModal' })
            .result.then(result => {

            });
    }

    gotoUserHidenInstances() {
        this.viewMode = Mode.hiden;
        this.getUserHidenInstancesPage(0);
    }

    showAssignedDeletedInstances() {
        this.viewMode = Mode.recycleBin;
        if (this.authService.isAdmin(this.user.username)) {
            this.getDeletedInstancesPage(0);
        }
        else {
            this.getAssignedDeletedInstancesPage(0);
        }
    }

    gotoInstances() {
        this.viewMode = Mode.instances;
        this.getInstancesPage(0);
    }

    getInstancesPageByMode(page: number):void
    {
        if (this.viewMode == Mode.recycleBin)
        {
            if (this.authService.isAdmin(this.user.username)) {

                console.log("getDeletedInstancesPage " + page);
                this.getDeletedInstancesPage(page);
            }
            else {

                console.log("getAssignedDeletedInstancesPage " + page);
                this.getAssignedDeletedInstancesPage(page);
            }

            return;
        }


        if (this.viewMode == Mode.hiden) {

            console.log("getUserHidenInstancesPage " + page);
            this.getUserHidenInstancesPage(page);
            return;
        }


        console.log("getInstancesPage "+page);
        this.getInstancesPage(page);
    }

    gotoDetail(instance: Instance): void {
        this.buffer.instanceForDetails = instance;
        let link = ['/instance', instance.id];
        this.router.navigate(link);
    }

    hideInstance(instance: Instance): void {
        this.instanceService.hideInstance(instance.id, this.user.id)
            .then(result => {
                this.instances.splice(this.instances.indexOf(instance), 1);
            });
    }
   
    deleteInstance(instance: Instance): void {
        this.instanceService.deleteInstance(instance.id)
            .then(result => {
                this.instances.splice(this.instances.indexOf(instance), 1);
            });
    }

    recoverInstance(instance: Instance): void {
        this.instanceService.recoverInstance(instance.id)
            .then(result => {
                this.instances.splice(this.instances.indexOf(instance), 1);
            });
    }

    showInstance(instance: Instance): void {
        this.instanceService.showInstance(instance.id, this.user.id)
            .then(result => {
                this.instances.splice(this.instances.indexOf(instance), 1);
            });
    }

    editInstance(editInstanceModal: any, instance: Instance): void {
        this.instToUpdate = instance.toAddUpdateDto();
        this.modalService.open(editInstanceModal, { size: 'sm' })
            .result.then(() => {
                this.instanceService.updateInstance(this.instToUpdate).then(result => {
                    instance.serverName = result.serverName;
                    instance.instanceName = result.instanceName;
                })
                .catch(reason => `Instance edit falled: ${reason}`);
                this.selectedInstance = null;
            })
            .catch(reason => console.info('Instance edit cancelled'));
    }

    navigateToDatabases(instanceId: number): void {
        let link = ['/instance', instanceId, 'databases'];
        this.router.navigate(link);
    }

    refreshInstance(instanceId: number): void {     

        this.instanceService.refreshInstance(instanceId)
            .then(result => {
                console.log(result);
            });
    }
}

enum Mode {
    instances,
    hiden,
    recycleBin
}