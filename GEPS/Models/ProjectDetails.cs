using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Graduate-Project-Evalution-System.GEPS.Models
{
    public class ProjectDetails
{
        public int ýd { get; set; }

        public string projectTitle { get; set; }

        public string projectDescription { get; set; }

        public string projectTeamName { get; set; }

        public string projectAdvisory { get; set; }

        public string TeamMember1FullName { get; set; }
        
        public string TeamMember1StudentNumber { get; set; }

        public string TeamMember2FullName { get; set; }

        public string TeamMember2StudentNumber { get; set; }

        public string TeamMember3FullName { get; set; }

        public string TeamMember3StudentNumber { get; set; }
}
}