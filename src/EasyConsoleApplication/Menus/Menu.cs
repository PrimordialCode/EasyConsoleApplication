﻿using System.Collections.Generic;

namespace EasyConsoleApplication.Menus
{
    public class Menu
    {
        public string Title { get; set; }

        public List<MenuItem> Items { get; set; } = new List<MenuItem>();

        public Menu(string title)
        {
            Title = title;
        }
    }
}