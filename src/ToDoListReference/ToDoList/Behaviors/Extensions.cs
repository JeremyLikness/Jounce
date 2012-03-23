using Jounce.Core.View;
using Jounce.Framework;

namespace ToDoList.Behaviors
{
    public static class Extensions
    {
            public static ViewNavigationArgs AsChildWindow(this ViewNavigationArgs args)
            {
                return args.AddNamedParameter(Global.Constants.PARM_WINDOW, true);
            }

        public static ViewNavigationArgs WindowWidth(this ViewNavigationArgs args, int width)
        {
            return args.AddNamedParameter(Global.Constants.PARM_WIDTH, width);
        }

        public static ViewNavigationArgs WindowHeight(this ViewNavigationArgs args, int height)
        {
            return args.AddNamedParameter(Global.Constants.PARM_HEIGHT, height);
        }

        public static ViewNavigationArgs WindowTitle(this ViewNavigationArgs args, string title)
        {
            return args.AddNamedParameter(Global.Constants.PARM_TITLE, title);
        }
    }
}