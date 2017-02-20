import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import {User} from './user';
import {VersionCheckbox,Instance} from '../instances/view-models';
import { UserService } from './user.service';
import { RoleService } from '../roles/role.service';
import {InstanceService} from '../instances/instance.service';
import {Assign} from '../assigns/assign';
import { Router } from "@angular/router";

@Component({
    moduleId: module.id,
    selector: 'user-detail',
    styleUrls: ['user-detail.component.css'],
    templateUrl: 'user-detail.component.html'
})
export class UserDetailComponent implements OnInit {
    user: User;
    userInstances: Instance[];
    notAssignedInstances: Instance[];
    password: string;
    role: string;
    confirmPassword: string;
    error: boolean = false;
    errorMsg: string = "error";
    appState: string = "default";
    visibility: string = "hidden";
    pages: number[];
    page: number;
    pageCount: number;
    pageSize: number;
    instanceNameFilter: string;
    instanceServerFilter: string;
    versionFiler: string;
    sqlVersions: VersionCheckbox[];
    viewMode: viewState;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private location: Location,
        private userService: UserService,
        private roleService: RoleService) { }

    ngOnInit(): void {
        this.viewMode = viewState.assignedInstances;
        this.initSQLVersionsArray();
        this.page = 0;
        this.pages = [];
        this.instanceNameFilter = "";
        this.instanceServerFilter = "";
        this.versionFiler = "";
        let userId: Number;
        this.route.params.forEach((params: Params) => { userId = +params['id']; });
        this.getUserById(userId);
        this.getAssignedInstances(userId);
    }

    initSQLVersionsArray(): void {
        this.sqlVersions = [];
        this.sqlVersions.push(new VersionCheckbox("others", true, ""));
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
        console.log("user detail set visible versions arr");
        console.dir(arr);
        for (var i = 0; i < arr.length; i++) {
            console.log(arr[i]);
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

    getVersionsString(): string {
        let res = "";
        for (var i = 0; i < this.sqlVersions.length; i++) {
            if (this.sqlVersions[i].ischecked) res = res + "1";
            else res = res + "0";
        }
        return res;
    }

    onVersionChange(event: any, index: number): void {
        console.log(event.target.checked);
        console.log(index);
        this.sqlVersions[index].ischecked = event.target.checked;
        this.getAssignedInstances(this.user.id);
    }

    filterInstancesNames(value: string, event: any): void {
        this.page = 0;
        this.instanceNameFilter = value;
        this.getAssignedInstances(this.user.id);
    }

    getInstancesPage(value: number)
    {
        this.page = value;  
        this.getAssignedInstances(this.user.id);
    }

    getNotAssignedInstancesPage(value: number) {
        this.page = value;
        this.getNotAssignedInstances(this.user.id);
    }

    filterInstancesServers(value: string, event: any): void {
        this.page = 0;
        this.instanceServerFilter = value;
        this.getAssignedInstances(this.user.id);
    }

    filterInstancesVersions(value: string, event: any): void {
        this.page = 0;
        this.versionFiler = value;
        this.getAssignedInstances(this.user.id);
    }

    checkPasswordEquality(): void {
        if (this.confirmPassword !== this.password && this.password != "") {
            this.visibility = "visible";
            this.errorMsg = "Password mismatch!";
        }
    }

    getUserById(userId: Number): void {
        this.userService.getUserById(userId)
            .then(us => this.user = us)
            .then(() => this.roleService.getRoleById(this.user.roles[0].roleId)
                .then(resp => this.role = resp.name));
    }

    updateUser(): void {
        this.visibility = "hidden";
        this.checkPasswordEquality();
        if (this.visibility == "hidden") {
            this.userService.updateUser(this.user, this.password, this.role).then(() => this.back())
                .catch(g => { this.visibility = "visible"; this.errorMsg += g });
        }
    }

    back(): void {
        this.location.back();
    }

    deassignInstance(instance: Instance): void {
        this.userService.deassignInstance(this.user.id, instance.id)
            .then(result => {
                this.userInstances.splice(this.userInstances.indexOf(instance), 1);
            });
    }

    assignInstance(instance: Instance): void {
        this.userService.assignInstance(this.user.id, instance.id)
            .then(result => {
                this.notAssignedInstances.splice(this.notAssignedInstances.indexOf(instance), 1);
            });
    }

    getAssignedInstances(userId: Number): void {
        this.userService.getUserInstances(userId, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
        .then(instances => {
            this.userInstances = instances;
            this.page = this.userService.getPage();
            this.pageCount = this.userService.getPageCount();
            this.pageSize = this.userService.getPageSize();
            this.setVisibleVersions(this.userService.getVersions());
            this.pages = [];
            for (var i = 0; i < this.pageCount; i++) {
                this.pages.push(i + 1);
            }})
            .catch(error => {
                console.error("user detail component: getAssignedInstances");
                console.error(error);
            });
    }

    getNotAssignedInstances(userId: Number): void {
        console.log('getting not assigned instances');
        this.userService.getNotAssignedInstances(userId, this.page, this.instanceNameFilter, this.instanceServerFilter, this.versionFiler, this.getVersionsString())
            .then(instances => {
                this.notAssignedInstances = instances;
                this.page = this.userService.getPage();
                this.pageCount = this.userService.getPageCount();
                this.pageSize = this.userService.getPageSize();
                this.setVisibleVersions(this.userService.getVersions());
                this.pages = [];
                for (var i = 0; i < this.pageCount; i++) {
                    this.pages.push(i + 1);
                }
            })
            .catch(error => {
                console.error("user detail component: getAssignedInstances");
                console.error(error);
            });
    }

    goToInstanceDetail(instance: Instance) {
        let link = ['/instance', instance.id];
        this.router.navigate(link);
    }

    changeViewState() {
        if (this.viewMode == viewState.notAssignedInstances) {
            this.viewMode = viewState.assignedInstances
            this.getAssignedInstances(this.user.id);
        }
        else {
            this.viewMode = viewState.notAssignedInstances
            this.getNotAssignedInstances(this.user.id);
        }
    }
}

enum viewState {
    assignedInstances,
    notAssignedInstances
}