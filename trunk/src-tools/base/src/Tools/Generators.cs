﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Generator;
using Sample.Project.Tools.Templates;
using System.IO;
using NVelocity;
using NVelocity.App;
using Simple.NVelocity;
using Sample.Project.Tools.Infra;
using Sample.Project.Tools.Migrations;
using Simple.Generator.Console;
using Sample.Project.Tools.Data;
using Sample.Project.Tools.Macros;

namespace Sample.Project.Tools
{
    public static class Generators
    {
        public static CommandResolver Define(this CommandResolver registry, bool production)
        {
            registry.Register<MigrateTool>("migrate")
                .WithOption("to", x => x.Version)
                .WithOption("dry", x => x.DryRun)
                .WithOption("script", x => x.FilePath)
                .WithOption("env", x => x.Environment);

            registry.Register<InsertDataCommand>("data").WithOption("testdata", x => x.ForceTestData);

            registry.Register<PrepareMacro>("prepare");

            if (production)
                registry.Register<TestPrepareMacro>("testprepare");

            if (!production)
            {
                registry.Register<NewMigrationTemplate>("new migration").WithArgument("name", x => x.Name);

                registry.Register<ScaffoldGenerator>("scaffold").AsTableGenerator();

                registry.Register<ServiceInterfaceTemplate>("g service interface").AsTableGenerator();
                registry.Register<ServiceImplTemplate>("g service impl").AsTableGenerator();
                registry.Register<EntityTemplate>("g entity").AsTableGenerator();
                registry.Register<ValidatorTemplate>("g validator").AsTableGenerator();
                registry.Register<MappingTemplate>("g mapping").AsTableGenerator();
            }


            return registry;
        }

        public static CommandOptions<T> AsTableGenerator<T>(this InitialCommandOptions<T> generator)
            where T : TableTemplate
        {
            return generator
                .WithArgumentList("tables", x => x.TableNames)
                .WithOption("delete", x => x.DeleteFlag);
        }

        public static SimpleTemplate ToTemplate(this ICommand generator)
        {
            var type = generator.GetType();
            var asm = type.Assembly;

            using (var stream = asm.GetManifestResourceStream(string.Format("{0}.txt", type.FullName)))
            {
                if (stream == null) throw new ParserException(string.Format("Couldn't find template for '{0}'...", type.FullName));
                return new SimpleTemplate(new StreamReader(stream).ReadToEnd());
            }
        }
    }
}
