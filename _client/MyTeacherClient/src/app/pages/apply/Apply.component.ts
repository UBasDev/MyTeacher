import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {MatDialog, MatDialogModule, MatDialogConfig} from '@angular/material/dialog';
import { ApplyModalComponent } from './applyModal/ApplyModal.component';

@Component({
  selector: 'app-apply',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule
  ],
  template: `
    <div>
      <h2 class="text-blue-950 !font-bold">Apply as a Teacher or Student</h2>
      <p class="text-blue-950">Individuals are now able to apply and start teaching or learning!</p>
      <button (click)="openApplicationForm()" mat-flat-button color="primary">Apply</button>
    </div>
  `,
  styleUrl: './Apply.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyComponent implements OnInit {
  private currentUsedDialog:any
  public openApplicationForm(){
    this.currentUsedDialog = this.dialog.open(ApplyModalComponent, this.CreateMatDialogConfig());
    this.currentUsedDialog.afterClosed().subscribe((result: any) => {
      console.log(`Dialog result: ${result}`);
    });
  }
  public closeApplicationForm(){
    this.dialog.closeAll()
  }
  private CreateMatDialogConfig() : MatDialogConfig{
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true
    dialogConfig.hasBackdrop = true
    dialogConfig.width = "30%"
    return dialogConfig
  }
  constructor(public dialog: MatDialog){}
  ngOnInit(): void {
    this.dialog.open(ApplyModalComponent, this.CreateMatDialogConfig());
  }
}