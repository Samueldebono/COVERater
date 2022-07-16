import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Images, ImageType, SubImageDto } from 'src/app/models/image.model';
import { Guess } from 'src/app/models/guess.model';
import { ImagesService } from 'src/app/services/images/images.service';
import { GuessService } from 'src/app/services/guess/guess.service';
import { PhaseType, UserGuessModel } from 'src/app/models/userGuess.model';
import { UpdateUser, UserModel } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user/user.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { AnswerSuccess } from 'src/app/models/enums.model';
import { Router } from '@angular/router';

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

  constructor(
    private imageService: ImagesService,
    private fb: FormBuilder,
    private guessService: GuessService,
    private userService: UserService,
    public dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit() {
    this.finishPercentage = 80;
    this.showProgress = false;
    this.selectImageType(null);
    this.createQuizForm();
    this.startTimer();
  }

  onSubmit() {
    this.phase = 1;
    this.submitAnswer();
    this.removeCurrentImageFromList();
    this.openDialog();
  }

  submitAnswer() {
    this.guessService.saveGuess(this.userData()).subscribe((data) => {
      this.answerSuccess = data;
      console.log(data);
    });
    this.update();
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
    console.log(
      this.currentSubImage.coverageRate + ' ' + this.currentSubImage.subImageId
    );
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
      console.log(data);
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

  createQuizForm() {
    this.quizForm = this.fb.group({});
  }

  userData(): Guess {
    const correctPercent = this.currentSubImage.coverageRate;
    var result = false;

    //convert to percentage
    var guess = this.guessPercent == 0 ? 0.0 : this.guessPercent / 100;

    //if range sits between 10 then answer is correct
    if (guess == correctPercent) {
      this.currentAnswer = AnswerSuccess.success;
    } else if (guess >= correctPercent - 0.05 && guess <= correctPercent + 0.05)
      this.currentAnswer = AnswerSuccess.close;
    else this.currentAnswer = AnswerSuccess.far;

    return (this.guess = {
      GuessPercentage: guess,
      SubImageId: this.currentSubImage.subImageId,
      RoleId: localStorage.getItem('id'),
      Phase: 1,
    });
  }

  getImageUrl(name: string) {
    return (
      'https://res.cloudinary.com/pegleg/image/upload/v1652009897/Coverater/Estimate/' +
      name
    );
  }

  getSubImageUrl(name: string) {
    return (
      'https://res.cloudinary.com/pegleg/image/upload/v1652009772/Coverater/Species/' +
      name
    );
  }

  sliderUpdate(event: any) {
    this.guessPercent = event.value;
  }

  update() {
    this.guessService.getGuesses().subscribe((guesses) => {
      this.userGuesses = guesses;

      //find difference
      if (this.userGuesses.length >= 10 && this.phase == PhaseType.TWO) {
        var list = this.userGuesses
          .sort((x) => x.usersGuessId)
          .splice(this.userGuesses.length - 10);
        this.finalPercentage = this.findAverageDifferenceOfList(list);
      } else if (this.phase == PhaseType.ONE || this.phase == PhaseType.THREE) {
        var list = this.userGuesses.sort((x) => x.usersGuessId);
      }

      this.finalPercentage = this.findAverageDifferenceOfList(list);
      let dateTime = new Date();

      this.showProgress = this.userGuesses.length >= 4;
      //save data so if user exits/refreshes can come back to this point
      // debugger;
      var editUserBinding: UpdateUser = {
        finishingPercent: this.finalPercentage,
        pictureCycled: this.userGuesses.length,
        time: dateTime,
        phase: PhaseType.ONE,
        finishedUtc: null,
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
        dif += this.findSingleHerbivoryDifference(guess);
      });

      dif = (1 - dif / list.length) * 100;
    }
    return Math.round(dif);
  }

  findHerbivoryDifferenceoOfList(list: UserGuessModel[]) {
    var guessHerbivoryList: any[];
    list.forEach((guess) => {
      guessHerbivoryList.concat(this.findSingleHerbivoryDifference(guess));
    });

    return guessHerbivoryList;
  }

  findSingleHerbivoryDifference(guess: UserGuessModel) {
    var calc = guess.guessPercentage - this.currentSubImage.coverageRate;
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
    console.log('=====>');
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
        this.userGuesses.length >= 4
      ) {
        this.router.navigate([
          '/finish/' + localStorage.getItem('id'),
          { state: localStorage.getItem('id') },
        ]);
      } else {
        this.refreshImage();
      }
    });
  }

  formatLabel(value: number) {
    if (value >= 1000) {
      return Math.round(value / 1000);
    }

    return value + '%';
  }
}
