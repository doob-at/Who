import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobCoreModule } from "@doob-ng/core";
import { DoobGridModule } from "@doob-ng/grid";
import { SimpleListModule } from "@shared/components/simple-list/simple-list.module";
import { KeyValueListModule } from "@shared/components/uri-list/key-value-list.module";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzCheckboxModule } from "ng-zorro-antd/checkbox";
import { NzCollapseModule } from "ng-zorro-antd/collapse";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzInputModule } from "ng-zorro-antd/input";
import { NzMenuModule } from "ng-zorro-antd/menu";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { ClientsRoutingModule, RoutingComponents } from "./clients-routing.module";


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ClientsRoutingModule,
        SimpleListModule,
        KeyValueListModule,
        DoobGridModule,
        DoobCoreModule,
        DoobAntdExtensionsModule,
        NzTabsModule,
        NzCollapseModule,
        NzFormModule,
        NzInputModule,
        NzButtonModule,
        NzCheckboxModule,
        NzMenuModule
    ],
    declarations: [
        ...RoutingComponents
    ]

})
export class ClientsModule {

    
}