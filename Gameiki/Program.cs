using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gameiki.Patcher.Events;
using Terraria;

namespace Gameiki {
    static class Program {
        private static Assembly TerrariaAssembly;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            // Handle assembly resolution
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            
            // Setup event handlers
            Hooks.PostUpdate += PostUpdate;

            // Run Terraria
            RunTerraria();
        }

        private static void RunTerraria() {
            Terraria.Program.LaunchGame(new string[] {});
        }

        private static void PostUpdate(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args) {
            var assemblyName = new AssemblyName(args.Name);
            if (assemblyName.Name == "Terraria") {
                return TerrariaAssembly = Assembly.LoadFrom("patched.exe");
            }

            var resource = TerrariaAssembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith(assemblyName.Name + ".dll"));
            if (resource == null) {
                return null;
            }

            using (var stream = TerrariaAssembly.GetManifestResourceStream(resource)) {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return Assembly.Load(buffer);
            }
        }
    }
}