using System.ComponentModel.DataAnnotations;

namespace AarhusWebDev.ViewModels
{
    public class MessageBoard
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Message { get; set; }
    }
}