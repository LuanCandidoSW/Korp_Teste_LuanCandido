import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export interface Produto {
  codigo: string;
  descricao: string;
  saldo: number;
}

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './produtos.html',
  styleUrls: ['./produtos.css']
})
export class Produtos implements OnInit {
  apiUrl = 'http://localhost:5293/api/produtos';
  novoProduto: Produto = { codigo: '', descricao: '', saldo: 0 };
  produtos: Produto[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.http.get<Produto[]>(this.apiUrl).subscribe({
      next: (res) => (this.produtos = res),
      error: (err) => console.error(err)
    });
  }

  adicionarProduto(): void {
    if (!this.novoProduto.codigo || !this.novoProduto.saldo) {
      alert('Preencha pelo menos o código e o saldo!');
      return;
    }

    this.http.post<Produto>(this.apiUrl, this.novoProduto).subscribe({
      next: () => {
        this.carregarProdutos();
        this.novoProduto = { codigo: '', descricao: '', saldo: 0 };
      },
      error: (err) => {
        if (err.status === 400 && err.error?.includes('já existe')) {
          this.adicionarSaldoExistente(this.novoProduto.codigo, this.novoProduto.saldo);
        } else {
          alert(typeof err.error === 'string' ? err.error : 'Erro ao cadastrar produto.');
        }
      }
    });
  }

  private adicionarSaldoExistente(codigo: string, quantidade: number): void {
    const url = `${this.apiUrl}/${codigo}/adicionar-saldo`;
    this.http.post<Produto>(url, { quantidade }).subscribe({
      next: () => {
        alert(`Saldo do produto ${codigo} atualizado com sucesso!`);
        this.carregarProdutos();
        this.novoProduto = { codigo: '', descricao: '', saldo: 0 };
      },
      error: (err) => alert(typeof err.error === 'string' ? err.error : 'Erro ao adicionar saldo.')
    });
  }
}