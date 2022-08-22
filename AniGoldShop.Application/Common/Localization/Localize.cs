using AniGoldShop.Application.Common.Localization.Text;
using Microsoft.Extensions.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;


namespace AniGoldShop.Application.Common.Localization
{
    public class Localize : IStringLocalizer<TextLocalizationResource>
    {

        private CultureInfo _currentCulture;

        public Localize()
        {
            //_currentCulture = CultureInfo.CurrentCulture;
            _currentCulture = new CultureInfo("fa-IR");
        }

        public Localize(CultureInfo currentCulture)
        {
            _currentCulture = currentCulture;
        }

        public LocalizedString this[string name] => new LocalizedString(
            name,
            TextLocalizationResource.ResourceManager.GetString(name, _currentCulture)
        );

        public LocalizedString this[string name, params object[] arguments] => new LocalizedString(
            name,
            string.Format(TextLocalizationResource.ResourceManager.GetString(name, _currentCulture), arguments)
        );

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var resourceSet = TextLocalizationResource.ResourceManager.GetResourceSet(_currentCulture, true, includeParentCultures);
            foreach (DictionaryEntry item in resourceSet)
            {
                yield return new LocalizedString(item.Key.ToString(), item.Value.ToString());
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            if (culture != null)
                return new Localize(culture);

            return new Localize();
        }
    }
}
