using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SC.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.DAL.Nhibernate
{
    class Cfg
    {
        public Cfg()
        {
            BuildSession();
        }

        public ISessionFactory BuildSession()
        {
            var sessionFactory = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2012
                                .ConnectionString(c => c
                                .FromAppSetting("SupportCenterDB_EFCodeFirst"))
                                .ShowSql())
                                .Mappings(m => m
                                .FluentMappings.AddFromAssemblyOf<Ticket>().ExportTo(@"..\Nhibernate\Mapping"))
                                .BuildSessionFactory();
            return sessionFactory;
        }
    }
}
