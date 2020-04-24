using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace RWSMoravia_7._0
{
    class GetPhilosophy
    {
        static void Main()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://en.wikipedia.org/wiki/Special:Random");

            WikipediaArticlePage wikipediaArticlePage = new WikipediaArticlePage(driver);

            int redirects = 0;

            List<string> visitedArticles = new List<string>();
            visitedArticles.Add(driver.Url);

            IWebElement firstLink;

            while (wikipediaArticlePage.WikipediaArticlePageTitle() != "Philosophy")
            {
                redirects++;

                try
                {
                    firstLink = wikipediaArticlePage.GetFirstLink();
                }
                catch
                {
                    break;
                }

                string nextArticle = firstLink.GetAttribute("href");

                if (visitedArticles.Contains(nextArticle)) break;

                visitedArticles.Add(nextArticle);
                driver.Navigate().GoToUrl(nextArticle);
            }

            Console.Clear();
            if (wikipediaArticlePage.WikipediaArticlePageTitle() == "Philosophy")
            {
                Console.WriteLine("It took -{0}- redirects to get to Philosophy from {1}.", redirects, visitedArticles[0]);
            }
            else
            {
                Console.WriteLine("After -{0}- redirects could't get to Philosophy from {1}.", redirects, visitedArticles[0]);
            }

            driver.Quit();
        }

    }
}