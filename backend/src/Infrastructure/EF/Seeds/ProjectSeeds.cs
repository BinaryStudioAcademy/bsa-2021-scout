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
                TeamInfo = "Quick Tech International Team",
                Name = "BISON",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://edeinici.fk/tualo",
                Description = "Project for the US, UK, and Canada markets. The mission is to eradicate white supremacy and build local power to intervene in violence inflicted on Black communities by the state and vigilantes.",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "p8b0e8ca-54ff-4186-8cc0-5f71e1ec1d3c",
                TeamInfo = "Future Solutions Team from Japan",
                Name = "AFTON",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://wutfug.ms/kolhav",
                Description = "Education platform project for little girls in uneducated arab counties.",
                CompanyId = "1"
            },
            new Project{
                Id = "p0679037-9b5e-45df-b24d-edc5bbbaaec4",
                TeamInfo = "Angiko Team from US",
                Name = "OXYGENE",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://zilavni.it/ovehakup",
                Description = "The climate change startup with aim to combate CO2 emmitions by 2023.",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "paa3320f-866a-4b02-9076-5e8d12796710",
                TeamInfo = "Fotetifuro Team from Europe",
                Name = "CUSHMAN",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://seusdez.bt/gubuz",
                Description = "Digitalization of Central Europe, by creating wide range of sevices provided via this project.",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "pd45e3b4-cdf6-4f67-99de-795780c70b8f",
                TeamInfo = "Merget GmbH Team from DE(NWR)",
                Name = "DEKA",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://polpigmu.gq/rongejpi",
                Description = "Project aim to solve illegal immigration ones and for all.",
                CompanyId = "1"
            },
             new Project{
                Id = "new10160-0522-4c2f-bfcf-a07e9faf0c04",
                TeamInfo = "Fast Tech Team for fast MVPs",
                Name = "PARAMOUNT",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://jur.sn/geknip",
                Description = "Creating leading age post AR/VR experience base on ML developments.",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "new0e8ca-54ff-4186-8cc0-5f71e1ec1d3c",
                TeamInfo = "Innovative Solutions Team in Pekin",
                Name = "MEADOWNS",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://dedubsa.cc/ze",
                Description = "Project researching investment strategies and innovation for world problems solutions.",
                CompanyId = "1"
            },
            new Project{
                Id = "new79037-9b5e-45df-b24d-edc5bbbaaec4",
                TeamInfo = "Magic Tech from HollyWood hill.",
                Name = "NAVAJAS",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://wevmafu.se/zicaege",
                Description = "Food waste problem solver. Organizing wasteless society with our innovationve technologies.",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "new3320f-866a-4b02-9076-5e8d12796710",
                TeamInfo = "Startup 4 U online based",
                Name = "KINETIC",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://nowagi.cc/zow",
                Description = "Starup with revolutionary anti-age formula. Combating ageing for affordable price.",
                CompanyId = "1",
                Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
            },
            new Project{
                Id = "new5e3b4-cdf6-4f67-99de-795780c70b8f",
                TeamInfo = "Scarlet GmbH huge Marvel fans",
                Name = "BRIGADOON",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "http://acaji.io/gulzu",
                Description = "Architecture building project for affordable luxurious alike housing all over the world.",
                CompanyId = "1"
            },
            new Project{
                Id = "snew3b4-cdf6-4f67-99de-795780c70b8f",
                TeamInfo = "Scout Team from BinaryStudio Academy",
                Name = "SCOUT",
                CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                WebsiteLink = "https://academy.binary-studio.com/ua/",
                Description = "Creating next gen HR managment service to grasp the best talant for lowest price.",
                CompanyId = "1"
            }
        };
    }
}