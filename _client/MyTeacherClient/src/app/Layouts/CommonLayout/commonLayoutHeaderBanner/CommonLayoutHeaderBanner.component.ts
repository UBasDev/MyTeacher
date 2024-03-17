import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-common-layout-header-banner',
  standalone: true,
  imports: [
    CommonModule,
  ],
  template: `
    <div style="backgroundColor: rgb(27, 42, 92)" class="py-4 text-center">
              <p class="text-white underline cursor-pointer hover:no-underline !m-0">{{bannerText}}</p>
      </div>
  `,
  styleUrl: './CommonLayoutHeaderBanner.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutHeaderBannerComponent {
  bannerText : string = "Self-serve options available online for College members, applicants, and third parties";
}
