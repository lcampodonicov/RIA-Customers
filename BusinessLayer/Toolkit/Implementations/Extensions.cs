
using Microsoft.Extensions.DependencyInjection;
using RIACustomers.BusinessLayer.Toolkit;

namespace BusinessLayer.Toolkit;


public static class __ToolkitExtensions__ {

    public static IServiceCollection RegisterToolkit(this IServiceCollection This){

        This.AddTransient<Validations>();
        This.AddTransient<Sorting>();
        This.AddTransient<CustomerHelper>();

        return This;

    }

}
