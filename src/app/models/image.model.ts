export interface Images {
  imageId: number;
  cloudinaryId: string;
  fileName: string;
  name: string;
  type: ImageType;
  subImageDto: SubImageDto[];
}

export interface SubImageDto {
  subImageId: number;
  cloudinaryId: string;
  fileName: string;
  name: string;
  coverageRate: number;
}

export enum ImageType {
  land = 0,
  water = 1,
}
