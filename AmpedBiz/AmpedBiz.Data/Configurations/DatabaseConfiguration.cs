﻿using AmpedBiz.Common.Configurations;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;

namespace AmpedBiz.Data.Configurations
{
    internal static class DatabaseConfiguration
    {
        public static IPersistenceConfigurer Configure()
        {
            switch(DatabaseConfig.Instance.Database)
            {
                case DatabaseProvider.MsSql:
                    return ConfigureMsSql();

                case DatabaseProvider.MySql:
                    return ConfigureMySql();

                case DatabaseProvider.Postgres:
                    return ConfigurePostgres();

                case DatabaseProvider.InMemory:
                    return ConfigureInMemory();

                default:
                    return ConfigureMsSql();
            }
        }

        private static IPersistenceConfigurer ConfigurePostgres()
        {
            return PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(x => x
                    .Host(DatabaseConfig.Instance.HostServer)
                    .Port(DatabaseConfig.Instance.Port)
                    .Database(DatabaseConfig.Instance.Name)
                    .Username(DatabaseConfig.Instance.Username)
                    .Password(DatabaseConfig.Instance.Password)
                )
                .QuerySubstitutions("true 1, false 0, yes y, no n")
                .DefaultSchema("public")
                .AdoNetBatchSize(1)
                .FormatSql()
                .ShowSql();
        }

        private static IPersistenceConfigurer ConfigureMsSql()
        {
            return MsSqlConfiguration.MsSql2012
                .ConnectionString(x => x
                    .Server(DatabaseConfig.Instance.HostServer)
                    .Database(DatabaseConfig.Instance.Name)
                    .Username(DatabaseConfig.Instance.Username)
                    .Password(DatabaseConfig.Instance.Password)
                )
                .QuerySubstitutions("true 1, false 0, yes y, no n")
                .DefaultSchema("dbo")
                .AdoNetBatchSize(1)
                .FormatSql()
                .ShowSql();
        }

        private static IPersistenceConfigurer ConfigureMySql()
        {
            return MySQLConfiguration.Standard
                .Dialect<MySQL55InnoDBDialect>()
                .ConnectionString(x => x
                    .Server(DatabaseConfig.Instance.HostServer)
                    .Database(DatabaseConfig.Instance.Name)
                    .Username(DatabaseConfig.Instance.Username)
                    .Password(DatabaseConfig.Instance.Password)
                )
                .QuerySubstitutions("true 1, false 0, yes y, no n")
                .DefaultSchema(DatabaseConfig.Instance.Name)
                .AdoNetBatchSize(1)
                .FormatSql()
                .ShowSql();
        }

        private static IPersistenceConfigurer ConfigureSqlLite()
        {
            return SQLiteConfiguration.Standard.InMemory()
                //.ConnectionString(x => x
                //    .Database(DatabaseConfig.Instance.Name)
                //    .Username(DatabaseConfig.Instance.Username)
                //    .Password(DatabaseConfig.Instance.Password)
                //)
                .QuerySubstitutions("true 1, false 0, yes y, no n")
                .DefaultSchema(DatabaseConfig.Instance.Name)
                .AdoNetBatchSize(1)
                .FormatSql()
                .ShowSql();
        }

        private static IPersistenceConfigurer ConfigureInMemory()
        {
            return SQLiteConfiguration.Standard.InMemory()
                .QuerySubstitutions("true 1, false 0, yes y, no n")
                .DefaultSchema(DatabaseConfig.Instance.Name)
                .AdoNetBatchSize(1)
                .FormatSql()
                .ShowSql();
        }
    }
}
