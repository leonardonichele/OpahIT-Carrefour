# GFI Cash & Report Platform
## Leonardo da Silva Nichele - Opah IT & Banco Carrefour 2025

Sistema moderno, modular e escalável, construída para o desafio técnico.
A solução é composta por dois microserviços independentes:

GFICash → Serviço de lançamentos financeiros (write model)
GFIReport → Serviço de relatórios consolidados (read model)

Ambos se comunicam de forma desacoplada usando Outbox Pattern + RabbitMQ, garantindo entrega confiável de eventos e consistência entre serviços.

Para detalhes aprofundados, convido acessar a pasta documentacoes.
