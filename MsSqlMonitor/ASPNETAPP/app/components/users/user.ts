export class User {
    id: Number;
    username: string;
    roles: UserRole[];
    role: string;
}

export class UserRole {
    userId: Number;
    roleId: Number;
}       