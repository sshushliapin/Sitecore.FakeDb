namespace Sitecore.FakeDb
{
    using Sitecore.Data;

    public class UnversionedDbField : DbField
    {
        public UnversionedDbField(ID id)
            : base(id)
        {
        }

        public UnversionedDbField(string name)
            : base(name)
        {
        }

        public UnversionedDbField(string name, ID id)
            : base(name, id)
        {
        }

        public override void Add(string language, int version, string value)
        {
            base.Add(language, version, value);

            for (var i = version - 1; i > 0; --i)
            {
                this.Values[language][i] = value;
            }
        }
    }
}