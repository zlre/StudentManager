import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { StudentListComponent, StudentCreateComponent, StudentUpdateComponent } from './student';
import { LoginComponent } from './login';
import { AuthGuard } from './helpers';


const routes: Routes = [
    { path: '', component: StudentListComponent, canActivate: [AuthGuard] },
    { path: 'add', component: StudentCreateComponent, canActivate: [AuthGuard] },
    { path: 'update/:id', component: StudentUpdateComponent, canActivate: [AuthGuard] },

    { path: 'login', component: LoginComponent },

    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
