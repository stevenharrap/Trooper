namespace Trooper.Ui.Mvc.Bootstrap.Controls.Options
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Option used by select, checkbox and radio. 
    /// Dictionaries don't cut it because the key cant't be null.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class OptionList<TKey, TValue> : IEnumerable<Option<TKey, TValue>>
    {
        public OptionList()
        {
            this.Items = new List<Option<TKey, TValue>>();
        }
        
        public List<Option<TKey, TValue>> Items { get; set; }

        public void Add(TKey key, TValue value)
        {
            this.Items.Add(new Option<TKey, TValue>(key, value));
        }

        public IEnumerator<Option<TKey, TValue>> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
