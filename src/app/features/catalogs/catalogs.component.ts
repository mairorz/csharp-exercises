import { Component } from '@angular/core';

@Component({
  selector: 'app-catalogs',
  standalone: true,
  template: `
    <section class="stack">
      <h1>Catálogos</h1>
      <p class="muted">Sección informativa.</p>
      <div class="card">
        <p>Componentes en desarrollo.</p>
      </div>
    </section>
  `,
  styles: [``]
})
export class CatalogsComponent {

}
