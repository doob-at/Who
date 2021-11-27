import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobGridModule } from "@doob-ng/grid";
import { NzMenuModule } from "ng-zorro-antd/menu";
import { KeyValueListComponent } from "./key-value-list.component";


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DoobAntdExtensionsModule,
        DoobGridModule,
        NzMenuModule
    ],
    declarations: [
        KeyValueListComponent
    ],
    exports: [
        KeyValueListComponent
    ]

})
export class KeyValueListModule {

    
}