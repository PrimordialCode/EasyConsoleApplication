using System;

namespace EasyConsoleApplication
{
	/// <summary>
	/// A helper class for console operations.
	/// </summary>
	public static class ConsoleHelpers
	{
		/// <summary>
		/// Writes a message to the console with the specified color.
		/// </summary>
		/// <param name="color">The color of the message.</param>
		/// <param name="message">The message to write.</param>
		/// <param name="newLine">Indicates whether to append a new line after the message. Default is true.</param>
		public static void Write(ConsoleColor color, string message, bool newLine = true)
		{
			ConsoleColor? originalConsoleColor = null;
			try
			{
				originalConsoleColor = Console.ForegroundColor;
				Console.ForegroundColor = color;
			}
			catch (IOException)
			{
				// Continue writing when the host has no usable console color buffer.
			}
			catch (InvalidOperationException)
			{
				// Continue writing when the console is unavailable.
			}

			if (newLine)
			{
				Console.WriteLine(message);
			}
			else
			{
				Console.Write(message);
			}
			if (originalConsoleColor.HasValue)
			{
				try
				{
					Console.ForegroundColor = originalConsoleColor.Value;
				}
				catch (IOException)
				{
					// The console may become unavailable while writing the message.
				}
				catch (InvalidOperationException)
				{
					// The console may become unavailable while writing the message.
				}
			}
		}

		/// <summary>
		/// Writes a message to the console with green color.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="newLine">Indicates whether to append a new line after the message. Default is true.</param>
		public static void WriteGreen(string message, bool newLine = true)
		{
			Write(ConsoleColor.Green, message, newLine);
		}

		/// <summary>
		/// Writes a message to the console with yellow color.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="newLine">Indicates whether to append a new line after the message. Default is true.</param>
		public static void WriteYellow(string message, bool newLine = true)
		{
			Write(ConsoleColor.Yellow, message, newLine);
		}

		/// <summary>
		/// Writes a message to the console with red color.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="newLine">Indicates whether to append a new line after the message. Default is true.</param>
		public static void WriteRed(string message, bool newLine = true)
		{
			Write(ConsoleColor.Red, message, newLine);
		}

		/// <summary>
		/// Asks the user a yes/no question and returns the key info.
		/// </summary>
		/// <param name="color">The color of the question.</param>
		/// <param name="question">The question to ask.</param>
		/// <param name="newLine">Indicates whether to append a new line after the question. Default is true.</param>
		/// <returns>The key info representing the user's response.</returns>
		public static ConsoleKeyInfo AskToUserYesNoQuestion(ConsoleColor color, string question, bool newLine = true)
		{
			Write(color, question + " [y/n] ", false);
			if (Console.IsInputRedirected)
			{
				// ReadKey requires an interactive terminal, so consume redirected input as a line.
				while (true)
				{
					var input = Console.ReadLine();
					var keyChar = input?.FirstOrDefault(char.IsLetter) ?? ' ';
					if (keyChar is not ('y' or 'Y' or 'n' or 'N'))
					{
						if (input is null)
						{
							return new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
						}
						continue;
					}

					var redirectedKey = new ConsoleKeyInfo(keyChar, keyChar is 'y' or 'Y' ? ConsoleKey.Y : ConsoleKey.N, false, false, false);
					if (newLine)
					{
						Console.WriteLine();
					}
					return redirectedKey;
				}
			}

			var key = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
			while (key.KeyChar != 'y' && key.KeyChar != 'n')
			{
				key = Console.ReadKey();
			}
			if (newLine)
			{
				Console.WriteLine();
			}
			return key;
		}

		/// <summary>
		/// Reads a line of input from the console with the specified color.
		/// </summary>
		/// <param name="color">The color of the input message.</param>
		/// <param name="message">The input message to display.</param>
		/// <param name="defaultValue">The default value to return if the input is empty or whitespace. Default is null.</param>
		/// <returns>The input string or the default value if the input is empty or whitespace.</returns>
		public static string? ReadLine(ConsoleColor color, string message, string? defaultValue = null)
		{
			Write(color, message, false);
			var input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input))
			{
				return defaultValue;
			}
			return input;
		}

		/// <summary>
		/// Waits for the user to hit the 'Enter' key to continue.
		/// </summary>
		public static void HitEnterToContinue()
		{
			Write(ConsoleSettings.HitEnterToContinueColor, "Hit 'Enter' to continue.", newLine: true);
			if (Console.IsInputRedirected)
			{
				// There is no key event to wait for when stdin is redirected.
				return;
			}

			ConsoleKeyInfo key;
			do
			{
				key = Console.ReadKey(true);
			}
			while (key.Key != ConsoleKey.Enter);
		}
	}
}
