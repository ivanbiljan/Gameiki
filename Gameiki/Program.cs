﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Threading.Tasks;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Gameiki = Gameiki.Gameiki;

namespace Gameiki {
    internal static class Program {
        private static Assembly _terrariaAssembly;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            // Handle assembly resolution
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            // Run Terraria
            var gameiki = new Gameiki();
            gameiki.Run(Environment.GetCommandLineArgs());
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args) {
            var assemblyName = new AssemblyName(args.Name);
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == assemblyName.Name);
            if (assembly != null) {
                return assembly;
            }
            
            if (assemblyName.Name == "Terraria") {
                return _terrariaAssembly = Assembly.LoadFrom("patched.exe");
            }

            var resource = _terrariaAssembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith(assemblyName.Name + ".dll"));
            if (resource == null) {
                return null;
            }

            using (var stream = _terrariaAssembly.GetManifestResourceStream(resource)) {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return Assembly.Load(buffer);
            }
        }
    }
}