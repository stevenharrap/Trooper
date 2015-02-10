namespace Trooper.Ui.Mvc.Bootstrap.Controls.Options
{
	public class Option<TKey> : Option<TKey, TKey>
	{
		public Option()
		{
		}

		public Option(TKey key)
		{
			this.Key = key;
			this.Value = key;
		}
	}


    /// <summary>
    /// Option used by select, checkbox and radio. 
    /// Dictionaries don't cut it because the key cant't be null.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Option<TKey, TValue>
    {
        public Option()
        {
        }

        public Option(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public TKey Key { get; set; }

        public TValue Value { get; set; }
    }
}
