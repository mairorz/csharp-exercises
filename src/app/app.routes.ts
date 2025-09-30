import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent) },
  { path: 'operations', loadComponent: () => import('./features/operations/operations.component').then(m => m.OperationsComponent) },
  { path: 'finance', loadComponent: () => import('./features/finance/finance.component').then(m => m.FinanceComponent) },
  { path: 'catalogs', loadComponent: () => import('./features/catalogs/catalogs.component').then(m => m.CatalogsComponent) },
  { path: '**', loadComponent: () => import('./shared/not-found/not-found.component').then(m => m.NotFoundComponent) },
];