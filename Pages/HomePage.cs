using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frais.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace Frais.Pages
{
    public class HomePage
    {
        AndroidDriver driver = null;
        AndroidUtils androidUtils;
        private static By header_logo = By.Id("com.cerebrent.frais.MainActivity:id/header_logo");

        public void LaunchApp(){
           
            AppiumDriverFactory.CreateAndroidDriver();
            driver = AppiumDriverFactory.GetDriver();
            androidUtils = new AndroidUtils(driver);

            if(androidUtils.ObjectExists(header_logo)){
                Console.WriteLine("Application launched");
            }
            else
            Assert.Fail("Unable to launch application!");

        }   
    }
}