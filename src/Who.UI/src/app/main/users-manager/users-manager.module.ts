import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { UsersManagerComponent } from "./users-manager.component";
import { AddUsersListComponent } from "./add-users-list.component";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobGridModule } from "@doob-ng/grid";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzMenuModule } from "ng-zorro-antd/menu";


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DoobAntdExtensionsModule,
        DoobGridModule,
        NzInputModule,
        NzFormModule,
        NzMenuModule
    ],
    declarations: [
        UsersManagerComponent,
        AddUsersListComponent
    ],
    exports: [
        UsersManagerComponent
    ]

})
export class UsersManagerModule {

    
}