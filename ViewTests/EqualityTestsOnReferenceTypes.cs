using View;
using ViewTests.Stubs;


namespace ViewTests;
 
public class EqualityTestsOnReferenceTypes
{
    [Fact]
    internal void IEquatable_ReturnsTrue_WhenViewsAreOf_TheSameLocation()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);
        var view02 = View<StubClass>.Of(ref a);

        Assert.True(view01.Equals(view02));
    }

    [Fact]
    internal void IEquatable_ReturnsFalse_WhenViewsAreOf_DifferentLocations()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);

        var b = new StubClass(5);
        var view02 = View<StubClass>.Of(ref b);

        Assert.False(view01.Equals(view02));
    }

    [Fact]
    internal void Views_ToSameLocation_Have_TheSameHashCode()
    {
        var a = new StubClass(5);

        var hash01 = View<StubClass>.Of(ref a).GetHashCode();
        var hash02 = View<StubClass>.Of(ref a).GetHashCode();

        Assert.Equal(hash01, hash02);
    }

    [Fact]
    internal void Views_ToDifferentLocations_Have_DifferentHashCodes()
    {
        var a = new StubClass(5);
        var hash01 = View<StubClass>.Of(ref a).GetHashCode();

        var b = new StubClass(5);
        var hash02 = View<StubClass>.Of(ref b).GetHashCode();

        Assert.NotEqual(hash01, hash02);
    }

    [Fact]
    internal void ClassicEquals_ReturnsTrue_WhenViewsAreOf_TheSameLocation()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);
        var view02 = View<StubClass>.Of(ref a) as object;

        Assert.True(view01.Equals(view02));
    }

    [Fact]
    internal void ClassicEquals_ReturnsFalse_WhenViewsAreOf_DifferentLocations()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);

        var b = new StubClass(5);
        var view02 = View<StubClass>.Of(ref b) as object;

        Assert.False(view01.Equals(view02));
    }

    [Fact]
    internal void ClassicEquals_ReturnsFalse_When_ArgProvided_IsNotOfType_View()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);

        var b = 5;

        Assert.False(view01.Equals(b));
    }

    [Fact]
    internal void EqualityOperator_ReturnsTrue_WhenViewsAreOf_TheSameLocation()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);
        var view02 = View<StubClass>.Of(ref a);

        Assert.True(view01 == view02);
    }

    [Fact]
    internal void EqualityOperator_ReturnsFalse_WhenViewsAreOf_DifferentLocations()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);

        var b = new StubClass(5);
        var view02 = View<StubClass>.Of(ref b);

        Assert.False(view01 == view02);
    }

    [Fact]
    internal void InequalityOperator_ReturnsFalse_WhenViewsAreOf_TheSameLocation()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);
        var view02 = View<StubClass>.Of(ref a);

        Assert.False(view01 != view02);
    }

    [Fact]
    internal void InequalityOperator_ReturnsTrue_WhenViewsAreOf_DifferentLocations()
    {
        var a = new StubClass(5);
        var view01 = View<StubClass>.Of(ref a);

        var b = new StubClass(5);
        var view02 = View<StubClass>.Of(ref b);

        Assert.True(view01 != view02);
    }
}