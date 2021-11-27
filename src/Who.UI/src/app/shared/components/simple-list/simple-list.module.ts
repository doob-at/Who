import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { DoobGridModule } from "@doob-ng/grid";
import { SimpleListComponent } from "./simple-list.component";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DoobAntdExtensionsModule,
        DoobGridModule
    ],
    declarations: [
        SimpleListComponent
    ],
    exports: [
        SimpleListComponent
    ]

})
export class SimpleListModule {

    
}