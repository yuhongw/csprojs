using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMemo
{
    [Table("Video")]
    public class Video
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int check { get; set; }
        public string Key { get; set; }
        public double Position { get; set; }
        public string PositionStr { get; set; }
        public string SnapShot { get; set; }
    }
}
