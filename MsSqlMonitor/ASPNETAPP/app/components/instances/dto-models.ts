export class InstanceDto {
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
    login: string;
    password: string;
    version: string;
}

export class InstanceAddUpdateDto {
    id: number;
    serverName: string;
    instanceName: string;
    login: string;
    password: string;
    isWindowsAuthentication: boolean;
}

export class PrincipalDto {
    id: number;
    name: string;
    type: string;
}

export class LoginDto extends PrincipalDto {
    isDisabled: boolean;
    assignedRolesIds: number[];
}

export class InstanceRoleDto extends PrincipalDto {
    assignedLoginsIds: number[];
}

export class DatabaseDto {
    id: number;
    name: string;
    size: number;
    createDate: Date;
}

export class DatabaseRoleDto extends PrincipalDto {
    assignedUsers: number[];
}

export class DatabaseUserDto extends PrincipalDto {
    assignedRoles: number[];
}