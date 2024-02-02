using TechTalk.SpecFlow;
using Frais.Pages;
using System;
using NUnit.Framework;

namespace Frais.Steps
{
    [Binding]
    public sealed class HomePageStepDef
    {
        HomePage objHomePage = new HomePage();

        [Given("I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            Console.WriteLine("Inside Given steo");
            //   objHomePage.LaunchApp();

        }

        [Then("print home page message")]
        public void ThenPrintHomePageMessage()
        {
            Console.WriteLine("Inside Then step");

        }
        
        [Then("forcefully fail this step")]
        public void ThenForcefullyFailThisStep()
        {
            Assert.Fail("Failed forcefully");

        }

    }
}