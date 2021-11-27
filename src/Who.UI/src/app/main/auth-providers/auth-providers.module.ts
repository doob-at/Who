import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { DoobGridModule } from "@doob-ng/grid";
import { NzCollapseModule } from "ng-zorro-antd/collapse";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { AuthProvidersRoutingModule, RoutingComponents } from "./auth-providers-routing.module";
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzMenuModule } from "ng-zorro-antd/menu";
import { NzCheckboxModule } from "ng-zorro-antd/checkbox";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AuthProvidersRoutingModule,
        DoobGridModule,
        NzTabsModule,
        NzInputModule,
        NzFormModule,
        NzCollapseModule,
        NzSelectModule ,
        NzMenuModule,
        NzCheckboxModule
    ],
    declarations: [
        ...RoutingComponents
    ]

})
export class AuthProvidersModule {

    
}