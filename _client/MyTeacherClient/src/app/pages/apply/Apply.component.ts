import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-apply',
  standalone: true,
  imports: [
    CommonModule,
  ],
  template: `<p>Apply works!</p>`,
  styleUrl: './Apply.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyComponent implements OnInit {
  constructor(private cd: ChangeDetectorRef){}
  ngOnInit(): void {
    
  }
}