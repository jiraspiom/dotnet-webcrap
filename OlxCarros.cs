using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webscrap
{
    public static class OlxCarros
    {
        public static void Inicio()
        {
            Console.WriteLine("Iniciando...");

            List<string> bookLinks = GetBookLink("https://www.olx.com.br/autos-e-pecas/carros-vans-e-utilitarios?sf=1");
            Console.WriteLine("Encontrado {0} link", bookLinks.Count);

            List<Carro> carros = GetBookDetails(bookLinks);

            //exportToCSV(books);
            foreach (var item in carros)
            {
                Console.WriteLine($"Titulo: {item.Titulo} " + "\n" +
                    $"Marca: {item.Marca} Ano: {item.Ano} " + "\n" +
                    $"Modelo: {item.Modelo} " + "\n" +
                    $"Link: {item.Link} " + "\n" +
                    $"Valor: {item.Valor} ");
            }
        }

        static List<string> GetBookLink(string url)
        {
            List<string> bookLinks = new List<string>();
            HtmlDocument doc = GetDocument(url);
            //HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"ad-list\"]/li/a");
            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"ad-list\"]/li/section/a");
            

            Uri baseUri = new Uri(url);
            
            foreach (HtmlNode link in htmlNodes)
            {
                string href = link.Attributes["href"].Value;
                bookLinks.Add(new Uri(baseUri, href).AbsoluteUri);
            }

            return bookLinks;
        }


        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static List<Carro> GetBookDetails(List<string> urls)
        {
            List<Carro> carros = new List<Carro>();
            foreach (string url in urls)
            {
                HtmlDocument document = GetDocument(url);
                string titleXpath = "//*[@id=\"content\"]/div[2]/div/div[2]/div[1]/div[21]/div/div/h1";
                string marcaXpath = "//*[@id=\"content\"]/div[2]/div/div[2]/div[1]/div[36]/div/div/div/div[4]/div[3]/div/div[2]/a";
                string modeloXpath = "//*[@id=\"content\"]/div[2]/div/div[2]/div[1]/div[36]/div/div/div/div[4]/div[2]/div/div[2]/a";
                string anoXpath = "//*[@id=\"content\"]/div[2]/div/div[2]/div[1]/div[36]/div/div/div/div[4]/div[5]/div/div[2]/a";
                string valorXpath = "//*[@id=\"content\"]/div[2]/div/div[2]/div[1]/div[22]/div/div/div/div[1]/div[1]/h2";


                Carro carro = new Carro();

                carro.Titulo = document.DocumentNode.SelectSingleNode(titleXpath)?.InnerText;
                carro.Marca = document.DocumentNode.SelectSingleNode(marcaXpath)?.InnerText;
                carro.Modelo = document.DocumentNode.SelectSingleNode(modeloXpath)?.InnerText;
                carro.Valor = document.DocumentNode.SelectSingleNode(valorXpath)?.InnerText;
                carro.Ano = document.DocumentNode.SelectSingleNode(anoXpath)?.InnerText;
                carro.Link = url;

                carros.Add(carro);

            }
            return carros;
        }

    }

    public class Carro
    {
        //Modelo, Marca, ano, quilometragem, cambio, combustível, valor, contato do anunciante, link do anúncio
        public string? Titulo { get; set; }
        public string? Modelo { get; set; }
        public string? Marca { get; set; }
        public string? Ano { get; set; }
        public string? Quilometragem { get; set; }
        public string? Cambio { get; set; }
        public string? Combustivel { get; set; }
        public string? Valor { get; set; }
        public string? Contato { get; set; }
        public string? Link { get; set; }

    }

}
