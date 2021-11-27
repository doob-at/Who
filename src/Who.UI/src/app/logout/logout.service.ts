import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { LogOutModel } from "@shared/models/log-out-model";
import { tap } from "rxjs/operators";


@Injectable({
    providedIn: 'root'
})
export class LogoutService {

    logOutModel!: LogOutModel;

    constructor(
        private http: HttpClient,
        private router: Router) {

    }

    public GetLogoutViewModel(queryString: string) {
        
        return this.http.get<LogOutModel>(`/connect/logout${queryString}`)
        .pipe(
            tap(model => {

                this.logOutModel = model;
                if(!this.logOutModel?.ShowLogoutPrompt){
                    this.router.navigate(['/logged-out']);
                }
                
            })
        );
    }

    public CompleteLogOut() {
        
        var headers = new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded',
        });

        return this.http.post(`/connect/logout`, this.logOutModel, {headers: headers});
    }

   

}