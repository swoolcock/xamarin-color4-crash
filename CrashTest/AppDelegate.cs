using Foundation;
using osuTK.Graphics;
using UIKit;
using System.Linq;

namespace CrashTest
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var caller = new Caller<Color4>();
            caller.Call(Color4.Red, Color4.Blue, 0);
            return true;
        }
    }

    class Caller<TValue>
    {
        public CallFunc<TValue> Call = Reflector<TValue>.Call;
    }

    public static class Reflector<TValue>
    {
        private static readonly CallFunc<TValue> call_func;

        static Reflector()
        {
            call_func =
                (CallFunc<TValue>)typeof(Implementation).GetMethod(
                    nameof(Implementation.Call),
                    typeof(CallFunc<TValue>)
                        .GetMethod(nameof(CallFunc<TValue>.Invoke))
                        ?.GetParameters().Select(p => p.ParameterType).ToArray()
                )?.CreateDelegate(typeof(CallFunc<TValue>));
        }

        public static void Call(TValue val1, TValue val2, double dbl) => call_func(val1, val2, dbl);
    }

    public static class Implementation
    {
        public static void Call(Color4 color1, Color4 color2, double dbl) { }
    }

    public delegate void CallFunc<TValue>(TValue value1, TValue value2, double dbl);
}

