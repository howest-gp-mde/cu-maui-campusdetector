// This file is used to declare the permissions required by the Android platform for location access.
// it is an alternative to declaring permissions in the AndroidManifest.xml file.

using Android.App;

[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]

#if ANDROID29_0_OR_GREATER
[assembly: UsesPermission(Android.Manifest.Permission.AccessBackgroundLocation)]
#endif
