# GFICash
## POST - /v1/lancamentos
{
  "valor": 0,
  "tipo": 0, 
  "dt": "2025-12-05T04:38:45.112Z"
}
Tipos = 0(Debito) e 1(Credito)

# GFIReport
## GET - /v1/reports/daily
Param: Date = formato yyyy/mm/dd.

## GET - /v1/reports/monthly
Param: Year = ano desejado e Month = mes desejado.

## GET - /v1/reports/range
Param: start e end no formato yyyy/mm/dd.

## GET - /v1/reports/summary
Retorna o total de todos os períodos.

Nas rotas daily, monthly e range, o retorno é composto por:
Total por tipo + saldo do período + lista de lancamentos com detalhes.
