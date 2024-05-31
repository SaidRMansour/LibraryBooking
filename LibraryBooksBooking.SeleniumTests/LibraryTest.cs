using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using Xunit; // try to do it in using.cs file (GLOBAL) look at henrik video
namespace LibraryBooksBooking.SeleniumTests;

[TestCaseOrderer("TestCaseOrderer", "LibraryBooksBooking.SeleniumTests")] 
public class LibraryTest : IDisposable
{
    private readonly IWebDriver driver;
    private readonly string bookingUrl = @"http://localhost:5238/";
    private readonly string bookUrl = @"http://localhost:5238/Book";
    private readonly string customerUrl = @"http://localhost:5238/Customer";

    public LibraryTest()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("headless");
        driver = new ChromeDriver(Environment.CurrentDirectory, options);
    }

    // Generating random ISBN
    private string GenerateISBN()
    {
        Random random = new Random();
        char[] digits = new char[10];

        for (int i = 0; i < 10; i++)
        {
            digits[i] = (char)('0' + random.Next(10));
        }

        return new string(digits);
    }

    public void Dispose()
    {
        driver.Quit();
    }

    // Test
    [Fact, TestOrder(1)]
    public void CreateCustomer()
    {
        driver.Navigate().GoToUrl(customerUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Customer", pageTitle);

        // Fill the formular
        driver.FindElement(By.Name("Name")).SendKeys("John Doe");
        driver.FindElement(By.Name("Email")).SendKeys("john.doe@example.com");
        driver.FindElement(By.Name("PhoneNumber")).SendKeys("123456789");

        // Submit
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var confirmedPageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Customers", confirmedPageTitle);
    }

    [Fact, TestOrder(2)]
    public void CreateBook_Failure()
    {
        driver.Navigate().GoToUrl(bookUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Book", pageTitle);
       
        // Fill the formular
        driver.FindElement(By.Name("Title")).SendKeys("Our new investigation");
        driver.FindElement(By.Name("Author")).SendKeys("John Doe");
        driver.FindElement(By.Name("ISBN")).SendKeys(GenerateISBN());
        driver.FindElement(By.Name("Genre")).SendKeys("Action");

        // Set the published date using JavaScript
        var publishedDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('PublishedDate')[0].value='{publishedDate}'");

        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var errorUrl = driver.Url;

        // Assert
        Assert.Contains("Error", errorUrl);
    }

    [Fact, TestOrder(3)]
    public void CreateBook_Completed()
    {
        driver.Navigate().GoToUrl(bookUrl);

        // Click the 'Create New' button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Book", pageTitle);

        // Fill the form
        driver.FindElement(By.Name("Title")).SendKeys("Our new investigation");
        driver.FindElement(By.Name("Author")).SendKeys("Hermann Koel");
        driver.FindElement(By.Name("ISBN")).SendKeys(GenerateISBN());
        driver.FindElement(By.Name("Genre")).SendKeys("Action");

        // Set the published date using JavaScript
        var publishedDate = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('PublishedDate')[0].value='{publishedDate}'");

        // Submit the form
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        var currentUrl = driver.Url;
        Assert.Contains("Book", currentUrl);
    }

    [Fact, TestOrder(4)]
    public void CreateBooking_StartDateInPast()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Booking", pageTitle);


        // Fill the form
        // Set the dates using JavaScript
        var bookingDate = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");

        var returnDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
        IJavaScriptExecutor jsReturn = (IJavaScriptExecutor)driver;
        jsReturn.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

       // Select first element in dropdown
        var customerDropdown = new SelectElement(driver.FindElement(By.Name("CustomerGuid")));
        customerDropdown.SelectByIndex(0);

        var bookDropdown = new SelectElement(driver.FindElement(By.Name("BookGuid")));
        bookDropdown.SelectByIndex(0);

        // Submit and navigate to Error page
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var errorUrl = driver.Url;

        // Assert
        Assert.Contains("Error", errorUrl);
    }

    [Fact, TestOrder(5)]
    public void CreateBooking_ReturnDateBeforeStartDate()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Booking", pageTitle);


        // Fill the formular
        // Set the dates using JavaScript
        var bookingDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");

        var returnDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
        IJavaScriptExecutor jsReturn = (IJavaScriptExecutor)driver;
        jsReturn.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

        // Select first element in dropdown
        var customerDropdown = new SelectElement(driver.FindElement(By.Name("CustomerGuid")));
        customerDropdown.SelectByIndex(0);

        var bookDropdown = new SelectElement(driver.FindElement(By.Name("BookGuid")));
        bookDropdown.SelectByIndex(0);

        // Submit and navigate to Error page
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var errorUrl = driver.Url;

        // Assert
        Assert.Contains("Error", errorUrl);
    }

    [Fact, TestOrder(6)]
    public void CreateBooking_Completed()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Booking", pageTitle);


        // Fill the formular
        // Set the dates using JavaScript
        var bookingDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");

        var returnDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        IJavaScriptExecutor jsReturn = (IJavaScriptExecutor)driver;
        jsReturn.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

        // Select first element in dropdown
        var customerDropdown = new SelectElement(driver.FindElement(By.Name("CustomerGuid")));
        customerDropdown.SelectByIndex(0);

        var bookDropdown = new SelectElement(driver.FindElement(By.Name("BookGuid")));
        bookDropdown.SelectByIndex(0);

        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var currentUrl = driver.Url;

        // Assert
        Assert.Equal("http://localhost:5238/", currentUrl);
    }

    [Fact, TestOrder(7)]
    public void CreateBooking_AlreadyBooked()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Booking", pageTitle);


        // Fill the formular
        // Set the dates using JavaScript
        var bookingDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");

        var returnDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        IJavaScriptExecutor jsReturn = (IJavaScriptExecutor)driver;
        jsReturn.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

        // Select first element in dropdown
        var customerDropdown = new SelectElement(driver.FindElement(By.Name("CustomerGuid")));
        customerDropdown.SelectByIndex(0);

        var bookDropdown = new SelectElement(driver.FindElement(By.Name("BookGuid")));
        bookDropdown.SelectByIndex(0);

        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var errorUrl = driver.Url;

        // Assert
        Assert.Contains("Error", errorUrl);
    }

    [Fact, TestOrder(8)]
    public void UpdateBook_Failure()
    {
        driver.Navigate().GoToUrl(bookUrl);

        var bookRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        var bookRow = bookRows.FirstOrDefault(b => b.Text.Contains("Our new investigation"));

        bookRow.FindElement(By.LinkText("Edit")).Click();

        // Edit the form
        var titleField = driver.FindElement(By.Name("Title"));
        titleField.Clear();
        titleField.SendKeys("Edited Investigation");

        var authorField = driver.FindElement(By.Name("Author"));
        authorField.Clear();
        authorField.SendKeys("Jane Doe");

        // Set the published date using JavaScript
        var publishedDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('PublishedDate')[0].value='{publishedDate}'");

        // Submit the form
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        var errorUrl = driver.Url;
        Assert.Contains("Error", errorUrl);
    }

    [Fact, TestOrder(9)]
    public void UpdateBook_Completed()
    {
        driver.Navigate().GoToUrl(bookUrl);

        var bookRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        var bookRow = bookRows.FirstOrDefault(b => b.Text.Contains("Our new investigation"));

        bookRow.FindElement(By.LinkText("Edit")).Click();

        // Edit the form
        var titleField = driver.FindElement(By.Name("Title"));
        titleField.Clear();
        titleField.SendKeys("Edited Investigation");

        var authorField = driver.FindElement(By.Name("Author"));
        authorField.Clear();
        authorField.SendKeys("Jane Doe");

        // Set the published date using JavaScript
        var publishedDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('PublishedDate')[0].value='{publishedDate}'");

        // Submit the form
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        var currentUrl = driver.Url;
        Assert.Contains("Book", currentUrl);
    }

    [Fact, TestOrder(10)]
    public void UpdateBooking_StartDateInPast()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        var bookingRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        var bookingRow = bookingRows.FirstOrDefault(b => b.Text.Contains(""));

        bookingRow.FindElement(By.LinkText("Edit")).Click();

        // Edit the form

        // Set the published date using JavaScript
        var bookingDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");
        // Set the published date using JavaScript

        var returnDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
        js.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

        // Submit the form
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        var errorUrl = driver.Url;
        Assert.Contains("Error", errorUrl);
    }

    [Fact, TestOrder(11)]
    public void UpdateBooking_Completed()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        var bookingRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        var bookingRow = bookingRows.FirstOrDefault(b => b.Text.Contains(""));

        bookingRow.FindElement(By.LinkText("Edit")).Click();

        // Edit the form

        // Set the published date using JavaScript
        var bookingDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");
        // Set the published date using JavaScript

        var returnDate = DateTime.Now.AddDays(10).ToString("yyyy-MM-dd");
        js.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

        // Submit the form
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        var currentUrl = driver.Url;
        Assert.Equal("http://localhost:5238/", currentUrl);
    }

    [Fact, TestOrder(12)]
    public void DeleteBooking()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        var bookingRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        var bookingRow = bookingRows.FirstOrDefault(b => b.Text.Contains(""));

        bookingRow.FindElement(By.LinkText("Delete")).Click();


        // Submit the delete action
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        var currentUrl = driver.Url;
        Assert.Equal("http://localhost:5238/", currentUrl);
    }

    [Fact, TestOrder(13)]
    public void CreateBooking_Completed_AnotherObjectToShow()
    {
        driver.Navigate().GoToUrl(bookingUrl);

        // Clicking 'Create new'-button
        driver.FindElement(By.LinkText("Create New")).Click();

        var pageTitle = driver.FindElement(By.TagName("h2")).Text;
        Assert.Contains("Create Booking", pageTitle);


        // Fill the formular
        // Set the dates using JavaScript
        var bookingDate = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd");
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"document.getElementsByName('BookingDate')[0].value='{bookingDate}'");

        var returnDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
        IJavaScriptExecutor jsReturn = (IJavaScriptExecutor)driver;
        jsReturn.ExecuteScript($"document.getElementsByName('ReturnDate')[0].value='{returnDate}'");

        // Select first element in dropdown
        var customerDropdown = new SelectElement(driver.FindElement(By.Name("CustomerGuid")));
        customerDropdown.SelectByIndex(0);

        var bookDropdown = new SelectElement(driver.FindElement(By.Name("BookGuid")));
        bookDropdown.SelectByIndex(0);

        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        var currentUrl = driver.Url;

        // Assert
        Assert.Equal("http://localhost:5238/", currentUrl);
    }
}
