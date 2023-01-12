using Microsoft.AspNetCore.Mvc.ModelBinding;
using Proj.Api.ViewModels.Produto;
using System.Text.Json;

namespace Proj.Api.Extensions
{
    public class AddProdutoModelBinder : IModelBinder
    {
        // Binder personalizado para envio de IFormFile e ViewModel dentro de um FormData compatível com .NET Core 3.1 ou superior (system.text.json)
        // Permite que uma rota receba ao mesmo tempo dados de FORM e dados de JSON
        // Com essa solução, não é necessário colocar '[FromForm]' nos parametros da rota
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = true
            };

            //'produto' é a chave que vai receber o corpo JSON dentro da requisição Form-Data do front-end (Postman, etc)
            var produtoImagemViewModel = JsonSerializer.Deserialize<AddProdutoModelBinderImageViewModel>(bindingContext.ValueProvider.GetValue("produto").FirstOrDefault(), serializeOptions);
            produtoImagemViewModel.ImagemUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

            bindingContext.Result = ModelBindingResult.Success(produtoImagemViewModel);
            return Task.CompletedTask;
        }
    }
}
