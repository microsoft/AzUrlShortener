using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TinyBlazorAdmin.Data
{
    public class ShortUrlRequest
    {
        private string _vanity;

        public string Title { get; set; }

        public string Vanity
        {
            get
            {
                return (_vanity != null) ? _vanity : string.Empty;
            }
            set
            {
                _vanity = value;
            }
        }

        [Required]
        public string Url { get; set; }

        private List<Schedule> _schedules;

        public List<Schedule> Schedules { 
            get{
                if(_schedules == null){
                    _schedules = new List<Schedule>();
                }
                return _schedules;
            } 
            set{
                _schedules = value;
            } 
        }
    }
}