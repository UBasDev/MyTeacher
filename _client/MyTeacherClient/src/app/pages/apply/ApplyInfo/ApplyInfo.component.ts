import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-apply-info',
  standalone: true,
  imports: [
    CommonModule,
  ],
  template: `<p>ApplyInfo works!</p>`,
  styleUrl: './ApplyInfo.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyInfoComponent { }
