import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { FirstSetupComponent } from './first-setup.component';
import { FirstSetupService } from './first-setup.service';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { HttpClientModule } from '@angular/common/http';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAlertModule } from 'ng-zorro-antd/alert';

const routes: Routes = [
    {
        path: '',
        component: FirstSetupComponent
    }
];

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        ReactiveFormsModule,
        HttpClientModule,
        NzFormModule,
        NzInputModule,
        NzButtonModule,
        NzResultModule,
        NzIconModule,
        NzAlertModule
    ],
    declarations: [
        FirstSetupComponent
    ],
    exports: [
    ],
    providers: [
        FirstSetupService
    ]
})
export class FirstSetupModule {

}
