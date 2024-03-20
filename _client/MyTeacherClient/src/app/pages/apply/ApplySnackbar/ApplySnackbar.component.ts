import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarAction, MatSnackBarActions, MatSnackBarLabel, MatSnackBarRef } from '@angular/material/snack-bar';

@Component({
  selector: 'app-apply-snackbar',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule, MatSnackBarLabel, MatSnackBarActions, MatSnackBarAction
  ],
  template: `<span class="!text-red-600" matSnackBarLabel>
  Your form is invalid
</span>
<span matSnackBarActions>
  <button class="px-6" mat-button matSnackBarAction (click)="snackBarRef.dismissWithAction()">🍕</button>
</span>`,
  styleUrl: './ApplySnackbar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplySnackbarComponent {
  public snackBarRef = inject(MatSnackBarRef)
}
