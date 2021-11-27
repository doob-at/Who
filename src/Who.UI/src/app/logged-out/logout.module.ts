import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { NzResultModule } from "ng-zorro-antd/result";
import { LoggedOutComponent } from "./logged-out.component";

const routes: Routes = [
    {
        path: '',
        component: LoggedOutComponent
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
        LoggedOutComponent
    ]
})
export class LoggedOutModule {

}