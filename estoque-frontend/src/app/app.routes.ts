import { Routes } from '@angular/router';
import { Produtos } from './produtos/produtos';
import { Notas } from './notas/notas';

export const routes: Routes = [
  { path: 'produtos', component: Produtos },
  { path: 'notas', component: Notas },
];