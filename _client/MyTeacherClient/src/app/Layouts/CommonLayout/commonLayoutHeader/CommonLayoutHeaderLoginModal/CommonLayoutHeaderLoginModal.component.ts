import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faSmile,
  IconDefinition
} from '@fortawesome/free-regular-svg-icons';
import { ICommonLayoutHeaderLoginModalComponentStates } from './CommonLayoutHeaderLoginModalComponent.types';

@Component({
  selector: 'app-common-layout-header-login-modal',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    FontAwesomeModule
  ],
  template: `
    <div class="text-end pr-2 pt-2">
      <mat-icon mat-dialog-close class="cursor-pointer hover:scale-125"
        >close</mat-icon
      >
    </div>
    <div class="text-center p-0 m-0">
      <h3 mat-dialog-title>Happy to see you again 
        <fa-icon [icon]="this.componentStates.smileyFaceIcon" />
      </h3>
    </div>
    <mat-dialog-content class="mat-typography !pt-0 !mt-0">
      SOME BODY
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close cdkFocusInitial>Cancel</button>
      <button mat-button mat-dialog-close>Login</button>
    </mat-dialog-actions>
  `,
  styleUrl: './CommonLayoutHeaderLoginModal.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutHeaderLoginModalComponent {
  public componentStates: ICommonLayoutHeaderLoginModalComponentStates = {
    smileyFaceIcon: faSmile
  }
  
  constructor(){
  }
  
}
