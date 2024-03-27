import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {
  IBreadcrumbItems,
  INavDropdownItems,
  INavItems,
} from './CommonLayoutHeaderComponent.types';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IUserInitialState } from '../../../StateStore/User/UserReducers/UserReducers';
import { Store } from '@ngrx/store';
import { Subject, takeUntil } from 'rxjs';
import {
  faUser,
  faPowerOff
} from '@fortawesome/free-solid-svg-icons';
import { CommonLayoutHeaderLoginModalComponent } from './CommonLayoutHeaderLoginModal/CommonLayoutHeaderLoginModal.component';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-common-layout-header',
  standalone: true,
  imports: [CommonModule, MatInputModule, MatIconModule, FontAwesomeModule],
  template: `
    <div class="grid grid-cols-24">
      <div class="col-span-8 pt-2">
        <div class="flex items-center justify-start gap-x-2">
          <img
            (click)="goToHomepage()"
            class="cursor-pointer"
            height="75px"
            width="75px"
            src="/assets/homepage/homepage_icon.png"
          />
          <h2 (click)="goToHomepage()" class="text-blue-900 cursor-pointer">
            UCBDev
          </h2>
        </div>
        <hr class="my-1 text-blue-900 bg-blue-900 py-0.5 w-2/6" />
        <p
          (click)="goToHomepage()"
          class="p-0 !m-0 pb-1 text-blue-900 tracking-normal cursor-pointer"
        >
          {{ textUnderIcon }}
        </p>
      </div>
      <div class="col-span-16 py-2">
        <div class="flex items-center justify-end gap-x-2">
          <ng-template
            ngFor
            let-currentItem
            [ngForOf]="breadcrumbItems"
            let-isFirst="first"
            let-isLast="last"
            let-index="index"
            let-count="count"
            let-isEven="even"
            let-isOdd="odd"
          >
            <a
              class="hover:underline text-blue-700 text-xs"
              href="currentItem1.href"
            >
              {{ currentItem.value }}
            </a>
            <span
              [ngStyle]="{
                display: !isLast ? 'inline' : 'none'
              }"
              >/</span
            >
          </ng-template>
          <div
            class="flex items-center justify-center gap-x-1 shrink py-0.5 px-1 rounded-md text-blue-900"
            [ngStyle]="{ border: '1px solid rgb(30, 58, 138)' }"
          >
            <mat-icon
              (click)="searchForText()"
              class="scale-90 cursor-pointer hover:text-blue-950 hover:scale-95"
              matSuffix
              >search</mat-icon
            >
            <input
              (keyup)="searchForTextFromKeyboard($event)"
              class="max-w-28 text-xs text-black focus:text-gray-500"
              placeholder="Search"
            />
          </div>
        </div>

        <div class="flex items-center justify-end gap-x-8 px-2 mt-2">
          <ng-template
            ngFor
            let-currentItem
            [ngForOf]="navItems"
            let-isFirst="first"
            let-isLast="last"
            let-index="index"
            let-count="count"
            let-isEven="even"
            let-isOdd="odd"
          >
            <h2
              (mouseenter)="navItemHoverAddDropdownContent(index)"
              (mouseleave)="navItemHoverRemoveDropdownContent(index)"
              (click)="currentItem.onClickFunction()"
              class="navMenuUnderlineEffect text-blue-900 cursor-pointer relative"
            >
              {{ currentItem.value }}
              @if(currentItem.icon){
              <fa-icon [icon]="currentItem.icon" />
              } @if(currentItem?.childItems != null &&
              currentItem.childItems.length > 0){
              <div
                [ngClass]="
                  'absolute items-stretch justify-center p-1 py-2 gap-y-2 bg-gray-100 text-sm font-semibold z-10 rounded-sm cursor-default w-max ' +
                  (isItemActive(index) ? 'grid grid-cols-24' : 'hidden')
                "
              >
                <ng-template
                  ngFor
                  let-currentChildItem
                  [ngForOf]="currentItem.childItems"
                  let-isChildFirst="first"
                  let-isChildLast="last"
                  let-childIndex="index"
                  let-childCount="count"
                  let-childIsEven="even"
                  let-childIsOdd="odd"
                >
                  <p class="childNavItem col-span-24 p-1 cursor-pointer">
                    {{ currentChildItem.value }}
                    @if(currentChildItem.icon){
                    <fa-icon [icon]="currentChildItem.icon" />
                    }
                  </p>
                </ng-template>
              </div>
              }
            </h2>
          </ng-template>
        </div>
      </div>
    </div>
  `,
  styleUrl: './CommonLayoutHeader.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutHeaderComponent implements OnInit, OnDestroy {
  componentDestroyed$: Subject<boolean> = new Subject()
  private currentUsedDialog?: MatDialogRef<CommonLayoutHeaderLoginModalComponent, any>
  constructor(private readonly router: Router, private state_store: Store<{ globalUserState: IUserInitialState }>, public dialog: MatDialog) {}
  ngOnDestroy(): void {
    this.componentDestroyed$.next(true)
    this.componentDestroyed$.complete()
  }
  ngOnInit(): void {
    this.state_store.select("globalUserState").pipe(
      takeUntil(this.componentDestroyed$)
    ).subscribe((data: IUserInitialState)=>{
      console.log(data)
      if(data.isLoggedIn) {
        this.navItems = this.authenticatedNavItems
        this.authenticatedNavItems.push({
          id: 4,
          key: "profile",
          value: `${data.firstname} ${data.lastname}`,
          href: "/profile",
          icon: faUser,
          onClickFunction: this.doNothingWhenClicked.bind(this),
          childItems:[
            {
              id: 1,
              href: "/profile",
              key: "profile",
              value: "My profile",
              icon: null,
              onClickFunction: this.doNothingWhenClicked.bind(this)
            },
            {
              id: 2,
              href: "/profileSettings",
              key: "profileSettings",
              value: "Profile Settings",
              icon: null,
              onClickFunction: this.doNothingWhenClicked.bind(this)
            },
            {
              id: 2,
              href: "/logout",
              key: "logout",
              value: "Logout",
              icon: faPowerOff,
              onClickFunction: this.doNothingWhenClicked.bind(this)
            }
          ]
        })
      }
      else this.navItems = this.unAuthenticatedNavItems
    })
  }

  navDropdownItems: INavDropdownItems[] = [];

  public goToHomepage() {
    this.router.navigate(['/']);
  }
  public isItemActive(index: number): boolean {
    return this.navDropdownItems.some(
      (d) => d.index == index && d.isACtive == true
    );
  }
  public navItemHoverAddDropdownContent(index: number) {
    var foundActiveItem = this.navDropdownItems.find((d) => d.index == index);
    if (foundActiveItem == null) {
      this.navDropdownItems.push({
        index: index,
        isACtive: true,
      });
    } else {
      foundActiveItem.isACtive = true;
    }
  }
  public navItemHoverRemoveDropdownContent(index: number) {
    var foundActiveItem = this.navDropdownItems.find((d) => d.index == index);
    if (foundActiveItem == null) return;
    else foundActiveItem.isACtive = false;
  }

  public readonly textUnderIcon: string = 'To Inspire You To Learn';
  public searchText: string = '';
  public readonly breadcrumbItems: IBreadcrumbItems[] = [
    {
      id: 1,
      key: 'contact',
      value: 'Contact',
      href: '/',
    },
    {
      id: 2,
      key: 'faq',
      value: 'FAQ',
      href: '/',
    },
    {
      id: 3,
      key: 'yourCollegeAndYou',
      value: 'Your College and You',
      href: '/',
    },
    {
      id: 4,
      key: 'employers',
      value: 'Employers',
      href: '/',
    },
    {
      id: 5,
      key: 'providers',
      value: 'Providers',
      href: '/',
    },
  ];
  public navItems: INavItems[] = []
  public unAuthenticatedNavItems: INavItems[] = [
    {
      id: 1,
      key: 'publicProtection',
      value: 'Public Protection',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'subscribeForCouncilMeetingNews',
          value: 'Subscribe for Council Meeting News',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'greatTeaching',
          value: 'Great Teaching',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 3,
          href: '/',
          key: 'ourFreePublicNewspaper',
          value: 'Our Free Public Newspaper',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    },
    {
      id: 2,
      key: 'parents',
      value: 'Parents',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'ourFreePublicNewspaper',
          value: 'Our Free Public Newspaper',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'greatTeaching',
          value: 'Great Teaching',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    },
    {
      id: 3,
      key: 'members',
      value: 'Members',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'members',
          value: 'Members',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'membershipAndOtherFees',
          value: 'Membership and Other Fees',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    },
    {
      id: 4,
      key: 'signUp',
      value: 'Sign up',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/signUp',
          key: 'applying',
          value: 'Applying',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'requirements',
          value: 'Requirements',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    },
    {
      id: 5,
      key: 'login',
      value: 'Login',
      href: '/',
      icon: null,
      onClickFunction: this.openLoginModal.bind(this),
      childItems: [
      ],
    },
  ];
  public authenticatedNavItems: INavItems[] = [
    {
      id: 1,
      key: 'publicProtection',
      value: 'Public Protection',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'subscribeForCouncilMeetingNews',
          value: 'Subscribe for Council Meeting News',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'greatTeaching',
          value: 'Great Teaching',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 3,
          href: '/',
          key: 'ourFreePublicNewspaper',
          value: 'Our Free Public Newspaper',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    },
    {
      id: 2,
      key: 'parents',
      value: 'Parents',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'ourFreePublicNewspaper',
          value: 'Our Free Public Newspaper',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'greatTeaching',
          value: 'Great Teaching',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    },
    {
      id: 3,
      key: 'members',
      value: 'Members',
      href: '/',
      icon: null,
      onClickFunction: this.doNothingWhenClicked.bind(this),
      childItems: [
        {
          id: 1,
          href: '/',
          key: 'members',
          value: 'Members',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
        {
          id: 2,
          href: '/',
          key: 'membershipAndOtherFees',
          value: 'Membership and Other Fees',
          icon: null,
          onClickFunction: this.doNothingWhenClicked.bind(this)
        },
      ],
    }
  ];
  public doNothingWhenClicked(): void{

  }
  public openLoginModal(){
    this.currentUsedDialog = this.dialog.open(CommonLayoutHeaderLoginModalComponent, this.CreateMatDialogConfig());
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
  public searchForText() {
    if (this.searchText.length > 0) console.log(this.searchText);
  }
  public searchForTextFromKeyboard(e: any) {
    if (e.key == 'Enter' || e.code == 'Enter' || e.which == 13) {
      if (this.searchText.length > 0) console.log(this.searchText);
      else alert('There is nothing to search here');
    } else {
      this.searchText = e.target.value;
    }
  }
}
