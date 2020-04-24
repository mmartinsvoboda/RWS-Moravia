using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace RWSMoravia_7._0
{
    class WikipediaArticlePage
    {
        readonly IWebDriver driver;

        public WikipediaArticlePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string WikipediaArticlePageTitle()
        {
            return driver.FindElement(By.Id("firstHeading")).Text;
        }

        public IWebElement GetFirstLink()
        {
            string parrent;
            string parrentClass;
            string topParrentClassName;
            int lastClassCount;

            var possibleLinks = driver.FindElement(By.CssSelector(".mw-parser-output")).FindElements(By.CssSelector("a[href*=\"wiki\"]:not([href*=\":\"])"));

            for (int i = 0; i < possibleLinks.Count;)
            {
                // When on https://en.wikipedia.org/wiki/United_States the code wouldn't behave correctly. Element "Coordinates" is defined differently on all other pages by Wikipedia and the code works.
                if (possibleLinks[i].Text == "Coordinates") i++;

                parrent = "..";
                parrentClass = possibleLinks[i].FindElement(By.XPath(parrent)).GetAttribute("class");
                topParrentClassName = "";

                while (parrentClass != "mw-parser-output")
                {
                    if (parrentClass != "") topParrentClassName = parrentClass;

                    parrentClass = possibleLinks[i].FindElement(By.XPath(parrent)).GetAttribute("class");
                    parrent += "/..";
                }

                if (topParrentClassName == "") return possibleLinks[i];

                topParrentClassName = "." + topParrentClassName.Replace(" ", ".");
                lastClassCount = driver.FindElement(By.CssSelector(topParrentClassName)).FindElements(By.CssSelector("a[href*=\"wiki\"]:not([href*=\":\"])")).Count;

                if (lastClassCount <= 2)
                {
                    i++;
                    continue;
                }

                //Skips all links that have the same class in their top parrent
                i += lastClassCount;
            }
            return null;
        }

    }
}
