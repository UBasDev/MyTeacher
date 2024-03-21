import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ApplySnackbarComponent } from '../ApplySnackbar/ApplySnackbar.component';
import { map, Observable, startWith } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-apply-modal',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    ReactiveFormsModule,
    MatInputModule,
    MatAutocompleteModule,
    AsyncPipe,
    MatFormFieldModule,
  ],
  template: `
    <div class="text-end pr-2 pt-2">
      <mat-icon mat-dialog-close class="cursor-pointer hover:scale-125"
        >close</mat-icon
      >
    </div>
    <div class="text-center p-0 m-0">
      <h3 mat-dialog-title>Application Form</h3>
    </div>
    <mat-dialog-content class="mat-typography !pt-0 !mt-0">
      <form
        (ngSubmit)="onApplyFormSubmit()"
        [formGroup]="this.applyForm"
        class="grid grid-cols-24 gap-y-4"
      >
        <div class="col-span-24 flex items-center gap-x-4 justify-center">
          <ng-template [ngIf]="isProfilePictureChanged">
            <button
              (click)="triggerFileInputLabel($event)"
              mat-fab
              extended
              color="primary"
              type="button"
            >
              <mat-icon>autorenew</mat-icon>
              Change
            </button>
          </ng-template>
          <label
            #fileInputLabelElement
            [class.pointer-events-none]="this.isProfilePictureChanged"
            class="!m-0"
            for="file-upload"
          >
            <img
              id="profilePhotoPreview"
              class="rounded-full cursor-pointer !p-0 !m-0 !w-48 !h-48"
              [src]="this.imagePreviewSource"
              width="250"
              height="250"
            />
          </label>
          <ng-template [ngIf]="isProfilePictureChanged">
            <button
              (click)="removeProfilePicturePreview($event)"
              class="!m-0"
              mat-fab
              extended
              color="warn"
              type="button"
            >
              <mat-icon>autorenew</mat-icon>
              Delete
            </button>
          </ng-template>
        </div>
        <div class="col-span-24 text-center">
          <small>Your Picture</small>
        </div>
        <div class="col-span-24 flex items-center gap-x-4 justify-center">
          <mat-form-field style="flex-grow: 1">
            <mat-label>Username</mat-label>
            <input
              [name]="this.usernameInputKey"
              type="text"
              matInput
              [formControlName]="this.usernameInputKey"
              placeholder="Ex. John Doe"
            />
            @if (this.usernameInputCurrentValue) {
            <button
              matSuffix
              mat-icon-button
              aria-label="Clear"
              (click)="this.resetUsernameInputCurrentValue()"
            >
              <mat-icon>close</mat-icon>
            </button>
            } @if (!this.isUsernameInputValid) {
            <mat-error
              ><strong>{{ usernameInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
          <mat-form-field style="flex-grow: 1">
            <mat-label>Email</mat-label>
            <input
              [name]="this.emailInputKey"
              type="text"
              matInput
              [formControlName]="this.emailInputKey"
              placeholder="Ex. john.doe@example.com"
            />
            @if (this.emailInputCurrentValue) {
            <button
              matSuffix
              mat-icon-button
              aria-label="Clear"
              (click)="this.resetEmailInputCurrentValue()"
            >
              <mat-icon>close</mat-icon>
            </button>
            } @if (!this.isEmailInputValid) {
            <mat-error
              ><strong>{{ emailInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
        </div>

        <div class="col-span-24 flex items-center gap-x-4 justify-center">
          <mat-form-field style="flex-grow: 1">
            <mat-label>Password</mat-label>
            <input
              [name]="this.passwordInputKey"
              type="text"
              matInput
              [formControlName]="this.passwordInputKey"
              placeholder="***"
            />
            @if (!this.isPasswordInputValid) {
            <mat-error
              ><strong>{{ passwordInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
          <mat-form-field style="flex-grow: 1">
            <mat-label>Password Again</mat-label>
            <input
              [name]="this.passwordVerifyInputKey"
              type="text"
              matInput
              [formControlName]="this.passwordVerifyInputKey"
              placeholder="***"
            />
            @if (!this.isPasswordVerifyInputValid) {
            <mat-error
              ><strong>{{ passwordVerifyInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
        </div>
        <div class="col-span-24 flex items-center justify-center gap-x-4">
          <mat-form-field class="ageInput">
            <mat-label>Age</mat-label>
            <input
              [name]="this.ageInputKey"
              type="number"
              matInput
              [formControlName]="this.ageInputKey"
              placeholder="Your age"
            />
            @if (!this.isAgeInputValid) {
            <mat-error class="!text-xs"
              ><strong>{{ ageInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
          <mat-form-field class="firstnameInput">
            <mat-label>Firstname</mat-label>
            <input
              [name]="this.firstnameInputKey"
              type="text"
              matInput
              [formControlName]="this.firstnameInputKey"
              placeholder="Your firstname"
            />
            @if (this.firstnameInputCurrentValue) {
            <button
              matSuffix
              mat-icon-button
              aria-label="Clear"
              (click)="this.resetFirstnameInputCurrentValue()"
            >
              <mat-icon>close</mat-icon>
            </button>
            } @if (!this.isFirstnameInputValid) {
            <mat-error
              ><strong>{{ firstnameInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
          <mat-form-field class="lastnameInput">
            <mat-label>Lastname</mat-label>
            <input
              [name]="this.lastnameInputKey"
              type="text"
              matInput
              [formControlName]="this.lastnameInputKey"
              placeholder="Your lastname"
            />
            @if (this.lastnameInputCurrentValue) {
            <button
              matSuffix
              mat-icon-button
              aria-label="Clear"
              (click)="this.resetLastnameInputCurrentValue()"
            >
              <mat-icon>close</mat-icon>
            </button>
            } @if (!this.isLastnameInputValid) {
            <mat-error
              ><strong>{{ lastnameInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
        </div>

        <div class="col-span-24">
          <mat-form-field class="w-full">
            <mat-label>Company Name</mat-label>
            <input
              [name]="this.companyNameInputKey"
              type="text"
              placeholder="Pick one"
              aria-label="Number"
              matInput
              [formControlName]="this.companyNameInputKey"
              [matAutocomplete]="auto"
            />
            <mat-autocomplete #auto="matAutocomplete">
              @for (option of filteredOptions | async; track option) {
              <mat-option [value]="option">{{ option }}</mat-option>
              }
            </mat-autocomplete>
          </mat-form-field>
        </div>

        <button type="submit">SUBMIT</button>
        <input
          #fileInputElement
          [value]="this.initialImagePreviewValue"
          (change)="changeProfilePicture($event)"
          (click)="resetInputElementValue($event)"
          hidden
          multiple="false"
          type="file"
          accept=".jpg, .jpeg, .png"
          name="file-upload"
          id="file-upload"
        />
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close cdkFocusInitial>Cancel</button>
    </mat-dialog-actions>
  `,
  styleUrl: './ApplyModal.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyModalComponent implements OnInit {
  @ViewChild('fileInputElement') fileInputElement: ElementRef | null = null;
  @ViewChild('fileInputLabelElement') fileInputLabelElement: ElementRef | null =
    null;

  constructor(
    private readonly _elementRef: ElementRef,
    private readonly _changeDetector: ChangeDetectorRef,
    private readonly _formBuilder: FormBuilder,
    private readonly _snackBar: MatSnackBar
  ) {}
  ngOnInit(): void {
    this.imagePreviewSource = this.initialImagePreviewSource;
    this.options = [
      ...this.options,
      'One', 'Two', 'Three'
    ]
    this.filteredOptions = this.applyForm.controls[
      this.companyNameInputKey
    ].valueChanges.pipe(
      startWith(''),
      map((value: any) => this.filter1(value || ''))
    );
  }

  private readonly initialImagePreviewSource: string =
    '/assets/apply/blank_profile_picture_apply_form.png';
  public initialImagePreviewValue: any = null;
  public imagePreviewSource: string | ArrayBuffer | null | undefined;
  private selectedProfilePicture: File | null = null;

  private isFormValidationSnackBarOpen: boolean = false;

  public usernameInputKey: string = 'username';
  public emailInputKey: string = 'email';
  public passwordInputKey: string = 'password';
  public passwordVerifyInputKey: string = 'passwordVerify';
  public ageInputKey: string = 'age';
  public firstnameInputKey: string = 'firstname';
  public lastnameInputKey: string = 'lastname';
  public companyNameInputKey: string = 'companyName';
  public companyDescriptionInputKey: string = 'companyDescription';
  public companyAdressInputKey: string = 'companyAdress';

  public applyForm = this._formBuilder.group({
    [this.usernameInputKey]: ['', this.usernameInputValidator()],
    [this.emailInputKey]: ['', this.emailInputValidator()],
    [this.passwordInputKey]: ['', this.passwordInputValidator()],
    [this.passwordVerifyInputKey]: ['', this.passwordVerifyInputValidator()],
    [this.ageInputKey]: [1, this.ageInputValidator()],
    [this.firstnameInputKey]: ['', this.firstnameInputValidator()],
    [this.lastnameInputKey]: ['', this.lastnameInputValidator()],
    [this.companyNameInputKey]: ['', this.companyNameInputValidator()],
  });

  options: string[] = [];
  public filteredOptions: Observable<string[]> | null = null;

  private filter1(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter((option) =>
      option.toLowerCase().includes(filterValue)
    );
  }

  get usernameInputCurrentValue() {
    return this.applyForm.controls[this.usernameInputKey].value;
  }
  get emailInputCurrentValue() {
    return this.applyForm.controls[this.emailInputKey].value;
  }
  get firstnameInputCurrentValue() {
    return this.applyForm.controls[this.firstnameInputKey].value;
  }
  get lastnameInputCurrentValue() {
    return this.applyForm.controls[this.lastnameInputKey].value;
  }
  public resetUsernameInputCurrentValue() {
    this.applyForm.controls[this.usernameInputKey].markAsUntouched();
    this.applyForm.controls[this.usernameInputKey].setValue('');
  }
  public resetEmailInputCurrentValue() {
    this.applyForm.controls[this.emailInputKey].markAsUntouched();
    this.applyForm.controls[this.emailInputKey].setValue('');
  }
  public resetFirstnameInputCurrentValue() {
    this.applyForm.controls[this.firstnameInputKey].markAsUntouched();
    this.applyForm.controls[this.firstnameInputKey].setValue('');
  }
  public resetLastnameInputCurrentValue() {
    this.applyForm.controls[this.lastnameInputKey].markAsUntouched();
    this.applyForm.controls[this.lastnameInputKey].setValue('');
  }
  get isUsernameInputValid() {
    return this.applyForm.controls[this.usernameInputKey].errors?.['isValid'];
  }
  get usernameInputErrorMessage() {
    return this.applyForm.controls[this.usernameInputKey].errors?.[
      'errorMessage'
    ];
  }
  get isEmailInputValid() {
    return this.applyForm.controls[this.emailInputKey].errors?.['isValid'];
  }
  get emailInputErrorMessage() {
    return this.applyForm.controls[this.emailInputKey].errors?.['errorMessage'];
  }
  get isPasswordInputValid() {
    return this.applyForm.controls[this.passwordInputKey].errors?.['isValid'];
  }
  get passwordInputErrorMessage() {
    return this.applyForm.controls[this.passwordInputKey].errors?.[
      'errorMessage'
    ];
  }
  get isPasswordVerifyInputValid() {
    return this.applyForm.controls[this.passwordVerifyInputKey].errors?.[
      'isValid'
    ];
  }
  get passwordVerifyInputErrorMessage() {
    return this.applyForm.controls[this.passwordVerifyInputKey].errors?.[
      'errorMessage'
    ];
  }
  get isAgeInputValid() {
    return this.applyForm.controls[this.ageInputKey].errors?.['isValid'];
  }
  get ageInputErrorMessage() {
    return this.applyForm.controls[this.ageInputKey].errors?.['errorMessage'];
  }
  get isFirstnameInputValid() {
    return this.applyForm.controls[this.firstnameInputKey].errors?.['isValid'];
  }
  get firstnameInputErrorMessage() {
    return this.applyForm.controls[this.firstnameInputKey].errors?.[
      'errorMessage'
    ];
  }
  get isLastnameInputValid() {
    return this.applyForm.controls[this.lastnameInputKey].errors?.['isValid'];
  }
  get lastnameInputErrorMessage() {
    return this.applyForm.controls[this.lastnameInputKey].errors?.[
      'errorMessage'
    ];
  }
  get iscompanyNameInputValid() {
    return this.applyForm.controls[this.companyNameInputKey].errors?.['isValid'];
  }
  get companyNameInputErrorMessage() {
    return this.applyForm.controls[this.companyNameInputKey].errors?.[
      'errorMessage'
    ];
  }
  get isFormValid(): boolean {
    return (
      this.isUsernameInputValid &&
      this.isEmailInputValid &&
      this.isPasswordInputValid &&
      this.isPasswordVerifyInputValid &&
      this.isAgeInputValid &&
      this.isFirstnameInputValid &&
      this.isLastnameInputValid && 
      this.iscompanyNameInputValid
    );
  }
  private usernameInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      ) {
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      }
      return null;
    };
  }
  private emailInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      )
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
  }
  private passwordInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      )
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
  }
  private passwordVerifyInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      )
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
  }

  private ageInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (value == null || value == undefined)
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      else if (value < 1)
        return {
          isValid: false,
          errorMessage: "Age can't be smaller than 0",
        };
      else if (value > 100)
        return {
          isValid: false,
          errorMessage: "Age can't be greater than 100",
        };
      return null;
    };
  }

  private firstnameInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      )
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
  }

  private lastnameInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      )
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
  }

  private companyNameInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (
        value == '' ||
        value == null ||
        value == undefined ||
        typeof value != 'string'
      )
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
  }

  public onApplyFormSubmit() {
    Object.keys(this.applyForm.controls).forEach((field) => {
      const control = this.applyForm.get(field);
      control?.markAsTouched({ onlySelf: true });
    });
    const formValues = this.applyForm.value;
    if (this.applyForm.valid) {
    } else this.openSnackBar();
  }

  public triggerFileInputLabel(event: MouseEvent) {
    event.stopPropagation();
    this.fileInputLabelElement?.nativeElement.click();
  }

  public removeProfilePicturePreview(event: MouseEvent) {
    this.imagePreviewSource = this.initialImagePreviewSource;
    this.isProfilePictureChanged = false;
  }
  public isProfilePictureChanged: boolean = false;
  public resetInputElementValue(event: any) {
    event.target.value = '';
  }
  public changeProfilePicture(event: any) {
    const element = event.currentTarget as HTMLInputElement;
    let fileList: FileList | null = element.files;
    if (fileList) {
      const fileReader = new FileReader();
      fileReader.onload = (event) => {
        this.imagePreviewSource = event?.target?.result;
        this.isProfilePictureChanged = true;
        this.selectedProfilePicture = fileList![0];
        this._changeDetector.markForCheck();
      };
      fileReader.readAsDataURL(fileList[0]);
    }
  }
  openSnackBar() {
    if (!this.isFormValidationSnackBarOpen) {
      const snackbarRef = this._snackBar.openFromComponent(
        ApplySnackbarComponent,
        {
          duration: 3 * 1000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
          panelClass: ['snackbarBackgroundColor'],
        }
      );
      this.isFormValidationSnackBarOpen = true;
      snackbarRef.afterDismissed().subscribe((e: any) => {
        this.isFormValidationSnackBarOpen = false;
      });
    }
  }
}
