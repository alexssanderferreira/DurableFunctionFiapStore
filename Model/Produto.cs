namespace DurableFunction.Model;

public class Produto : Base
{
    public string Nome { get; set; }
    public string Descricao { get; set;}
    public decimal Valor {  get; set; }
    public int Quantidade { get; set; }
}
