import { UserForLogin, UserModel } from './user.model';

export interface authUser {
  RoleId: number;
  UserName: string;
  Email: string;
  roleType: number;
  experienceLevel: number;
  UserStats: UserForLogin;
}

export interface authUserWithUserStats {
  roleId: number;
  userName: string;
  email: string;
  roleType: number;
  experienceLevel: number;
  userStats: UserModel[];
}
