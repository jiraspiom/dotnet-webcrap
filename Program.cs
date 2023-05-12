// See https://aka.ms/new-console-template for more information
using CsvHelper;
using HtmlAgilityPack;
using System.Globalization;
using webscrap;

Console.WriteLine("Iniciando...");

List<string> bookLinks = GetBookLink("http://books.toscrape.com/catalogue/category/books/fiction_10/index.html");
Console.WriteLine("Encontrado {0} link", bookLinks.Count);

List<Book> books = GetBookDetails(bookLinks);

//exportToCSV(books);
foreach (var item in books)
{
    System.Console.WriteLine($"{item.Title}: {item.Price}");
}

static HtmlDocument GetDocument(string url)
{
    HtmlWeb web = new HtmlWeb();
    HtmlDocument doc = web.Load(url);
    return doc;
}

static List<string> GetBookLink(string url)
{
    List<string> bookLinks = new List<string>();
    HtmlDocument doc = GetDocument(url);
    HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//h3/a");

    Uri baseUri = new Uri(url);

    foreach (HtmlNode link in htmlNodes)
    {
        string href = link.Attributes["href"].Value;
        bookLinks.Add(new Uri(baseUri, href).AbsoluteUri);
    }

    return bookLinks;
}

static List<Book> GetBookDetails(List<string> urls)
{
    List<Book> books = new List<Book>();
    foreach (string url in urls)
    {
        HtmlDocument document = GetDocument(url);
        string titleXpath = "//h1";
        string priceXpath = "//*[@id=\"content_inner\"]/article/div[1]/div[2]/p[1]";
        

        Book book = new Book();

        book.Title = document.DocumentNode.SelectSingleNode(titleXpath).InnerText;
        book.Price = document.DocumentNode.SelectSingleNode(priceXpath).InnerText;
        
        books.Add(book);

    }
    return books;
}

static void exportToCSV(List<Book> books)
{
    StreamWriter writer = new StreamWriter("./books.csv");
        
    using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    {
        csv.WriteRecord(books);
    }
}

Livros.Inicio();


class Book
{
    public string? Title { get; set; }
    public string? Price { get; set; }
}