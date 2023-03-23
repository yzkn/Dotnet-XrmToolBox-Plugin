﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MyXrmToolBoxTool1
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "My First Plugin"),
        ExportMetadata("Description", "This is a description for my first plugin"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "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LmNvbS94YXAvMS4wLyIKICAgeG1wTU06RG9jdW1lbnRJRD0iZ2ltcDpkb2NpZDpnaW1wOjY2M2ZhOGQ1LWM4NDctNGEwMS05OTIwLTQ0NDE1Mzc4ODAzZCIKICAgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDoyZTYwMzBjZS02MGIxLTQ4MGUtYmJkYi0yMDUwZTYxNWViNWMiCiAgIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDplOTEyMzEwNC1iMzdhLTQyNDItODJmNS00MzNmMjhiMmFhMmQiCiAgIGRjOkZvcm1hdD0iaW1hZ2UvcG5nIgogICBHSU1QOkFQST0iMi4wIgogICBHSU1QOlBsYXRmb3JtPSJXaW5kb3dzIgogICBHSU1QOlRpbWVTdGFtcD0iMTY3OTQ4NTM0ODIyNDE0NSIKICAgR0lNUDpWZXJzaW9uPSIyLjEwLjI4IgogICB0aWZmOk9yaWVudGF0aW9uPSIxIgogICB4bXA6Q3JlYXRvclRvb2w9IkdJTVAgMi4xMCI+CiAgIDx4bXBNTTpIaXN0b3J5PgogICAgPHJkZjpTZXE+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjk4Yjc3ZDEzLWM2YzAtNGQzMS04YjZkLWUxZWFjNjMxOThlNCIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iR2ltcCAyLjEwIChXaW5kb3dzKSIKICAgICAgc3RFdnQ6d2hlbj0iMjAyMi0xMC0yNFQxMzoyNDo0NiIvPgogICAgIDxyZGY6bGkKICAgICAgc3RFdnQ6YWN0aW9uPSJzYXZlZCIKICAgICAgc3RFdnQ6Y2hhbmdlZD0iLyIKICAgICAgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDozOThhNDI1NS0zMTkxLTQ2ZDItYWY0MS04ZTI2M2FlNGRiYzgiCiAgICAgIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkdpbXAgMi4xMCAoV2luZG93cykiCiAgICAgIHN0RXZ0OndoZW49IjIwMjMtMDMtMjJUMjA6NDI6MjgiLz4KICAgIDwvcmRmOlNlcT4KICAgPC94bXBNTTpIaXN0b3J5PgogIDwvcmRmOkRlc2NyaXB0aW9uPgogPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSJ3Ij8+u+gDxAAAAAZiS0dEAFAAUABQxocL8AAAAAlwSFlzAAALEwAACxMBAJqcGAAAAAd0SU1FB+cDFgsqHIJ4jI8AAASXSURBVFjDtZd9TFV1GMe/v3Pu4Z4LXLi8c4kLKthWFs17cYDSC0KKQQhE8iKhgqzWH8WaW+AoKy1za7Y1t1q1tdpSYFqAoqLOXtSy4MIAFU1wBRTI5eIVgXu595zz6w+IuPJ2LuCznT/O7/f8nn3O8/yel0MgU7YWtoTcNgm7CEGGKFB/VkEGJRHHggK5jw9/FdWNBQqRo7Q+xRgvirQGgO8M23cJQdqPp6J/XggAM59CalazD5XoUQC+Kp4gLloF4oztzRBUp21pDl4IgGKuzeyCVsPwiJAvUQTlZflgW344lEoWZrMNe/bdxJXrdgCAKMHHapVeA7B7yTyQmGr8tq/f3jg8IpUAQG62DqYBG+wOCX5+PArytE76DoHGLmkIKEWZ0o0cYRiYxxcAbbA77lrss+mzSwpwvs7QdabWkMey5DMAqKjqhiBICAjgMTBgwzeHe530OY788UCyYGtha1jPP/Z2AO4BvgxWRijxS4N1mh7HkbZzxw1RDyQND3x0o+Li5ZHsoWFpTj1PDyax7pj+/JICZOQ2+x7cH9nj4aFQnajrRe1JC+4MzQzCEIyyCvLqueOGr5cM4NPPO99+pXjFu/+9D5htyMq/OucZnieVLEOKTn6nH1lUIXohr4VdF+dTNHXN34+HIUo5p1GbjWaPWqWrG9KM6xYFoPEiSRER6rD71+PXauZ1LaUIdwj0wvoU4/sHD/3FLQgg58WAbe6q6cUy6nEvWfGVJBBRpLtr60xntrzUEugSwBtl1xldqCplpr1l4Wo8FKyQfdMpxTPmQeHX7S+3hcoG6OiwxoSEuM/4qSxLkJnm61K+CyJd0dVjr3pr7y0iCyBqFf+Y2nPW0CF+rb/LVU8UaVyD0ZIoCyAuRq2ay1hQkAoFOb4uQwgiTZLbjOaVrMxQaANd60GCQP1kAdzssArzGfNScyh/c9ms+yxLPlB7Mq+zLBpcTsNLv438LYrzu2HVoxqUlmhnrohK4nbiqP6T4h2hMTzPfDnh2SFZAIMW8drg4JgstyZvDEFu5vTiZLVKOgDIzQqm9dX6Yo4j9QD+lAWwOTWg02wec8iNbeH25chMnZa1kVNftEFcjtqTaZDdjC7/bmqIWeMfLRdibExEaXk7mq9Meq77p9PRYQvuBXcG7ZdcueFKJYudO5yKndalZpSa1fTO08mNwxvTjXsB4IcLlnY56ThVHl7pBWbCqoIlvS4BBAVwhwD02Wy0/Nk0Y9W1G/b2zltD6O+3Tj5msw1zZQfHMeAUk5Gtc3kgSU5vihQE2uQQqJoQ3KMU6vt1dFoFdpXo8ETU9EposdiRnts23s692Q01lavPujwRJWxqfJICZymFEgAYBpU+3ooKNzfGs6/fHksIiiQJfIyeh4/GuSuaBhwwto5fQkIwlPCUn2ZP2XLq8ki2KaNp56hV+kLFMwdOV+tLnSflNg/TgN3gEOgjvJLxULD//2FRIPLesFQ8AXA7do0m7MP3Iu0Lmgk3ZzeH1VSu7pJ7AZOeNxZQin2CQHUTlTCvvsZwZNFjuRxJeK4xlFJcJAThlKJVxTP7T32vr5jv3L/gUKSPz48FCgAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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NU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iWE1QIENvcmUgNC40LjAtRXhpdjIiPgogPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iCiAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgeG1sbnM6R0lNUD0iaHR0cDovL3d3dy5naW1wLm9yZy94bXAvIgogICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgIHhtcE1NOkRvY3VtZW50SUQ9ImdpbXA6ZG9jaWQ6Z2ltcDo2NjNmYThkNS1jODQ3LTRhMDEtOTkyMC00NDQxNTM3ODgwM2QiCiAgIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MWI1NDg2NmQtYmEzZS00YTZiLWE2NDQtMjEyZTFmMGNmYWNjIgogICB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZTkxMjMxMDQtYjM3YS00MjQyLTgyZjUtNDMzZjI4YjJhYTJkIgogICBkYzpGb3JtYXQ9ImltYWdlL3BuZyIKICAgR0lNUDpBUEk9IjIuMCIKICAgR0lNUDpQbGF0Zm9ybT0iV2luZG93cyIKICAgR0lNUDpUaW1lU3RhbXA9IjE2Nzk0ODUzNjQ5MzU5MTgiCiAgIEdJTVA6VmVyc2lvbj0iMi4xMC4yOCIKICAgdGlmZjpPcmllbnRhdGlvbj0iMSIKICAgeG1wOkNyZWF0b3JUb29sPSJHSU1QIDIuMTAiPgogICA8eG1wTU06SGlzdG9yeT4KICAgIDxyZGY6U2VxPgogICAgIDxyZGY6bGkKICAgICAgc3RFdnQ6YWN0aW9uPSJzYXZlZCIKICAgICAgc3RFdnQ6Y2hhbmdlZD0iLyIKICAgICAgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo5OGI3N2QxMy1jNmMwLTRkMzEtOGI2ZC1lMWVhYzYzMTk4ZTQiCiAgICAgIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkdpbXAgMi4xMCAoV2luZG93cykiCiAgICAgIHN0RXZ0OndoZW49IjIwMjItMTAtMjRUMTM6MjQ6NDYiLz4KICAgICA8cmRmOmxpCiAgICAgIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiCiAgICAgIHN0RXZ0OmNoYW5nZWQ9Ii8iCiAgICAgIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6MzAwMDJhMGUtOTRlZS00MDA0LWI2ZDAtNjQ2NzhkYjQ3ZDg1IgogICAgICBzdEV2dDpzb2Z0d2FyZUFnZW50PSJHaW1wIDIuMTAgKFdpbmRvd3MpIgogICAgICBzdEV2dDp3aGVuPSIyMDIzLTAzLTIyVDIwOjQyOjQ0Ii8+CiAgICA8L3JkZjpTZXE+CiAgIDwveG1wTU06SGlzdG9yeT4KICA8L3JkZjpEZXNjcmlwdGlvbj4KIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/PmzxC0AAAAAGYktHRABQAFAAUMaHC/AAAAAJcEhZcwAACxMAAAsTAQCanBgAAAAHdElNRQfnAxYLKiykobwjAAAM8ElEQVR42uWc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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}