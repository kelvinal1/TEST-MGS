import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { es_ES, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import es from '@angular/common/locales/es';
import { FormsModule } from '@angular/forms';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideClientHydration } from '@angular/platform-browser';
import { icons } from './icons-provider';
import { InterceptorService } from './core/interceptor.service';
import { environment } from './enviroments/environment';


registerLocaleData(es);

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    { provide: 'BASE_APP', useFactory: getBaseUrl, deps: [] },
    provideAnimationsAsync(),
    provideClientHydration(),
    importProvidersFrom(FormsModule),
    provideRouter(routes, withComponentInputBinding()),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideNzI18n(es_ES),
    //{ provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true }
  ]
};

export function getBaseUrl() {
  if (!environment.production)
    return environment.API_APP
  else
    return environment.API_APP
}
