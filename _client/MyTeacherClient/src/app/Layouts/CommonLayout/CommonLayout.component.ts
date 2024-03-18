import { CommonModule, Location } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationStart, Router, RouterEvent, RouterOutlet } from '@angular/router';
import { CommonLayoutHeaderComponent } from './commonLayoutHeader/CommonLayoutHeader.component';
import { CommonLayoutFooterComponent } from './commonLayoutFooter/CommonLayoutFooter.component';
import { CommonLayoutHeaderBannerComponent } from './commonLayoutHeaderBanner/CommonLayoutHeaderBanner.component';
import { CommonLayoutHeaderCarouselComponent } from './commonLayoutHeaderCarousel/CommonLayoutHeaderCarousel.component';
import { INavItems } from './commonLayoutHeader/CommonLayoutHeaderComponent.types';
import { Subscription, filter } from 'rxjs';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faGithubSquare, faLinkedin, faInstagramSquare, IconDefinition } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-common-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    CommonLayoutHeaderComponent,
    CommonLayoutFooterComponent,
    CommonLayoutHeaderBannerComponent,
    CommonLayoutHeaderCarouselComponent,
    FontAwesomeModule
  ],
  template: `
    <app-common-layout-header [navItems]="navItems" />
    <app-common-layout-header-banner />
    <app-common-layout-header-carousel />
    <div class="grid grid-cols-24 mx-36">
      <ng-template [ngIf]="!this.isHomePageRoute" [ngIfElse]="homeMainContainer">
        <div class="col-span-18">
          <router-outlet />
        </div>
        <div class="col-span-6 text-blue-950">
          <h2>Let's Connect</h2>
          <div class="flex items-center justify-center gap-x-4">
          <fa-icon [classes]="this.iconClasses" [icon]="githubIcon"></fa-icon>
          <fa-icon [classes]="this.iconClasses" [icon]="linkedinIcon"></fa-icon>
          <fa-icon [classes]="this.iconClasses" [icon]="instagramIcon"></fa-icon>
          </div>
          <h2>About Us</h2>
        </div>
      </ng-template>
      <ng-template #homeMainContainer>
        <div class="col-span-24">
          <router-outlet />
        </div>
      </ng-template>
    </div>
    <app-common-layout-footer />
  `,
  styleUrl: './CommonLayout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutComponent implements OnInit, OnDestroy {
  public iconClasses : string[] = ["text-3xl"]
  public githubIcon : IconDefinition = faGithubSquare
  public linkedinIcon : IconDefinition = faLinkedin
  public instagramIcon : IconDefinition = faInstagramSquare
  constructor(private location: Location, private router: Router) {
    this.subscriptionToListenRouteChange = this.router.events.pipe(
      filter((e: any): e is RouterEvent => e instanceof NavigationStart)
    ).subscribe((e: any) => {
      if (e instanceof NavigationStart) {
        if (e.url == '/') this.isHomePageRoute = true
        else this.isHomePageRoute = false;
      }
    });
  }
  ngOnDestroy(): void {
    this.subscriptionToListenRouteChange.unsubscribe()
  }
  public subscriptionToListenRouteChange : Subscription;
  public isHomePageRoute: boolean = false;
  ngOnInit(): void {
    if (this.location.path() == '') this.isHomePageRoute = true;
  }

  public navItems: INavItems[] = [
    {
      id: 1,
      key: 'publicProtection',
      value: 'Public Protection',
      href: '/',
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'subscribeForCouncilMeetingNews',
          value: 'Subscribe for Council Meeting News',
        },
        {
          id: 2,
          href: '/',
          key: 'greatTeaching',
          value: 'Great Teaching',
        },
        {
          id: 3,
          href: '/',
          key: 'ourFreePublicNewspaper',
          value: 'Our Free Public Newspaper',
        },
      ],
    },
    {
      id: 2,
      key: 'parents',
      value: 'Parents',
      href: '/',
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'ourFreePublicNewspaper',
          value: 'Our Free Public Newspaper',
        },
        {
          id: 2,
          href: '/',
          key: 'greatTeaching',
          value: 'Great Teaching',
        },
      ],
    },
    {
      id: 3,
      key: 'members',
      value: 'Members',
      href: '/',
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'members',
          value: 'Members',
        },
        {
          id: 2,
          href: '/',
          key: 'membershipAndOtherFees',
          value: 'Membership and Other Fees',
        },
      ],
    },
    {
      id: 4,
      key: 'becomingATeacher',
      value: 'Becoming a Teacher',
      href: '/',
      childItems: [
        {
          id: 1,
          href: '/applyUs',
          key: 'applying',
          value: 'Applying',
        },
        {
          id: 2,
          href: '/',
          key: 'requirements',
          value: 'Requirements',
        },
      ],
    },
    {
      id: 5,
      key: 'aboutTheCollege',
      value: 'About the College',
      href: '/',
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'strategicPlan',
          value: 'Strategic Plan',
        },
        {
          id: 2,
          href: '/',
          key: 'whatWeDo',
          value: 'What We Do',
        },
      ],
    },
  ];
}
