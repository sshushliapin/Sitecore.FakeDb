using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Diagnostics;

namespace Sitecore.FakeDb.Pipelines
{
    public class DsItemLoadingArgs : DbArgs
    {
        private readonly IDsDbItem dsDbItem;

        public DsItemLoadingArgs(Db db, IDsDbItem dsDbItem)
            : base(db)
        {
            Assert.ArgumentNotNull(dsDbItem, "dbItem");

            this.dsDbItem = dsDbItem;
        }

        public IDsDbItem DsDbItem
        {
            get { return this.dsDbItem; }
        }
    }
}
