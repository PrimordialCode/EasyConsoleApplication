using System.Globalization;
using EasyConsoleApplication.Menus;

namespace EasyConsoleApplication.Rendering
{
	internal sealed class MenuRenderer
	{
		/// <summary>
		/// variable that will be check to know if it's time
		/// to terminate the loop that keep asking for new commands
		/// from this menu
		/// </summary>
		internal bool _shouldTerminate;

		/// <summary>
		/// Call exit whenever you want to terminate the loop that
		/// keep asking for new commands from this menu
		/// </summary>
		public void Exit()
		{
			_shouldTerminate = true;
		}

		public void Render(
			string? title,
			ConsoleColor titleColor,
			string? body,
			ConsoleColor bodyColor,
			Menu menu)
		{
			var menuItems = menu.Items;
			var menuActions = menuItems
				.Where(mi => mi.GetType() == typeof(MenuItem))
				.Cast<MenuItem>()
				.ToList();
			while (true)
			{
				ClearConsole();
				if (!string.IsNullOrWhiteSpace(title))
				{
					ConsoleHelpers.Write(titleColor, title!, true);
				}
				if (!string.IsNullOrWhiteSpace(body))
				{
					ConsoleHelpers.Write(bodyColor, body!, true);
				}
				if (menuItems != null)
				{
					RenderMenuItems(menuItems);

					Console.WriteLine();
					var value = ConsoleHelpers.ReadLine(Console.ForegroundColor, "Select an option: ");

					if (value is null)
					{
						// Closed or redirected stdin has reached EOF; avoid waiting forever for another option.
						break;
					}

					if (!string.IsNullOrWhiteSpace(value))
					{
						// check the string commands first
						var selectedMi = menuActions.Find(m => m.Command == value);
						if (selectedMi != null)
						{
							ExecuteAction(selectedMi);
						}
						else
						{
							// use the positional value (only if the Command property does not have a value).
							if (int.TryParse(value, out int option))
							{
								var selectedIdx = option - 1;
								if (selectedIdx > -1 && selectedIdx < menuActions.Count)
								{
									var mi = menuActions[selectedIdx];
									if (string.IsNullOrWhiteSpace(mi.Command))
									{
										ExecuteAction(mi);
									}
								}
							}
						}
					}

					if (_shouldTerminate)
					{
						break;
					}
				}
			}
		}

		private static void ClearConsole()
		{
			if (Console.IsOutputRedirected)
			{
				// Console.Clear requires an interactive output buffer.
				return;
			}

			try
			{
				Console.Clear();
			}
			catch (IOException)
			{
				// The host can expose output without a usable console buffer.
			}
			catch (PlatformNotSupportedException)
			{
				// Some hosts do not support clearing their output stream.
			}
			catch (InvalidOperationException)
			{
				// The console can be present but not currently have a usable buffer.
			}
		}

		private static void RenderMenuItems(List<IMenuItem> menuItems)
		{
			int commandIndex = 0;
			for (int idx = 0; idx < menuItems.Count; idx++)
			{
				switch (menuItems[idx])
				{
					case MenuItem mi:
						commandIndex++;
						var cmd = mi.Command;
						if (string.IsNullOrWhiteSpace(cmd))
						{
							cmd = commandIndex.ToString(CultureInfo.InvariantCulture);
						}
						ConsoleHelpers.Write(mi.Color, $" {cmd} - {mi.Title}", true);
						break;
					case Separator sep:
						ConsoleHelpers.Write(sep.Color, $" {sep.Title}", true);
						break;
				}
			}
		}

		/// <summary>
		/// Execute the command and decide if its time to terminate the
		/// command loop
		/// </summary>
		/// <returns>
		/// true = terminate the loop
		/// false = keep asking for another command
		/// </returns>
		private void ExecuteAction(MenuItem mi)
		{
			mi.Action?.Invoke();
			mi.ActionAsync?.Invoke().Wait();

			if (!_shouldTerminate)
			{
				ConsoleHelpers.HitEnterToContinue();
			}
		}
	}
}
