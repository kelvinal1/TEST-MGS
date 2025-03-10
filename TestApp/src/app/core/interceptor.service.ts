import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class InterceptorService implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log('Interceptor ejecutado');
    const clonedRequest = request.clone({
      setHeaders: {
        'Content-Type': 'application/json'
      }
    });
    return next.handle(clonedRequest);
  }
}
