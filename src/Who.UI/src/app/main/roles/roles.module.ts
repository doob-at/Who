import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RoutingComponents, RolesRoutingModule } from "./roles-routing.module";
import { RolesManagerModule } from "../roles-manager/roles-manager.module";
import { UsersManagerModule } from "../users-manager/users-manager.module";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobGridModule } from "@doob-ng/grid";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzDrawerModule } from "ng-zorro-antd/drawer";
import { NzMenuModule } from "ng-zorro-antd/menu";
import { DoobCoreModule } from "@doob-ng/core";
import { NzButtonModule } from "ng-zorro-antd/button";


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RolesRoutingModule,
        UsersManagerModule,
        DoobAntdExtensionsModule,
        DoobGridModule,
        DoobCoreModule,
        NzTabsModule,
        NzFormModule,
        NzInputModule,
        NzDrawerModule,
        NzButtonModule,
        NzMenuModule
    ],
    declarations: [
        ...RoutingComponents
    ]

})
export class RolesModule {

    
}