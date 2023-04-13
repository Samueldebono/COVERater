import { ExperienceType } from '../enums/enums';
import { PhaseType, UserGuessModel } from './userGuess.model';

export interface UserForRegister {
  userName: string;
  email: string;
  password: string;
  experience: ExperienceType;
  roleType: number;
}
export interface UserExperienceUpdate {
  userId: string;
  experience: ExperienceType;
}

export interface UserForLogin {
  roleId?: number;
  userName: string;
  password: string;
  bearerToken: string;
  status: number;
  roleType?: number;
  userStats?: UserModel[];
  phase?: PhaseType;
  guesses: UserGuessModel[];
}

export interface UserForLoginForgotPassword {
  email: string;
}

export interface UserCreate {
  roleId: string;
  phase: PhaseType;
}

export interface UpdateUser {
  finishedUtc: Date;
  finishingPercent: number;
  pictureCycled: number;
  time: Date;
  phase: PhaseType;
}

export interface UserModel {
  userId: number;
  hashUser: string;
  finishedPhaseUtc: Date;
  timePhase: Date;
  pictureCycledPhase: string;
  finishingPercentPhase: string;
  phase: PhaseType;
  email?: string;
  role?: number;
  experience?: number;
  guesses: UserGuessModel[];
  authRoleId: number;
  deleted?: boolean;
}
