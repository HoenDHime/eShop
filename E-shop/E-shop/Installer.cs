using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using E_shop.Code;
using E_shop.Models;
using E_shop.Models.Repository;
using E_shop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace E_shop.Installer
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
        //    container.Register(Classes.FromThisAssembly().BasedOn(typeof(IRepository<>)).WithService.Base());

            container.Register(Classes.FromThisAssembly()
                            .Where(Component.IsInSameNamespaceAs<IOrderStatusRepository>())
                            .WithService.DefaultInterfaces()
                            .LifestyleTransient());

            container.Register(Component.For<OrderFilling>());
            container.Register(Component.For<OrderItemFilling>());
            container.Register(Component.For<ViewOrders>());
            container.Register(Component.For<ViewOrderProfit>());
            
            container.Register(Component.For<Error>());
            container.Register(Component.For<Filter>());
            container.Register(Component.For<ProductFilterViewModel>());
            

            container.Register(Component.For<SmtpClient>());

            container.Register(Classes.FromThisAssembly()
                            .Where(Component.IsInSameNamespaceAs<Entities>())
                            .WithService.DefaultInterfaces()
                            .LifestyleTransient());
        }
    }
}