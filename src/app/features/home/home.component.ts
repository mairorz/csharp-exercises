import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="stack">
      <h1>Inicio</h1>
      <p class="muted">Bienvenido a la aplicación Services Management UI.</p>

      <div class="grid">
        <article class="card stack">
          <h3>Órdenes Programadas</h3>
          <p>Tareas pendientes de ser asignadas o iniciadas.</p>
          <a class="button button--primary">Ver Órdenes</a>
        </article>

        <article class="card stack">
          <h3>Alerta de Insumos</h3>
          <p>Insumos con un stock crítico o bajo el mínimo.</p>
          <button class="button button--primary">Ver Insumos</button>
        </article>

        <article class="card stack">
          <h3>Facturas Pendientes</h3>
          <p>Facturas generadas y aún sin marcar como pagadas.</p>
          <button class="button button--primary">Ver Facturas</button>
        </article>
      </div>
    </section>
  `,
  styles: [``]
})
export class HomeComponent {}