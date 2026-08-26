import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export interface ItemNota {
  codigoProduto: string;
  quantidade: number;
}

export interface NotaFiscal {
  id?: number;
  numero?: string;
  status?: string;
  itens: ItemNota[];
}

@Component({
  selector: 'app-notas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './notas.html',
  styleUrls: ['./notas.css']
})
export class Notas implements OnInit {
  apiUrl = 'http://localhost:5000/api/notas';
  notas: NotaFiscal[] = [];

  novoItem: ItemNota = { codigoProduto: '', quantidade: 1 };
  itensNovaNota: ItemNota[] = [];

  imprimindo: number | null = null;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.carregarNotas();
  }

  carregarNotas(): void {
    this.http.get<NotaFiscal[]>(this.apiUrl).subscribe({
      next: (res) => {
        this.notas = [...res];
        this.cdr.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  adicionarItem(): void {
    if (!this.novoItem.codigoProduto || this.novoItem.quantidade <= 0) {
      alert('Preencha o código do produto e uma quantidade válida.');
      return;
    }
    this.itensNovaNota.push({ ...this.novoItem });
    this.novoItem = { codigoProduto: '', quantidade: 1 };
  }

  removerItem(index: number): void {
    this.itensNovaNota.splice(index, 1);
  }

  criarNota(): void {
    if (this.itensNovaNota.length === 0) {
      alert('Adicione ao menos um item antes de criar a nota.');
      return;
    }

    const nota: NotaFiscal = { itens: this.itensNovaNota };

    this.http.post<NotaFiscal>(this.apiUrl, nota).subscribe({
      next: () => {
        this.itensNovaNota = [];
        this.carregarNotas();
      },
      error: (err) => alert(typeof err.error === 'string' ? err.error : 'Erro ao criar nota.')
    });
  }

  imprimir(id: number): void {
    this.imprimindo = id;
    this.http.post<NotaFiscal>(`${this.apiUrl}/${id}/imprimir`, {}).subscribe({
      next: (notaAtualizada) => {
        this.imprimindo = null;
        const index = this.notas.findIndex(n => n.id === id);
        if (index !== -1) {
          this.notas[index] = notaAtualizada;
          this.notas = [...this.notas];
        }
        this.cdr.detectChanges();
        alert('Nota impressa e estoque atualizado com sucesso!');
      },
      error: (err) => {
        this.imprimindo = null;
        this.cdr.detectChanges();
        alert(typeof err.error === 'string' ? err.error : 'Erro ao imprimir nota.');
      }
    });
  }
}