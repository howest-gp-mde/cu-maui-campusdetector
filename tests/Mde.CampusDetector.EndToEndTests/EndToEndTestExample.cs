/// This is a sample end-to-end test using Appium.
/// To run this, you require the following:
/// 
/// - Appium installed on your machine (see https://appium.io/ or https://appium.io/docs/en/latest/quickstart/)
/// - Android emulator running with the app be.howest.mde.campusdetector deployed
/// - Appium running:
/// 
///    - you can start Appium with the command `appium --relaxed-security`
///    - in Windows powershell, you can start Appium with the command:
///        env:ANDROID_HOME='C:\Program Files (x86)\Android\android-sdk'; appium.cmd --relaxed-security
///      
///      ensure to set the ANDROID_HOME variable with the correct path to your Android SDK
/// using OpenQA.Selenium;

using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace Mde.CampusDetector.EndToEndTests
{
    public class EndToEndTests : IDisposable
    {
        private AndroidDriver _driver;

        private const string AppPackageName = "be.howest.mde.campusdetector";

        public EndToEndTests()
        {
            var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
            var driverOptions = new AppiumOptions()
            {
                AutomationName = AutomationName.AndroidUIAutomator2,
                PlatformName = "Android",
                DeviceName = "Android Emulator",
            };

            driverOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, AppPackageName);
            driverOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, $"{AppPackageName}.MainActivity");

            // NoReset assumes the app be.howest.mde.campusdetector is preinstalled on the emulator
            driverOptions.AddAdditionalAppiumOption("noReset", true);

            _driver = new AndroidDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }

        public void Dispose()
        {
            _driver.Dispose();
        }

        [Fact]
        public async void When_UserAllowsInsufficientPermissions_Then_DistanceToSelectedCampus_IsZeroMeter()
        {
            // Arrange
            string expectedDistanceText = "0 m";
            _driver.TerminateApp(AppPackageName);

            //revoke permission to trigger permission dialog
            var command = new Dictionary<string, object>
            {
                { "command", "pm" },
                { "args", $"revoke {AppPackageName} android.permission.ACCESS_FINE_LOCATION" }
            };
            _driver.ExecuteScript("mobile: shell", command);

            _driver.ActivateApp(AppPackageName);

            // Act

            //focus permission dialog and select least permissive option
            _driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);
            _driver.PressKeyCode(AndroidKeyCode.Keycode_TAB);
            _driver.PressKeyCode(AndroidKeyCode.Keycode_TAB);
            _driver.PressKeyCode(AndroidKeyCode.Keycode_TAB);
            _driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);

            //close alert
            await Task.Delay(50);
            _driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);

            var picker = _driver.FindElement(By.Id("campusPicker"));

            //open picker                       
            picker.Click();
            await Task.Delay(50);


            //highlight second element
            _driver.PressKeyCode(AndroidKeyCode.Keycode_DPAD_DOWN);
            _driver.PressKeyCode(AndroidKeyCode.Keycode_DPAD_DOWN);
            await Task.Delay(50);


            //select second element
            _driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);

            //get distance label
            var distance = _driver.FindElement(By.Id("campusDistance"));
            string actualDistanceText = distance.GetAttribute("Text");

            // Assert
            Assert.Equal(expectedDistanceText, actualDistanceText);
        }
    }
}
