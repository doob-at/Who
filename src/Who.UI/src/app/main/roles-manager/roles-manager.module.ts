import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RolesManagerComponent } from './roles-manager.component';
import { AddRolesListComponent } from './add-roles-list.component';
import { RoleGridCellComponent } from './role-grid-cell.component';
import { DoobGridModule } from "@doob-ng/grid";
import { DoobAntdExtensionsModule } from "@doob-ng/antd-extensions";
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFormModule } from "ng-zorro-antd/form";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DoobGridModule,
        DoobAntdExtensionsModule,
        NzInputModule,
        NzFormModule
    ],
    declarations: [
        RolesManagerComponent,
        AddRolesListComponent,
        RoleGridCellComponent
    ],
    exports: [
        RolesManagerComponent
    ]

})
export class RolesManagerModule {

    
}