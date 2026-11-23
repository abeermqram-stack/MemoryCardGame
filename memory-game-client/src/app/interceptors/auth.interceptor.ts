import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthStateService } from '../services/auth-state.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authState = inject(AuthStateService);
  const url = req.url.toLowerCase();
  const method = req.method.toUpperCase();

  const isAdminEndpoint =
    (url.includes('/api/cards') && method !== 'GET') ||
    (url.endsWith('/api/players') && method === 'GET') ||
    url.includes('/api/players/search') ||
    (url.includes('/api/players/') && method === 'DELETE') ||
    (url.includes('/api/gamesessions/') && (method === 'PUT' || method === 'DELETE'));

  const token = isAdminEndpoint
    ? (authState.getAdminToken() || authState.getPlayerToken())
    : (authState.getPlayerToken() || authState.getAdminToken());

  if (!token) {
    return next(req);
  }

  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(authReq);
};

