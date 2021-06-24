using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Modules.Swagger
{
    public static class SwaggerExtensions
    {
        public class MyBaseClass
        {
        }
        public class MyDerivedClass : MyBaseClass
        {
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Prueba de concepto para Prodigio",
                    Description = "Especificaciones ",
                    TermsOfService = new Uri("https://www.kapsch.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kapsch Chile",
                        Email = "kapsch@kapsch.net",
                        Url = new Uri("https://www.kapsch.net/contact")

                    },
                    License = new OpenApiLicense
                    {
                        Name = "Uso exclusivo para Prodigio",
                        Url = new Uri("https://www.kapsch.net/licence")
                    }
                });
                //c.EnableAnnotations();
                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml ";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                MyBaseClass myBase = new MyBaseClass();

                var basePath = AppContext.BaseDirectory;
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");
                //c.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName));
                c.IncludeXmlComments(Path.Combine(
                              Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{myBase.GetType().Assembly.GetName().Name}.xml"));

                Assembly.GetExecutingAssembly().GetReferencedAssemblies();

                // Recopilar todas las rutas de archivo de documentos XML de salida de proyectos referenciados  
                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new AssemblyName[] { currentAssembly.GetName() })
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                .Where(f => File.Exists(f)).ToArray();

                Array.ForEach(xmlDocs, (d) =>
                {
                    c.IncludeXmlComments(d);
                });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization por API Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement   {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                        new  string []{}
                    }
                });

            });
            return services;

        }


    }
}
