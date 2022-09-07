import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Images, ImageType, SubImageDto } from 'src/app/models/image.model';
import { Guess } from 'src/app/models/guess.model';
import { ImagesService } from 'src/app/services/images/images.service';
import { GuessService } from 'src/app/services/guess/guess.service';
import { PhaseType, UserGuessModel } from 'src/app/models/userGuess.model';
import { UpdateUser, UserForLogin, UserModel } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user/user.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { AnswerSuccess } from 'src/app/models/enums.model';
import { Router } from '@angular/router';
import { Options } from '@angular-slider/ngx-slider';

@Component({
  selector: 'app-stageOne',
  templateUrl: './stageOne.component.html',
  styleUrls: ['./stageOne.component.less'],
})
export class StageOneComponent implements OnInit {
  imageList: Images[];
  currentUrl: string;
  currentSubUrl: string;
  quizForm: FormGroup;
  answerSuccess: boolean;
  guess: Guess;
  currentImage: Images;
  currentSubImage: SubImageDto;
  guessPercent: number = 0;
  time: number = 0;
  display: any;
  interval: any;
  userGuesses: UserGuessModel[];
  pictureCount: number;
  phase: number;
  finishPercentage: number;
  finalPercentage: number;
  showProgress: boolean;
  currentAnswer: AnswerSuccess;
  userModel: UserModel;
  stepIndex: number = 0;
  finalValue: number = 0;
  user: UserForLogin;
  sliderControl: FormControl = new FormControl(0);
  finishImageCount = 49;
  loading: boolean = true;
  loading2: boolean = true;

  ticks = [
    0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 3, 4, 5, 6, 7, 8,
    9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
    28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46,
    47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65,
    66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84,
    85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100,
  ];
  options: Options = {
    floor: 0,
    ceil: 100,
    step: 0.05,
    enforceStep: false,
    ticksArray: this.ticks,
    customPositionToValue: (
      percent: number,
      minVal: number,
      maxVal: number
    ): number => {
      const rawValue = percent * (maxVal - minVal) + minVal;
      // Find nearest tick
      let nearest = this.ticks[0];
      for (const tick of this.ticks) {
        if (Math.abs(rawValue - tick) > Math.abs(rawValue - nearest)) {
          return nearest;
        }
        nearest = tick;
      }
      return nearest;
    },
    getTickColor: (value: number): string => {
      if (value < 30) {
        return '#2AE02A';
      }
      if (value < 60) {
        return 'yellow';
      }
      if (value < 80) {
        return 'orange';
      }
      return 'red';
    },
  };
  local: boolean = true;

  constructor(
    private imageService: ImagesService,
    private fb: FormBuilder,
    private guessService: GuessService,
    private userService: UserService,
    public dialog: MatDialog,
    private router: Router
  ) {}

  onInputChange($event: any) {
    this.stepIndex = +$event.value;
    this.finalValue = this.ticks[this.stepIndex];
  }

  ngOnInit() {
    // this.createQuizForm();
    this.finishPercentage = 98;
    this.showProgress = false;
    if (
      history.state.data !== undefined &&
      history.state.data.userModel !== undefined
    ) {
      this.user = history.state.data.userModel;
      this.phase = this.user.phase;
    } else {
      this.router.navigate(['/home']);
    }
    this.selectImageType(null);
    this.startTimer();
  }
  onLoad() {
    this.loading = false;
  }
  onLoad2() {
    this.loading2 = false;
  }

  onSubmit() {
    this.guessService.saveGuess(this.userData()).subscribe((data) => {
      this.answerSuccess = data;
      this.update();
      this.loading = true;
      this.loading2 = true;
      this.removeCurrentImageFromList();
      if (this.user != undefined && this.user.phase == 2) {
        this.openDialog();
      } else if (
        this.userGuesses !== undefined &&
        this.userGuesses.length >= this.finishImageCount
      ) {
        this.finish();
      } else {
        this.refreshImage();
        this.reset();
      }
    });
  }

  submitAnswer() {
    this.guessService.saveGuess(this.userData()).subscribe((data) => {
      this.answerSuccess = data;
      this.update();
    });
  }

  refreshImage() {
    const cImage =
      this.imageList[Math.floor(Math.random() * this.imageList.length)];
    this.currentImage = cImage;
    const cSubImgae =
      this.currentImage.subImageDto[
        Math.floor(Math.random() * this.currentImage.subImageDto.length)
      ];
    this.currentSubImage = cSubImgae;
    this.currentUrl = this.getImageUrl(this.currentImage.fileName);
    this.currentSubUrl = this.getSubImageUrl(this.currentSubImage.fileName);
  }

  removeCurrentImageFromList() {
    this.imageList.forEach((image) => {
      if (image.imageId === this.currentImage.imageId) {
        const index = image.subImageDto.indexOf(this.currentSubImage, 0);
        if (index > -1) {
          image.subImageDto.splice(index, 1);
        }
        if (image.subImageDto.length <= 0) {
          const imageIndex = this.imageList.indexOf(this.currentImage, 0);
          this.imageList.splice(imageIndex, 1);
        }
        return;
      }
    });
  }

  selectImageType(type?: ImageType) {
    var images: Images[];
    this.imageService.getAllImages().subscribe((data) => {
      images = data;
      switch (type) {
        case ImageType.land:
          this.imageList = images.filter((type) => type.type == ImageType.land);
          break;
        case ImageType.water:
          this.imageList = images.filter(
            (type) => type.type == ImageType.water
          );
          break;
        default:
          this.imageList = images;
          break;
      }
      this.refreshImage();
    });
  }

  userData(): Guess {
    const correctPercent = this.currentSubImage.coverageRate;
    var result = false;

    //convert to percentage
    this.guessPercent = this.sliderControl.value;
    var guess = this.guessPercent == 0 ? 0.0 : this.guessPercent / 100;

    //if range sits between 10 then answer is correct
    if (guess == correctPercent) {
      this.currentAnswer = AnswerSuccess.success;
    } else if (guess >= correctPercent - 0.03 && guess <= correctPercent + 0.03)
      this.currentAnswer = AnswerSuccess.close;
    else this.currentAnswer = AnswerSuccess.far;
    return (this.guess = {
      GuessPercentage: guess,
      SubImageId: this.currentSubImage.subImageId,
      RoleId: localStorage.getItem('id'),
      Phase: this.phase,
    });
  }

  getImageUrl(name: string) {
    if (!this.local) {
      return (
        'https://res.cloudinary.com/pegleg/image/upload/v1652009897/Coverater/Estimate/' +
        name
      );
    } else {
      return '../../../assets/images/estimates/' + name + '.jpg';
    }
  }

  getSubImageUrl(name: string) {
    if (!this.local) {
      return (
        'https://res.cloudinary.com/pegleg/image/upload/v1652009772/Coverater/Species/' +
        name
      );
    } else {
      return '../../../assets/images/species/' + name + '.jpg';
    }
  }

  sliderUpdate(event: any) {
    this.guessPercent = event.value;
  }

  reset() {
    this.guessPercent = 0;
    this.stepIndex = 0;
    this.finalValue = 0;
    this.sliderControl.reset(0);
  }

  update() {
    this.guessService.getGuesses().subscribe((guesses) => {
      this.userGuesses = guesses;

      //find difference
      if (
        this.userGuesses.length >= 9 &&
        this.phase ==
          PhaseType[
            'Training (users estimate images with feedback until they reach 98% running accuracy on last 10 images)'
          ]
      ) {
        var list: UserGuessModel[] = [];
        this.userGuesses.forEach((val) => list.push(Object.assign({}, val)));
        list = list
          .sort((x) => x.usersGuessId)
          .splice(this.userGuesses.length - 9);
        this.finalPercentage = this.findAverageDifferenceOfList(list);
      } else {
        var list = this.userGuesses.sort((x) => x.usersGuessId);
      }

      // this.finalPercentage = this.findAverageDifferenceOfList(list);

      let dateTime = new Date();
      if (
        this.phase ===
        PhaseType[
          'Training (users estimate images with feedback until they reach 98% running accuracy on last 10 images)'
        ]
      ) {
        this.showProgress = this.userGuesses.length >= 9;
      }

      //save data so if user exits/refreshes can come back to this point
      var editUserBinding: UpdateUser = {
        finishingPercent: this.finalPercentage,
        pictureCycled: this.userGuesses.length,
        time: dateTime,
        phase: this.phase,
        finishedUtc: new Date(),
      };

      this.userService.updateUser(editUserBinding).subscribe((results) => {
        this.userModel = results;
      });
    });
  }

  findAverageDifferenceOfList(list: UserGuessModel[]) {
    var dif = 0.0;
    if (list.length > 0) {
      list.forEach((guess) => {
        dif += this.findSingleDifference(guess);
      });

      dif = (1 - dif / list.length) * 100;
    }
    return Math.round(dif);
  }

  findHerbivoryDifferenceoOfList(list: UserGuessModel[]) {
    var guessHerbivoryList: any[];
    list.forEach((guess) => {
      guessHerbivoryList.concat(this.findSingleDifference(guess));
    });

    return guessHerbivoryList;
  }

  findSingleDifference(guess: UserGuessModel) {
    var calc = guess.guessPercentage - guess.subImage.coverageRate; //this.currentSubImage.coverageRate;
    var dif = calc < 0 ? calc * -1 : calc;
    return dif;
  }

  transform(value: number): string {
    var sec_num = value;
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - hours * 3600) / 60);
    var seconds = sec_num - hours * 3600 - minutes * 60;

    if (hours < 10) {
      hours = 0;
    }
    if (minutes < 10) {
      minutes = 0;
    }
    if (seconds < 10) {
      seconds = 0;
    }
    return hours + ':' + minutes + ':' + seconds;
  }

  startTimer() {
    this.interval = setInterval(() => {
      if (this.time === 0) {
        this.time++;
      } else {
        this.time++;
      }
      this.display = this.transform(this.time);
    }, 1000);
  }

  openDialog() {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '400px',
      data: {
        percentage: this.guessPercent == 0 ? 0.0 : this.guessPercent / 100,
        correctPercentage: this.currentSubImage.coverageRate,
        answer: this.currentAnswer,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (
        this.finalPercentage > this.finishPercentage &&
        this.userGuesses.length >= 10
      ) {
        this.finish();
      } else {
        this.refreshImage();
        this.reset();
      }
    });
  }

  finish() {
    this.router.navigate([
      '/finish/' + localStorage.getItem('id'),
      { state: localStorage.getItem('id') },
    ]);
  }

  formatLabel(value: number) {
    let values = [
      0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 3, 4, 5, 6, 7, 8,
      9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
      28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45,
      46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
      64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
      82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
      100,
    ];
    return values[value] + '%';
  }
}
