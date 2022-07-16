import { SubImageDto } from './image.model';

export interface UserGuessModel {
  usersGuessId: number;

  guessPercentage: number;

  roleId: number;

  subImageId: number;

  phase: PhaseType;

  subImage: SubImageDto;
}

export enum PhaseType {
  ONE = 1,
  TWO = 2,
  THREE = 3,
}
