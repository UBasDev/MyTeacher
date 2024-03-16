import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { CommonLayoutComponent } from './Layouts/CommonLayout/CommonLayout.component';

export const routes: Routes = [
    {
        path: '',
        component: CommonLayoutComponent,
        children:[
            {
                path: '',
                component: HomeComponent
            }
        ]
    }
];
