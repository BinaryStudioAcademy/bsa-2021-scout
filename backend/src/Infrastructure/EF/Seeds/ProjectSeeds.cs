using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class ProjectSeeds
    {
        public static IEnumerable<Project> Projects { get; } = new List<Project>
        {
            new Project{
                Id = "p9e10160-0522-4c2f-bfcf-a07e9faf0c04",
                TeamInfo = "Quick Tech",
                Description = "Innovation in heart",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "p8b0e8ca-54ff-4186-8cc0-5f71e1ec1d3c",
                TeamInfo = "Future Solutions",
                Description = "Eco by deafault",
                CompanyId = "1"
            },
            new Project{
                Id = "p0679037-9b5e-45df-b24d-edc5bbbaaec4",
                TeamInfo = "Angiko",
                Description = "The best investment",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "paa3320f-866a-4b02-9076-5e8d12796710",
                TeamInfo = "Fotetifuro",
                Description = "Making the difference",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "pd45e3b4-cdf6-4f67-99de-795780c70b8f",
                TeamInfo = "Merget GmbH",
                Description = "Creating something new",
                CompanyId = "1"
            },
             new Project{
                Id = "new10160-0522-4c2f-bfcf-a07e9faf0c04",
                TeamInfo = "Fast Tech",
                Description = "Innovation right now",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "new0e8ca-54ff-4186-8cc0-5f71e1ec1d3c",
                TeamInfo = "Innovative Solutions",
                Description = "Eco in mind",
                CompanyId = "1"
            },
            new Project{
                Id = "new79037-9b5e-45df-b24d-edc5bbbaaec4",
                TeamInfo = "Magic Tech",
                Description = "The right investment",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "new3320f-866a-4b02-9076-5e8d12796710",
                TeamInfo = "Startup 4 U",
                Description = "Making you rich",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "new5e3b4-cdf6-4f67-99de-795780c70b8f",
                TeamInfo = "Scarlet GmbH",
                Description = "Doing some Marvel show",
                CompanyId = "1"
            },
            new Project{
                Id = "snew3b4-cdf6-4f67-99de-795780c70b8f",
                TeamInfo = "Scout",
                Description = "Innovating right now.",
                CompanyId = "1"
            }
        };
    }
}