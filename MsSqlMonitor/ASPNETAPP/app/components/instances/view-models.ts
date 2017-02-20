import { InstanceService, ListServerResponse } from './instance.service';
import { InstanceDto, LoginDto, InstanceRoleDto, InstanceAddUpdateDto, DatabaseDto, PrincipalDto, DatabaseRoleDto, DatabaseUserDto } from './dto-models';
import { User } from '../users/user';




export class BrowsableInstance {

    serverName: string;
    instanceName: string;
    version: string;

    constructor(
        private instanceService: InstanceService
    ) { }

    loadFromDto(instanceDto: InstanceDto): void {
   
        this.serverName = instanceDto.serverName;
        this.instanceName = instanceDto.instanceName;

        this.version = instanceDto.version;


    }




}


export class VersionCheckbox {

    version: string;
    ischecked: boolean;
    isvisible: boolean;
   public versionnumber: string;

    constructor(version: string, ischecked: boolean, versionnumber: string)
    {
        this.version = version;
        this.ischecked = ischecked;
        this.isvisible = true;
        this.versionnumber = versionnumber;


    }

    public setVisibleFalse(): void
    {
        this.isvisible = false;
       // this.ischecked = true;
    }

    public setVisibleTrue(): void {
        this.isvisible = true;
    }

}



// Instance
export class Instance {
    id: number;
    serverName: string;
    instanceName: string;
    status: number;
    osVersion: string;
    cpuCount: number;
    memory: number;
    isOK: boolean;
    isDeleted: boolean;
    alias: string;
    isHidden: boolean;
    databasesCount: number;
    version: string;

    constructor(
        private instanceService: InstanceService
    ) { }

    loadFromDto(instanceDto: InstanceDto): void {
        this.id = instanceDto.id;
        this.serverName = instanceDto.serverName;
        this.instanceName = instanceDto.instanceName;
        this.status = instanceDto.status;
        this.osVersion = instanceDto.osVersion;
        this.cpuCount = instanceDto.cpuCount;
        this.memory = instanceDto.memory;
        this.isOK = instanceDto.isOK;
        this.isDeleted = instanceDto.isDeleted;
        this.alias = instanceDto.alias;
        this.isHidden = instanceDto.isHidden;
        this.databasesCount = instanceDto.databasesCount;
        this.version = instanceDto.version;
        this.isHidden = instanceDto.isHidden;

    }

    toAddUpdateDto(): InstanceAddUpdateDto {
        let result: InstanceAddUpdateDto = new InstanceAddUpdateDto();
        result.id = this.id;
        result.serverName = this.serverName;
        result.instanceName = this.instanceName;
        result.isWindowsAuthentication = false;
        return result;
    }

    private _logins: Login[];

    get logins(): Login[] {
        if (this._logins == null) {
            this.instanceService.getLogins(this).then(response => this._logins = response);
            if (this._logins == null)
                this._logins = [];
        }
        return this._logins;
    }

    private _roles: InstanceRole[];

    get roles(): InstanceRole[] {
        if (this._roles == null) {
            this.instanceService.getInstanceRoles(this).then(response => { this._roles = response });
            if (this._roles == null)
                this._roles = [];
        }
        return this._roles;
    }

    private _assignedUsers: User[];

    get assignedUsers(): User[] {
        if (this._assignedUsers == null) {
            this._assignedUsers = [];
            this.instanceService.getAssignedUsers(this.id).then(response => this._assignedUsers = response );
        }
        return this._assignedUsers;
    }

    private _notAssignedUsers: User[];

    get notAssignedUsers(): User[] {
        if (this._notAssignedUsers == null) {
            this._notAssignedUsers = [];
            this.instanceService.getNotAssignedUsers(this.id).then(response => this._notAssignedUsers = response );
        }
        return this._notAssignedUsers;
    }

    grantAccess(user: User): void {
        this.instanceService.grantInstanceAccess(this.id, user.id).then(response => {
            if (response as boolean) {
                this._assignedUsers.push(user);
                this._notAssignedUsers.splice(this._notAssignedUsers.indexOf(user), 1);
            }
        });
    }

    revokeAccess(user: User): void {
        this.instanceService.revokeInstanceAccess(this.id, user.id).then(response => {
            if (response as boolean) {
                this._notAssignedUsers.push(user);
                this._assignedUsers.splice(this._assignedUsers.indexOf(user), 1);
            }
        });
    }
}

// Base Principal
export class Principal<TOwner> {
    id: number;
    name: string;
    type: string;

    constructor(protected owner: TOwner,
        private getPermissions: (principalId: number) => Promise<Permission[]>) { }

    loadFromDto(dto: PrincipalDto): void {
        this.id = dto.id;
        this.name = dto.name;
        this.type = dto.type;
    }

    private _permissions: Permission[];

    get permissions(): Permission[] {
        if (this._permissions == null) {
            this.getPermissions(this.id).then(response => this._permissions = response);
            if (this._permissions == null)
                this._permissions = [];
        }
        return this._permissions;
    }
}

// Instance Login
export class Login
    extends Principal<Instance> {

    isDisabled: boolean;
    assignedRolesIds: number[];

    loadFromDto(loginDto: LoginDto) {
        super.loadFromDto(loginDto);
        this.isDisabled = loginDto.isDisabled;
        if (loginDto.assignedRolesIds == null)
            this.assignedRolesIds = [];
        else
            this.assignedRolesIds = loginDto.assignedRolesIds;
    }

    private _roles: InstanceRole[];

    get roles(): InstanceRole[] {
        if (this._roles == null) {
            this._roles = [];
            for (var role of this.owner.roles) {
                if (this.assignedRolesIds.includes(role.id))
                    this._roles.push(role);
            }
        }
        return this._roles;
    }
}

// Instance Role
export class InstanceRole
    extends Principal<Instance> {

    assignedLoginsIds: number[];

    loadFromDto(instanceRoleDto: InstanceRoleDto) {
        super.loadFromDto(instanceRoleDto);

        if (instanceRoleDto.assignedLoginsIds == null)
            this.assignedLoginsIds = [];
        else
            this.assignedLoginsIds = instanceRoleDto.assignedLoginsIds;
    }

    private _logins: Login[];

    get logins(): Login[] {
        if (this._logins == null) {
            this._logins = [];
            for (var login of this.owner.logins) {
                if (this.assignedLoginsIds.includes(login.id))
                    this._logins.push(login);
            }
        }
        return this._logins;
    }
}

//Permission
export class Permission {
    name: string;
    state: string;
}

// Database
export class Database {
    id: number;
    name: string;
    size: number;
    createDate: Date;

    constructor(private instanceService: InstanceService) { }

    loadFromDto(dto: DatabaseDto): void {
        this.id = dto.id;
        this.name = dto.name;
        this.size = dto.size;
        this.createDate = dto.createDate;
    }

    private _users: DatabaseUser[];
    private _roles: DatabaseRole[];

    get users(): DatabaseUser[] {
        if (this._users == null) {
            this.instanceService.getDatabaseUsers(this).then(response => { this._users = response });
            if (this._users == null)
                this._users = [];
        }
        return this._users;
    }

    get roles(): DatabaseRole[] {
        if (this._roles == null) {
            this.instanceService.getDatabaseRoles(this).then(response => { this._roles = response });
            if (this._roles == null)
                this._roles = [];
        }
        return this._roles;
    }
}

// DatabaseUser
export class DatabaseUser extends Principal<Database> {
    private _assignedRoles: number[];
    private _roles: DatabaseRole[];

    loadFromDto(dto: DatabaseUserDto): void {
        super.loadFromDto(dto);
        this._assignedRoles = dto.assignedRoles;
    }

    get roles(): DatabaseRole[] {
        if (this._roles == null) {
            this._roles = [];
            for (var role of this.owner.roles) {
                if (this._assignedRoles.includes(role.id))
                    this._roles.push(role);
            }
        }
        return this._roles;
    }
}

// DatabaseRole
export class DatabaseRole extends Principal<Database> {
    private _assignedUsers: number[];
    private _users: DatabaseUser[];

    loadFromDto(dto: DatabaseRoleDto): void {
        super.loadFromDto(dto);
        this._assignedUsers = dto.assignedUsers;
    }

    get users(): DatabaseUser[] {
        if (this._users == null) {
            this._users = [];
            for (var user of this.owner.users) {
                if (this._assignedUsers.includes(user.id))
                    this._users.push(user);
            }
        }
        return this._users;
    }
}