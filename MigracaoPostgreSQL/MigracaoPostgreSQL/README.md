// README.md
# Sistema de Migração PostgreSQL

## Descrição
Sistema desenvolvido em C# .NET Framework 4.8 para migração de dados entre bancos PostgreSQL versão 17. 
O sistema migra dados de múltiplas tabelas de origem para um banco de destino unificado, seguindo regras específicas de negócio.

## Funcionalidades
- ✅ Migração de 4 tabelas distintas para estrutura unificada
- ✅ Processamento em lotes para melhor performance (500 itens por padrão)
- ✅ Verificação de produtos existentes baseada em código de barras
- ✅ Atualização condicional baseada em ICMS pendente
- ✅ Carregamento e conversão de fotos para byte array
- ✅ Sistema completo de logging com timestamps
- ✅ Tratamento robusto de erros com rollback por lote
- ✅ Pool de conexões otimizado
- ✅ Padrão Repository + Service para organização

## Estrutura do Projeto
```
MigracaoPostgreSQL/
├── Models/                 # Modelos de dados
├── Repositories/           # Camada de acesso a dados
├── Services/              # Lógica de negócio
├── Utils/                 # Utilitários (Logger, DatabaseConfig)
├── Program.cs             # Ponto de entrada da aplicação
├── App.config             # Configurações
└── packages.config        # Dependências NuGet
```

## Bancos de Dados

### Bancos de Origem
- `tbl_autopecas_postgresql` → `tbl_autopecas`
- `tbl_construcao_2025_postgresql` → `tbl_construcao_2025`  
- `tbl_eans_1555_postgresql` → `tbl_eans_1555_cadastro`
- `tbl_petshop_ecommerce_2025_postgresql` → `tbl_petshop_ecommerce_2025`

### Banco de Destino
- `consulta_produtos` → tabelas: `produto`, `produto_codigo_barras`, `produto_foto`

## Configuração

### Conexão com Banco de Dados
- **Host:** 127.0.0.1
- **Porta:** 5434
- **Usuário:** postgres
- **Senha:** 123
- **Pool de Conexões:** Habilitado

### Diretório de Fotos
- **Caminho padrão:** `C:\FotosProdutos\`
- O sistema carrega fotos referenciadas nos campos `foto_jpg`, `foto_jpg580` e `foto_webp`

## Regras de Migração

### Mapeamento de Campos
- **Código de barras:** Detecta automaticamente (`gtin`, `codbar`, `ean`)
- **Descrição:** Usa `produto` ou `nome`, com fallback para `descricao`
- **Grupo:** Mapeado do campo `departamento`
- **Subgrupo:** Concatenação de `categoria` > `subcategoria` > `subcategoria_2`

### Campos Padrão
```csharp
unidade = "UN"
unidade_descricao = "Unidade"  
numero_casas_decimais = 0
marcador = false
```

### Lógica de Atualização
1. **Produto não existe:** Cria novo produto completo
2. **Produto existe + ICMS pendente:** Atualiza apenas tabela `produto`
3. **Produto existe sem ICMS pendente:** Ignora (não atualiza)

## Como Usar

### 1. Pré-requisitos
- Visual Studio 2017+ ou VS Code com C# extension
- .NET Framework 4.8
- PostgreSQL 17 com os bancos configurados
- Diretório de fotos criado em `C:\FotosProdutos\`

### 2. Instalação
```bash
# 1. Clone ou baixe o projeto
# 2. Restaurar pacotes NuGet
nuget restore

# 3. Compilar o projeto
msbuild MigracaoPostgreSQL.csproj /p:Configuration=Release
```

### 3. Execução
```bash
# Executar via prompt de comando
MigracaoPostgreSQL.exe

# Ou via Visual Studio (F5 ou Ctrl+F5)
```

### 4. Acompanhamento
- **Console:** Mostra progresso em tempo real
- **Log File:** `migracao_log.txt` com detalhes completos
- **Estatísticas:** Resumo ao final da execução

## Logs e Monitoramento

### Tipos de Log
- **INFO:** Operações normais e progresso
- **WARNING:** Situações que merecem atenção
- **ERROR:** Erros com stack trace completo
- **DEBUG:** Informações detalhadas (desenvolvimento)

### Exemplo de Log
```
[2025-01-08 10:15:30.123] [INFO] === INÍCIO DA MIGRAÇÃO DE DADOS ===
[2025-01-08 10:15:35.456] [INFO] Processando tabela tbl_autopecas_postgresql.tbl_autopecas
[2025-01-08 10:15:40.789] [INFO] Carregados 1250 produtos da tabela tbl_autopecas
[2025-01-08 10:15:45.012] [INFO] Produto criado: 7891234567890 - Filtro de Óleo Motor
[2025-01-08 10:16:50.345] [INFO] === ESTATÍSTICAS FINAIS DA MIGRAÇÃO ===
[2025-01-08 10:16:50.678] [INFO] Produtos criados: 2847
[2025-01-08 10:16:50.901] [INFO] Produtos atualizados: 156
[2025-01-08 10:16:51.234] [INFO] Produtos ignorados: 89
```

## Tratamento de Erros

### Estratégia de Recuperação
- **Erro em produto específico:** Continua processando outros produtos
- **Erro em lote:** Rollback do lote, continua próximo lote
- **Erro em tabela:** Continua processando outras tabelas
- **Erro crítico:** Para execução e registra no log

### Integridade de Dados
- Transações por lote garantem consistência
- Rollback automático em caso de erro
- Validações antes de inserção/atualização

## Performance e Otimizações

### Processamento em Lotes
- **Tamanho padrão:** 500 produtos por lote
- **Pool de conexões:** 1-10 conexões simultâneas
- **Timeout de conexão:** 30 segundos

### Otimizações Implementadas
- Queries otimizadas com índices
- Reutilização de conexões via pool
- Carregamento lazy de fotos
- Validações em memória antes de persistir

## Dependências

### Pacotes NuGet
```xml
<package id="Npgsql" version="4.1.14" targetFramework="net48" />
<package id="System.Runtime.CompilerServices.Unsafe" version="4.5.3" targetFramework="net48" />
<package id="System.Threading.Tasks.Extensions" version="4.5.4" targetFramework="net48" />
<package id="System.ValueTuple" version="4.5.0" targetFramework="net48" />
```

## Troubleshooting

### Problemas Comuns

#### 1. Erro de Conexão
```
Erro: Connection refused
Solução: Verificar se PostgreSQL está rodando na porta 5434
```

#### 2. Fotos não Carregam
```
Erro: Arquivo não encontrado
Solução: Verificar se pasta C:\FotosProdutos\ existe e tem as imagens
```

#### 3. Erro de Permissão
```
Erro: Access denied
Solução: Executar como administrador ou ajustar permissões da pasta
```

#### 4. Timeout de Conexão
```
Erro: Timeout expired
Solução: Aumentar timeout no DatabaseConfig ou verificar rede
```

### Debug e Desenvolvimento

#### Habilitar Debug Detalhado
```csharp
// No App.config, alterar:
<add key="LogLevel" value="DEBUG" />
```

#### Reduzir Tamanho do Lote (Teste)
```csharp
// Em MigrationService.cs, alterar:
private const int BATCH_SIZE = 50; // Para testes
```

## Extensibilidade

### Adicionar Nova Tabela de Origem
1. **DatabaseConfig.cs:** Adicionar entrada no dicionário `_databases`
2. **MigrationService.cs:** Adicionar no dicionário `_tabelasOrigem`
3. **ProdutoRepository.cs:** Adicionar case no `BuildOriginQuery()` e `MapFromOrigin()`

### Personalizar Mapeamento
- **Método:** `MapToDestinationProduct()` em `MigrationService.cs`
- **Campos:** Ajustar lógica de mapeamento conforme necessário

### Adicionar Validações
- **Local:** Método `ProcessSingleProductAsync()`
- **Tipo:** Validações de negócio antes de inserir/atualizar

## Suporte e Manutenção

### Arquivos de Configuração
- **App.config:** Configurações gerais
- **DatabaseConfig.cs:** Strings de conexão
- **Logger.cs:** Configurações de log

### Backup e Rollback
- Sistema não possui rollback automático completo
- Recomenda-se backup dos bancos antes da execução
- Logs permitem rastreabilidade de todas as operações

### Versionamento
- **Versão atual:** 1.0.0.0
- **Compatibilidade:** .NET Framework 4.8, PostgreSQL 17
- **Última atualização:** Janeiro 2025

## Contato e Suporte
Para dúvidas ou problemas, consulte os logs gerados e verifique as configurações de conexão e permissões de arquivo.
