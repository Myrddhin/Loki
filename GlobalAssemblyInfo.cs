using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Loki")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("2.0.0.5")]
[assembly: AssemblyFileVersion("2.0.0.0")]
[assembly: NeutralResourcesLanguage("fr-FR")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif QUALITY
[assembly: AssemblyConfiguration("Quality")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif