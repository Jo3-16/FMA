﻿using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Automation;
using FMA.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace FMA.UnitTests.UITests
{
    [TestClass]
    public class DefaultViewLogoTest : TestBase
    {

        [TestMethod]
        public void AddLogo_CreateFlyer_CreatesFlyerJpg()
        {
            if (File.Exists(FlyerTestAppSettings.TestappflyerJpg))
            {
                File.Delete(FlyerTestAppSettings.TestappflyerJpg);
            }

            AddLogo();

            var create = MainWindow.Get<Button>(SearchCriteria.ByAutomationId("Create"));

            create.Click();

            Assert.IsTrue(File.Exists(FlyerTestAppSettings.TestappflyerJpg));
        }

        [TestMethod]
        public void PerDefault_LogoWidth_AndDeleteLogo_AreNotVisible()
        {
            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);
        }

        [TestMethod]
        public void AddLogo_LogoWidthIsVisibleAndHasCorrectValue()
        {
            AddLogo();

            var logoWidth = MainWindow.Get(SearchCriteria.ByAutomationId("LogoWidth"));
            Assert.IsTrue(logoWidth.Visible);

            var logoWidthValue = GetValueFromNumericUpDown(logoWidth);
            Assert.AreEqual(180, logoWidthValue);
        }

        [TestMethod]
        public void PerDefault_LogoImageWidth_IsZero()
        {
            var logoImage = MainWindow.Get<Image>(SearchCriteria.ByAutomationId("CanvasLogoImage"));

            Assert.IsTrue(logoImage.Bounds.Width < 0);
        }

        [TestMethod]
        public void AddLogo_LogoImageWidth_IsPositive()
        {
            AddLogo();

            var logoImage = MainWindow.Get<Image>(SearchCriteria.ByAutomationId("CanvasLogoImage"));

            Assert.IsTrue(logoImage.Bounds.Width > 0);
        }

        [TestMethod]
        public void AddLogo_IncreasWidthViaNumericUpdown_ImageWidthIsIncreased()
        {
            AddLogo();

            var logoImage = MainWindow.Get<Image>(SearchCriteria.ByAutomationId("CanvasLogoImage"));

            var oldWidth = logoImage.Bounds.Width;

            var logoWidth = MainWindow.Get(SearchCriteria.ByAutomationId("LogoWidth"));

            var logoWidthValue = GetValueFromNumericUpDown(logoWidth);
            SetValueToNumericUpDown(logoWidth, logoWidthValue + 20);


            Assert.IsTrue(logoImage.Bounds.Width > oldWidth);
        }

        [TestMethod]
        public void AddLogo_Drag_ChangesPosition()
        {
            AddLogo();

            var logoLeftMargin = MainWindow.Get(SearchCriteria.ByAutomationId("LogoLeftMargin"));
            Assert.IsTrue(logoLeftMargin.Visible);
            var oldLogoLeftMarginValue = GetValueFromNumericUpDown(logoLeftMargin);

            var logoImage = MainWindow.Get<Image>(SearchCriteria.ByAutomationId("CanvasLogoImage"));
            logoImage.Click();
            Mouse.Instance.DragHorizontally(logoImage, 20);

            var newLogoLeftMarginValue = GetValueFromNumericUpDown(logoLeftMargin);

            Assert.IsTrue(newLogoLeftMarginValue > oldLogoLeftMarginValue);
        }

        [TestMethod]
        public void AddLogo_AndChangeLogo_LogoWidthIsVisibleAndHasCorrectValue()
        {
            AddLogo();

            var logoWidth = MainWindow.Get(SearchCriteria.ByAutomationId("LogoWidth"));
            Assert.IsTrue(logoWidth.Visible);

            var logoWidthValue = GetValueFromNumericUpDown(logoWidth);
            Assert.AreEqual(180, logoWidthValue);

            AddLogo("bigLogo.png");

            logoWidthValue = GetValueFromNumericUpDown(logoWidth);
            Assert.AreEqual(216, logoWidthValue);
        }


        [TestMethod]
        public void AddLogo_AndReset_LogoWidthIsNotVisible()
        {
            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);

            AddLogo();

            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(true);

            ClickButton("Reset");

            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);
        }

        [TestMethod]
        public void AddLogo_AndDelete_LogoWidthIsNotVisible()
        {
            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);

            AddLogo();

            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(true);

            ClickButton("DeleteLogo");

            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);
        }

        [TestMethod]
        public void AddLogo_AndChangeMaterial_LogoWidthIsNotVisible()
        {
            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);

            AddLogo();

            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(true);

            ChangeMaterial(DummyData.GetNotDefaultSelectedMaterial());

            LogoImage_LogoWidth_AndDeleteLogo_AreVisible(false);
        }


        private void AddLogo(string logoFile = "smallLogo.png")
        {
            logoFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Logos\" + logoFile;

            ClickButton("AddLogo");

            var lastwindow = Application.GetWindows().Last();
            //    lastwindow  =application.GetWindow(SearchCriteria.All.("Select logo file"), InitializeOption.WithCache);

            //nicht schön
            string fileNameText;
            string openButtonText;
            if (Equals(CultureInfo.CurrentUICulture, new CultureInfo("en-Us")))
            {
                fileNameText = "File name:";
                openButtonText = "Open";
            }
            else
            {
                fileNameText = "Dateiname:";
                openButtonText = "Öffnen";
            }


            var textField = lastwindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndByText(fileNameText));
            textField.Text = logoFile;
            var openButton = lastwindow.Get<Button>(SearchCriteria.ByText(openButtonText));
            openButton.Click();
        }

        private void LogoImage_LogoWidth_AndDeleteLogo_AreVisible(bool visible)
        {
            var logoWidth = MainWindow.Get(SearchCriteria.ByAutomationId("LogoWidth"));
            Assert.AreEqual(visible, logoWidth.Visible);

            var deleteLogo = MainWindow.Get<Button>(SearchCriteria.ByAutomationId("DeleteLogo"));
            Assert.AreEqual(visible, deleteLogo.Visible);

            var logoImage = MainWindow.Get<Image>(SearchCriteria.ByAutomationId("CanvasLogoImage"));

            Assert.AreEqual(visible, logoImage.Bounds.Width > 0);
        }

    }
}
