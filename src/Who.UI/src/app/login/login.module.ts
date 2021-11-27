import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { LoginComponent } from "./login.component";
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { RouterModule, Routes } from "@angular/router";
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

const routes: Routes = [
    {
        path: '',
        component: LoginComponent
    }
];

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        NzButtonModule,
        NzFormModule,
        NzInputModule,
        RouterModule.forChild(routes),
        NzCheckboxModule
    ],
    declarations: [
        LoginComponent
    ]
})
export class LoginModule {

}