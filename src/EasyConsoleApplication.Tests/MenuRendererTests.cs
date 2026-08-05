using EasyConsoleApplication.Menus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyConsoleApplication.Tests;

[TestClass]
[DoNotParallelize]
public sealed class MenuRendererTests
{
	[TestMethod]
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

	[TestMethod]
	public void Render_stops_when_input_reaches_eof()
	{
		var originalOut = Console.Out;
		var originalIn = Console.In;
		try
		{
			Console.SetOut(new StringWriter());
			Console.SetIn(new StringReader(string.Empty));

			var menu = new Menu("Test");
			menu.Items.Add(new MenuItem("Quit", Application.Exit));

			Application.Render(menu);
		}
		finally
		{
			Console.SetOut(originalOut);
			Console.SetIn(originalIn);
		}
	}

	[TestMethod]
	public void Key_prompts_do_not_require_interactive_input()
	{
		var originalOut = Console.Out;
		var originalIn = Console.In;
		try
		{
			Console.SetOut(new StringWriter());
			Console.SetIn(new StringReader("invalid\ny\n"));

			var answer = ConsoleHelpers.AskToUserYesNoQuestion(ConsoleColor.Gray, "Continue?");
			ConsoleHelpers.HitEnterToContinue();

			Assert.AreEqual(ConsoleKey.Y, answer.Key);
		}
		finally
		{
			Console.SetOut(originalOut);
			Console.SetIn(originalIn);
		}
	}
}