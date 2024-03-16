import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';

interface SlideInterface {
  id: number;
  url: string;
  title: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>Home works!</div>
    <div class="w-3/5 my-0 mx-auto" style="height:450px">
      <ng-template [ngIf]="this.slides.length>0">
      <div (mouseenter)="func1()" (mouseleave)="func2()" class="slider">
        <div>
          <div (click)="goToPrevious()" class="leftArrow">
            <span>❰</span>
          </div>
          <div (click)="goToNext()" class="rightArrow">❱</div>
        </div>
        <div
          class="slide bg-red-500"
        >
        <img class="h-full w-full" [src]="this.currentActivatedCarouselImageUrl">
        </div>
        <!-- [ngStyle]="{'background-image': 'url(' + this.currentActivatedCarouselImageUrl + ')'}" -->
        <div class="dotsContainer">
          <div
            *ngFor="let slide of slides; let slideIndex = index"
            class="dot"
            (click)="goToSlide(slideIndex)"
          >
            ●
          </div>
        </div>
      </div>
      </ng-template>
    </div>
  `,
  styleUrl: './home.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent implements OnInit, OnDestroy {
  constructor(
    private cd: ChangeDetectorRef
  ) {}
  @Input() slides: SlideInterface[] = [
    {
      id: 1,
      title: 'carousel1',
      url: '/assets/homepage/homepage-carousel-images/homepage-carousel1.jpg',
    },
    {
      id: 2,
      title: 'carousel2',
      url: '/assets/homepage/homepage-carousel-images/homepage-carousel2.jpg',
    },
  ];

  ngOnInit(): void {
    this.currentActivatedCarouselImageUrl = this.slides[0].url;
    this.setIntervalForCarouselImageChange();
  }
  ngOnDestroy() {
    window.clearInterval(this.activeInterval)
  }
  currentActivatedCarouselImageUrl: string = '';
  private currentIndex: number = 0;
  private activeInterval?: ReturnType<typeof setInterval>;

  public func1(){
    window.clearInterval(this.activeInterval)
  }

  public func2(){
    window.clearInterval(this.activeInterval)
    this.setIntervalForCarouselImageChange()
  }

  private setIntervalForCarouselImageChange() {
    this.activeInterval = setInterval(() => {
      const isLastSlide = this.currentIndex === this.slides.length - 1;
    const newIndex = isLastSlide ? 0 : this.currentIndex + 1;

    this.currentIndex = newIndex;
    this.currentActivatedCarouselImageUrl = this.slides[newIndex].url;
      this.cd.detectChanges()
    }, 1000);
  }

  public goToPrevious(): void {
    window.clearInterval(this.activeInterval)
    const isFirstSlide = this.currentIndex === 0;
    const newIndex = isFirstSlide
      ? this.slides.length - 1
      : this.currentIndex - 1;

    this.currentIndex = newIndex;
    this.currentActivatedCarouselImageUrl = this.slides[newIndex].url;
    this.setIntervalForCarouselImageChange()
  }

  public goToNext(): void {
    window.clearInterval(this.activeInterval)
    const isLastSlide = this.currentIndex === this.slides.length - 1;
    const newIndex = isLastSlide ? 0 : this.currentIndex + 1;

    this.currentIndex = newIndex;
    this.currentActivatedCarouselImageUrl = this.slides[newIndex].url;
    this.setIntervalForCarouselImageChange()
  }

  goToSlide(slideIndex: number): void {
    window.clearInterval(this.activeInterval)
    this.currentIndex = slideIndex;
    this.currentActivatedCarouselImageUrl = this.slides[slideIndex].url;
    this.setIntervalForCarouselImageChange()
  }
}
