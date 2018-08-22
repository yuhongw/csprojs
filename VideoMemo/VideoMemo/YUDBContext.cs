using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMemo
{
    public class YUDBContext:DbContext
    {
        public YUDBContext():base("VideoMemo.Properties.Settings.cnn")
        {

        }

        public DbSet<Video> Videos { get; set; }

        internal static YUDBContext GetInstance()
        {
            return new YUDBContext();
        }
    }
}
