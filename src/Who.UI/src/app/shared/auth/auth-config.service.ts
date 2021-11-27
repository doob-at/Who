import { Injectable, InjectionToken } from "@angular/core";
import { LogLevel, OpenIdConfiguration } from "angular-auth-oidc-client";
import { CustomStorage } from "./auth-storage";


@Injectable({ providedIn: 'root' })
export class AuthConfigService {

    getConfig(): OpenIdConfiguration {
        
        return {
            authority: window.location.origin,
            redirectUrl: window.location.origin,
            postLogoutRedirectUri: window.location.origin,
            clientId: 'whoUI',
            scope: 'openid profile email roles offline_access who_api',
            responseType: 'code',
            silentRenew: true,
            useRefreshToken: true,
            renewTimeBeforeTokenExpiresInSeconds: 30,
            refreshTokenRetryInSeconds: 30,
            logLevel: LogLevel.Warn,
            autoUserInfo: true,
            secureRoutes: ["api/", "/api/", "_test", "/_test", "/connect/", "connect/"],
            storage: new CustomStorage(),
            ignoreNonceAfterRefresh: true
            // customParamsAuthRequest: {
            //     prompt: 'none'
            // }
        };
    }
}

