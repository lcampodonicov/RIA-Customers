using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RIACustomers.Database.Context;

namespace RIACustomers.Database.Managers;


public abstract class EFCRUDAdapter<ContextType> where ContextType : DbContext {

    public ContextType Context { get; set; }

    public EFCRUDAdapter(ContextType Context){

        this.Context = Context;

    }

    private Func<ModelType, PropertyInfo, ModelType> ToNewModel<DTOType, ModelType>(DTOType DTO, PropertyInfo[] ModelProperties) => 
        (ModelType Seed, PropertyInfo NextProperty) => {

            var Value = NextProperty.GetValue( DTO );
                    
            var ModelProperty = ModelProperties
                .First( ModelProperty => ModelProperty.Name == NextProperty.Name )
            ;

            ModelProperty.SetValue( Seed, Value );

            return Seed;

        }
    ;

    private Func<PropertyInfo, Boolean> PropertyExists(List<String> ModelPropertiesNames) =>
        (PropertyInfo Property) => 
            ModelPropertiesNames.Contains( Property.Name )
    ;

    protected async Task<ModelType> Create<DTOType, ModelType>(DTOType DTO) where ModelType : class, new() {

        var ModelRuntime = typeof( ModelType );
        var ModelProperties = ModelRuntime.GetProperties();
        var ModelPropertiesNames = ModelProperties
            .Select( Property => Property.Name )
            .ToList()
        ;

        var NewModelSeed = new ModelType();

        var NewModel = typeof(DTOType)
            .GetProperties()
            .Where( PropertyExists(ModelPropertiesNames) )
            .Aggregate( NewModelSeed, ToNewModel<DTOType, ModelType>(DTO, ModelProperties) )
        ;

        Context.Set<ModelType>().Add( NewModel );
        await Context.SaveChangesAsync();

        return NewModel;

    }

    protected async Task<List<ModelType>> Create<DTOType, ModelType>(List<DTOType> DTOs) where ModelType : class, new() {

        var ModelRuntime = typeof( ModelType );
        var ModelProperties = ModelRuntime.GetProperties();
        var ModelPropertiesNames = ModelProperties
            .Select( Property => Property.Name )
            .ToList()
        ;

        var NewModels = DTOs
            .Select(
                DTO =>
                    typeof( DTOType )
                    .GetProperties()
                    .Where( PropertyExists( ModelPropertiesNames ) )
                    .Aggregate( new ModelType(), ToNewModel<DTOType, ModelType>(DTO, ModelProperties))
            )
            .ToList()
        ;

        Context.Set<ModelType>().AddRange( NewModels );
        await Context.SaveChangesAsync();

        return NewModels;

    }

}
