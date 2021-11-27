import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { ClaimsManagerModule } from "../claims-manager/claims-manager.module";
import { RolesManagerModule } from "../roles-manager/roles-manager.module";
import { SetPasswordModalComponent } from "./set-password-modal.component";
import { UsersRoutingModule, RoutingComponents } from "./users-routing.module";
import { SimpleListModule } from "../../shared/components/simple-list/simple-list.module";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobGridModule } from "@doob-ng/grid";
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { DoobCdkHelperModule } from "@doob-ng/cdk-helper";
import { NzCheckboxModule } from "ng-zorro-antd/checkbox";
import { NzInputModule } from "ng-zorro-antd/input";
import { DoobCoreModule } from "@doob-ng/core";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMenuModule } from "ng-zorro-antd/menu";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        UsersRoutingModule,
        ClaimsManagerModule,
        RolesManagerModule,
        SimpleListModule,
        DoobAntdExtensionsModule,
        DoobGridModule,
        NzTabsModule,
        NzCollapseModule,
        NzDatePickerModule,
        NzFormModule,
        DoobCdkHelperModule,
        NzCheckboxModule,
        NzInputModule,
        DoobCoreModule,
        NzButtonModule,
        NzMenuModule
    ],
    declarations: [
        ...RoutingComponents,
        SetPasswordModalComponent
    ]

})
export class UsersModule {

    
}