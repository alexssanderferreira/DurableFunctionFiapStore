using DurableFunction.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunction;

public class DurableFunction
{
    [FunctionName("Orchestrator")]
    public static async Task<Pedido> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        Pedido pedido = context.GetInput<Pedido>();

        try
        {
            Pedido pedidoValidado = await context.CallActivityAsync<Pedido>("ValidarPedido", pedido);
            Pedido pedidoProcessado = await context.CallActivityAsync<Pedido>("ProcessaPedido", pedidoValidado);
            Pedido pedidoFinalizado = await context.CallActivityAsync<Pedido>("FinalizaPedido", pedidoProcessado);
            return pedidoFinalizado;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [FunctionName("ValidarPedido")]
    public static Pedido ValidaPedido([ActivityTrigger] Pedido pedido)
    {
        if (pedido == null) pedido.Status = Enum.EStatusProcessamento.ProcessamentoError;
        else if (pedido.Produtos is null) pedido.Status = Enum.EStatusProcessamento.SemProduto;
        else if (pedido.Cliente is null) pedido.Status = Enum.EStatusProcessamento.SemCliente;
        else pedido.Status = Enum.EStatusProcessamento.Processamento;
        return pedido;
    }

    [FunctionName("ProcessaPedido")]
    public static Pedido ProcessaPedido([ActivityTrigger] Pedido pedido)
    {
        if (pedido.Status != Enum.EStatusProcessamento.Processamento) return pedido;

        pedido.DataProcessamento = DateTime.Now;
        decimal total = 0;

        foreach(Produto produto in pedido.Produtos)
        {
            total += (produto.Valor * produto.Quantidade);
        }
        pedido.ValorPedido = total;

        return pedido;
    }

    [FunctionName("FinalizaPedido")]
    public async Task<HttpResponseMessage> FinalizaPedido([ActivityTrigger] Pedido pedido, ILogger log)
    {
        if (pedido.Status != Enum.EStatusProcessamento.Processamento)
        {
            log.LogInformation($"Pedido {pedido.Id} não processado.");
            var erroResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Mensagem = "Pedido não processado, verifique as informações." }), Encoding.UTF8, "application/json")
            };
            return erroResponse;
        }

        pedido.IsEnvio = true;
        pedido.Status = Enum.EStatusProcessamento.ProcessamentoSuccesso;

        log.LogInformation($"Pedido {pedido.Id} processado.");
        var successResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(pedido), Encoding.UTF8, "application/json")
        };
        return successResponse;
    }

    [FunctionName("Pedido_HttpStart")]
    public static async Task<ActionResult> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        string data = await req.Content.ReadAsStringAsync();

        Pedido pedido = JsonConvert.DeserializeObject<Pedido>(data);

        pedido.Id = Guid.NewGuid();

        log.LogInformation($"Pedido {pedido.Id}");
        
        await starter.StartNewAsync("Orchestrator", pedido);

        return new OkObjectResult(pedido);
    }

    [FunctionName("GetPedido")]
    public async Task<ActionResult> GetPedido(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPedido")] HttpRequest req,
        ILogger log)
    {
        var id = req.Query["id"];

        Pedido pedido = new Pedido();

        string idGuid = "DC8AC6C3-5E91-4B95-BB5C-43F0031C0A5F";

        if(id == idGuid) 
        {
            log.LogInformation($"Consulta pedido {pedido.Id}.");

            pedido.Id = Guid.Parse(idGuid);
            pedido.Status = Enum.EStatusProcessamento.ProcessamentoSuccesso;
            pedido.DataProcessamento = DateTime.Now;
            pedido.ValorPedido = 100;
            pedido.IsEnvio = true;
            pedido.Cliente = new Cliente() { Id = Guid.NewGuid(), Nome = "Cliente Teste" };
            pedido.Produtos = new List<Produto>() { new Produto() { Id = Guid.NewGuid(), Nome = "Produto Teste", Valor = 100, Quantidade = 1 } };

            return new OkObjectResult(pedido);
        }

        return new NotFoundResult();
    }

}