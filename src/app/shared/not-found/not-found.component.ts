import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink],
  template: `
    <h1>404</h1>
    <p>Página no encontrada.</p>
    <a routerLink="/">Volver al Home</a>
  `,
  styles: [`
    h1 { color: #d32f2f; }
    p { font-size: 1.1rem; margin-bottom: 1rem; }
    a { color: #1976d2; text-decoration: underline; }
  `]
})
export class NotFoundComponent {}
