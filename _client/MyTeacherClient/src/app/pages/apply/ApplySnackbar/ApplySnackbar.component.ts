import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MAT_SNACK_BAR_DATA, MatSnackBarAction, MatSnackBarActions, MatSnackBarLabel, MatSnackBarRef } from '@angular/material/snack-bar';
import { IApplySnackbarMessage } from '../ApplyModal/ApplyModal.component.types';

@Component({
  selector: 'app-apply-snackbar',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule, MatSnackBarLabel, MatSnackBarActions, MatSnackBarAction,
    MatIconModule
  ],
  template: `<span class="text-white" matSnackBarLabel>
  {{this.data.message}}
</span>
<span matSnackBarActions>
  <button class="px-6 text-red-900" mat-button matSnackBarAction (click)="snackBarRef.dismissWithAction()">
  <mat-icon>close</mat-icon>
  </button>
</span>`,
  styleUrl: './ApplySnackbar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplySnackbarComponent {
  public snackBarRef = inject(MatSnackBarRef)
  public data: IApplySnackbarMessage = inject(MAT_SNACK_BAR_DATA)
  constructor(){
  }
}
