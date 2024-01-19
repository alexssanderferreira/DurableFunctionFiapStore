using DurableFunction.Enum;
using System;
using System.Collections.Generic;

namespace DurableFunction.Model;

public class Pedido : Base
{
    public Pedido()
    {
        Status = EStatusProcessamento.NaoProcessado;
        IsEnvio = false;
    }

    public Cliente Cliente { get; set; }
    public List<Produto> Produtos { get; set; }
    public DateTime DataPedido { get; set; }
    public DateTime DataProcessamento { get; set; }
    public decimal ValorPedido { get; set; }
    public bool IsEnvio { get; set; }

    public EStatusProcessamento Status { get; set; }
}
