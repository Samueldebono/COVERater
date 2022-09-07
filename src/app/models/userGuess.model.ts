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
  'Baseline (the first phase where users estimate 50 images without feedback)' = 1,
  'Training (users estimate images with feedback until they reach 98% running accuracy on last 10 images)',
  'Interval 1 (users estimate 50 images without feedback immediately after Training)',
  'Interval 2 (users estimate 50 images without feedback 1 hour after Training)',
  'Interval 3 (users estimate 50 images without feedback 1 day after Training)',
  'Interval 4 (users estimate 50 images without feedback 1 week after Training)',
  'Interval 5 (users estimate 50 images without feedback 1 month after Training)',
  'Interval 6 (users estimate 50 images without feedback 3 months after Training)',
}
