import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faGithubSquare,
  faLinkedin,
  faInstagramSquare,
  IconDefinition,
} from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-common-layout-lets-connect',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  template: `
    <div class="grid grid-cols-24 gap-y-2">
      <h2 class="col-span-24 !m-0 text-center">Let's Connect</h2>
      <div
        class="col-span-24 flex items-center justify-center gap-x-4"
      >
      <ng-template ngFor
                    let-currentIcon
                    [ngForOf]="icons"
                    let-isChildFirst="first"
                    let-isChildLast="last"
                    let-childIndex="index"
                    let-childCount="count"
                    let-childIsEven="even"
                    let-childIsOdd="odd">
        <span (click)="goToSocialAccount(currentIcon.href)">
        <fa-icon [classes]="currentIcon.classes" [icon]="currentIcon.icon"/>
        </span>
      </ng-template>
      </div>
    </div>
  `,
  styleUrl: './CommonLayoutLetsConnect.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonLayoutLetsConnectComponent {
  public goToSocialAccount(url: string){
    window.open(url, "_blank")
  }
  public iconClasses: string[] = ["text-3xl", "scale-105"];
  public githubIcon: IconDefinition = faGithubSquare;
  public linkedinIcon: IconDefinition = faLinkedin;
  public instagramIcon: IconDefinition = faInstagramSquare;
  public icons : { id: number, icon: IconDefinition, href:string, classes: string[] }[] = [
    {
      id: 1,
      icon: faGithubSquare,
      href: "https://github.com/UBasDev",
      classes: ["text-3xl", "scale-105", "text-black", "cursor-pointer", "hover:scale-110"]
    },
    {
      id: 2,
      icon: faLinkedin,
      href: "https://www.linkedin.com/in/u%C4%9Furcan-ba%C5%9F-84b91a206/",
      classes: ["text-3xl", "scale-105", "text-blue-600", "cursor-pointer", "hover:scale-110"]
    },
    {
      id: 3,
      icon: faInstagramSquare,
      href: "https://www.instagram.com/ugurcanbas_/",
      classes: ["text-3xl", "scale-105", "text-red-600", "cursor-pointer", "hover:scale-110"]
    }
  ]
}
