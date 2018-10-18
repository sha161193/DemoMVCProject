using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Drawing.Imaging;
using NUnit.Framework;
namespace TestApplication.Tests.Controllers
{
    [TestFixture]
    public class EmployeeControllerTest
    {
        private const string IE_DRIVER_PATH = @"E:\SeleniumDriver";
        private const string ScreenShotLocation = @"E:\ScreenShot";
        [Test]
        public void Get()
        {
            IWebDriver driver = null;
            try
            {
                driver = new ChromeDriver(IE_DRIVER_PATH);

                driver.Navigate().GoToUrl("http://localhost:60006/");

                // Find the text input element by its name
                IWebElement enter = driver.FindElement(By.TagName("a"));

                enter.Click();

                TestEmployeeListScreen(driver, "Launch");

                TestAddEmployee(driver);

                TestEmployeeListScreen(driver, "Add");

                TestEditEmployee(driver);

                TestEmployeeListScreen(driver, "Edit");

                TestDeleteEmployees(driver);

                TestEmployeeListScreen(driver, "AfterDelete");
            }
            catch (Exception ex) { }
            finally
            {
                driver.Quit();
            }
        }


        private void TestDeleteEmployees(IWebDriver driver)
        {
            //Find the last employee
            IWebElement checkBox = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('input:checkbox:last')[0]");
            checkBox.Click();

            //Find delete button and click
            IWebElement delBtn = driver.FindElement(By.Id("Delete"));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(1000);
            delBtn.Click(); //Perfome delete operation
            Thread.Sleep(1000);

        }

        private void TestEditEmployee(IWebDriver driver)
        {
            //Find the element anchortag for the employee having name as 'Anderson'
            IWebElement anchorTag = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('a',$(\"td:contains('Anderson')\").parent())[0]");
            anchorTag.Click();

            //Wait and then check until the control with id=Name is available
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.FindElement(By.Id("Name")); });

            //Find all the lements
            IWebElement name = driver.FindElement(By.Id("Name"));
            IWebElement sal = driver.FindElement(By.Id("Sal"));
            IWebElement submit = driver.FindElement(By.Id("Submit"));

            //Set the data (Change the name)
            name.SendKeys(" James");
            submit.Click();

            IAlert alert = null;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { alert = d.SwitchTo().Alert(); return alert; });
            alert.Accept();
        }

        private void TestAddEmployee(IWebDriver driver)
        {
            driver.FindElement(By.LinkText("Add")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.FindElement(By.Id("Name")); });

            IWebElement name = driver.FindElement(By.Id("Name"));
            IWebElement sal = driver.FindElement(By.Id("Sal"));
            IWebElement submit = driver.FindElement(By.Id("Submit"));

            name.SendKeys("Natasha");
            sal.SendKeys("5000");
            submit.Click();

            IAlert alert = null;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { alert = d.SwitchTo().Alert(); return alert; });
            alert.Accept();
        }


        public void TestEmployeeListScreen(IWebDriver driver, string type)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.ToLower().StartsWith("employeelist"); });
            IWebElement addLink = driver.FindElement(By.LinkText("Add"));
            IWebElement delLink = driver.FindElement(By.Id("Delete"));
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(ScreenShotLocation + "\\" + type + "EmpList.jpeg", ScreenshotImageFormat.Jpeg);
        }

    }
}
