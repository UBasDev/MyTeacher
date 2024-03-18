import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-apply',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule
  ],
  template: `
    <div>
      <h2 class="text-blue-950 !font-bold">Apply as a Teacher or Student</h2>
      <p class="text-blue-950">Individuals are now able to apply and start teaching or learning!</p>
      <button mat-flat-button color="primary">Primary</button>
    </div>
  `,
  styleUrl: './Apply.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyComponent implements OnInit {
  constructor(private cd: ChangeDetectorRef){}
  ngOnInit(): void {
    
  }
}