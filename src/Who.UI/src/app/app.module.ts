import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AkitaNgRouterStoreModule } from '@datorama/akita-ng-router-store';
import { AkitaNgDevtools } from '@datorama/akita-ngdevtools';
import { AuthConfigModule } from '@shared/auth/auth-config.module';
import { AuthInterceptor } from 'angular-auth-oidc-client';
import { en_US, NZ_I18N } from 'ng-zorro-antd/i18n';
import { environment } from 'src/environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { FontAwesomeModule, FaIconLibrary, FaConfig } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from "@fortawesome/free-brands-svg-icons"
import { DoobCoreModule } from '@doob-ng/core';
import { DoobCdkHelperModule } from '@doob-ng/cdk-helper';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    environment.production ? [] : AkitaNgDevtools.forRoot(),
    AkitaNgRouterStoreModule,
    AuthConfigModule,
    DoobCoreModule,
    DoobCdkHelperModule
  ],
  providers: [
    {
      provide: NZ_I18N,
      useValue: en_US
    },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(library: FaIconLibrary, faConfig: FaConfig) {
    library.addIconPacks(fas, far, fab);
    faConfig.fixedWidth = true;
  }

 }
