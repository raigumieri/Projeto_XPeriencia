# 🎲 XPeriencia - Aplicativo de Apoio contra o Vício em Apostas

## 👥 Integrantes
- Guilherme Doretto Sobreiro **RM:99674** 
- Guilherme Fazito Ziolli Sordili **RM:550539** 
- Raí Gumieri dos Santos **RM:98287**  

---

## 📌 Descrição do Projeto
O **XPeriencia** é uma aplicação desenvolvida em **C# (console application)** com o objetivo de apoiar pessoas que desejam **monitorar seus hábitos de apostas** e refletir sobre suas ações, auxiliando no combate ao vício em jogos de azar.  

O sistema permite cadastrar usuários, registrar apostas, escrever reflexões pessoais e gerar relatórios.  
Para simplificar o armazenamento (substituindo um banco de dados real), foram utilizados **arquivos JSON** para persistência e **relatórios em TXT** para acompanhamento.  

---

## ⚙️ Funcionalidades
- **Usuários**  
  - Criar, listar, editar e excluir usuários.  
  - Cada usuário tem: ID, Nome, Email, Data de criação e Pontos.  

- **Apostas**  
  - Registrar apostas vinculadas a um usuário.  
  - Ganhos e perdas impactam diretamente os pontos do usuário.  
  - Cada aposta contém: ID, Descrição, Valor, Data e Resultado (G/P).  

- **Reflexões**  
  - Espaço para que o usuário registre pensamentos e reflexões sobre sua jornada.  
  - CRUD completo (incluir, editar, listar e excluir).  

- **Relatórios (TXT)**  
  - Relatório detalhado por usuário.  
  - Inclui lista de apostas, reflexões e resumo estatístico:  
    - Total de apostas feitas  
    - Quantidade de ganhos e perdas  
    - Pontos finais  

---
## 🗂️ Diagrama de Pacotes / Estrutura do Projeto

📦 XPeriencia

┣ 📂 Models # Classes que representam as entidades

┃ ┣ 📜 Aposta.cs # Estrutura de dados para Aposta

┃ ┣ 📜 Reflexao.cs # Estrutura de dados para Reflexão

┃ ┗ 📜 Usuario.cs # Estrutura de dados para Usuário

┣ 📂 Services # Serviços responsáveis pelo CRUD

┃ ┣ 📜 ApostaService.cs # Operações de CRUD para Apostas

┃ ┣ 📜 DataManager.cs # Gerencia leitura e escrita em arquivos JSON/TXT

┃ ┣ 📜 ReflexaoService.cs # Operações de CRUD para Reflexões

┃ ┣ 📜 RelatorioService.cs # Geração de relatórios em TXT com estatísticas

┃ ┗ 📜 UsuarioService.cs # Operações de CRUD para Usuários




┣ 📜 Program.cs # Menu principal e fluxo do sistema


┗ 📜 README.md # Este arquivo

---

## 💻 Tecnologias Utilizadas
- **C# .NET 8.0**  
- **JSON** (persistência de dados simulando banco)  
- **TXT** (relatórios)  
- **Console Application** (interface)  

---

## 📊 Requisitos Atendidos
✔ Estruturação de classes e código limpo (25%)  
✔ Manipulação de arquivos JSON/TXT (20%)  
✔ CRUD completo (20%)  
✔ Interface Console (15%)  
✔ Documentação do projeto (10%)  
✔ Arquitetura em diagramas (10%)  

---

## 🚀 Como Executar
1. Clone este repositório.  
2. Abra o projeto no **Visual Studio** ou **Visual Studio Code**.  
3. Compile e execute o programa.  
4. Os arquivos de dados serão armazenados automaticamente na pasta `Data/`.  

---

## 📎 Observações
- O projeto não utiliza banco de dados real, mas sim arquivos JSON, conforme alternativa permitida pelo professor.  
- O relatório TXT é gerado para cada usuário, facilitando a análise dos resultados.  

--- 

## Diagrama de Classes 
<div align="center"> 
  <img width="600" height="600" align="center" alt="Image" src="https://github.com/user-attachments/assets/4a6b00f7-bf0b-4956-96f6-9c5ecbc556f4" />
</div>
