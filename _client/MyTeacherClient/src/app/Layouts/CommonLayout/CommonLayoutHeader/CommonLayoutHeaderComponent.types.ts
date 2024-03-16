export interface INavDropdownItems{
    index: number; isACtive: boolean
}
export interface INavItems{
    id: number;
    key: string;
    value: string;
    href: string;
    childItems: INavChildItems[]
}
export interface INavChildItems{ id: number; key: string; value: string; href: string }
export interface IBreadcrumbItems{
    id: number;
    key: string;
    value: string;
    href: string;
}