using DemoApplication.Localization;
using FluentValidation.Resources;
using System.Collections.Concurrent;
using System.Globalization;

namespace DemoApplication.Model
{
	//public interface IMessageResolver
 //   {
	//	string GetString(string key, CultureInfo culture);
 //   }

	//public class MessageResolver: IMessageResolver
	//{
	//	public string GetString(string key, CultureInfo culture)
 //       {
	//		return GetTranslation(culture?.Name, key);

	//	}

	//	private static string GetTranslation(string culture, string key)
	//	{
	//		return culture switch
	//		{
	//			HindiCulture.Culture => HindiCulture.GetTranslation(key),
	//			_ => null,
	//		};
	//	}
	//}

	public class MessageLanguageManager : LanguageManager
    {
		//private IMessageResolver _messageResolver;
   //     public MessageLanguageManager(IMessageResolver messageResolver)
   //     {
			//_messageResolver = messageResolver;
   //     }

        public override string GetString(string key, CultureInfo culture = null)
        {
			//benefit of creating the custom language manager that you can read it from database
			//define your custom logic and display the message accordingle

			//var message = _messageResolver.GetString(key, culture);
            return base.GetString(key, culture);
        }
    }    
}
