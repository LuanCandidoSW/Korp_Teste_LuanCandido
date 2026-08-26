# Korp_Teste_LuanCandido

Sistema de emissão de Notas Fiscais desenvolvido como teste técnico para a vaga de estágio em Desenvolvimento na **Korp ERP**.

O projeto foi construído com uma arquitetura de microsserviços: um frontend em **Angular** consumindo duas APIs independentes em **C# / .NET**, uma para controle de estoque e outra para emissão de notas fiscais.

## 🧱 Arquitetura

```
├── estoque-frontend/        # Aplicação Angular (SPA)
├── estoque-backend/
│   ├── EstoqueService/      # Microsserviço de controle de produtos e estoque
│   └── FaturamentoService/  # Microsserviço de emissão de notas fiscais
```

| Serviço              | Tecnologia                  | Porta |
|----------------------|------------------------------|-------|
| Frontend             | Angular                      | 4200  |
| EstoqueService       | ASP.NET Core + EF Core + SQLite | 5293  |
| FaturamentoService   | ASP.NET Core + EF Core + SQLite | 5000  |

A comunicação entre os microsserviços ocorre via HTTP: ao imprimir uma nota fiscal, o `FaturamentoService` chama o `EstoqueService` para dar baixa no saldo dos produtos envolvidos.

## ✨ Funcionalidades

- **Cadastro de Produtos** — código, descrição e saldo em estoque, com opção de reposição de saldo para produtos já existentes.
- **Cadastro de Notas Fiscais** — numeração sequencial automática, múltiplos produtos por nota, status inicial "Aberta".
- **Impressão de Notas Fiscais** — indicador de processamento, baixa automática de estoque, bloqueio de impressão para notas que não estejam "Abertas", e atualização do status para "Fechada".
- **Persistência real** em banco de dados SQLite via Entity Framework Core (não há dados em memória).
- **Tratamento de falhas** — se o serviço de Estoque estiver indisponível durante a impressão de uma nota, o sistema exibe uma mensagem amigável ao usuário e se recupera automaticamente assim que o serviço volta, sem necessidade de reiniciar o frontend ou o outro microsserviço.

## 🚀 Como executar o projeto

### Pré-requisitos

- [Node.js](https://nodejs.org) (LTS) e Angular CLI (`npm install -g @angular/cli`)
- [.NET SDK](https://dotnet.microsoft.com/download) 9 ou superior

### 1. Backend — EstoqueService

```bash
cd estoque-backend/EstoqueService
dotnet ef database update
dotnet run
```

### 2. Backend — FaturamentoService

```bash
cd estoque-backend/FaturamentoService
dotnet ef database update
dotnet run
```

### 3. Frontend — Angular

```bash
cd estoque-frontend
npm install
ng serve
```

Acesse `http://localhost:4200` no navegador.

> 💡 Alternativamente, o arquivo `iniciar.bat`, na raiz do projeto, sobe os três serviços de uma vez.

## 📄 Detalhamento técnico

O detalhamento técnico completo (ciclos de vida do Angular, uso de RxJS, bibliotecas utilizadas, frameworks do backend, tratamento de erros e uso de LINQ) está disponível no arquivo [`detalhamento_tecnico.docx`](./detalhamento_tecnico.docx), na raiz deste repositório.

## 🎥 Vídeo de apresentação

https://drive.google.com/file/d/1gsyhSdrbItpiX5Ta2Xn7G4d4yBDgvwb4/view?usp=sharing

## 👤 Autor

Luan Candido
