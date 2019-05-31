﻿using EasyConsoleApplication.Menus;
using EasyConsoleApplication.Pages;

namespace EasyConsoleApplication
{
    public static class Application
    {
        private static Rendering MenuRendering;

        public static void Render(Menu menu)
        {
            MenuRendering = new Rendering();
            MenuRendering.Render(menu.Title, "", menu);
        }

        private static readonly Router Router = new Router();

        public static void GoTo<TPage>(params object[] args) where TPage : Page
        {
            Router.GoTo<TPage>(args);
        }

        public static void GoBack()
        {
            Router.GoBack();
        }

        /// <summary>
        /// Call exit whenever you want to terminate the loop that
        /// keep asking for new commands from this menu
        /// </summary>
        public static void Exit()
        {
            MenuRendering?.Exit();
            Router.Exit();
        }
    }
}
