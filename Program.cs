using AngleSharp;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SurfboardBrokerScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {




                var connectionString = xyz.MLab;
                var client = new MongoClient(connectionString);
                var db = client.GetDatabase("surfengine");


                var collection = db.GetCollection<ScrapedBoard>("Surfboard");
                collection.DeleteMany(Builders<ScrapedBoard>.Filter.Eq("Source", "SurfboardBroker"));










                int? ParseLength(string input)
                {
                    var match = Regex.Match(input, @"(\d{1,2})\s*’\s*(\d{1,2})?"); // swaped ' for ’
                    if (!match.Success)
                        return null;

                    var inches = match.Groups[2].Value;
                    if (inches.Length == 0)
                        inches = "0";
                    var Answer = int.Parse(match.Groups[1].Value) * 12 + int.Parse(inches);

                    if (Answer >= 180)
                    {
                        return null;
                    }
                    return Answer;
                }






                var context = BrowsingContext.New(AngleSharp.Configuration.Default.WithDefaultLoader());

                for (var i = 1; i < 34; i++)
                {



                    var doc = context.OpenAsync("https://surfboardbroker.com/collections/surfboard-count?page=" + i).Result;


                    doc = doc;

                    var nodes =
                        doc.QuerySelectorAll(".grid-view-item")
                        .Select(x =>

                         new
                         {
                             Title =
                                x
                                .QuerySelector(".grid-view-item__title")
                                .TextContent
                                .Trim(),
                             Price =
                                x
                                .QuerySelector(".product-price__price")
                                ?.TextContent // this checks for null reference error
                                .Trim(),

                             Image =
                                x
                                .QuerySelector(".grid-view-item__image")
                                ?.Attributes["src"]
                                .Value,

                             Link =
                                x
                                .QuerySelector(".grid-view-item__link")
                                ?.Attributes["href"]
                                ?.Value,


                         });


                    nodes = nodes;



                    foreach (var node in nodes)
                    {


                        var mySurfboard = new ScrapedBoard();


                        mySurfboard.Name = node.Title;
                        mySurfboard.Brand = node.Title;

                        //price
                        var FormattedPrice = node.Price.Replace(",", "");
                        var blah = FormattedPrice.Split('$', '.');
                        mySurfboard.Price = Convert.ToInt32(blah[1]);



                        //height
                        //if (node.Title.IndexOf("\’") > -1)
                        //{
                            mySurfboard.Height = ParseLength(node.Title);
                        //}

                        //Image
                        mySurfboard.Image = node.Image;

                        //Link
                        mySurfboard.Link = "surfboardbroker.com" + node.Link;

                        //Date created
                        mySurfboard.Created = DateTime.UtcNow.Date;


                        mySurfboard = mySurfboard;

                        Console.WriteLine(mySurfboard.Name);



                        collection.InsertOne(mySurfboard);


                    }


                }

            }
            catch (Exception e)
            {

                throw;
            }



            Console.ReadKey();

        }
    }
}
