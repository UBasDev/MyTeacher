import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {MatDialog, MatDialogModule, MatDialogConfig, MatDialogRef} from '@angular/material/dialog';
import { ApplyModalComponent } from './ApplyModal/ApplyModal.component';
import { Subject, takeUntil } from 'rxjs';
import { ApplyInfoComponent } from './ApplyInfo/ApplyInfo.component';

@Component({
  selector: 'app-apply',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    ApplyInfoComponent
  ],
  template: `
    <div class="grid grid-cols-24">
      <div class="col-span-20">
      <h2 class="text-blue-950 !font-bold">Apply as a Teacher or Student</h2>
      <p class="text-blue-950">Individuals are now able to apply and start teaching or learning!</p>
      <button (click)="openApplicationForm()" mat-flat-button color="primary">Apply</button>
      </div>
      <div class="col-span-4">
        <app-apply-info/>
      </div>
    </div>
  `,
  styleUrl: './Apply.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyComponent implements OnInit, OnDestroy {
  private currentUsedDialog?: MatDialogRef<ApplyModalComponent, any>
  componentDestroyed$: Subject<boolean> = new Subject()
  public openApplicationForm(){
    this.currentUsedDialog = this.dialog.open(ApplyModalComponent, this.CreateMatDialogConfig());
    this.currentUsedDialog.afterClosed().pipe(
      takeUntil(this.componentDestroyed$)
    ).subscribe((result: any) => {
      console.log(`Dialog result: ${result}`);
    });
  }
  
  private CreateMatDialogConfig() : MatDialogConfig{
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true
    dialogConfig.hasBackdrop = true
    dialogConfig.width = "40%"
    return dialogConfig
  }
  constructor(public dialog: MatDialog){}
  ngOnDestroy(): void {
    this.componentDestroyed$.next(true)
    this.componentDestroyed$.complete()
  }
  ngOnInit(): void {
  }
}