using NHibernate;
using NHibernate.Cfg;
using SC.BL.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SC.DAL.Nhibernate
{
    public class NhibernateHelper
    {
        private static ISessionFactory SessionFactory;

        public static string GeTExecutingAssemblyDirectory(Assembly assembly)
        {
            //geeft de assembly trg waarin het startup project zich bevind (UI-MVC)
            string filepath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            string assemblyDirectory = Path.GetDirectoryName(filepath);
            return assemblyDirectory;
        }

        public static ISession OpenSession()
        {
            if (SessionFactory == null)
            {
                string assemblyPath = GeTExecutingAssemblyDirectory(Assembly.GetAssembly(typeof(Ticket)));
                string ticketMappingPath = assemblyPath.Replace("bin", "mappings\\ticket.hbm.xml").Replace("UI-MVC", "DAL\\Nhibernate");
                string ticketResponseMappingPath = assemblyPath.Replace("bin", "mappings\\ticketResponse.hbm.xml").Replace("UI-MVC", "DAL\\Nhibernate");
                //we tweaken het pad zodat het leidt naar onze mapping files + voegen het aan de configuratie toe
                var configuration = new Configuration().AddFile(ticketMappingPath).AddFile(ticketResponseMappingPath);
                configuration.Configure();//laat Web.config in
                SessionFactory = configuration.BuildSessionFactory();
            }
            return SessionFactory.OpenSession();
        }
    }
}
