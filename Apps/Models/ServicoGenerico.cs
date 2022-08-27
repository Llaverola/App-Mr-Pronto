using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Apps.Models
{
    public class DataModel
    {
        public Categorias Categorias { get; set; }
        public Definicoes Definicoes { get; set; }
        public Utilizador Utilizador { get; set; }
        public Faqs Faqs { get; set; }
        public DataModel()
        {
            Categorias = new Categorias();
            Definicoes = new Definicoes();
            Utilizador = new Utilizador();
            Faqs = new Faqs();
        }
    }

    public class Faqs
    {
        public List<Faq> list { get; set; }
        public Faqs()
        {
            list = new List<Faq>();
        }
    }

    public class Faq
    {
        public string pergunta { get; set; }
        public string resposta { get; set; }
    }

    public class Utilizador
    {
        public int UmbracoMemberId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DispositivoId { get; set; }
        public string Username { get; set; }
        public byte[] FotoByteArray { get; set; }
        public string Telemovel { get; set; }
        public string Morada { get; set; }
        public string NumeroDaPorta { get; set; }
        public string Andar { get; set; }
        public string Cidade { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Area { get; set; }
    }

    public class UtilizadorRegisto
    {
        public string email { get; set; }
        public string nome { get; set; }
        public string deviceId { get; set; }
        public string password { get; set; }
        public string area { get; set; }
    }

    public class UtilizadorNovaPwd
    {
        public string email { get; set; }
        public string password { get; set; }
    }


    public class PedidoDeContactoPostItem
    {
        public string primeiro_nome { get; set; }
        public string ultimo_nome { get; set; }
        public string email { get; set; }
        public string mensagem { get; set; }
    }

    public class UtilizadorEditarDados
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string telemovel { get; set; }
        public string email { get; set; }
        public string rua { get; set; }
        public string andar { get; set; }
        public string porta { get; set; }
        public string cidade { get; set; }
        public string area { get; set; }
    }

    public class UtilizadorUploadFoto
    {
        public int id { get; set; }
        public byte[] fotoArray { get; set; }
    }

    public class Notificacoes
    {
        public List<Notificacao> List { get; set; }
    }

    public class Notificacao
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public string data { get; set; }
        public string hora { get; set; }
        public bool isLida { get; set; }
    }

    public class OutrosContactos
    {
        public string telefone_1 { get; set; }
        public string telefone_2 { get; set; }
        public string telefone_3 { get; set; }
        public string email_1 { get; set; }
        public string email_2 { get; set; }
        public string email_3 { get; set; }
        public string website { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
    }

    public class Categorias
    {
        public List<Categoria> List { get; set; }

        public Categorias()
        {
            List = new List<Categoria>();
        }
    }

    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public SubCategorias SubCategorias { get; set; }
    }

    public class SubCategorias
    {
        public List<SubCategoria> List { get; set; }

        public SubCategorias()
        {
            List = new List<SubCategoria>();
        }
    }

    public class SubCategoria
    {
        public int Id { get; set; }
        public int CategoriaParentId { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public Servicos Servicos { get; set; }
    }

    public class Servicos
    {
        public List<Servico> List { get; set; }

        public Servicos()
        {
            List = new List<Servico>();
        }
    }

    public class Servico
    {
        public int Id { get; set; }
        public int CategoriaParentId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoDbl { get; set; }
        public string Preco { get; set; }
        public string ImagemUrl { get; set; }
        public bool isInCart { get; set; }
    }

    public class Pedidos
    {
        public List<Pedido> List { get; set; }

        public Pedidos()
        {
            List = new List<Pedido>();
        }
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int CategoriaParentId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double PrecoDbl { get; set; }
        public string Preco { get; set; }
        public string ImagemUrl { get; set; }
    }

    public class UtilizadorAlterarPwd
    {
        public string username { get; set; }
        public string password { get; set; }
    }


    public class InsertIntoCartPost
    {
        public int memberId { get; set; }
        public int servicoId { get; set; }
        public decimal preco { get; set; }
    }

    public class ComprovativoPostModel
    {
        public string order_number { get; set; }
        public byte[] file { get; set; }
        public string extension { get; set; }
    }

    public class DeleteFromCartPost
    {
        public int memberId { get; set; }
        public int servicoId { get; set; }
    }

    public class MyCart
    {
        public int memberId { get; set; }
        public string order_number { get; set; }
        public decimal total { get; set; }
        public decimal perc_a_pagar { get; set; }
        public List<MyCart_Itens> servicos { get; set; }
        public bool is_draft { get; set; }
        public DateTime data_datetime { get; set; }
        public string data_string { get; set; }
    }

    public class MyOrder
    {
        public string order_number { get; set; }
        public string estado { get; set; }
        public int memberId { get; set; }
        public decimal total { get; set; }
        public string comprovativo_de_pagamento_entrada_url { get; set; }
        public List<MyCart_Itens> servicos { get; set; }
        public bool pagamento_validado { get; set; }
        public DateTime data_datetime { get; set; }
        public string data_string { get; set; }
        public string total_str { get; set; }
        public DateTime? dataDoServico { get; set; }
        public string dataDoServico_String { get; set; }
        public bool terminado { get; set; }
        public string morada { get; set; }
    }

    public class MyCart_Itens
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public decimal preco { get; set; }
        public int qty { get; set; }
    }

    public class Definicoes
    {
        public string nome { get; set; }

        public string email { get; set; }

        public string telefone1 { get; set; }

        public string telefone2 { get; set; }

        public string telefone3 { get; set; }
        public string morada { get; set; }
        public string smtp_host { get; set; }

        public int smtp_port { get; set; }

        public bool smtp_enableSsl { get; set; }

        public string smtp_username { get; set; }

        public string smtp_pwd { get; set; }

        public string smtp_nome { get; set; }

        public List<string> areas_de_atuacao { get; set; }
        public string pagamento_iban { get; set; }

        public string pagamento_nome { get; set; }
    }

    public class FavoritoPost
    {
        public int memberId { get; set; }
        public int servicoId { get; set; }
        public FavoritoPost()
        {
            memberId = 0;
            servicoId = 0;
        }
    }

    public class MoradaPostServico
    {
        public int memberId { get; set; }
        public string rua { get; set; }
        public string andar { get; set; }
        public string porta { get; set; }
        public string localidade { get; set; }
        public string area { get; set; }
        public string notas { get; set; }
        public byte[] foto1 { get; set; }
        public byte[] foto2 { get; set; }
    }
}
