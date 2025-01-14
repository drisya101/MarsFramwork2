﻿using MarsFramework.Config;
using MarsFramework.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using RelevantCodes.ExtentReports;
using System;
using static MarsFramework.Global.GlobalDefinitions;

namespace MarsFramework.Global
{
    class Base
    {
        #region To access Path from resource file

        public static int Browser = Int32.Parse(MarsResource.Browser);

        public static string ExcelPath = MarsResource.ExcelPath;
        public static string ScreenshotPath = MarsResource.ScreenShotPath;
        public static string ReportPath = MarsResource.ReportPath;
        public static string Url = MarsResource.URL;
        //public static GlobalDefinitions GlobalDefinitions;
        #endregion

        #region reports
        public static ExtentTest test;
        public static ExtentReports extent;
        public static void ExtentReports()
        {
            extent = new ExtentReports(MarsResource.ReportPath, false, DisplayOrder.NewestFirst);
            extent.LoadConfig(MarsResource.ReportXMLPath);
        }
        #endregion

        #region setup and tear down
        [SetUp]
        public void Inititalize()
        {

            switch (Browser)
            {

                case 1:
                    GlobalDefinitions.driver = new FirefoxDriver();
                    break;
                case 2:
                    GlobalDefinitions.driver = new ChromeDriver();
                    GlobalDefinitions.driver.Manage().Window.Maximize();
                    GlobalDefinitions.driver.Navigate().GoToUrl(Url);
                    break;

            }

            #region Initialise Reports

            //Extent = new ExtentReports(ReportPath, false, DisplayOrder.NewestFirst);
            //Extent.LoadConfig(MarsResource.ReportXMLPath);
            var testName = TestContext.CurrentContext.Test.Name;
            //var description= TestContext.CurrentContext.Test.Properties.
            //ExtentTestContext = new ExtentTest(testName, "description");
            #endregion

            if (MarsResource.IsLogin == "true")
            {
                SignIn loginobj = new SignIn();
                loginobj.Login("bdddemo@gmail.com", "bdd.demo123");
            }
            else
            {
                SignUp obj = new SignUp();
                obj.register();
            }

        }


        [TearDown]
        public void TearDown()
        {

            // Screenshot
            String img = SaveScreenShotClass.SaveScreenshot(GlobalDefinitions.driver, "Report");//AddScreenCapture(@"E:\Dropbox\VisualStudio\Projects\Beehive\TestReports\ScreenShots\");
            test.Log(LogStatus.Info, "Image example: " + img);

            switch (TestContext.CurrentContext.Result.Outcome.Status)
            {


                case NUnit.Framework.Interfaces.TestStatus.Passed:
                    test.Log(LogStatus.Pass, "Screenshot: " + img);
                    break;

                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    test.Log(LogStatus.Fail, "Screenshot: " + img);
                    break;
                default:
                    test.Log(LogStatus.Warning, "Screenshot: " + img);
                    break;
            }
            // end test. (Reports)
            extent.EndTest(test);
            // calling Flush writes everything to the log file (Reports)
            extent.Flush();
            // Close the driver :)            
            GlobalDefinitions.driver.Close();
            GlobalDefinitions.driver.Quit();
        }
        #endregion

    }
}