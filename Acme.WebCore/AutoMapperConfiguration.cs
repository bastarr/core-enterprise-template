using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;

namespace Acme.WebCore
{
public static class AutoMapperConfiguration
    {
        public static void LoadAllMappingProfiles(string executingAssemblyLocation, string targetNamespace = "Acme.WebCore.MappingProfiles", string assemblyPrefix = "Acme", string[] additionalProfiles = null)
        {
            // Recurse all dependent assemblies beginning with the parameter location and force them to load.
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.FullName).ToList();
            LoadDependencies(loadedAssemblies, Assembly.GetCallingAssembly().Location);

            try
            {
                // Discover all classes of type AutoMapper.Profile present in the loaded assemblies.
                var profileType = typeof(Profile);

                var profiles = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName.StartsWith(assemblyPrefix))
                    .SelectMany(a => a.GetTypes()
                        .Where(t => t.FullName.StartsWith(assemblyPrefix) && profileType.IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null &&
                        (t.Namespace.Contains(targetNamespace) || additionalProfiles.Any(s => s == t.FullName)))
                        .Select(Activator.CreateInstance)
                        .Cast<Profile>());

                // Initialize AutoMapper with all discovered profiles.
                var cfg = new MapperConfigurationExpression();
                foreach(var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
                Mapper.Initialize(cfg);
#if DEBUG
                Mapper.AssertConfigurationIsValid();
#endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        private static void LoadDependencies(List<string> loadedAssemblies, string assemblyLocation)
        {
            var callingAssembly = Assembly.GetExecutingAssembly();
            var references = callingAssembly.GetReferencedAssemblies().Select(r => r.FullName);
            foreach (var reference in references)
            {
                if (loadedAssemblies.Contains(reference)) continue;

                var loadedAssembly = AppDomain.CurrentDomain.Load(reference);
                loadedAssemblies.Add(reference);

                LoadDependencies(loadedAssemblies, loadedAssembly.Location);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Init()
        {
            //want to load this assembly so need to call the init
        }
    }
}