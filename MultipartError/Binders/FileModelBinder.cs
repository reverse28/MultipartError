using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MultipartError.Binders
{
    public class FileModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            //Получаем все файлы из запроса
            var files = bindingContext.HttpContext.Request.Form.Files.ToList();

            bindingContext.Result = ModelBindingResult.Success(files);
            return Task.CompletedTask;
        }
    }
}
