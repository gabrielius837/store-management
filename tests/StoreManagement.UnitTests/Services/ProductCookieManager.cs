namespace StoreManagement.WebApp;

public class ProductCookieManager_Tests
{
    [Fact]
    public void ParseProductHistoryCookie_MustParseItAsExpected()
    {
        // arrange
        var manager = new ProductCookieManager();
        var context = new Mock<HttpContext>();
        var request = new Mock<HttpRequest>();
        var cookies = new Mock<IRequestCookieCollection>();
        var principal = new Mock<ClaimsPrincipal>();
        const string name = "test";
        int[] expected = { 1, 2, 3 };
        var cookieValue = string.Join('-', expected);
        var claim = new Claim(ClaimTypes.Name, name);
        principal.SetupGet(x => x.Claims).Returns(new Claim[] { claim });
        context.SetupGet(x => x.User).Returns(principal.Object);
        const string cookie = $"{ProductCookieManager.PRODUCT_HISTORY}-{name}";
        cookies.SetupGet(x => x[cookie]).Returns(cookieValue);
        request.SetupGet(x => x.Cookies).Returns(cookies.Object);
        context.SetupGet(x => x.Request).Returns(request.Object);

        // act
        var result = manager.ParseProductHistoryCookie(context.Object);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpdateProductHistoryCookie_MustAddNewIdAndDiscordOldest()
    {
        // arrange
        var manager = new ProductCookieManager();
        var context = new Mock<HttpContext>();
        var request = new Mock<HttpRequest>();
        var cookies = new Mock<IRequestCookieCollection>();
        var principal = new Mock<ClaimsPrincipal>();
        var flag = false;
        var response = new Mock<HttpResponse>();
        var respCookies = new Mock<IResponseCookies>();
        const string name = "test";
        int[] initial = { 1, 2, 3 };
        int[] expected = { 2, 3, 4 };
        var cookieValue = string.Join('-', initial);
        var claim = new Claim(ClaimTypes.Name, name);
        principal.SetupGet(x => x.Claims).Returns(new Claim[] { claim });
        context.SetupGet(x => x.User).Returns(principal.Object);
        const string cookie = $"{ProductCookieManager.PRODUCT_HISTORY}-{name}";
        cookies.SetupGet(x => x[cookie]).Returns(cookieValue);
        request.SetupGet(x => x.Cookies).Returns(cookies.Object);
        context.SetupGet(x => x.Request).Returns(request.Object);
        respCookies.Setup(x => x.Append(cookie, string.Join('-', expected), It.IsAny<CookieOptions>())).Callback(() => flag = true);
        response.SetupGet(x => x.Cookies).Returns(respCookies.Object);
        context.SetupGet(x => x.Response).Returns(response.Object);

        // act
        var result = manager.UpdateProductHistoryCookie(context.Object, 4);

        // assert
        Assert.True(flag);
        Assert.Equal(expected, result);
    }
}