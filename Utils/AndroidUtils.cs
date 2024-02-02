using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Frais.Utils
{
    public class AndroidUtils
    {
        
        private AppiumDriver driver;

        public AndroidUtils(AppiumDriver driver)
        {
            this.driver = driver;
        }

        public Boolean ObjectExists(By by){
            try
            {
                if(driver.FindElements(by).Count==0){
                    return false;
                }
                else
                return true;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
                
            }
        }
    }
}