# Fiap - Durable Function 

|Alunos| E-mail|
|------|-------|
|Antonio Andderson de Freitas Soares|andderson.freitas@gmail.com|
|Elielson do Nascimento Rodrigues|elielsonrj@hotmail.com|
|Rafael Faustino Magalhaes Pontin|rfmpontin@gmail.com|
|Alexssander Ferreira do Nascimento|alexssanderferreira@hotmail.com|



### Instruções para Executar o Projeto: 

#### Novopedido

Para cadastrar um Pedido é necessário chamar o end point **/novopedido**
Exemplo: 
**Verbo: Post**
[http://localhost:7283/api/novopedido](http://localhost:7283/api/novopedido)

```json
{
  "cliente": {
    "nome": string,
    "id": guid
  },
  "produtos": [
    {
      "nome": string,
      "descricao": string,
      "valor": decimal,
      "quantidade": int,
      "id": guid
    }
  ],
  "dataPedido": date,
  "dataProcessamento": date,
  "valorPedido": int,
  "isEnvio": bool,
  "status": int,
  "id": guid
}
```
Exemplo de Json: 

```json
	{
  "cliente": {
    "nome": "Cliente Teste",
    "id": "1f159044-c16f-4bff-bcd3-52efb044ea42"
  },
  "produtos": [
    {
      "nome": "Produto Teste",
      "descricao": null,
      "valor": 100,
      "quantidade": 1,
      "id": "242e10be-fe44-4697-a6b9-af3c4dbb7232"
    }
  ],
  "dataPedido": "0001-01-01T00:00:00",
  "dataProcessamento": "2024-01-19T22:45:30.8404645+00:00",
  "valorPedido": 100,
  "isEnvio": true,
  "status": 1,
  "id": "006a9022-2fa2-4a09-8b12-68a5848a2c74"
}
```

##
#### Consulta Pedido 

Método para consultar o status do pedido no end point /pedido

|Id| Status|
|------|-------|
|0| Processamento |
|1| Processamento com Sucesso|
|2| Processamento com Erro|
|3| Não Processado|
|4| Sem Produto no pedido| 
|5| Sem Cliente no pedido|


**verbo:  get**

Exemplo: 
[http://localhost:7283/api/pedido?Id=GUID](http://localhost:7283/api/pedido?Id=DC8AC6C3-5E91-4B95-BB5C-43F0031C0A5F)
```curl
	http://localhost:7283/api/pedido?Id=DC8AC6C3-5E91-4B95-BB5C-43F0031C0A5F
```
