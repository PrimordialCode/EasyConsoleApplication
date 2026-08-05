using EasyConsoleApplication.Menus;
using Xunit;

namespace EasyConsoleApplication.Tests;

public sealed class MenuRendererTests
{
    [Fact]
    public void Render_does_not_clear_redirected_output()
    {
        var originalOut = Console.Out;
        var originalIn = Console.In;
        try
        {
            Console.SetOut(new StringWriter());
            Console.SetIn(new StringReader("quit"));

            var menu = new Menu("Test");
            menu.Items.Add(new MenuItem("quit", "Quit", Application.Exit));

            Application.Render(menu);
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetIn(originalIn);
        }
    }
}