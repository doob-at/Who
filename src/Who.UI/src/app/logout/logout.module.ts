import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { NzResultModule } from "ng-zorro-antd/result";
import { LogoutComponent } from "./logout.component";

const routes: Routes = [
    {
        path: '',
        component: LogoutComponent
    }
];

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild(routes),
        NzResultModule
    ],
    declarations: [
        LogoutComponent
    ]
})
export class LogoutModule {

}