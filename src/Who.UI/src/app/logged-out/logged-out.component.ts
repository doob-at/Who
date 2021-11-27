import { Component, ChangeDetectionStrategy } from "@angular/core";
import { LogOutModel } from "@shared/models/log-out-model";
import { LogoutService } from "../logout/logout.service";


@Component({
    templateUrl: './logged-out.component.html',
    styleUrls: ['./logged-out.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoggedOutComponent {

    logOutModel!: LogOutModel;
   

    constructor(private logOutService: LogoutService) { }

    ngOnInit() {

        console.log(this.logOutService.logOutModel);
        
        // if (!this.idpService.logOutModel) {
        //     this.location.back();
        //     return;
        // }

        // if (!this.idpService.logOutModel.ClientName) {
        //     window.location.href = '/';
        // }
        // if (this.idpService.logOutModel.AutomaticRedirectAfterSignOut) {
        //     window.location.href = this.idpService.logOutModel.PostLogoutRedirectUri;
        //     return;
        // }

        this.logOutModel = this.logOutService.logOutModel;
    }

}