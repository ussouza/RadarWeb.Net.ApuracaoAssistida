# RadarWeb.Net.ApuracaoAssistida

Solução .NET para integração com as APIs de Apuração CBS da Receita Federal, persistência dos dados em SQL Server e execução automatizada.

## Estrutura

- **RadarWeb.Net.ApuracaoAssistida.Domain** — entidades e regras centrais do domínio.
- **RadarWeb.Net.ApuracaoAssistida.Application** — casos de uso, contratos e orquestração.
- **RadarWeb.Net.ApuracaoAssistida.Infrastructure** — acesso ao SQL Server, Entity Framework Core e integrações externas.
- **RadarWeb.Net.ApuracaoAssistida.Worker** — processo de execução agendada das consultas à Receita Federal.
- **RadarWeb.Net.ApuracaoAssistida.Api** — API HTTP para endpoints técnicos e webhook de notificações assíncronas.

## Fluxo previsto

1. O Task Scheduler do Windows inicia o Worker diariamente.
2. O Worker autentica na Receita Federal e abre as solicitações permitidas.
3. Cada solicitação é registrada no SQL Server com seu identificador.
4. A Receita Federal processa a solicitação de forma assíncrona.
5. A API recebe a notificação de conclusão pelo webhook.
6. A aplicação baixa o arquivo de retorno usando a URL assinada.
7. O retorno é validado e persistido no SQL Server.
8. Solicitações e processamentos ficam registrados para auditoria e recuperação.

## Princípios

- Não armazenar segredos diretamente no código-fonte.
- Não registrar tokens ou URLs assinadas completas em logs.
- Garantir idempotência no processamento dos retornos.
- Controlar o limite diário de solicitações da Receita Federal.
- Manter o Worker e o webhook desacoplados.
- Registrar erros e permitir reprocessamento.

## Próximas etapas

1. Configurar autenticação OAuth 2.0.
2. Modelar o banco de dados.
3. Implementar a API de Débitos CBS.
4. Implementar o webhook e o download dos retornos.
5. Implementar controle de solicitações e idempotência.
6. Configurar execução pelo Windows Task Scheduler.
7. Adicionar Créditos, Pagamentos e Recolhimentos.

## Framework

O projeto está preparado para **.NET 10** e utiliza nullable reference types e implicit usings.

## Observação

Os contratos de integração com a Receita Federal devem ser implementados diretamente a partir da documentação oficial vigente antes da entrada em produção.
