using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using DatabaseSchemaReader.DataSchema;
using Framework.Utilities.PocoGenerator.Utilities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Framework.Utilities.PocoGenerator
{
    public class Config
    {
        private const string MSSQLDBTYPE = "mssql";
        private const string MYSQLDBTYPE = "mysql";
        private const string SQLITE3DBTYPE = "sqlite3";

        private const string PARAMETERCONFIG = "-config";

        private const string SECTIONCONNECTIONSTRING = "connectionString";
        private const string SECTIONNAMESPACE = "nameSpace";
        private const string SECTIONDBTYPE = "dbType";
        private const string SECTIONCODEFILENAME = "codeFileName";
        private const string SECTIONTEMPLATEFILENAME = "templateFileName";
        private const string SECTIONGENERATE = "generate";

        private const string VARIABLECONFIGFILEFOLDER = "{ConfigFolder}";
        private const string VARIABLEEXEFOLDER = "{EXEFolder}";

        private string[] args;

        public Config(string[] args)
        {
            this.args = args;
        }

        public string ConfigFileLocation { get; private set; }

        public string DbType { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public string Namespace { get; private set; } = string.Empty;

        public string ClassPrefix { get; private set; } = string.Empty;

        public string ClassSuffix { get; private set; } = string.Empty;

        public string SchemaName { get; private set; } = null;

        public bool IncludeViews { get; private set; } = false;

        public List<TemplateAndCodeFile> InputOutputFiles { get; private set; } = new List<TemplateAndCodeFile>();

        public SqlType ServerType
        {
            get
            {
                if (DbType == null)
                {
                    throw new Exception("DbType not provided.");
                }

                var dbType = DbType.Trim().ToLower();

                switch (dbType)
                {
                    case MSSQLDBTYPE:
                        return SqlType.SqlServer;
                    case MYSQLDBTYPE:
                        return SqlType.MySql;
                    case SQLITE3DBTYPE:
                        return SqlType.SQLite;
                    default:
                        throw new Exception($"DbType {DbType} is not valid.");
                }
            }
        }

        public void Load()
        {
            if (args.IsParamAvailable(PARAMETERCONFIG) == false)
            {
                throw new Exception("-config configuration option is not specified");
            }

            if (args.IsParamValueAvailable(PARAMETERCONFIG) == false)
            {
                throw new Exception("Configuration file is not specified");
            }

            ConfigFileLocation = args.GetParamValueAsString(PARAMETERCONFIG);

            if (File.Exists(ConfigFileLocation) == false)
            {
                throw new Exception($"Configuration file is not specified from {ConfigFileLocation}");
            }

            // Convert the relative paths to absulute path
            ConfigFileLocation = Path.GetFullPath(ConfigFileLocation);

            IConfigurationRoot configuration;
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(ConfigFileLocation);
                configuration = builder.Build();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to load the Configuration file from {ConfigFileLocation}", ex);
            }

            ConnectionString = SettingsValue(configuration, SECTIONCONNECTIONSTRING);
            DbType = SettingsValue(configuration, SECTIONDBTYPE);
            Namespace = SettingsValue(configuration, SECTIONNAMESPACE);

            var section = configuration.GetSection(SECTIONGENERATE);
            foreach (var child in section.GetChildren())
            {
                var item = new TemplateAndCodeFile()
                {
                    TemplateFile = SettingsValue(child, SECTIONTEMPLATEFILENAME),
                    CodeFile = SettingsValue(child, SECTIONCODEFILENAME),
                };

                if (item.TemplateFile == null || item.TemplateFile.Trim() == string.Empty)
                {
                    throw new Exception("Template File is not specified in the configuration");
                }

                if (File.Exists(item.TemplateFile) == false)
                {
                    throw new Exception("Template File specified in the configuration does not exists");
                }

                if (item.CodeFile == null || item.CodeFile.Trim() == string.Empty)
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
                    catch (Exception ex)
                    {
                        throw new Exception($"Unable to create Code File directory {codeFileDir}", ex);
                    }
                }

                InputOutputFiles.Add(item);
            }
        }

        public DbConnection GetConnection()
        {
            if (DbType == null)
            {
                throw new Exception("DbType not provided.");
            }

            var dbType = DbType.Trim().ToLower();
            DbConnection dbConnection = null;

            switch (dbType)
            {
                case MSSQLDBTYPE:
                    {
                        dbConnection = new SqlConnection(ConnectionString);
                    }

                    break;
                case MYSQLDBTYPE:
                    {
                        dbConnection = new MySqlConnection(ConnectionString);
                    }

                    break;
                case SQLITE3DBTYPE:
                    {
                        throw new NotImplementedException();
                    }

                default:
                    throw new Exception($"DbType {DbType} is not valid.");
            }

            return dbConnection;
        }

        private string SubstituteVariables(string str)
        {
            if (str == null)
            {
                return str;
            }

            str = str.Replace(VARIABLECONFIGFILEFOLDER, Path.GetDirectoryName(ConfigFileLocation));
            str = str.Replace(VARIABLEEXEFOLDER, Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            return str;
        }

        private string SettingsValue(IConfiguration configuration, string sectionString, bool substituteVariables = true)
        {
            var section = configuration.GetSection(sectionString);
            if (section == null)
            {
                throw new Exception($"Unable find the {sectionString} settings in config file");
            }

            if (section.Value == null || section.Value.Trim() == string.Empty)
            {
                throw new Exception($"Value for {sectionString}  settings is not set in config file");
            }

            return substituteVariables ? SubstituteVariables(section.Value) : section.Value;
        }
    }
}
