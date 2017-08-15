using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Framework.Utilities.PocoGenerator.Utilities;
using System.IO;
using MySql.Data.MySqlClient;
using DatabaseSchemaReader.DataSchema;

namespace Framework.Utilities.PocoGenerator
{

    public class Config
    {
        public string ConfigFileLocation { get; private set; }


        public string DbType { get; set; } = "";
        public string ConnectionString { get; set; } = "";


        public string Namespace { get; private set; } = "";
        public string ClassPrefix { get; private set; } = "";
        public string ClassSuffix { get; private set; } = "";
        public string SchemaName { get; private set; } = null;
        public bool IncludeViews { get; private set; } = false;

        public List<TemplateAndCodeFile> InputOutputFiles { get; private set; } = new List<TemplateAndCodeFile>();

        const string MSSQL_DBTYPE = "mssql";
        const string MYSQL_DBTYPE = "mysql";
        const string SQLITE3_DBTYPE = "sqlite3";

        const string PARAMETER_CONFIG = "-config";

        const string SECTION_CONNECTIONSTRING = "connectionString";
        const string SECTION_NAMESPACE = "nameSpace";
        const string SECTION_DBTYPE = "dbType";
        const string SECTION_CODEFILENAME = "codeFileName";
        const string SECTION_TEMPLATEFILENAME = "templateFileName";
        const string SECTION_GENERATE = "generate";

        const string VARIABLE_CONFIGFILEFOLDER = "{ConfigFolder}";
        const string VARIABLE_EXEFOLDER = "{EXEFolder}";

        private string[] args;


        public Config(string[] args)
        {
            this.args = args;
        }

        public void Load()
        {
            if (args.IsParamAvailable(PARAMETER_CONFIG) == false)
            {
                throw new Exception("-config configuration option is not specified");
            }

            if (args.IsParamValueAvailable(PARAMETER_CONFIG) == false)
            {
                throw new Exception("Configuration file is not specified");
            }

            ConfigFileLocation = args.GetParamValueAsString(PARAMETER_CONFIG);

            if (File.Exists(ConfigFileLocation) == false)
            {
                throw new Exception($"Configuration file is not specified from {ConfigFileLocation}");
            }

            //Convert the relative paths to absulute path
            ConfigFileLocation = Path.GetFullPath(ConfigFileLocation);


            IConfigurationRoot configuration;
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(ConfigFileLocation);
                configuration = builder.Build();
            }
            catch(Exception ex)
            {
                throw new Exception($"Unable to load the Configuration file from {ConfigFileLocation}",ex);
            }

            ConnectionString = SettingsValue(configuration, SECTION_CONNECTIONSTRING);
            DbType = SettingsValue(configuration, SECTION_DBTYPE);
            Namespace = SettingsValue(configuration, SECTION_NAMESPACE);

            var section = configuration.GetSection(SECTION_GENERATE);
            foreach(var child in section.GetChildren())
            {
                var item = new TemplateAndCodeFile()
                {
                    TemplateFile = SettingsValue(child, SECTION_TEMPLATEFILENAME),
                    CodeFile = SettingsValue(child, SECTION_CODEFILENAME)
                };

                if (item.TemplateFile == null || item.TemplateFile.Trim() == "" )
                {
                    throw new Exception("Template File is not specified in the configuration");
                }

                if ( File.Exists(item.TemplateFile) == false)
                {
                    throw new Exception("Template File specified in the configuration does not exists");
                }

                if (item.CodeFile == null || item.CodeFile.Trim() == "")
                {
                    throw new Exception("Code File is not specified in the configuration");
                }

                var codeFileDir = Path.GetDirectoryName(item.CodeFile);
                if (Directory.Exists(codeFileDir) == false)
                {
                    try
                    {
                        Directory.CreateDirectory(codeFileDir);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"Unable to create Code File directory {codeFileDir}", ex);
                    }
                }

                InputOutputFiles.Add(item);
            }
        }

        private string SettingsValue(IConfiguration configuration, string sectionString, bool substituteVariables = true)
        {
            var section = configuration.GetSection(sectionString);
            if (section == null)
            {
                throw new Exception($"Unable find the {sectionString} settings in config file");
            }

            if (section.Value == null || section.Value.Trim() == "")
            {
                throw new Exception($"Value for {sectionString}  settings is not set in config file");
            }

            return substituteVariables ? SubstituteVariables(section.Value) : section.Value;
        }

        public DbConnection GetConnection()
        {
            if (DbType == null)
                throw new Exception("DbType not provided.");

            var dbType = DbType.Trim().ToLower();
            DbConnection dbConnection = null;

            switch(dbType)
            {
                case MSSQL_DBTYPE:
                    {
                        dbConnection = new SqlConnection(ConnectionString);
                    }
                    break;
                case MYSQL_DBTYPE:
                    {
                        dbConnection = new MySqlConnection(ConnectionString);
                    }
                    break;
                case SQLITE3_DBTYPE:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new Exception($"DbType {DbType} is not valid.");
            }
            return dbConnection;
        }


        public SqlType ServerType
        {
            get
            {
                if (DbType == null)
                    throw new Exception("DbType not provided.");

                var dbType = DbType.Trim().ToLower();

                switch (dbType)
                {
                    case MSSQL_DBTYPE:
                        return SqlType.SqlServer;
                    case MYSQL_DBTYPE:
                        return SqlType.MySql;
                    case SQLITE3_DBTYPE:
                        return SqlType.SQLite;
                    default:
                        throw new Exception($"DbType {DbType} is not valid.");
                }
            }
        }


        private string SubstituteVariables(string str)
        {
            if (str == null)
                return str;
            str = str.Replace(VARIABLE_CONFIGFILEFOLDER, Path.GetDirectoryName(ConfigFileLocation));
            str = str.Replace(VARIABLE_EXEFOLDER, Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            return str;
        }

    }
}
