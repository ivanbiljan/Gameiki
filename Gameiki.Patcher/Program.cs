using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Cecil;
using Microsoft.Win32;
using Mono.Cecil;

namespace Gameiki.Patcher {
    internal class Program {
        public static void Main(string[] args) {
            var key = Environment.Is64BitOperatingSystem
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                : RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

            // TODO check for GOG/whatever paths
            var terrariaDirectory = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 105600")
                .GetValue("InstallLocation").ToString();
            var terrariaPath = Path.Combine(terrariaDirectory, "Terraria.exe");
            if (string.IsNullOrWhiteSpace(terrariaDirectory) || !File.Exists(terrariaPath)) {
                throw new Exception("You do not seem to own a legal copy of Terraria.");
            }

            var terraria = AssemblyDefinition.ReadAssembly(terrariaPath);
            var mods = from type in Assembly.GetExecutingAssembly().GetTypes()
                where !type.IsInterface && !type.IsAbstract && typeof(IModification).IsAssignableFrom(type)
                select Activator.CreateInstance(type) as IModification;
            foreach (var mod in mods) {
                Console.WriteLine($"Running '{mod.GetType().Name}'...");
                mod.Execute(terraria);
            }

            Console.WriteLine("Writing to file...");
            using (var stream = new MemoryStream()) {
                terraria.Write(stream);
                File.WriteAllBytes("patched.exe", stream.ToArray());
            }
            
            Console.WriteLine("> Done");
        }
    }
}