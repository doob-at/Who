import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ExternalLoginModel } from "@shared/models/external-login-model";
import { LoginInputModel } from "@shared/models/login-input-model";
import { LoginViewModel } from "@shared/models/login-view-model";
import { tap } from "rxjs/operators";


@Injectable({
    providedIn: 'root'
})
export class LoginService {

    constructor(
        private http: HttpClient) {

    }

    public GetLoginViewModel(returnUrl: string) {
        let params: HttpParams;
        if (returnUrl) {
            params = new HttpParams().set('returnUrl', returnUrl);
        } else {
            params = new HttpParams();
        }

        return this.http.get<LoginViewModel>(`/api/account/login`, { params });
    }

    public SendLoginInputModel(loginInputModel: LoginInputModel) {
        return this.http.post<any>(`/api/account/login`, loginInputModel);
    }

    public SendExternalLoginInputModel(loginModel: ExternalLoginModel) {

        return this.http.post<any>(`/api/account/login-external`, loginModel);
    }
   

}