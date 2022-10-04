namespace StoreManagement.UnitTests;

public class RoleUserResolver_Tests
{
    [Fact]
    public async Task ResolveUser_MustBeManager()
    {
        // arrange
        var resolver = new RoleUserResolver();
        const string user = "administrator";
        const string role = "Manager";
        var roleUser = new RoleUser(role, user);

        // act
        var result = await resolver.ResolveUser(user);

        // assert
        Assert.Equal(user, result?.User);
        Assert.Equal(role, result?.Role);
    }

    [Fact]
    public async Task ResolveUser_MustBeCustomer()
    {
        // arrange
        var resolver = new RoleUserResolver();
        const string user = "user";
        const string role = "Customer";
        var roleUser = new RoleUser(role, user);

        // act
        var result = await resolver.ResolveUser(user);

        // assert
        Assert.Equal(user, result?.User);
        Assert.Equal(role, result?.Role);
    }

    [Fact]
    public async Task ResolveUser_MustBeNull()
    {
        // arrange
        var resolver = new RoleUserResolver();
        const string user = "random";
        const string role = "input";
        var roleUser = new RoleUser(role, user);

        // act
        var result = await resolver.ResolveUser(user);

        // assert
        Assert.Null(result);
    }
}