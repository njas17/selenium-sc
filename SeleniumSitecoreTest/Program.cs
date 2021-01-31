using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumSitecoreTest
{
    class Program
    {
        static string curFolder = @"C:\InterviewSitecore";
        static string newFolder = @"C:\SitecoreTest";
        static string fileFullPath = @"C:\InterviewSitecore\somefile.txt";
        static string newFileFullPath = @"C:\SitecoreTest\somefile.txt";
        static string chromeDriver = @"C:\chromedriver";

        static void Main(string[] args)
        {
            try
            {
                Test1();
                Test2();
            }
            catch (Exception e)
            {
                Console.WriteLine("============= Catch Error or Assert failed =============");
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }

        private static void Test1()
        {
            using (IWebDriver driver = new ChromeDriver(chromeDriver))
            {
                //1) Navigate to "https://gofile.io/d/9YsNjv"
                WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl("https://gofile.io/d/9YsNjv");

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(webDriver => webDriver.FindElement(By.XPath("//*[@id='fileInfoContent']/div/pre/code[@class=' language-txt']")).Displayed);

                //2) Get the value of the element "code.language-txt"
                IWebElement element = driver.FindElement(By.XPath("//code[@class=' language-txt']"));
                string strValues = element.Text;
                //Console.WriteLine(strValues);

                //3) Navigate to http://www.unit-conversion.info/texttools/hexadecimal/
                driver.Navigate().GoToUrl("http://www.unit-conversion.info/texttools/hexadecimal/");
                //Console.WriteLine(strValues);

                ////4) On the Convert dropdown, select the "hex numbers to text" option
                wait.Until(webDriver => webDriver.FindElement(By.Id("field_convert")).Displayed);
                var selectElement = new SelectElement(driver.FindElement(By.Id("field_convert")));

                // select by text
                selectElement.SelectByText("hex numbers to text");

                //5) In the input data text area, input the data from step 2
                var textElement = driver.FindElement(By.Id("field_text"));
                driver.FindElement(By.Id("field_text")).SendKeys(strValues);

                //6) Copy the output and save it into a text file(Using Code)
                //    *Location of the text file must be inside "C:\InterviewSitecore"
                //    * The Folder & text file must be created new (Using Code)
                wait.Until(webdriver => webdriver.FindElement(By.Id("output")).GetAttribute("value").Length > 2);
                var outputStr = driver.FindElement(By.Id("output")).GetAttribute("value");
                //Console.WriteLine("Output ====" + outputStr);
                createFile(outputStr, curFolder);

                // - Assert that file in C:\InterviewSitecore is present. 
                bool result = File.Exists(fileFullPath);

                Assert.IsTrue(result, "File exists in path, assertion passed.");
                Console.WriteLine("Assertion is " + result);
            }
        }

        private static void Test2()
        {
            //1) Change the Folder name from InterviewSitecore to SitecoreTest.
            renameDirectory();

            //2)Read the file in C:/SitecoreTest.
            string pw = getFileContent(); //Console.WriteLine(pw);

            //3) Navigate to "https://gofile.io/d/E77oZ4" and input the password
            //    *The password must be read from textfile in C:/ InterviewSitecore  // I changed this to sitecoretest   

            //String downloadFilepath = "/path/to/download";
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", newFolder);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");

            using (IWebDriver driver = new ChromeDriver(chromeDriver, chromeOptions))
            {
                WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(15));
                driver.Navigate().GoToUrl("https://gofile.io/d/E77oZ4");

                wait.Until(webDriver => webDriver.FindElement(By.ClassName("swal2-input")).Displayed);
                driver.FindElement(By.ClassName("swal2-input")).SendKeys(pw);

                //4) Click OK
                driver.FindElement(By.CssSelector(".swal2-confirm.swal2-styled")).Click();

                //5) Download the image then store it in C:/ InterviewSitecore
                // I changed to c:/sitecoretest
                wait.Until(webDriver => webDriver.FindElement(By.CssSelector(".btn.btn-primary.btn-sm.mt-3.mb-3")).Displayed);
                wait.Until(webDriver => webDriver.FindElement(By.XPath("//*[@id='fileInfoContent']/div/img[@class='img-fluid']")).Displayed);

                driver.FindElement(By.CssSelector(".btn.btn-primary.btn-sm.mt-3.mb-3")).Click();

                Console.WriteLine("Downloading...");
                Thread.Sleep(7000); // to wait for download to complete

                Console.WriteLine(".....");
                Thread.Sleep(2000);

                //- Assert Folder name is change.
                bool folderExist = (!Directory.Exists(curFolder) && Directory.Exists(newFolder));
                Assert.IsTrue(folderExist, "Folder name has changed");
                Console.WriteLine("Assertion 'folder-exists' is " + folderExist);

                // - Assert image is there
                //  - U can send the image to us as well as the code for the autotest :)
                bool ifFound = fileExistInFolder();
                Assert.IsTrue(ifFound, "Image file in folder.");
                Console.WriteLine("Assertion 'image-exists in folder' is " + ifFound);
            }
        }

        private static bool fileExistInFolder()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(newFolder);
            FileInfo[] fileArr = dirInfo.GetFiles("*.gif");
            if (fileArr.Length == 0) return false;

            return true;
        }
        private static void createFile(string val, string folderPath)
        {
            try
            {
                string fileName = fileFullPath;

                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                if (File.Exists(fileName)) File.Delete(fileName);

                File.WriteAllText(fileName, val);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private static void renameDirectory()
        {
            try
            {
                if (Directory.Exists(newFolder)) Directory.Delete(newFolder, true);
                if (Directory.Exists(curFolder)) Directory.Move(curFolder, newFolder);
                else throw new Exception(String.Format("The {0} is missing.", curFolder));
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private static string getFileContent()
        {
            string str = "";
            using (StreamReader reader = new StreamReader(newFileFullPath))
            {
                str = reader.ReadLine(); //just read/get the first line only
            }
            return str;
        }
    }

}
