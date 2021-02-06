using Microsoft.AspNetCore.Components.Forms;
using System.Linq;

namespace Blazor.Samples.Core
{
    public class BootstrapStyleProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return isValid ? "is-valid was-validated" : "is-invalid was-validated";
        }
    }
}
