import { CommonModule, Location } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { Subscription, filter } from 'rxjs';

interface SlideInterface {
  id: number;
  url: string;
  title: string;
}

@Component({
  selector: 'app-common-layout-header-carousel',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  template: `
    <div class="my-0 mx-36" style="height:450px">
      <ng-template [ngIf]="this.slides.length > 0">
        <div
          (mouseenter)="userHoveredSlider()"
          (mouseleave)="userStoppedHoveringSlider()"
          class="slider"
        >
          <div>
            <ng-template [ngIf]="this.slides.length > 1">
              <div (click)="goToPrevious()" class="leftArrow rounded-l-sm">
                <span>❰</span>
              </div>
              <div (click)="goToNext()" class="rightArrow rounded-r-sm">
                <span>❱</span>
              </div>
            </ng-template>
            <ng-template [ngIf]="this.isLeftSliderTextActive">
              <div (click)="goToApplyPage()" class="sliderLeftText rounded-sm">
                <h3 class="py-3 px-2 !m-0 underline hover:no-underline">
                  Join us. Apply now for positions and start teaching.
                </h3>
              </div>
            </ng-template>
            <div class="sliderRightText rounded-sm py-3 px-2 text-blue-900">
              <h3 class="!m-0 !font-semibold !text-lg">Find a teacher.</h3>
              <h4>Search our Public Register</h4>
              <div
                class="flex items-center justify-center gap-x-1 shrink pl-1 rounded-md bg-white"
              >
                <input
                  class="text-xs text-black focus:text-gray-500 py-0.5 px-1 max-w-28 md:max-w-24 2xl:max-w-36"
                  placeholder="E.g. John Doe"
                />
                <button
                  style="border-top-left-radius: 0; border-bottom-left-radius: 0;"
                  class="cursor-pointer hover:text-blue-950"
                  mat-flat-button
                  color="primary"
                >
                  Find
                </button>
              </div>
            </div>
          </div>
          <div class="slide rounded-sm">
            <img
              class="h-full w-full rounded-sm"
              [src]="this.currentActivatedCarouselImageUrl"
            />
          </div>
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
  styleUrl: './CommonLayoutHeaderCarousel.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutHeaderCarouselComponent implements OnInit, OnDestroy {
  constructor(private cd: ChangeDetectorRef, private location: Location, private router: Router) {
    this.subscriptionToListenRouteChange = this.router.events.pipe(
      filter((e: any): e is RouterEvent => e instanceof NavigationStart)
    ).subscribe((e: any)=>{
      if(e instanceof NavigationStart){
        if (e.url == '/') {
          this.isLeftSliderTextActive = true
          this.cd.markForCheck()
        }
        else this.isLeftSliderTextActive = false
      }
    })
  }
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
  public subscriptionToListenRouteChange : Subscription;
  ngOnInit(): void {
    if (this.location.path() == '') this.isLeftSliderTextActive = true
    else this.isLeftSliderTextActive = false

    this.currentActivatedCarouselImageUrl = this.slides[0].url;
    if (this.slides.length > 1) this.setIntervalForCarouselImageChange();
  }
  ngOnDestroy() {
    this.subscriptionToListenRouteChange.unsubscribe()
    window.clearInterval(this.activeInterval);
  }
  public isLeftSliderTextActive = false;
  public currentActivatedCarouselImageUrl: string = '';
  private currentIndex: number = 0;
  private activeInterval?: ReturnType<typeof setInterval>;

  public goToApplyPage(){
    this.router.navigate(['/applyUs'])
  }

  public userHoveredSlider() {
    window.clearInterval(this.activeInterval);
  }

  public userStoppedHoveringSlider() {
    window.clearInterval(this.activeInterval);

    if (this.slides.length > 1) this.setIntervalForCarouselImageChange();
  }

  private setIntervalForCarouselImageChange() {
    this.activeInterval = setInterval(() => {
      const newIndex = this.checkIsLastSlideAndReturnNewIndex();

      this.currentIndex = newIndex;

      this.currentActivatedCarouselImageUrl = this.slides[newIndex].url;

      this.cd.markForCheck();
    }, 5000);
  }

  public goToPrevious(): void {
    window.clearInterval(this.activeInterval);
    const isFirstSlide = this.currentIndex == 0;

    const newIndex = isFirstSlide
      ? this.slides.length - 1
      : this.currentIndex - 1;

    this.currentIndex = newIndex;

    this.currentActivatedCarouselImageUrl = this.slides[newIndex].url;
  }

  public goToNext(): void {
    window.clearInterval(this.activeInterval);

    const newIndex = this.checkIsLastSlideAndReturnNewIndex();

    this.currentIndex = newIndex;

    this.currentActivatedCarouselImageUrl = this.slides[newIndex].url;
  }

  public goToSlide(slideIndex: number): void {
    window.clearInterval(this.activeInterval);

    this.currentIndex = slideIndex;

    this.currentActivatedCarouselImageUrl = this.slides[slideIndex].url;
  }
  private checkIsLastSlideAndReturnNewIndex(): number {
    return this.currentIndex == this.slides.length - 1
      ? 0
      : this.currentIndex + 1;
  }
}
