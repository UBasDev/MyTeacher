import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input
} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatMenuModule } from '@angular/material/menu';
import { IBreadcrumbItems, INavDropdownItems, INavItems } from './CommonLayoutHeaderComponent.types';
import { Router } from '@angular/router';

@Component({
  selector: 'app-common-layout-header',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatIconModule,
  ],
  template: `
    <div class="grid grid-cols-24">
      <div class="col-span-8 pt-2 mx-32">
        <div class="flex items-center justify-start gap-x-2">
          <img (click)="goToHomepage()"
            class="cursor-pointer"
            height="75px"
            width="75px"
            src="/assets/homepage/homepage_icon.png"
          />
          <h2 (click)="goToHomepage()" class="text-blue-900 cursor-pointer">UCBDev</h2>
        </div>
        <hr class="my-1 text-blue-900 bg-blue-900 py-0.5 w-2/6" />
        <p (click)="goToHomepage()" class="p-0 !m-0 pb-1 text-blue-900 tracking-normal cursor-pointer">
          {{ textUnderIcon }}
        </p>
      </div>
      <div class="col-span-16 py-2 mx-32">
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
              class="navMenuUnderlineEffect text-blue-900 cursor-pointer"
            >
              {{ currentItem.value }}
              <div
                [ngClass]="
                  'absolute flex-col items-stretch justify-center p-1 py-2 gap-y-2 bg-gray-100 text-sm font-semibold ' +
                  (isItemActive(index) ? 'flex' : 'hidden')
                "
              >
              <ng-template [ngIf]="currentItem?.childItems != null && currentItem.childItems.length > 0">
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
                    <p class="childNavItem p-1">{{ currentChildItem.value }}</p>
                  </ng-template>
              </ng-template>
              </div>
            </h2>
          </ng-template>
        </div>
      </div>
    </div>
  `,
  styleUrl: './CommonLayoutHeader.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutHeaderComponent {
  @Input() navItems: INavItems[] = [];
  
  navDropdownItems: INavDropdownItems[] = [];
  
  public goToHomepage(){
    this.router.navigate(['/'])
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
  constructor(private readonly router:Router) {}
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
