@echo off
start powershell -NoExit -Command "cd estoque-backend\EstoqueService; dotnet run"
start powershell -NoExit -Command "cd estoque-backend\FaturamentoService; dotnet run"
start powershell -NoExit -Command "cd estoque-frontend; ng serve"