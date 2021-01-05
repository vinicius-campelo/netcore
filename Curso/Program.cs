using CursoEFCore.Data;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CursoEFCore
{
    public class Program
    {

        static void Main(string[] args)
        {
            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultaPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoverRegistro();
        }


        private static void RemoverRegistro()
        {
            using var _db = new ApplicationContext();
           var cliente = _db.Clientes.Find(3);
            _db.Clientes.Remove(cliente);

            _db.SaveChanges();


        }
        private static void AtualizarDados()
        {
            using var _db = new ApplicationContext();

            //var cliente = _db.Clientes.FirstOrDefault(c => c.Id == 3);
            // cliente.Nome = "Anderson da Costa";



            var cliente = new Cliente
            {
                Id = 1
            };


            var clientesDesconectados = new
            {
                Nome = "Cleinte desconectado",
                Telefone = "991759170"
            };

            _db.Attach(cliente);
            _db.Entry(cliente).CurrentValues.SetValues(clientesDesconectados);
           // _db.Clientes.Update(consultaCliente);
            _db.SaveChanges();
        }

        private static void ConsultaPedidoCarregamentoAdiantado()
        {
            using var _db = new ApplicationContext();
            var pedidos = _db.Pedidos.Include(p=>p.Itens)
                .ThenInclude(p=>p.Produto).ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var _db = new ApplicationContext();

            var cliente = _db.Clientes.FirstOrDefault();
            var produto = _db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                //IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            _db.Pedidos.Add(pedido);
            _db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var _db = new ApplicationContext();

            var query = (from c in _db.Clientes.Where(c => c.Id > 0)
                         select c).ToList();

            var metodo = _db.Clientes.Where(c => c.Nome.Contains("Vinicius")).ToList();

            foreach (var item in metodo)
            {
                Console.WriteLine("> " +
                               item.Nome
                     + " - " + item.CEP
                     + " - " + item.Cidade
                     + " - " + item.Telefone);
            }

        }


        private static void InserirDados()
        {
            using var _db = new ApplicationContext();

            var produto = new List<Produto>()
            {
               new Produto(){ Descricao = "Produto XPTO", CodigoBarras = "12309049030940", Valor = 10m, TipoProduto = ValueObjects.TipoProduto.MercadoriaParaRevenda, Ativo = true},
               new Produto(){ Descricao = "Produto ABCD", CodigoBarras = "34095093904509", Valor = 15m, TipoProduto = ValueObjects.TipoProduto.Servico, Ativo = true},
               new Produto(){ Descricao = "Produto PPTP", CodigoBarras = "34095094994949", Valor = 3m, TipoProduto = ValueObjects.TipoProduto.Embalagem, Ativo = true},
            };

            _db.Produtos.AddRange(produto);
            var registros = _db.SaveChanges();

            Console.WriteLine($"Total de Registros: {registros}");
        }


        private static void InserirDadosEmMassa()
        {
            using var _db = new ApplicationContext();

            var cliente = new List<Cliente>()
            {
               new Cliente(){  Nome="Vinicius Campelo", Telefone = "991759170", Cidade = "Gama", Estado = "DF", CEP = "7242240" },
               new Cliente(){  Nome="Marisa Lima", Telefone = "991763254", Cidade = "Brasilia", Estado = "DF", CEP = "7214452" },
               new Cliente(){ Nome="João Marcelo Vieira", Telefone = "997589254", Cidade = "Goiania", Estado = "GO", CEP = "7288000" },
            };

           
            var produto = new List<Produto>()
            {
               new Produto(){ Descricao = "Produto TTPA", CodigoBarras = "12309049030944", Valor = 20m, TipoProduto = ValueObjects.TipoProduto.MercadoriaParaRevenda, Ativo = true},
               new Produto(){ Descricao = "Produto DDDA", CodigoBarras = "34095093904545", Valor = 5m, TipoProduto = ValueObjects.TipoProduto.Servico, Ativo = true},
               new Produto(){ Descricao = "Produto CCCA", CodigoBarras = "34095094994946", Valor = 9m, TipoProduto = ValueObjects.TipoProduto.Embalagem, Ativo = true},
            };


             _db.Clientes.AddRange(cliente);

             _db.Produtos.AddRange(produto);
         
             var registros = _db.SaveChanges();

            Console.WriteLine($"Total de Registros: {registros}");
        }

    }
            
}
