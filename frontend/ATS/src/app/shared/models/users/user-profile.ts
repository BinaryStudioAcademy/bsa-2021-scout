import { Role } from "src/app/users/models/role";

export interface UserProfile{
    image: string,
    firstName:string,
    lastName:string,
    roles: Role[],
    skype?: string,
    phone?:string
}