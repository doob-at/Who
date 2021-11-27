import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobGridModule } from "@doob-ng/grid";
import { ClaimsManagerComponent } from "./claims-manager.component";

@NgModule({
    imports: [
        CommonModule,
        DoobGridModule,
        DoobAntdExtensionsModule,
    ],
    declarations: [
        ClaimsManagerComponent
    ],
    exports: [
        ClaimsManagerComponent
    ]
})
export class ClaimsManagerModule {

}