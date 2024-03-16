import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-common-layout-footer',
  standalone: true,
  imports: [
    CommonModule,
  ],
  template: `<p>CommonLayoutFooter works!</p>`,
  styleUrl: './CommonLayoutFooter.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutFooterComponent { }
