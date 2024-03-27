import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
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
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApplySnackbarComponent } from '../ApplySnackbar/ApplySnackbar.component';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import {
  HttpClient,
  HttpClientModule,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import {
  GetRolesWithoutAdminResponse,
  GetRolesWithoutAdminResponseModel,
} from '../../../ResponseModels/Role/GetRolesWithoutAdmin/GetRolesWithoutAdminResponse';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CreateSingleUserResponse, ErrorCreateSingleUserResponse } from '../../../ResponseModels/User/CreateSingleUserResponse';
import { IApplySnackbarMessage } from './ApplyModal.component.types';
import { Store } from '@ngrx/store';
import { IUserInitialState } from '../../../StateStore/User/UserReducers/UserReducers';
import { UserStateActions } from '../../../StateStore/User/UserActions/UserActions';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-apply-modal',
  standalone: true,
  template: `
    <div class="text-end pr-2 pt-2">
      <mat-icon mat-dialog-close class="cursor-pointer hover:scale-125"
        >close</mat-icon
      >
    </div>
    <div class="text-center p-0 m-0">
      <h3 mat-dialog-title>Are you ready to join us?</h3>
    </div>
    <mat-dialog-content class="mat-typography !pt-0 !mt-0">
      <form
        (ngSubmit)="onFormSubmitHandle($event)"
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

          <mat-form-field class="birthDateInput">
            <mat-label>Your Birthdate</mat-label>
            <input
              (change)="checkIfBirthDateInputContainsCharacters($event)"
              [name]="this.birthDateInputKey"
              [formControlName]="this.birthDateInputKey"
              matInput
              [matDatepicker]="dp"
              placeholder="dd/mm/yyyy"
            />
            <mat-datepicker-toggle
              matIconSuffix
              [for]="dp"
            ></mat-datepicker-toggle>
            <mat-datepicker #dp></mat-datepicker>
            @if (!this.isBirthDateInputValid) {
            <mat-error
              ><strong>{{ birthDateInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
        </div>

        <div class="col-span-24 grid grid-cols-24 items-center gap-x-4">
          <!-- <mat-form-field class="w-full">
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
          </mat-form-field> -->
          @if(this.selectInputRoles.length>0){
          <mat-form-field class="col-span-6">
            <mat-label>What is your role?</mat-label>
            <mat-select name="roleCode" [formControlName]="this.roleInputKey">
              @for (currentRole of selectInputRoles; track currentRole.id) {
              <mat-option [value]="currentRole.shortCode">
                {{ currentRole.name }}
              </mat-option>
              }
            </mat-select>
            @if (!this.isRoleInputValid) {
            <mat-error
              ><strong>{{ roleInputErrorMessage }}</strong></mat-error
            >
            }
          </mat-form-field>
          } @if(this.isroleInputSelectedAsTeacher){
          <p class="text-xs col-span-18">
            <strong>*</strong> I am here as a teacher, want to earn some money
            and expand my network from this platform.
          </p>
          } @else if(this.isroleInputSelectedAsStudent){
          <p class="text-xs col-span-18">
            <strong>*</strong> I am here as a student, want to learn new
            information and improve myself.
          </p>
          }
        </div>
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
        <button #submitFormButton type="submit" class="hidden"></button>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close cdkFocusInitial>Cancel</button>
      <button (click)="handleFormApply()" mat-button>Apply</button>
    </mat-dialog-actions>
  `,
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
    MatSelectModule,
    HttpClientModule,
    MatDatepickerModule,
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-gb' },
    provideNativeDateAdapter(),
  ],
  styleUrl: './ApplyModal.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplyModalComponent implements OnInit, OnDestroy {
  @ViewChild('fileInputElement') fileInputElement: ElementRef | null = null;
  @ViewChild('fileInputLabelElement') fileInputLabelElement: ElementRef | null =
    null;
  @ViewChild('submitFormButton') submitFormButton: ElementRef | null = null;
  componentDestroyed$: Subject<boolean> = new Subject()
  public currentMatDialogRef = inject(MatDialogRef);
  public readonly turkishAlphabetRegex =
    /[abcçdefgğhıijklmnoöprsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVWXYZ]/;
  public onFormSubmitHandle(event: any) {
    event.preventDefault();
    var formData1: any = new FormData();
    formData1.append('Username', this.usernameInputCurrentValue);
    formData1.append('Email', this.emailInputCurrentValue);
    formData1.append('Password', this.passwordInputCurrentValue);
    formData1.append('Age', this.ageInputCurrentValue);
    formData1.append('Firstname', this.firstnameInputCurrentValue);
    formData1.append('Lastname', this.lastnameInputCurrentValue);
    formData1.append(
      'BirthDate',
      new Date(this.birthDateInputCurrentValue ?? new Date()).getTime()
    );
    formData1.append('RoleCode', this.roleInputCurrentValue);
    if (this.selectedProfilePictureData != null) {
      //If there is no profile picture
      formData1.append('ProfilePicture', this.selectedProfilePictureData);
    }
    this._httpClient
      .post<CreateSingleUserResponse>('http://localhost:5028/api/users/create-single-user', formData1,{
        withCredentials: true
      }).pipe(
        takeUntil(this.componentDestroyed$)
      )
      .subscribe({
        next: (res: CreateSingleUserResponse) =>{
          console.log('SUCCESS WOKRS', res)
          if(res.isSuccessful) {
            localStorage.setItem("userProfile", JSON.stringify(res.payload))
            this.state_store.dispatch(UserStateActions.userLoggedIn({
              loggedInUser: {
                username: res.payload?.username ?? "",
                email: res.payload?.email ?? "",
                firstname: res.payload?.firstname ?? "",
                lastname: res.payload?.lastname ?? "",
                roleName: res.payload?.roleName ?? "",
                birthDate: res.payload?.birthDate ?? 0,
                age: res.payload?.age ?? 0
              }
            }))
            this.currentMatDialogRef.close()
          };
        },
        error: (err: ErrorCreateSingleUserResponse)=>{
          console.log('ERROR WOKRS', err)
          this.openSnackBar(err.error.errorMessage ?? "Something went wrong while registering")
        },
        complete: ()=>{
          console.log('OPERATION IS FINISHED')
        }
      });
      /*
        if(res.isSuccessful) this.currentMatDialogRef.close();
        else {
          console.log(res.errorMessage)
          this.openSnackBar(res.errorMessage ?? "Something went wrong while registering")
        }
      */
  }
  public handleFormApply() {
    Object.keys(this.applyForm.controls).forEach((field) => {
      const control = this.applyForm.get(field);
      control?.markAsTouched({ onlySelf: true });
    });
    if (this.applyForm.valid) this.submitFormButton?.nativeElement.click();
    else this.openSnackBar("Your form is invalid");
  }

  selectInputRoles: GetRolesWithoutAdminResponseModel[] = [];
  constructor(
    private readonly _elementRef: ElementRef,
    private readonly _changeDetector: ChangeDetectorRef,
    private readonly _formBuilder: FormBuilder,
    private readonly _snackBar: MatSnackBar,
    private _httpClient: HttpClient,
    private state_store: Store<{ globalUserState: IUserInitialState }>
  ) {}
  ngOnDestroy(): void {
    this.componentDestroyed$.next(true)
    this.componentDestroyed$.complete()
  }
  ngOnInit(): void {
    this.imagePreviewSource = this.initialImagePreviewSource;
    this._httpClient
      .get<GetRolesWithoutAdminResponse>(
        'http://localhost:5028/api/Roles/get-roles-without-admin'
      ).pipe(
        takeUntil(this.componentDestroyed$)
      )
      .subscribe((response: GetRolesWithoutAdminResponse) => {
        if (response.isSuccessful) {
          let idIndexer = 1;
          response.payload?.forEach(
            (currentValue: GetRolesWithoutAdminResponseModel) => {
              currentValue.id = idIndexer;
              this.selectInputRoles.push(currentValue);
              idIndexer++;
            }
          );
          this._changeDetector.markForCheck();
        }
      });
    // this.options = [
    //   ...this.options,
    //   'One', 'Two', 'Three'
    // ]
    // this.filteredOptions = this.applyForm.controls[
    //   this.companyNameInputKey
    // ].valueChanges.pipe(
    //   startWith(''),
    //   map((value: any) => this.filter1(value || ''))
    // );
  }

  private readonly initialImagePreviewSource: string =
    '/assets/apply/blank_profile_picture_apply_form.png';
  public initialImagePreviewValue: any = null;
  public imagePreviewSource: string | ArrayBuffer | null | undefined;
  private selectedProfilePictureData: File | null = null;

  private isFormValidationSnackBarOpen: boolean = false;

  public usernameInputKey: string = 'username';
  public emailInputKey: string = 'email';
  public passwordInputKey: string = 'password';
  public passwordVerifyInputKey: string = 'passwordVerify';
  public ageInputKey: string = 'age';
  public firstnameInputKey: string = 'firstname';
  public lastnameInputKey: string = 'lastname';
  public roleInputKey: string = 'roleCode';
  public birthDateInputKey: string = 'birthDate';

  public applyForm = this._formBuilder.group({
    [this.usernameInputKey]: ['', this.usernameInputValidator()],
    [this.emailInputKey]: ['', this.emailInputValidator()],
    [this.passwordInputKey]: ['', this.passwordInputValidator()],
    [this.passwordVerifyInputKey]: ['', this.passwordVerifyInputValidator()],
    [this.ageInputKey]: [1, this.ageInputValidator()],
    [this.firstnameInputKey]: ['', this.firstnameInputValidator()],
    [this.lastnameInputKey]: ['', this.lastnameInputValidator()],
    [this.roleInputKey]: ['', this.roleInputValidator()],
    [this.birthDateInputKey]: [null, this.birthDateInputValidator()],
  });

  /*
  options: string[] = [];
  public filteredOptions: Observable<string[]> | null = null;

  private filter1(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter((option) =>
      option.toLowerCase().includes(filterValue)
    );
  }
  */

  get usernameInputCurrentValue() {
    return this.applyForm.controls[this.usernameInputKey].value;
  }
  get passwordInputCurrentValue() {
    return this.applyForm.controls[this.passwordInputKey].value;
  }
  get emailInputCurrentValue() {
    return this.applyForm.controls[this.emailInputKey].value;
  }
  get ageInputCurrentValue() {
    return this.applyForm.controls[this.ageInputKey].value;
  }
  get firstnameInputCurrentValue() {
    return this.applyForm.controls[this.firstnameInputKey].value;
  }
  get lastnameInputCurrentValue() {
    return this.applyForm.controls[this.lastnameInputKey].value;
  }
  get roleInputCurrentValue() {
    return this.applyForm.controls[this.roleInputKey].value;
  }
  get birthDateInputCurrentValue() {
    return this.applyForm.controls[this.birthDateInputKey].value;
  }
  get isroleInputSelectedAsTeacher() {
    return (
      this.applyForm.controls[this.roleInputKey].value ==
      this.selectInputRoles.find((r) => r.name == 'Teacher')?.shortCode
    );
  }
  get isroleInputSelectedAsStudent() {
    return (
      this.applyForm.controls[this.roleInputKey].value ==
      this.selectInputRoles.find((r) => r.name == 'Student')?.shortCode
    );
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
  get isRoleInputValid() {
    return this.applyForm.controls[this.roleInputKey].errors?.['isValid'];
  }
  get roleInputErrorMessage() {
    return this.applyForm.controls[this.roleInputKey].errors?.['errorMessage'];
  }
  get isBirthDateInputValid() {
    return this.applyForm.controls[this.birthDateInputKey].errors?.['isValid'];
  }
  get birthDateInputErrorMessage() {
    return this.applyForm.controls[this.birthDateInputKey].errors?.[
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
      this.isRoleInputValid &&
      this.isBirthDateInputValid
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

  private roleInputValidator(): ValidatorFn {
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

  private birthDateInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (value == '' || value == null || value == undefined)
        return {
          isValid: false,
          errorMessage: 'This field is required',
        };
      return null;
    };
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
        this.selectedProfilePictureData = fileList![0];
        this._changeDetector.markForCheck();
      };
      fileReader.readAsDataURL(fileList[0]);
    }
  }
  openSnackBar(message: string) {
    if (!this.isFormValidationSnackBarOpen) {
      const snackbarMessage: IApplySnackbarMessage = {
        message
      }
      const snackbarRef = this._snackBar.openFromComponent(
        ApplySnackbarComponent,
        {
          duration: 3 * 1000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
          panelClass: ['snackbarBackgroundColor'],
          data: snackbarMessage
        }
      );
      this.isFormValidationSnackBarOpen = true;
      snackbarRef.afterDismissed().pipe(
        takeUntil(this.componentDestroyed$)
      ).subscribe((e: any) => {
        this.isFormValidationSnackBarOpen = false;
      });
    }
  }
  public checkIfBirthDateInputContainsCharacters(event: any) {
    if (this.turkishAlphabetRegex.test(event.target.value))
      this.applyForm.controls[this.birthDateInputKey].setValue(null);
  }
}
