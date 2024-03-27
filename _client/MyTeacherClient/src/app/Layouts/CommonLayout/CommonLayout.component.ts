import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import {  RouterOutlet } from '@angular/router';
import { CommonLayoutHeaderComponent } from './commonLayoutHeader/CommonLayoutHeader.component';
import { CommonLayoutFooterComponent } from './commonLayoutFooter/CommonLayoutFooter.component';
import { CommonLayoutHeaderBannerComponent } from './commonLayoutHeaderBanner/CommonLayoutHeaderBanner.component';
import { CommonLayoutHeaderCarouselComponent } from './commonLayoutHeaderCarousel/CommonLayoutHeaderCarousel.component';

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
    <div class="p-0 m-0 mx-32">
      <app-common-layout-header />
    </div>
    <app-common-layout-header-banner />
    <div class="p-0 m-0 mx-32">
      <app-common-layout-header-carousel />
    </div>
      <div class="p-0 m-0 mx-32">
        <router-outlet />
      </div>
    <app-common-layout-footer />
  `,
  styleUrl: './CommonLayout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutComponent {
}
