using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace GHM.Infrastructure.ModelBinders
{
    public class FromBodyWithDateTime : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource { get; }
    }
}
