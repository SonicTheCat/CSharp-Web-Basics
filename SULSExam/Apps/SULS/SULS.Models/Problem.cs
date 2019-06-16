using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SULS.Models
{
    public class Problem
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int Points { get; set; }

        public IEnumerable<Submission> Submissions { get; set; } = new HashSet<Submission>(); 
    }
}