import { IconDefinition } from '@fortawesome/fontawesome-svg-core';

export interface INavDropdownItems {
  index: number;
  isACtive: boolean;
}
export interface INavItems {
  id: number;
  key: string;
  value: string;
  href: string;
  childItems: INavChildItems[];
  icon: IconDefinition | null;
  onClickFunction: ()=>void;
}
export interface INavChildItems {
  id: number;
  key: string;
  value: string;
  href: string;
  icon: any;
  onClickFunction: ()=>void;
}
export interface IBreadcrumbItems {
  id: number;
  key: string;
  value: string;
  href: string;
}
