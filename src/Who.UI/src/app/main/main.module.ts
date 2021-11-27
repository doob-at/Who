import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FaConfig, FaIconLibrary } from "@fortawesome/angular-fontawesome";
import { MainRoutingModule, RoutingComponents } from "./main-routing.module";
import { MainComponent } from "./main.component";
import { IconGridCellComponent } from "../shared/components/icon-cell/icon-cell.component";
import { DoobCoreModule } from "@doob-ng/core";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzLayoutModule } from 'ng-zorro-antd/layout';

@NgModule({
    imports: [
        CommonModule,
        MainRoutingModule,
        DoobCoreModule,
        DoobAntdExtensionsModule,
        NzButtonModule,
        NzMenuModule,
        NzPopoverModule,
        NzLayoutModule
    ],
    declarations: [
        MainComponent,
        IconGridCellComponent,
        ...RoutingComponents
    ]
})
export class MainModule {

}