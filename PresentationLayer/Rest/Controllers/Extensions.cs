using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RIACustomers.Rest.Controllers;


public static class __ControllersExtensions__ {

    public static List<String> GetErrors(this ModelStateDictionary State) =>
        State.Values
            .Select(Value => Value.Errors)
            .Select(Errors => Errors.Select( Error => Error.ErrorMessage ))
            .Aggregate(new List<String>(), (C, N) => C.Concat(N).ToList())
            .ToList()
        ;

}
