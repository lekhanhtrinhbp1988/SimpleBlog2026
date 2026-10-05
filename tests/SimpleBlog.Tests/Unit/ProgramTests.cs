namespace SimpleBlog.Tests.Unit;

public class ProgramTests
{
    [Fact]
    public void Program_IsPublic()
    {
        Assert.True(typeof(Program).IsPublic);
    }
}
