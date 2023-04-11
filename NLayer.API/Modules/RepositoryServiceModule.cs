using System.Reflection;
using Autofac;
using NLayer.Caching;
using NLayer.Core.Repository;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using Module = Autofac.Module;

namespace NLayer.API.Modules;

public class RepositoryServiceModule : Module
{
    //Autofac kullanmamızın avantajı: Ne kadar repository ve service class-interface var ise,
    //Hepsini tek tek implement etmek durumunda kalmıyoruz. Built-In injection özelliklerinde bu bulunmuyor.
    protected override void Load(ContainerBuilder builder)
    {
        //Autofac'te ilk önce class daha sonra interface eklenir.
        //Repository Generic bir class olduğu için RegisterGeneric kullandık.
        builder.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();
        
        //Service Generic bir class olduğu için RegisterGeneric kullandık.
        builder.RegisterGeneric(typeof(Service<>))
            .As(typeof(IService<>))
            .InstancePerLifetimeScope();

        //UnitOfWork Generic bir class olmadığı için RegisterType kullandık.
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

        //builder.RegisterType<ProductServiceWithNoCaching>().As<IProductService>().InstancePerLifetimeScope();

        builder.RegisterType<ProductServiceWithNoCaching>().As<IProductService>().InstancePerLifetimeScope();
        
        //Projede service-repo interface-class'larının kullanıldığı yerlerin assembly'lerini alıyoruz.
        var apiAssembly = Assembly.GetExecutingAssembly();
        //Katman içerisinde herhangi bir class'ın tipini vermemiz, o katman için assembly almamız için yeterli.
        //Direkt katmanın kendisini de verebiliriz(NLayer.Repository) fakat tip ile almak daha güvenli. 
        var repositoryAssembly = Assembly.GetAssembly(typeof(AppDbContext));
        var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

        //Assembly'lerini aldığımız katmanlarda 'Repository' anahtar kelimesi geçen class'ları bellek'e ekliyoruz.
        builder.RegisterAssemblyTypes(apiAssembly, repositoryAssembly, serviceAssembly)
            .Where(x => x.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        //Assembly'lerini aldığımız katmanlarda 'Service' anahtar kelimesi geçen class'ları bellek'e ekliyoruz.
        builder.RegisterAssemblyTypes(apiAssembly, repositoryAssembly, serviceAssembly)
            .Where(x => x.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        //InstancePerLifetimeScope => Scope -> Request başlayıp bitene kadar aynı instance'ı kullanır!
        //InstancePerDependency => Transient -> Herhangi bir interface nerede geçildiyse her seferinde yeni bir instance oluşturur!
    }
}