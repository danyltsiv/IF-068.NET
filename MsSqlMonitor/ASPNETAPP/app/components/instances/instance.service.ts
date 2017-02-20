import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { AuthHttp } from '../auth.http';
import { ShowError } from './show-error.component';
import { VersionCheckbox,BrowsableInstance,Instance, InstanceRole, Database, Login, Permission, DatabaseRole, DatabaseUser } from './view-models';
import { InstanceAddUpdateDto } from './dto-models';
import { User } from '../users/user';
import { AuthService } from '../auth.service';

@Injectable()
export class InstanceService {

    private baseUrl = 'https://localhost:44301/api/instances/';
    private baseUrl2 = 'https://localhost:44301/api/instances';
    private loginsSuffix = '/logins';
    private instanceRolesSuffix = '/roles';
    private instancePermissionsSuffix = '/permissions';
    private databasesSuffix = '/databases';
    private databaseUsersSuffix = '/users';
    private databaseRolesSuffix = '/database-roles';
    private databasePermissionsSuffix = '/database-permissions';
    private assignedUsersSuffix = '/assigned-users';
    private notAssignedUsersSuffix = '/not-assigned-users';
    private grantAssignSuffix = '/grant-instance-access';
    private revokeAssignSuffix = '/revoke-instance-access';
    private hideInsSuffix = '/hide-instance';
    private showInsSuffix = '/show-instance';
    private browsableSuffix = '/browsable';
    private checkConnectionSuffix = '/checkconnection';
    private refreshSuffix = '/refresh';
    private pageParameter = 'page=';
    private nameFilterParameter = 'namefilter=';
    private serverFilterParameter = 'serverNamefilter=';
    private versionFilterParameter = 'versionFilter=';
    private sqlVersionsParameter = 'sqlversions=';

    private headers = new Headers({ 'Content-Type': 'application/json' });

    private instances: Instance[];

    page: number;
    pageCount: number;
    pageSize: number;
    versions: string[] =[];


    constructor(
        private authService: AuthService,
        private http: AuthHttp,
        private showError: ShowError
    ) { }

    getPage(): number
    {
        return this.page;
    }

    getPageCount(): number {
        return this.pageCount;
    }

    getPageSize(): number {
        return this.pageSize;
    }

    getVersions(): string[] {

       // this.versions.
        return this.versions;
    }

    checkConnection(instance: InstanceAddUpdateDto): Promise<string> {

        let url = this.joinStrings([this.baseUrl2, this.checkConnectionSuffix,
            "?", "servername=", instance.serverName,
            "&", "instanceName=", instance.instanceName,
            "&", "login=" + instance.login,
            "&", "pswd=" + instance.password,
            "&", "windowsoauth=" + instance.isWindowsAuthentication]);
       
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as string; 
                return parsed;
            })
            .catch((error: any) => {

                console.error("iinstanceservice:checkConnection error");
                console.error(error);
            });
    }

    getBrowsableInstances(): Promise<BrowsableInstance[]> {

        let url = this.joinStrings([this.baseUrl2, this.browsableSuffix]);

        let instances: BrowsableInstance[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;

                if (!parsed.didError) {
                    for (var instance of parsed.model) {
                        let newInstance = new BrowsableInstance(this);
                        newInstance.loadFromDto(instance);
                        instances.push(newInstance);
                    }
                }
                return instances;
            })
            .catch((error: any) => {

                console.error("iinstanceservice:get getBrowsableInstances error");
                console.error(error);
                //this.showError.showError(error);
            });
    }

    getUserInstances(userid: Number, page: number, nameFilter: string, serverName: string, version: string, vesrsions:string): Promise<Instance[]> {       

        let url = this.joinStrings([this.baseUrl2, "/assigned?id=", userid, "&", this.pageParameter, page,]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);

        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);

        let instances: Instance[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var instance of parsed.model) {
                        let newInstance = new Instance(this);
                        newInstance.loadFromDto(instance);
                        instances.push(newInstance);
                    }
                    this.page = parsed.pageNumber;
                    this.pageCount = parsed.pagesCount;
                    this.pageSize = parsed.pageSize;
                    this.versions = parsed.versions;

                    //console.log("instance.service versions= ");
                    //console.dir(this.versions);

                }
                return instances;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getUserHidenInstances(userid: Number, page: number, nameFilter: string, serverName: string, version: string, vesrsions: string): Promise<Instance[]> {

        let url = this.joinStrings([this.baseUrl2, "/hiden-assigned?id=", userid,

            "&", this.pageParameter, page,

        ]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);

        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);

        let instances: Instance[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var instance of parsed.model) {
                        let newInstance = new Instance(this);
                        newInstance.loadFromDto(instance);
                        instances.push(newInstance);
                    }
                    this.page = parsed.pageNumber;
                    this.pageCount = parsed.pagesCount;
                    this.pageSize = parsed.pageSize;
                    this.versions = parsed.versions;

                }
                return instances;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getDeletedInstancesPage(page: number, nameFilter: string, serverName: string, version: string, vesrsions: string): Promise<Instance[]> {

        let url = this.joinStrings([this.baseUrl2, "/deleted", "?", this.pageParameter, page,]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);

        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);

        let instances: Instance[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var instance of parsed.model) {
                        let newInstance = new Instance(this);
                        newInstance.loadFromDto(instance);
                        instances.push(newInstance);
                    }
                    this.page = parsed.pageNumber;
                    this.pageCount = parsed.pagesCount;
                    this.pageSize = parsed.pageSize;
                    this.versions = parsed.versions;

                }
                return instances;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getAssignedDeletedInstancesPage(userid: Number, page: number, nameFilter: string, serverName: string, version: string, vesrsions: string): Promise<Instance[]> {

        let url = this.joinStrings([this.baseUrl2, "/deleted-assigned?id=", userid, "&", this.pageParameter, page,]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url, "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);

        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);

        let instances: Instance[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var instance of parsed.model) {
                        let newInstance = new Instance(this);
                        newInstance.loadFromDto(instance);
                        instances.push(newInstance);
                    }
                    this.page = parsed.pageNumber;
                    this.pageCount = parsed.pagesCount;
                    this.pageSize = parsed.pageSize;
                    this.versions = parsed.versions;

                }
                return instances;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getInstances(page: number, nameFilter: string, serverName: string, version: string, vesrsions: string): Promise<Instance[]> {

        let url = this.joinStrings([this.baseUrl2, "?", this.pageParameter, page ]);

        if (nameFilter.length > 0) url = this.joinStrings([url, "&", this.nameFilterParameter, nameFilter]);
        if (serverName.length > 0) url = this.joinStrings([url,  "&", this.serverFilterParameter, serverName]);
        if (version.length > 0) url = this.joinStrings([url, "&", this.versionFilterParameter, version]);

        url = this.joinStrings([url, "&", this.sqlVersionsParameter, vesrsions]);

        let instances: Instance[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError)
                {
                    for (var instance of parsed.model)
                    {
                        let newInstance = new Instance(this);
                        newInstance.loadFromDto(instance);
                        instances.push(newInstance);
                    }
                    this.page = parsed.pageNumber;
                    this.pageCount = parsed.pagesCount;
                    this.pageSize = parsed.pageSize;
                    this.versions = parsed.versions;

                    console.log("instance.service versions= ");
                    console.dir(this.versions);

                }
                return instances;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getInstanceById(id: number): Promise<Instance> {

        let url = this.joinStrings([this.baseUrl, `${id}`]);
        let instance: Instance;
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as SingleServerResponse;
                if (!parsed.didError) {
                    instance = new Instance(this);
                    instance.loadFromDto(parsed.model);
                }
                return instance;
            })
            .catch((error: any) =>
                this.showError.showError(error));
    }

    getLogins(instance: Instance): Promise<Login[]> {
        let url = this.joinStrings([this.baseUrl, instance.id, this.loginsSuffix]);
        let logins: Login[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var login of parsed.model) {
                        let newLogin = new Login(instance, (principalId) => this.getInstancePrincipalPermissions(principalId));
                        newLogin.loadFromDto(login);
                        logins.push(newLogin);
                    }
                }
                return logins;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getInstanceRoles(instance: Instance): Promise<InstanceRole[]> {
        let url = this.joinStrings([this.baseUrl, instance.id, this.instanceRolesSuffix]);
        let roles: InstanceRole[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;

                if (!parsed.didError) {
                    for (var role of parsed.model) {
                        let newRole = new InstanceRole(instance, (principalId) => this.getInstancePrincipalPermissions(principalId));
                        newRole.loadFromDto(role);
                        roles.push(newRole);
                    }
                }
                return roles;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getInstancePrincipalPermissions(principalId: number): Promise<Permission[]> {
        let url = this.joinStrings([this.baseUrl, principalId, this.instancePermissionsSuffix]);
        let permissions: Permission[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    permissions = parsed.model as Permission[];
                }
                return permissions;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    updateInstance(instance: InstanceAddUpdateDto): Promise<Instance> {
        return this.http.put(this.baseUrl, `'${JSON.stringify(instance)}'`, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => response.json().model as Instance)
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    addInstance(newInstance: InstanceAddUpdateDto): Promise<Instance> {
        let addedInstance: Instance;
        return this.http.post(this.baseUrl, `'${JSON.stringify(newInstance)}'`, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as SingleServerResponse;
                if (!parsed.didError) {
                    console.info(parsed);
                    addedInstance = new Instance(this);
                    addedInstance.loadFromDto(parsed.model);
                    this.grantInstanceAccess(addedInstance.id, this.authService.getUser().id);
                }
                return addedInstance;
            })
            .catch((error: any) => {
                console.error(error);
                this.showError.showError(error);
            });
    }

    hideInstance(instanceId: number, userId: Number): Promise<Instance> {
        let url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.hideInsSuffix]);
        let result = false;
        return this.http.put(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    result = true;
                }
                else {
                    this.showError.showError(parsed.errorMessage);
                }
                return result;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    showInstance(instanceId: number, userId: Number): Promise<Instance> {
        let url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.showInsSuffix]);
        return this.http.put(url, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => response.json().model as Instance)
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    recoverInstance(instanceId: number): Promise<Instance> {
        let url = this.joinStrings([this.baseUrl, `${instanceId}`]);
        return this.http.post(url, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => response.json().model as Instance)
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    deleteInstance(instanceId: number): Promise<Instance> {
        let url = this.joinStrings([this.baseUrl, `${instanceId}`]);
        return this.http.delete(url, new RequestOptions({ headers: this.headers }))
            .toPromise()
            .then((response: any) => response.json().model as Instance)
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    ////// Work with Databases, their Principals and Permissions

    getDatabases(instanceId: number): Promise<Database[]> {
        let url = this.joinStrings([this.baseUrl, `${instanceId}`, this.databasesSuffix]);
        let databases: Database[] = [];
        return this.http.get(url)
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var database of parsed.model) {
                        let newDatabase = new Database(this);
                        newDatabase.loadFromDto(database);
                        databases.push(newDatabase);
                    }
                }
                return databases;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getDatabaseRoles(database: Database): Promise<DatabaseRole[]> {
        let url = this.joinStrings([this.baseUrl, database.id, this.databaseRolesSuffix]);
        let roles: DatabaseRole[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var role of parsed.model) {
                        let newRole = new DatabaseRole(database, (principalId) => this.getDatabasePrincipalPermissions(principalId));
                        newRole.loadFromDto(role);
                        roles.push(newRole);
                    }
                }
                return roles;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    refreshInstance(instanceId: number): Promise<string>
    {
        let url = this.joinStrings([this.baseUrl, instanceId, this.refreshSuffix]);

        console.error("refreshInstance url = " + url);

      return  this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ServerResponse;
       
                return parsed.errorMessage;
            })
            .catch((error: any) => {

                console.error("iinstanceservice:refreshInstance error");
                console.error(error);
                this.showError.showError(error);
            });

  
    }

    getDatabaseUsers(database: Database): Promise<DatabaseUser[]> {
        let url = this.joinStrings([this.baseUrl, database.id, this.databaseUsersSuffix]);
        let users: DatabaseUser[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    for (var user of parsed.model) {
                        let newUser = new DatabaseUser(database, (principalId) => this.getDatabasePrincipalPermissions(principalId));
                        newUser.loadFromDto(user);
                        users.push(newUser);
                    }
                }
                return users;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getDatabasePrincipalPermissions(principalId: number): Promise<Permission[]> {
        let url = this.joinStrings([this.baseUrl, principalId, this.databasePermissionsSuffix]);
        let permissions: Permission[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    permissions = parsed.model as Permission[];
                }
                else {
                    this.showError.showError(parsed.errorMessage);
                }
                return permissions;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    ////// Work with Assignments

    getAssignedUsers(instanceId: number): Promise<User[]> {
        let url = this.joinStrings([this.baseUrl, instanceId, this.assignedUsersSuffix]);
        let users: User[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    users = parsed.model as User[];
                }
                else {
                    this.showError.showError(parsed.errorMessage);
                }
                return users;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    getNotAssignedUsers(instanceId: number): Promise<User[]> {
        let url = this.joinStrings([this.baseUrl, instanceId, this.notAssignedUsersSuffix]);
        let users: User[] = [];
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    users = parsed.model as User[];
                }
                else {
                    this.showError.showError(parsed.errorMessage);
                }
                return users;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    grantInstanceAccess(instanceId: number, userId: Number): Promise<boolean> {
        let url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.grantAssignSuffix]);
        let result = false;
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    result = true;
                }
                else {
                    this.showError.showError(parsed.errorMessage);
                }
                return result;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    revokeInstanceAccess(instanceId: number, userId: Number): Promise<boolean> {
        let url = this.joinStrings([this.baseUrl, instanceId, '/', userId, this.revokeAssignSuffix]);
        let result = false;
        return this.http.get(url, new Headers(this.headers))
            .toPromise()
            .then((response: any) => {
                let parsed = response.json() as ListServerResponse;
                if (!parsed.didError) {
                    result = true;
                }
                else {
                    this.showError.showError(parsed.errorMessage);
                }
                return result;
            })
            .catch((error: any) => {
                this.showError.showError(error);
            });
    }

    private joinStrings(values: any[]) {
        return values.join('');
    }
}

// Server Response
class ServerResponse {
    didError: boolean;
    errorMessage: string;
    message: string;
}

// Single Server Response
export class SingleServerResponse extends ServerResponse {
    model: any;
}

// List Server Response
export class ListServerResponse extends ServerResponse {
    model: any[];
    pageNumber: number;
    pageSize: number;
    pagesCount: number;
    versions: string[];
}