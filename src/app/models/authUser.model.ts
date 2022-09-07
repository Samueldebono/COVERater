import { UserForLogin } from './user.model';

export interface authUser {
  RoleId: number;
  UserName: string;
  Email: string;
  RoleType: number;
  ExperienceLevel: number;
  UserStats: UserForLogin;
}
