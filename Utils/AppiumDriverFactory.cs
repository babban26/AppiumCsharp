using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using System.Xml;
using Microsoft.Extensions.Configuration;


public class AppiumDriverFactory
{
    private static AndroidDriver _driver;

    private static AppiumLocalService _appiumLocalService;

    static IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
    static IConfigurationRoot root = builder.Build();

    private static string appPackage = root["appconfig:appPackage"];
    private static string appActivity = root["appconfig:appActivity"];
    private static string deviceName = root["appconfig:deviceName"];


    // private static string appPackage = "com.cerebrent.frais";
    //  private static string appActivity = "com.cerebrent.frais.MainActivity";
    // private static string deviceName = "Pixel6";

    public static void CreateAndroidDriver()
    {
        StartServer();
        _driver = new AndroidDriver(SetAppiumOptions(), TimeSpan.FromSeconds(120));
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

    }

    public static AndroidDriver GetDriver()
    {
        return _driver;
    }

    private static AppiumOptions SetAppiumOptions()
    {

        var appiumOptions = new AppiumOptions
        {
            AutomationName = "uiautomator2",
            DeviceName = root["deviceName"],
            PlatformName = "Android"
        };
        appiumOptions.AddAdditionalAppiumOption("appPackage", root["appPackage"]);
        appiumOptions.AddAdditionalAppiumOption("appActivity", root["appActivity"]);
        appiumOptions.AddAdditionalAppiumOption("noReset", "true");
        return appiumOptions;

    }


    public static void StartServer()
    {
        var args = new OptionCollector()
        .AddArguments(
            new KeyValuePair<string, string>("–base-path", "/wd/hub"));

        _appiumLocalService = new AppiumServiceBuilder()
        .WithIPAddress("127.0.0.1")
        .UsingPort(4723)
        .UsingAnyFreePort()
        // .WithArguments(args)
        .Build();
        _appiumLocalService.Start();
    }

    public static void StopAppiumServer()

    {
        _appiumLocalService?.Dispose();
    }

}
