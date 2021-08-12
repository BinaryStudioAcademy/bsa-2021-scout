import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessTokenJson = localStorage.getItem('accessToken');
        
    if (accessTokenJson) {
      const accessToken: string = JSON.parse(accessTokenJson);
      req = req.clone({ setHeaders: { Authorization: `Bearer ${accessToken}` } });
    }

    return next.handle(req);
  }
}
