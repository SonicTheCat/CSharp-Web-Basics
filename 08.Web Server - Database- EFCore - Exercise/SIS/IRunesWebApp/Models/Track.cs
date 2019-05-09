namespace IRunesWebApp.Models
{
    using System.Collections.Generic;

    public class Track : BaseModel<string>
    {
        public string Name { get; private set; }

        public string Link { get; private set; }

        public decimal Price { get; private set; }

        public virtual ICollection<AlbumTrack> Albums { get; set; } = new HashSet<AlbumTrack>(); 
    }
}