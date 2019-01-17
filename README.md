Xamarin Crash Example
======

Workaround PR: https://github.com/ppy/osu-framework/pull/2088

Context: https://github.com/ppy/osu-framework/blob/master/osu.Framework/MathUtils/Interpolation.cs

`Color4`: https://github.com/ppy/osuTK/blob/netstandard/src/osuTK/Graphics/Color4.cs

Minimal example of a crash caused when using `CreateDelegate` with particular generic method signatures, where the selected generic type is osuTK's `Color4` struct.

Crashes seem to occur when the method signature has at least two generic arguments and ends in a floating point type.  The return type does not seem to make a difference.

Signatures that crash:
* `(TValue, TValue, double)`
* `(TValue, TValue, float)`
* `(double, double, double, double, TValue, TValue, double)`

Signatures that work:
* `(TValue)`
* `(TValue, double)`
* `(TValue, TValue)`
* `(TValue, TValue, int)`
* `(TValue, TValue, long)`
* `(double, TValue)`
* `(double, TValue, TValue)`
* `(double, double, double, double, TValue, TValue)`

Directly calling `Reflector<Color4>.Call` will not crash, it is only when passing through the `Caller<TValue>` class.

I have not been able to replicate this issue with any type other than `Color4`.

Running on macOS Mojave 10.14.  This example uses Xamarin 12.2.1.12, but newer versions still exhibit the same problem.

```
2019-01-17 22:04:38.308 CrashTest[2689:10844184] Xamarin.iOS: Skipping assembly registration for CrashTest since it's not needed (dynamic registration is not supported)
2019-01-17 22:04:41.925 CrashTest[2689:10844184] error: * Assertion: should not be reached at ../../../../../mono/mini/mini-arm64-gsharedvt.c:119
2019-01-17 22:04:41.926 CrashTest[2689:10844184] critical: Stacktrace:

2019-01-17 22:04:41.926 CrashTest[2689:10844184] critical:   at <unknown> <0xffffffff>
2019-01-17 22:04:41.926 CrashTest[2689:10844184] critical:   at CrashTest.AppDelegate.FinishedLaunching (UIKit.UIApplication,Foundation.NSDictionary) [0x00007] in /Users/samah/Projects/CrashTest/CrashTest/AppDelegate.cs:16
2019-01-17 22:04:41.926 CrashTest[2689:10844184] critical:   at (wrapper runtime-invoke) object.runtime_invoke_dynamic (intptr,intptr,intptr,intptr) [0x0001e] in <01f05773bcda4d0b9642de54d8f3cec3#F040338C-D1D0-BBB8-2E98-6BF7995BFC45>:0
2019-01-17 22:04:41.927 CrashTest[2689:10844184] critical:   at <unknown> <0xffffffff>
2019-01-17 22:04:41.927 CrashTest[2689:10844184] critical:   at (wrapper managed-to-native) UIKit.UIApplication.UIApplicationMain (int,string[],intptr,intptr) <0x00007>
2019-01-17 22:04:41.927 CrashTest[2689:10844184] critical:   at UIKit.UIApplication.Main (string[],intptr,intptr) [0x00005] in /Library/Frameworks/Xamarin.iOS.framework/Versions/12.2.1.12/src/Xamarin.iOS/UIKit/UIApplication.cs:79
2019-01-17 22:04:41.927 CrashTest[2689:10844184] critical:   at UIKit.UIApplication.Main (string[],string,string) [0x0002c] in /Library/Frameworks/Xamarin.iOS.framework/Versions/12.2.1.12/src/Xamarin.iOS/UIKit/UIApplication.cs:63
2019-01-17 22:04:41.927 CrashTest[2689:10844184] critical:   at CrashTest.Application.Main (string[]) [0x00001] in /Users/samah/Projects/CrashTest/CrashTest/Main.cs:12
2019-01-17 22:04:41.928 CrashTest[2689:10844184] critical:   at (wrapper runtime-invoke) object.runtime_invoke_dynamic (intptr,intptr,intptr,intptr) [0x0001e] in <01f05773bcda4d0b9642de54d8f3cec3#F040338C-D1D0-BBB8-2E98-6BF7995BFC45>:0
2019-01-17 22:04:41.928 CrashTest[2689:10844184] critical: 
Native stacktrace:

2019-01-17 22:04:41.940 CrashTest[2689:10844184] critical: 	0   libmonosgen-2.0.dylib               0x0000000103490d18 mono_handle_native_crash + 224
2019-01-17 22:04:41.940 CrashTest[2689:10844184] critical: 	1   libsystem_platform.dylib            0x000000018139cb58 _sigtramp + 52
2019-01-17 22:04:41.940 CrashTest[2689:10844184] critical: 	2   libsystem_pthread.dylib             0x00000001813a2288 <redacted> + 376
2019-01-17 22:04:41.940 CrashTest[2689:10844184] critical: 	3   libsystem_c.dylib                   0x000000018116fd0c abort + 140
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	4   libxamarin-debug.dylib              0x0000000100544188 _ZL12log_callbackPKcS0_S0_iPv + 76
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	5   libmonosgen-2.0.dylib               0x000000010344da84 monoeg_g_logv + 100
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	6   libmonosgen-2.0.dylib               0x000000010344dca4 monoeg_assertion_message + 72
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	7   libmonosgen-2.0.dylib               0x0000000103489a68 get_arg_slots + 472
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	8   libmonosgen-2.0.dylib               0x00000001034891b0 mono_arch_get_gsharedvt_call_info + 248
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	9   libmonosgen-2.0.dylib               0x00000001034939e8 mini_get_gsharedvt_wrapper + 216
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	10  libmonosgen-2.0.dylib               0x00000001034a3f58 mini_add_method_trampoline + 408
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	11  libmonosgen-2.0.dylib               0x00000001034a5340 mono_delegate_trampoline + 1064
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	12  libmscorlib.dll.dylib               0x00000001007f712c generic_trampoline_delegate + 252
2019-01-17 22:04:41.941 CrashTest[2689:10844184] critical: 	13  libCrashTest.exe.dylib              0x0000000100305370 CrashTest_AppDelegate_FinishedLaunching_UIKit_UIApplication_Foundation_NSDictionary + 384
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	14  libmscorlib.dll.dylib               0x00000001007d94b8 wrapper_runtime_invoke_object_runtime_invoke_dynamic_intptr_intptr_intptr_intptr + 248
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	15  libmonosgen-2.0.dylib               0x00000001034a1bb4 mono_jit_runtime_invoke + 904
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	16  libmonosgen-2.0.dylib               0x00000001035279e0 mono_runtime_invoke_checked + 148
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	17  libmonosgen-2.0.dylib               0x000000010352b2f8 mono_runtime_invoke + 84
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	18  CrashTest                           0x00000001002e88e0 _ZL30native_to_managed_trampoline_7P11objc_objectP13objc_selectorPP11_MonoMethodP13UIApplicationP12NSDictionaryj + 688
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	19  CrashTest                           0x00000001002e8620 -[AppDelegate application:didFinishLaunchingWithOptions:] + 60
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	20  UIKit                               0x000000018b34ae38 <redacted> + 408
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	21  UIKit                               0x000000018b34a240 <redacted> + 3484
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	22  UIKit                               0x000000018b31765c <redacted> + 1680
2019-01-17 22:04:41.942 CrashTest[2689:10844184] critical: 	23  UIKit                               0x000000018b947a0c <redacted> + 784
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	24  UIKit                               0x000000018b316e4c <redacted> + 160
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	25  UIKit                               0x000000018b316ce8 <redacted> + 240
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	26  UIKit                               0x000000018b315b78 <redacted> + 724
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	27  UIKit                               0x000000018bfab72c <redacted> + 296
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	28  UIKit                               0x000000018b315268 <redacted> + 432
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	29  UIKit                               0x000000018bd909b8 <redacted> + 220
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	30  UIKit                               0x000000018bedeae8 _performActionsWithDelayForTransitionContext + 112
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	31  UIKit                               0x000000018b314c88 <redacted> + 248
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	32  UIKit                               0x000000018b314624 <redacted> + 368
2019-01-17 22:04:41.943 CrashTest[2689:10844184] critical: 	33  UIKit                               0x000000018b31165c <redacted> + 540
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	34  UIKit                               0x000000018b3113ac <redacted> + 364
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	35  FrontBoardServices                  0x0000000183f78470 <redacted> + 364
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	36  FrontBoardServices                  0x0000000183f80d6c <redacted> + 224
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	37  libdispatch.dylib                   0x000000018106cae4 <redacted> + 16
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	38  libdispatch.dylib                   0x00000001810741f4 <redacted> + 224
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	39  FrontBoardServices                  0x0000000183fac878 <redacted> + 36
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	40  FrontBoardServices                  0x0000000183fac51c <redacted> + 404
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	41  FrontBoardServices                  0x0000000183facab8 <redacted> + 56
2019-01-17 22:04:41.944 CrashTest[2689:10844184] critical: 	42  CoreFoundation                      0x0000000181723404 <redacted> + 24
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	43  CoreFoundation                      0x0000000181722c2c <redacted> + 276
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	44  CoreFoundation                      0x000000018172079c <redacted> + 1204
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	45  CoreFoundation                      0x0000000181640da8 CFRunLoopRunSpecific + 552
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	46  GraphicsServices                    0x0000000183623020 GSEventRunModal + 100
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	47  UIKit                               0x000000018b62178c UIApplicationMain + 236
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	48  libXamarin.iOS.dll.dylib            0x0000000100345b68 wrapper_managed_to_native_UIKit_UIApplication_UIApplicationMain_int_string___intptr_intptr + 328
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	49  libXamarin.iOS.dll.dylib            0x000000010032e30c UIKit_UIApplication_Main_string___intptr_intptr + 44
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	50  libXamarin.iOS.dll.dylib            0x000000010032e2cc UIKit_UIApplication_Main_string___string_string + 172
2019-01-17 22:04:41.945 CrashTest[2689:10844184] critical: 	51  libCrashTest.exe.dylib              0x0000000100305010 CrashTest_Application_Main_string__ + 128
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	52  libmscorlib.dll.dylib               0x00000001007d94b8 wrapper_runtime_invoke_object_runtime_invoke_dynamic_intptr_intptr_intptr_intptr + 248
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	53  libmonosgen-2.0.dylib               0x00000001034a1bb4 mono_jit_runtime_invoke + 904
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	54  libmonosgen-2.0.dylib               0x00000001035279e0 mono_runtime_invoke_checked + 148
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	55  libmonosgen-2.0.dylib               0x000000010352d8c4 mono_runtime_exec_main_checked + 120
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	56  libmonosgen-2.0.dylib               0x000000010348145c mono_jit_exec + 268
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	57  libxamarin-debug.dylib              0x0000000100554c88 xamarin_main + 2220
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	58  CrashTest                           0x00000001002ea334 main + 96
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 	59  libdyld.dylib                       0x00000001810d1fc0 <redacted> + 4
2019-01-17 22:04:41.946 CrashTest[2689:10844184] critical: 
=================================================================
Got a SIGABRT while executing native code. This usually indicates
a fatal error in the mono runtime or one of the native libraries 
used by your application.
=================================================================
```