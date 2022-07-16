import { ExperienceType } from '../enums/enums';

export interface UserForRegister {
  userName: string;
  email: string;
  password: string;
  experience: ExperienceType;
  roleType: number;
}

export interface UserForLogin {
  roleId?: string;
  userName: string;
  password: string;
  bearerToken: string;
  status: number;
}

export interface UserForLoginForgotPassword {
  email: string;
}

export interface UserCreate {
  roleId: string;
}

export interface UpdateUser {
  finishedUtc: Date;
  finishingPercent: number;
  pictureCycled: number;
  time: Date;
  phase: number;
}

export interface UserModel {
  userId: number;
  hashUser: string;
  finishedPhase1Utc: Date;
  finishedPhase2Utc: Date;
  finishedPhase3Utc: Date;
  timePhase1: Date;
  timePhase2: Date;
  timePhase3: Date;
  pictureCycledPhase1: string;
  pictureCycledPhase2: string;
  pictureCycledPhase3: string;
  finishingPercentPhase1: string;
  finishingPercentPhase2: string;
  finishingPercentPhase3: string;
}
