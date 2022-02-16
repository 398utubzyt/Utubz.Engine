using Utubz.Internal.Discord;

namespace Utubz.Discord
{
    public static class Status
    {
        private static Internal.Discord.Discord ctx;
        private static Activity act;
        private static bool init;
        private static bool attempt;

        public static void Initialize(long application)
        {
            if (attempt)
                return;

            attempt = true;
            try
            {
                ctx = new Internal.Discord.Discord(application, 1);
                init = true;
            } catch (ResultException)
            {
                init = false;
            }
        }

        internal static void Quit()
        {
            if (!init)
                return;

            init = false;
            ctx.Dispose();
        }

        internal static void Run()
        {
            if (!init)
                return;

            ctx.RunCallbacks();
        }

        private static void UpdateActivity()
        {
            if (!init)
                return;

            ctx.GetActivityManager().UpdateActivity(act, Check);
        }

        private static void Check(Result result)
        {
            if (result != Result.Ok)
                throw new ResultException(result);
        }

        public static string Name { get => act.Name; }
        public static string State { get => act.State; set { act.State = value; UpdateActivity(); } }
        public static string Details { get => act.Details; set { act.Details = value; UpdateActivity(); } }
        public static string LargeImage { get => act.Assets.LargeImage; set { act.Assets.LargeImage = value; UpdateActivity(); } }
        public static string LargeText { get => act.Assets.LargeText; set { act.Assets.LargeText = value; UpdateActivity(); } }
        public static string SmallImage { get => act.Assets.SmallImage; set { act.Assets.SmallImage = value; UpdateActivity(); } }
        public static string SmallText { get => act.Assets.SmallText; set { act.Assets.SmallText = value; UpdateActivity(); } }
    }
}
