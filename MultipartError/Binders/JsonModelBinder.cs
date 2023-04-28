using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;

namespace MultipartError.Binders
{
    public class JsonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var logger = LogManager.GetLogger("JsonModelBinder");
            logger.Info($"Зашли в {nameof(JsonModelBinder)}");

            try
            {
                // Проверяем полученное значение: ключ должен соотвествовать ModelName (в том числе может быть пустым)
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (valueProviderResult == ValueProviderResult.None && bindingContext.HttpContext.Request.Form.Keys.Count > 0)
                {
                    var firstKey = bindingContext.HttpContext.Request.Form.Keys.First();
                    valueProviderResult = bindingContext.ValueProvider.GetValue(firstKey);
                }

                if (valueProviderResult != ValueProviderResult.None)
                {
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                    // Попытка преобразовать входное значение (берется первое, если вдруг будет несколько пустых ключей)
                    var valueAsString = valueProviderResult.FirstValue;
                    try
                    {
                        logger.Info($"{nameof(JsonModelBinder)} тело запроса: {valueAsString}");
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType);
                        bindingContext.Result = ModelBindingResult.Success(result);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn($"Ошибка преобразования {nameof(JsonModelBinder)}: {ex.Message}");
                        bindingContext.Result = ModelBindingResult.Success(null);
                    }
                }

                logger.Info($"Конец работы {nameof(JsonModelBinder)}");
            }
            catch (Exception ex) 
            {
                logger.Error($"Непредвиденная ошибка {ex.Source}: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
