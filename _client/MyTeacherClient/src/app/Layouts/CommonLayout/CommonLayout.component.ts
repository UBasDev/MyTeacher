import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonLayoutHeaderComponent } from './CommonLayoutHeader/CommonLayoutHeader.component';
import { CommonLayoutFooterComponent } from './CommonLayoutFooter/CommonLayoutFooter.component';

@Component({
  selector: 'app-common-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    CommonLayoutHeaderComponent,
    CommonLayoutFooterComponent
  ],
  template: `
    <app-common-layout-header/>
    <router-outlet />
    <app-common-layout-footer/>
  `,
  styleUrl: './CommonLayout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutComponent { }
