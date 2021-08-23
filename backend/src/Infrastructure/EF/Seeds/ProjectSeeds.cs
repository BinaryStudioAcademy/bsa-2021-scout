using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class ProjectSeeds
    {
        public static IEnumerable<Project> GetProjects()
        {
            return new List<Project> {
                new Project
                {
                    Id = "p9e10160-0522-4c2f-bfcf-a07e9faf0c04",
                    TeamInfo = "Quick Tech International Team",
                    Name = "BISON",
                    WebsiteLink = "http://edeinici.fk/tualo",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Project for the US, UK, and Canada markets. The mission is to eradicate white supremacy and build local power to intervene in violence inflicted on Black communities by the state and vigilantes.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                },
                new Project
                {
                    Id = "p8b0e8ca-54ff-4186-8cc0-5f71e1ec1d3c",
                    TeamInfo = "Future Solutions Team from Japan",
                    Name = "AFTON",
                    WebsiteLink = "http://wutfug.ms/kolhav",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Education platform project for little girls in uneducated arab counties.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
                },
                new Project
                {
                    Id = "p0679037-9b5e-45df-b24d-edc5bbbaaec4",
                    TeamInfo = "Angiko Team from US",
                    Name = "OXYGENE",
                    WebsiteLink = "http://zilavni.it/ovehakup",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "The climate change startup with aim to combate CO2 emmitions by 2023.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                },
                new Project
                {
                    Id = "paa3320f-866a-4b02-9076-5e8d12796710",
                    TeamInfo = "Fotetifuro Team from Europe",
                    Name = "CUSHMAN",
                    WebsiteLink = "http://seusdez.bt/gubuz",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Digitalization of Central Europe, by creating wide range of sevices provided via this project.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                },
                new Project
                {
                    Id = "pd45e3b4-cdf6-4f67-99de-795780c70b8f",
                    TeamInfo = "Merget GmbH Team from DE(NWR)",
                    Name = "DEKA",
                    WebsiteLink = "http://polpigmu.gq/rongejpi",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Project aim to solve illegal immigration ones and for all.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
                },
                new Project
                {
                    Id = "new10160-0522-4c2f-bfcf-a07e9faf0c04",
                    TeamInfo = "Fast Tech Team for fast MVPs",
                    Name = "PARAMOUNT",
                    WebsiteLink = "http://jur.sn/geknip",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Creating leading age post AR/VR experience base on ML developments.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                },
                new Project
                {
                    Id = "new0e8ca-54ff-4186-8cc0-5f71e1ec1d3c",
                    TeamInfo = "Innovative Solutions Team in Pekin",
                    Name = "MEADOWNS",
                    WebsiteLink = "http://dedubsa.cc/ze",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Project researching investment strategies and innovation for world problems solutions.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
                },
                new Project
                {
                    Id = "new79037-9b5e-45df-b24d-edc5bbbaaec4",
                    TeamInfo = "Magic Tech from HollyWood hill.",
                    Name = "NAVAJAS",
                    WebsiteLink = "http://wevmafu.se/zicaege",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Food waste problem solver. Organizing wasteless society with our innovationve technologies.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                },
                new Project
                {
                    Id = "new3320f-866a-4b02-9076-5e8d12796710",
                    TeamInfo = "Startup 4 U online based",
                    Name = "KINETIC",
                    WebsiteLink = "http://nowagi.cc/zow",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Starup with revolutionary anti-age formula. Combating ageing for affordable price.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                },
                new Project
                {
                    Id = "new5e3b4-cdf6-4f67-99de-795780c70b8f",
                    TeamInfo = "Scarlet GmbH huge Marvel fans",
                    Name = "BRIGADOON",
                    WebsiteLink = "http://acaji.io/gulzu",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Architecture building project for affordable luxurious alike housing all over the world.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
                },
                new Project
                {
                    Id = "snew3b4-cdf6-4f67-99de-795780c70b8f",
                    TeamInfo = "Scout Team from BinaryStudio Academy",
                    Name = "SCOUT",
                    WebsiteLink = "https://academy.binary-studio.com/ua/",
                     CreationDate = Common.GetRandomDateTime(new DateTime(2020, 12, 30), new DateTime(2021, 7, 30)),
                    Description = "Creating next gen HR managment service to grasp the best talant for lowest price.",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
                }
            };
        }
    }
}