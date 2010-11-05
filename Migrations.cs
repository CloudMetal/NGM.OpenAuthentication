﻿using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace NGM.OpenAuthentication {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OpenAuthenticationPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Identifier")
                );

            ContentDefinitionManager.AlterTypeDefinition("User",
               cfg => cfg
                   .WithPart("OpenAuthenticationPartRecord")
                );
        
            return 1;
        }
    }
}