import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonLayoutHeaderComponent } from './commonLayoutHeader/CommonLayoutHeader.component';
import { CommonLayoutFooterComponent } from './commonLayoutFooter/CommonLayoutFooter.component';
import { CommonLayoutHeaderBannerComponent } from './commonLayoutHeaderBanner/CommonLayoutHeaderBanner.component';
import { CommonLayoutHeaderCarouselComponent } from './commonLayoutHeaderCarousel/CommonLayoutHeaderCarousel/CommonLayoutHeaderCarousel.component';

@Component({
  selector: 'app-common-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    CommonLayoutHeaderComponent,
    CommonLayoutFooterComponent,
    CommonLayoutHeaderBannerComponent,
    CommonLayoutHeaderCarouselComponent
  ],
  template: `
    <app-common-layout-header/>
    <app-common-layout-header-banner/>
    <app-common-layout-header-carousel/>
    <router-outlet />
    <app-common-layout-footer/>
  `,
  styleUrl: './CommonLayout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutComponent { }
