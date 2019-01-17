using Swagger.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Swagger.Services
{
    [SwaggerApi]
    public class DefaultSwaggerModelCatalog : List<SwaggerModelData>, ISwaggerModelCatalog
    {
        public DefaultSwaggerModelCatalog(IEnumerable<ISwaggerModelDataProvider> dataProviders)
        {
            AddRange(dataProviders.Select(p => p.GetModelData()));
        }

        public SwaggerModelData AddModel<T>()
        {
            var t = typeof (T);
            var model = (SwaggerModelData) null;
            if (GetModelForType(t, false) == null)
            {
                model = new SwaggerModelData(t);
                Add(model);

                foreach (var subType in model.ModelType.GetProperties())
                {
                    GetModelForType(subType.PropertyType);
                }
            }
            return model;
        }

        public void AddModels(params Type[] types)
        {
            if (types == null || types.Any() == false)
            {
                return;
            }

            foreach (var t in types)
            {
                var currentType = t.GetElementType() ?? t;

                if (GetModelForType(currentType, false) == null)
                {
                    var model = new SwaggerModelData(currentType);

                    if (Primitive.IsPrimitive(currentType) == false)
                    {
                        Add(model);
                    }

                    var subTypes = currentType?
                        .GetProperties().Where(r => Primitive.IsPrimitive(r.PropertyType) == false)                        
                        .Select(r => r.PropertyType);

                    if (subTypes?.Any() == true)
                    {
                        AddModels(subTypes.ToArray());
                    }

                    var generics = currentType
                       .GenericTypeArguments.Where(r => Primitive.IsPrimitive(r) == false);

                    if (generics?.Any() == true)
                    {
                        AddModels(generics.ToArray());
                    }
                }
            }
        }

        public SwaggerModelData GetModelForType<T>(bool addIfNotSet = true)
        {
            var t = typeof(T);
            return GetModelForType(t, addIfNotSet);
        }

        public SwaggerModelData GetModelForType(Type t, bool addIfNotSet = true)
        {
            if (Primitive.IsPrimitive(t)) return null;

            var model = this.FirstOrDefault(x => x.ModelType == t);
            if (model == null && addIfNotSet)
            {
                AddModels(t);
                model = this.FirstOrDefault(x => x.ModelType == t);
            }
            return model;
        }
            
    }
}
