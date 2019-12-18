using Neis.ProductKeyManager.Data;
using Neis.ProductKeyManager.Data.Microsoft;
using System;
using System.Collections.ObjectModel;

namespace Neis.ProductKeyManager.Extensions
{
    /// <summary>
    /// Generic class for keys
    /// </summary>
    public static class GenericDataExtensions
    {
        public static bool FindKey(this ObservableCollection<GenericKey> col, string key)
        {
            foreach(var gk in col)
            {
                if (gk.Value == key)
                    return true;
            }
            return false;
        }
        public static NotifiableBase GetExtraDataObject(this GenericKey gk)
        {
            var ed = gk.ExtraData;
            if (ed != null && 
                !string.IsNullOrWhiteSpace(ed.Type) && 
                !string.IsNullOrWhiteSpace(ed.Data))
            {
                Type t = Type.GetType(ed.Type);

                if (t == typeof(GenericKey))
                {
                    return Data.DataUtility<GenericKey>.FromJsonString(ed.Data);
                }
                else if (t == typeof(MicrosoftKey))
                {
                    return Data.DataUtility<MicrosoftKey>.FromJsonString(ed.Data);
                }
            }
            return null;
        }
    }
}